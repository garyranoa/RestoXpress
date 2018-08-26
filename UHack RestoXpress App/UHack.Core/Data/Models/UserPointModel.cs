using System;
namespace CashClubApp.Core.Data.Models
{
    public class UserPointModel
    {
        public UserPointModel()
        {
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int? BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string BusinessImageUrl{ get; set; }
        public string BusinessAddress { get; set; }
        public string BusinessDistanceInMiles { get; set; }

        public double EarnedPoints { get; set; }
        public string SourcePoint { get; set; }
        public DateTime DateSpinned { get; set; }
    }
}
