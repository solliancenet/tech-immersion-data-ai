using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoAutoOpsWeb.Models
{
    [Table("SalesPerson", Schema = "Sales")]
    public class SalesPerson
    {
        [Key]
        public int BusinessEntityID { get; set; }
        public int TerritoryID { get; set; }
        public decimal? SalesQuota { get; set; }
        public decimal Bonus { get; set; }
        public decimal CommissionPct { get; set; }
        public decimal SalesYTD { get; set; }
        public decimal SalesLastYear { get; set; }
        public Guid rowguid { get; set; }
        [DataType(DataType.Date)]
        public DateTime ModifiedDate { get; set; }
    }
}