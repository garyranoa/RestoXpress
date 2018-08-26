using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
//using UHack.Core.Data.Models;
using UHack.Core.Services;
using Xamarin.Forms;
using UIKit;
using Foundation;
using System.Threading;

namespace CashClubApp.Core.Helpers
{
    public static class GeoLocationHelper
    {
        

        public static async Task<Position> GetCurrentPosition(int accuracy = 50)
        {
            Position position = null;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = accuracy;

                position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                {
                    //got a cahched position, so let's use it.
                    return position;
                }

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                {
                    //not available or enabled
                    return GetDefaultGeoLocation();
                    //return null;
                }

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location: " + ex);
            }

            if (position == null)
                return GetDefaultGeoLocation(); //return null;

            var output = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                    position.Timestamp, position.Latitude, position.Longitude,
                    position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);



            return position;
        }

        public static Position GetDefaultGeoLocation()
        {
            var position = new Position();
            position.Latitude = 39.521228;
            position.Longitude = -76.181314; 


            return position;
        }



    }
}
