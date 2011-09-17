/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using Anito.Data.Schema;
using Anito.Data.Common;

namespace Anito.Data.SqliteClient
{
    public class Translator : AdoTranslatorBase
    {
        private const string SELECT = "SELECT";
        private const string FROM = "FROM";
        private const string SPACE = " ";
        private const string COMMA = ",";

        public Translator(IProvider provider)
            : base(provider)
        {
            Provider = provider;
        }

        protected override Expression ProcessLambda(LambdaExpression expression)
        {
            LambdaExpression lambda = (LambdaExpression)StripQuotes(expression);
            ProcessExpression(lambda.Body);
            return expression;
        }


        protected override Expression ProcessMethodCall(MethodCallExpression expression)
        {
            if (expression.Method.DeclaringType == typeof(Queryable) && expression.Method.Name == "Where")
            {
                ProcessExpression(expression.Arguments[0]);
                CommandTextBuilder.Append(" AS T WHERE ");

                LambdaExpression lambda = (LambdaExpression)StripQuotes(expression.Arguments[1]);
                ProcessExpression(lambda.Body);
                return expression;
            }
            else if (expression.Method.DeclaringType == typeof(Queryable) && expression.Method.Name == "Single")
            {
                ProcessExpression(expression.Arguments[0]);
                CommandTextBuilder.Append(" AS T WHERE ");

                LambdaExpression lambda = (LambdaExpression)StripQuotes(expression.Arguments[1]);
                ProcessExpression(lambda.Body);
                return expression;
            }
            else if (expression.Method.DeclaringType == typeof(Queryable) && expression.Method.Name == "Select")
            { 
            
            }
            throw new NotSupportedException(string.Format("The method '{0}' is not supported", expression.Method.Name));
        }

        protected override Expression ProcessBinary(BinaryExpression expression)
        {

            CommandTextBuilder.Append("(");

            ProcessExpression(expression.Left);

            switch (expression.NodeType)
            {
                case ExpressionType.And | ExpressionType.AndAlso:
                    CommandTextBuilder.Append(" AND ");
                    break;
                case ExpressionType.Or | ExpressionType.OrElse:
                    CommandTextBuilder.Append(" OR ");
                    break;
                case ExpressionType.Equal:
                    CommandTextBuilder.Append(" = ");
                    break;
                case ExpressionType.NotEqual:
                    CommandTextBuilder.Append(" <> ");
                    break;
                case ExpressionType.LessThan:
                    CommandTextBuilder.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    CommandTextBuilder.Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    CommandTextBuilder.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    CommandTextBuilder.Append(" >= ");
                    break;
                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", expression.NodeType));

            }

            ProcessExpression(expression.Right);

            CommandTextBuilder.Append(")");

            return expression;

        }

        protected override Expression ProcessConstant(ConstantExpression expression)
        {
            IQueryable q = expression.Value as IQueryable;
            if (q != null)
            {
                StringBuilder columnBuilder = new StringBuilder();
                TypeTable schemaTable = Provider.GetSchema(q.ElementType);
                
                foreach (Anito.Data.Schema.TypeColumn column in schemaTable)
                {
                    if (columnBuilder.Length > 0)
                        columnBuilder.Append(COMMA + SPACE);
                    columnBuilder.Append(column.Name);
                }

                CommandTextBuilder.Append(string.Format(
                    "{0} {1} {2} {3}",
                    SELECT,
                    columnBuilder.ToString(),
                    FROM,
                    schemaTable.ViewSource
                    ));
                
            }
            else if (expression.Value == null)
            {
                CommandTextBuilder.Append("NULL");
            }
            else
            {
                switch (Type.GetTypeCode(expression.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        CommandTextBuilder.Append(((bool)expression.Value) ? 1 : 0);
                        break;
                    case TypeCode.String:
                        CommandTextBuilder.Append("'");
                        CommandTextBuilder.Append(expression.Value);
                        CommandTextBuilder.Append("'");
                        break;

                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", expression.Value));
                    default:
                        CommandTextBuilder.Append(expression.Value);
                        break;
                }
            }
            return expression;
        }

        protected override Expression ProcessMemberAccess(MemberExpression expression)
        {
            if (expression.Expression != null && expression.Expression.NodeType == ExpressionType.Parameter)
            {
                TypeTable schemaTable = Provider.GetSchema(expression.Member.DeclaringType);

                CommandTextBuilder.Append(schemaTable.GetDbColumn(expression.Member.Name));
                
                return expression;
            }
            else if (expression.Expression != null && expression.Expression.NodeType == ExpressionType.MemberAccess)
            {
                PropertyInfo propertyInfo = expression.Member as PropertyInfo;
                MemberExpression memberExpression = expression.Expression as MemberExpression;
                FieldInfo fieldInfo = memberExpression.Member as FieldInfo;
                ConstantExpression cExpression = memberExpression.Expression as ConstantExpression;

                object objectValue = fieldInfo.GetValue(cExpression.Value);
                object value = propertyInfo.GetValue(objectValue, null);

                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.String | TypeCode.DateTime:
                        CommandTextBuilder.Append(string.Format("'{0}'", value));
                        break;
                    default:
                        CommandTextBuilder.Append(value);
                        break;
                }
                return expression;
            }
            else if (expression.Expression != null && expression.Expression.NodeType == ExpressionType.Constant)
            {
                FieldInfo fieldInfo = expression.Member as FieldInfo;
                ConstantExpression cExpression = expression.Expression as ConstantExpression;
                object value = fieldInfo.GetValue(cExpression.Value);

                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.String | TypeCode.DateTime:
                        CommandTextBuilder.Append(string.Format("'{0}'", value));
                        break;
                    default:
                        CommandTextBuilder.Append(value);
                        break;
                }
                return expression;
            }
            else if (expression.Expression != null && expression.Expression.NodeType == ExpressionType.Call)
            {
                PropertyInfo propertyInfo = expression.Member as PropertyInfo;

                MethodCallExpression callExpression = expression.Expression as MethodCallExpression;

                MethodInfo methodInfo = callExpression.Method;
                object[] methodParams = new object[callExpression.Arguments.Count];
                for (int i = 0; i < callExpression.Arguments.Count; i++)
                {
                    ConstantExpression consExpression = callExpression.Arguments[i] as ConstantExpression;
                    methodParams[i] = consExpression.Value;
                }

                MemberExpression memberExpression = callExpression.Object as MemberExpression;
                FieldInfo fieldInfo = memberExpression.Member as FieldInfo;

                ConstantExpression cExpression = memberExpression.Expression as ConstantExpression;

                object objectContainer = fieldInfo.GetValue(cExpression.Value);

                object objectValue = methodInfo.Invoke(objectContainer, methodParams);

                object value = propertyInfo.GetValue(objectValue, null);

                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.String | TypeCode.DateTime:
                        CommandTextBuilder.Append(string.Format("'{0}'", value));
                        break;
                    default:
                        CommandTextBuilder.Append(value);
                        break;
                }
                return expression;
            }
            throw new NotSupportedException(string.Format("The member '{0}' is not supported", expression.Member.Name));
        }

        protected override Expression ProcessUnary(UnaryExpression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Not:
                    CommandTextBuilder.Append(" NOT ");
                    ProcessExpression(expression.Operand);
                    break;
                case ExpressionType.Quote:
                    LambdaExpression lambda = (LambdaExpression) StripQuotes(expression);
                    ProcessExpression(lambda.Body);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", expression.NodeType));
            }
            return expression;
        }
    }
}
