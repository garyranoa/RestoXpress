using System;
using SQLite;

namespace UHack.Core.Data.Domain
{
    [Table("sales")]
    public class Sales
    {

        [PrimaryKey]
        public int Id { get; set; }
        public string SalesGuid { get; set; }
        public string InvoiceId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public bool IsShipped { get; set; }
        public double Total { get; set; }
        public double Markup { get; set; }
        public DateTime CreatedOn { get; set; }


    }
}
