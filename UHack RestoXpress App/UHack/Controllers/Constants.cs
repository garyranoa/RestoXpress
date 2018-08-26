using System;
using UIKit;
using Foundation;
using System.Security.Cryptography;
using System.Drawing;
using CoreGraphics;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
//using UHack.Core.Helpers;

namespace UHack.Controllers
{
    /// <summary>
    /// UIView Drop Shadow : https://stackoverflow.com/questions/4754392/uiview-with-rounded-corners-and-drop-shadow
    /// </summary>
    public static class Constants
    {
        public const bool DEBUG_MODE = true;
        public const bool NOTIFICATION_DEBUG_MODE = false;
        public const long TightMargin = DefaultMargin / 2;
        public const long DefaultMargin = 16;
        public const long WideMargin = DefaultMargin * 2;
        public static UIColor MAIN_BACKGROUNDCOLOR { get { return new UIColor(red: 0.69f, green: 0.36f, blue: 0.76f, alpha: 1.0f); } }
        public const float DefaultPadding = 10;



        public static void SendLocalPushNotification(string message = "", string title = "")
        {
            UILocalNotification notification = new UILocalNotification();
            notification.FireDate = NSDate.Now.AddSeconds(10);
            if (!string.IsNullOrEmpty(title))
                notification.AlertAction = title;

            notification.SoundName = "default";
            notification.AlertBody = message;
            notification.ApplicationIconBadgeNumber = 1;
            UIApplication.SharedApplication.ScheduleLocalNotification(notification);
        }

        public static double DEGREES_TO_RADIANS(double angle)
        {
            return ((angle) / 180.0 * Math.PI);
        }

        public static float RandomFloat(Random random)
        {
            float f;
            do
            {
                byte[] bytes = new byte[4];
                random.NextBytes(bytes);
                f = BitConverter.ToSingle(bytes, 0);
            }
            while (float.IsInfinity(f) || float.IsNaN(f));
            return f;
        }

        public static UIColor UIColorFromRGB(int rgbValue)
        {
            return UIColor.FromRGB(
                (((float)((rgbValue & 0xFF0000) >> 16)) / 255.0f),
                (((float)((rgbValue & 0xFF00) >> 8)) / 255.0f),
                (((float)(rgbValue & 0xFF)) / 255.0f)
            );
        }

        private static RNGCryptoServiceProvider Rand = new RNGCryptoServiceProvider();
        public static int RandomInteger(int min, int max)
        {
            uint scale = uint.MaxValue;
            while (scale == uint.MaxValue)
            {
                // Get four random bytes.
                byte[] four_bytes = new byte[4];
                Rand.GetBytes(four_bytes);

                // Convert that into an uint.
                scale = BitConverter.ToUInt32(four_bytes, 0);
            }

            // Add min to the scaled difference between max and min.
            return (int)(min + (max - min) *
                (scale / (double)uint.MaxValue));
        }

        // Return a random integer between a min and max value.
        public static int GenerateRandomInteger(int min, int max, int?[] excluded)
        {
            if (min == max)
            {
                return min;
            }

            if (min < 1 && max < 2)
            {
                return new Random(DateTime.UtcNow.Millisecond).NextDouble() >= 0.5 ? 0 : 1;
            }

            if (min < 1 && max < 3)
            {
                var r = new Random(DateTime.UtcNow.Millisecond).NextDouble();
                if (r <= 0.2)
                    return 2;
                else if (r <= 0.3)
                    return 1;
                else if (r >= 0.4)
                    return 0;
            }

            uint scale = uint.MaxValue;
            while (scale == uint.MaxValue)
            {
                // Get four random bytes.
                byte[] four_bytes = new byte[4];

                Rand.GetBytes(four_bytes);

                // Convert that into an uint.
                scale = BitConverter.ToUInt32(four_bytes, 0);
            }

            // Add min to the scaled difference between max and min.
            var ret = (int)(min + (max - min) *
                (scale / (double)uint.MaxValue));

            if (excluded != null && excluded.Any(e => e == ret))
            {
                return GenerateRandomInteger(min, max, excluded);
            }

            return ret;

        }

        public static UIImage MaxResizeImage(UIImage sourceImage, float maxWidth, float maxHeight)
        {
            var sourceSize = sourceImage.Size;
            var maxResizeFactor = Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
            if (maxResizeFactor > 1) return sourceImage;
            var width = maxResizeFactor * sourceSize.Width;
            var height = maxResizeFactor * sourceSize.Height;
            UIGraphics.BeginImageContext(new SizeF((float)width, (float)height));
            sourceImage.Draw(new RectangleF(0, 0, (float)width, (float)height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return resultImage;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }


        public static string GetName<T>(Expression<Func<T>> expr)
        {
            var body = ((MemberExpression)expr.Body);
            return body.Member.Name;
        }

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]", "", RegexOptions.Compiled);
        }
    }
}
