using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHack.Core.Domain.Users
{
    public partial class Product : BaseEntity
    {
        public Product()
        {
            this.ProductGuid = Guid.NewGuid().ToString();
        }

        public string ProductGuid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public decimal CostPrice { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Location { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
