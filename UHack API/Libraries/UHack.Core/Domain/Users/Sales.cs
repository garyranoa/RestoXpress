using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHack.Core.Domain.Users
{
    public partial class Sales : BaseEntity
    {
        public Sales()
        {
            this.SalesGuid = Guid.NewGuid().ToString();
        }

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
