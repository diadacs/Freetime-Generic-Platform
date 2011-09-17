/////////////////////////////////////////////////////////////////////////////////////////////////////
// Original Code by : Michael Dela Cuesta (michael.dcuesta@gmail.com)                              //
// Source Code Available : http://anito.codeplex.com                                               //
//                                                                                                 // 
// This source code is made available under the terms of the Microsoft Public License (MS-PL)      // 
///////////////////////////////////////////////////////////////////////////////////////////////////// 

using System.Linq.Expressions;
using System.Text;

namespace Anito.Data.Common
{
    public abstract class AdoTranslatorBase
    {
        protected virtual StringBuilder CommandTextBuilder { get; set; }

        protected IProvider Provider { get; set; }

        protected AdoTranslatorBase(IProvider provider)
        {
            Provider = provider;
        }

        public virtual string Translate(Expression expression)
        {
            CommandTextBuilder = new StringBuilder();
            ProcessExpression(expression);
            return CommandTextBuilder.ToString();
        }

        protected virtual void ProcessExpression(Expression expression)
        {
            var type = expression.GetType();

            if (type == typeof(MethodCallExpression))
                ProcessMethodCall(expression as MethodCallExpression);
            else if (type == typeof(BinaryExpression))
                ProcessBinary(expression as BinaryExpression);
            else if (type == typeof(ConstantExpression))
                ProcessConstant(expression as ConstantExpression);
            else if (type == typeof(MemberExpression))
                ProcessMemberAccess(expression as MemberExpression);
            else if (type == typeof(UnaryExpression))
                ProcessUnary(expression as UnaryExpression);
            else if (type == typeof(LambdaExpression))
                ProcessLambda(expression as LambdaExpression);
            else if (type.BaseType == typeof(LambdaExpression))
                ProcessLambda(expression as LambdaExpression);
 
        }

        protected abstract Expression ProcessLambda(LambdaExpression expression);

        protected abstract Expression ProcessMethodCall(MethodCallExpression expression);

        protected abstract Expression ProcessBinary(BinaryExpression expression);

        protected abstract Expression ProcessConstant(ConstantExpression expression);

        protected abstract Expression ProcessMemberAccess(MemberExpression expression);

        protected abstract Expression ProcessUnary(UnaryExpression expression);

        protected virtual Expression StripQuotes(Expression expression)
        {
            while (expression.NodeType == ExpressionType.Quote)
                expression = ((UnaryExpression)expression).Operand;
            return expression;
        }
    }
}
