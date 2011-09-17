using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anito.Data.MongoDBClient.Exception
{
    public class ColumnNotFoundException : System.Exception
    {
        private const string MESSAGE_FORMAT = "Column {0} does not exists";
        
        private string ColumnName { get; set; }
        
        public override string Message
        {
            get
            {
                return string.Format(MESSAGE_FORMAT, ColumnName);   
            }
        }

        internal ColumnNotFoundException(string columnName)
        {
            ColumnName = columnName;
        }
    }
}
