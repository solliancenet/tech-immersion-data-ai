using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoAutoOpsWeb.Models
{
    [Table("SalesOrderDetail", Schema = "Sales")]
    public class SalesOrderDetail
    {
        public int SalesOrderID { get; set; }
        [Key]
        public int SalesOrderDetailID { get; set; }
        public string CarrierTrackingNumber { get; set; }
        public short OrderQty { get; set; }
        public int ProductID { get; set; }
        public int SpecialOfferID { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitPriceDiscount { get; set; }
        public decimal LineTotal { get; set; }
        public Guid rowguid { get; set; }
        [DataType(DataType.Date)]
        public DateTime ModifiedDate { get; set; }
    }
}