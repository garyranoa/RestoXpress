using System;
using CoreLocation;
using MapKit;
using Foundation;
using UIKit;
using System.Drawing;

namespace UHack.Controllers
{
    public class MkMapViewExtensions
    {
        public static void SetCenterCoordinate(MKMapView MapToCenter, CLLocationCoordinate2D centerCoordinate, int zoomLevel, bool animated)
        {
            // clamp large numbers to 28
            zoomLevel = Math.Min(zoomLevel, 28);
            //zoomLevel = 80;

            // use the zoom level to compute the region
            MKCoordinateSpan span = CoordinateSpanWithMapView(MapToCenter, centerCoordinate, zoomLevel);
            MKCoordinateRegion region = new MKCoordinateRegion(centerCoordinate, span);

            // set the region like normal
            MapToCenter.SetRegion(region, animated);
        }

        static double MERCATOR_OFFSET = 268435456;
        static double MERCATOR_RADIUS = 85445659.44705395;

        static double LongitudeToPixelSpaceX(double longitude)
        {
            return Math.Round(MERCATOR_OFFSET + MERCATOR_RADIUS * longitude * Math.PI / 180.0);
        }

        static double LatitudeToPixelSpaceY(double latitude)
        {
            return Math.Round(MERCATOR_OFFSET - MERCATOR_RADIUS * Math.Log((1 + Math.Sin(latitude * Math.PI / 180.0)) / (1 - Math.Sin(latitude * Math.PI / 180.0))) / 2.0);
        }

        static double PixelSpaceXToLongitude(double pixelX)
        {
            return ((Math.Round(pixelX) - MERCATOR_OFFSET) / MERCATOR_RADIUS) * 180.0 / Math.PI;
        }

        static double PixelSpaceYToLatitude(double pixelY)
        {
            return (Math.PI / 2.0 - 2.0 * Math.Tan(Math.Exp((Math.Round(pixelY) - MERCATOR_OFFSET) / MERCATOR_RADIUS))) * 180.0 / Math.PI;
        }


        static MKCoordinateSpan CoordinateSpanWithMapView(MKMapView mapView, CLLocationCoordinate2D centerCoordinate, int zoomLevel)
        {
            // convert center coordiate to pixel space
            double centerPixelX = LongitudeToPixelSpaceX(centerCoordinate.Longitude);
            double centerPixelY = LatitudeToPixelSpaceY(centerCoordinate.Latitude);

            // determine the scale value from the zoom level
            int zoomExponent = 20 - zoomLevel;
            double zoomScale = Math.Pow(2, zoomExponent);

            // scale the map’s size in pixel space

            SizeF mapSizeInPixels = (SizeF)mapView.Bounds.Size;
            double scaledMapWidth = mapSizeInPixels.Width * zoomScale;
            double scaledMapHeight = mapSizeInPixels.Height;

            // figure out the position of the top-left pixel
            double topLeftPixelX = centerPixelX - (scaledMapWidth / 2);
            double topLeftPixelY = centerPixelY - (scaledMapHeight / 2);

            // find delta between left and right longitudes
            double minLng = PixelSpaceXToLongitude(topLeftPixelX);
            double maxLng = PixelSpaceXToLongitude(topLeftPixelX + scaledMapWidth);
            double longitudeDelta = maxLng - minLng;

            // find delta between top and bottom latitudes
            double minLat = PixelSpaceYToLatitude(topLeftPixelY);
            double maxLat = PixelSpaceYToLatitude(topLeftPixelY + scaledMapHeight);
            double latitudeDelta = -1 * (maxLat - minLat);

            double delta = 0.004f;
            latitudeDelta = delta;
            longitudeDelta = delta;

            // create and return the lat/lng span
            MKCoordinateSpan span = new MKCoordinateSpan(latitudeDelta, longitudeDelta);

            return span;
        }

    }
}
