using System;
using SQLite;

namespace CashClubApp.Core.Data.Domain
{
    [Table("merchants")]
    public class Merchant
    {
        public Merchant() {}

        [PrimaryKey]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string BusinessGuid { get; set; }
        public string Name { get; set; }
        public int ClubId { get; set; }
        public int BusinessTypeId { get; set; }
        public int BusinessTypeCategoryId { get; set; }
        public string ImageUrl { get; set; }
        public string GeoLatitude { get; set; }
        public string GeoLongitude { get; set; }
        public string Address { get; set; }
        public string Biography { get; set; }
        public string ZipCode { get; set; }
        public string StateId { get; set; }
        public string ContactNumber { get; set; }
        public string BusinessEmail { get; set; }
        public string BusinessHours { get; set; }
        public string Tags { get; set; }

    }
}
