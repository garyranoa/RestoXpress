using System;
using SQLite;
namespace UHack.Core.Data.Domain
{
    [Table("products")]
    public class Product
    {

        [PrimaryKey]
        public int Id { get; set; }
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
