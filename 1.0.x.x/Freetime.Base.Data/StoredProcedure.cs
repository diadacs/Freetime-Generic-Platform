using Anito.Data;

namespace Freetime.Base.Data
{
    public class StoredProcedure
    {
        public enum ParameterType
        {
            Binary,
            Boolean,
            Byte,
            Currency,
            DateTime,
            Decimal,
            Double,
            Int16,
            Int32,
            Int64,
            Single,
            String,
            UId
        }

        internal Procedure Procedure { get; private set; }

        public StoredProcedure(string name)
        {
            Procedure = new Procedure(name);
        }

        public void AddParameter(string parameterName, ParameterType parameterType, int size, object value)
        {
            Procedure.AddParameter(parameterName, GetParameterType(parameterType), size, value);
        }

        private static Anito.Data.ParameterType GetParameterType(ParameterType parameterType)
        {
            switch (parameterType)
            { 
                case ParameterType.Binary:
                    return Anito.Data.ParameterType.Binary;
                case ParameterType.Boolean:
                    return Anito.Data.ParameterType.Boolean;
                case ParameterType.Byte:
                    return Anito.Data.ParameterType.Byte;
                case ParameterType.Currency:
                    return Anito.Data.ParameterType.Currency;
                case ParameterType.DateTime:
                    return Anito.Data.ParameterType.DateTime;
                case ParameterType.Decimal:
                    return Anito.Data.ParameterType.Decimal;
                case ParameterType.Double:
                    return Anito.Data.ParameterType.Double;
                case ParameterType.Int16:
                    return Anito.Data.ParameterType.Int16;
                case ParameterType.Int32:
                    return Anito.Data.ParameterType.Int32;
                case ParameterType.Int64:
                    return Anito.Data.ParameterType.Int64;
                case ParameterType.Single:
                    return Anito.Data.ParameterType.Single;
                case ParameterType.String:
                    return Anito.Data.ParameterType.String;
                case ParameterType.UId:
                    return Anito.Data.ParameterType.UId;
                default:
                    return Anito.Data.ParameterType.String;
            }
        }


    }
}
