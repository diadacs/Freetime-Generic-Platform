
using Anito.Data.Mapping;

namespace Anito.Test.Entities
{
    [Source(View = "Customer", Update = "Customer")]
    public class Customer
    {

        [DataField(FieldName = "ID",
            Identity = true,
            PrimaryKey = true)]
        public int ID { get; set; }

        [DataField(FieldName = "Name")]
        public string Name { get; set; }

        [DataField(FieldName = "DefaultContactID")]
        public int DefaultContactID { get; set; }

        [DataField(FieldName = "ProfileID")]
        public int ProfileID { get; set; }

        [DataField(FieldName = "Balance")]
        public decimal Balance { get; set; }

        [DataField(FieldName = "BalanceRate")]
        public decimal BalanceRate { get; set; }
              
    }
}
