using System;
using System.Collections.Generic;
namespace CashClubApp.Core.Data.Models
{
    public partial class DirectoryModel
    {
        public DirectoryModel()
        {
            Deals = new List<MerchantDiscountModel>();
        
        }

        public int MerchantId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string SeoUrl { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double DistanceInMiles { get; set; }
        public double DistanceInMeters { get; set; }
        public double DistanceInFeet { get; set; }
        public string StateId { get; set; }
        public int ClubId { get; set; }
        public bool AllClub { get; set; }
        public List<MerchantDiscountModel> Deals { get; set; }
    }

    public partial class MerchantDiscountModel
    {
        public int Id { get; set; }
        public string DiscountText { get; set; }
        public string Description { get; set; }

        public string BusinessDealGuid { get; set; }
        public string FullDescription { get; set; }
        public int BusinessId { get; set; }
        public int DealTypeId { get; set; }
        public int SpecialDealTypeId { get; set; }
        public string ImageUrl { get; set; }
        public int DiscountTypeId { get; set; }
        public decimal Discount { get; set; }
        public decimal AverageCost { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool DuringOpenHours { get; set; }
        public bool RunOnSunday { get; set; }
        public bool RunOnMonday { get; set; }
        public bool RunOnTuesday { get; set; }
        public bool RunOnWednesday { get; set; }
        public bool RunOnThursday { get; set; }
        public bool RunOnFriday { get; set; }
        public bool RunOnSaturday { get; set; }
        public bool Active { get; set; }
    }

    public partial class HotDealsModel
    {
        public int MerchantId { get; set; }
        public string Name { get; set; }
        public string GeoLatitude { get; set; }
        public string GeoLongitude { get; set; }
        public int DiscountTypeId { get; set; }
        public decimal Discount { get; set; }
        public string ImageUrl { get; set; }
        public bool AllClub { get; set; }

        public string DiscountText { get; set; }
        public string Description { get; set; }
        public double DistanceInMiles { get; set; }
    }
}
