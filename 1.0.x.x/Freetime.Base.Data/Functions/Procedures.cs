using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Freetime.Base.Data.Functions
{
    public class Procedures
    {
        internal static StoredProcedure GenerateDocumentCode(string transactionType)
        {
            //TODO : XML config for SP name
            StoredProcedure sp = new StoredProcedure("GenerateDocumentCode");
            sp.AddParameter("@TransactionType", StoredProcedure.ParameterType.String, 30, transactionType);
            return sp;
        }
    }
}
