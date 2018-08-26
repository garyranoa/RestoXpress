using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using UHack.Core.ComponentModel;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Hosting;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using System.Device.Location;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace UHack.Core
{
    /// <summary>
    /// Represents a common helper
    /// </summary>
    public partial class CommonHelper
    {

        public static double CalculateDistanceInMeters(double sourceLat, double sourceLng, double destLat, double destLng)
        // from http://stackoverflow.com/questions/6366408/calculating-distance-between-two-latitude-and-longitude-geocoordinates
        {
            var sourceLocation = new GeoCoordinate(sourceLat, sourceLng);
            var targetLocation = new GeoCoordinate(destLat, destLng);

            return sourceLocation.GetDistanceTo(targetLocation);
        }

        public static string CalculateDistanceInMetersUsingDistanceMatrix(double sourceLat, double sourceLng, double destLat, double destLng, string apiKey, out string source)
        // from http://stackoverflow.com/questions/6366408/calculating-distance-between-two-latitude-and-longitude-geocoordinates
        {
           
            // sample
            //var src = @"https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=40.6655101,-73.89188969999998&destinations=40.6905615%2C-73.9976592&key=AIzaSyBip6bGftgvkkidrq_NdHCGgySDhW4mTz8";

            source = "";
            try
            {
                // using distance matrix
                var src = String.Format("https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&mode=driving&origins={0},{1}&destinations={2},{3}&key={4}", sourceLat, sourceLng, destLat, destLng, apiKey);
                source = src;

                var webClient = new WebClient();
                string responseString = webClient.DownloadString(src);
                return responseString;
            }
            catch(Exception ex)
            {

            }
            return string.Empty;
        }

        public static string CalculateDistanceInMetersUsingDistanceMatrix(string origin, string destination, string apiKey)
        // from http://stackoverflow.com/questions/6366408/calculating-distance-between-two-latitude-and-longitude-geocoordinates
        {
            // sample
            //var src = @"https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=40.6655101,-73.89188969999998&destinations=40.6905615%2C-73.9976592&key=AIzaSyBip6bGftgvkkidrq_NdHCGgySDhW4mTz8";

            try
            {
                // using distance matrix
                var src = String.Format("https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&mode=driving&origins={0}&destinations={1}&key={2}", origin, destination, apiKey);

                Uri uri = new Uri(src);
                var webClient = new WebClient();
                string responseString = webClient.DownloadString(uri);
                return responseString;
            }
            catch(Exception ex)
            {

            }
            return string.Empty;
        }

        public static string GetGeolocationUsingGoogle(string apiKey)
        {
            try
            {
                var src = String.Format("https://www.googleapis.com/geolocation/v1/geolocate?key={0}", apiKey);
                src = String.Format("https://maps.googleapis.com/maps/api/geocode/json?key={0}", apiKey);

                Uri uri = new Uri(src);
                var webClient = new WebClient();
                string responseString = webClient.DownloadString(uri);
                return responseString;
            }
            catch (Exception ex)
            {

            }
            return string.Empty;
        }

        /// <summary>
        /// Get current Address From Geo Latitude and Longitude
        /// </summary>
        /// <param name="latlng"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public static string GetAddressFromGeoLatLng(string latlng, string apiKey)
        {
            // sample
            //var src = @"https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=40.6655101,-73.89188969999998&destinations=40.6905615%2C-73.9976592&key=AIzaSyBip6bGftgvkkidrq_NdHCGgySDhW4mTz8";

            try
            {
                var src = String.Format("https://maps.googleapis.com/maps/api/geocode/json?latlng={0}&key={1}", latlng, apiKey);

                Uri uri = new Uri(src);
                var webClient = new WebClient();
                string responseString = webClient.DownloadString(uri);
                return responseString;
            }
            catch (Exception ex)
            {

            }
            return string.Empty;
        }

        public static async Task<string> AsyncCalculateDistanceInMetersUsingDistanceMatrix(double sourceLat, double sourceLng, double destLat, double destLng, string apiKey)
        // from http://stackoverflow.com/questions/6366408/calculating-distance-between-two-latitude-and-longitude-geocoordinates
        {
            ///address params
            ///https://developers.google.com/maps/documentation/distance-matrix/start
            ///https://developers.google.com/maps/documentation/distance-matrix/intro
            ///
            // sample
            //var src = @"https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=40.6655101,-73.89188969999998&destinations=40.6905615%2C-73.9976592&key=AIzaSyBip6bGftgvkkidrq_NdHCGgySDhW4mTz8";

            // using distance matrix
            var src = String.Format("https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins={0},{1}&destinations={2},{3}&key={4}", sourceLat, sourceLng, destLat, destLng, apiKey);

            using (var client = new HttpClient())
            {
                Uri uri = new Uri(src);
                var request = await client.GetAsync(uri);
                if (request.IsSuccessStatusCode)
                {
                    var content = await request.Content.ReadAsStringAsync();
                    return content;
                }
            }

            return string.Empty;
            
        }

        public static String[] GetLocation(string ipaddress)
        {

            // here is the API Key: 2000cf96f94f92711724080f77d60246ff9e5eaac83d14076a1fb090346e3a8c
            // User Id : s******.acs@gmail.com 
            // :)
            try
            {
                String responseString;
                String[] responseArray = new String[20];
                responseArray[0] = "ERROR";

                if (ipaddress != "127.0.0.1")
                {
                    var webClient = new WebClient();
                    responseString = webClient.DownloadString(String.Format("http://api.ipinfodb.com/v3/ip-city/?key=2000cf96f94f92711724080f77d60246ff9e5eaac83d14076a1fb090346e3a8c&ip=" + ipaddress));
                    // OUTPUT                
                    // the result is "OK;;183.182.87.10;IN;INDIA;MADHYA PRADESH;INDORE;-;22.7186;75.8558;+05:30"
                    responseArray = responseString.Split(';');
                    // or we can use http://api.hostip.info/?ip='' , this will return a xml file and then go for xmlReader
                }
                return responseArray;
            }
            catch (Exception ex)
            {
                throw ex;
                //Response.Write(ex.Message);
            }
        }

        /// <summary>
        /// Get System Timezones
        /// </summary>
        /// <returns></returns>
        public static ReadOnlyCollection<TimeZoneInfo> GetSystemTimezones()
        {
            return TimeZoneInfo.GetSystemTimeZones();
        }

        /// <summary>
        /// Decode QR Code
        /// </summary>
        /// <param name="qrCodePath"></param>
        /// <returns></returns>
        public static string DecodeQRCode(string qrCodePath)
        {
            string result = "";

            /*try
            {
                QRCodeReader reader = new QRCodeReader();

                // load a bitmap
                qrCodePath = qrCodePath.Replace(@"\\", @"/");
                Bitmap bitmap = new Bitmap(qrCodePath);

                LuminanceSource s = new RGBLuminanceSource(bitmap, bitmap.Width, bitmap.Height);
                BinaryBitmap binBitmap = new BinaryBitmap(new GlobalHistogramBinarizer(s));
                result = reader.decode(binBitmap).Text.ToString();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }*/

            return result;
        }

        public static Bitmap GenerateQRCode(string qrCodeText, ImageFormat imageFormat, int height = 600, int width = 600)
        {
            var _qrCodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Margin = 1,
                    Height = height,
                    Width = width,
                    ErrorCorrection = ErrorCorrectionLevel.M
                },
            };

            var writeableBitmap = _qrCodeWriter.Write(qrCodeText);
            return writeableBitmap;
        }

        public static void GenerateQRCode(string qrFilename, string qrCodeText, string qrCodePath, ImageFormat imageFormat, int height = 600, int width = 600)
        {
            var _qrCodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Margin = 1,
                    Height = height,
                    Width = width,
                    ErrorCorrection = ErrorCorrectionLevel.Q
                },
            };

            var writeableBitmap = _qrCodeWriter.Write(qrCodeText);
            writeableBitmap.Save(qrCodePath + qrFilename, imageFormat);
        }

        /// <summary>
        /// Generate QR Code
        /// </summary>
        /// <param name="qrCodeName"></param>
        /// <param name="qrCodePath"></param>
        /// <param name="size"></param>
        public static void GenerateQRCodeOld(string qrFilename, string qrCodeText, string qrCodePath, ImageFormat imageFormat, int size = 200)
        {

            if (System.IO.File.Exists(qrCodePath + qrFilename))
                return;
            
            /*QRCodeWriter writer = new QRCodeWriter();
            com.google.zxing.common.ByteMatrix matrix;
            
            matrix = writer.encode(qrCodeText, BarcodeFormat.QR_CODE, size, size);

            Bitmap img = new Bitmap(size, size);
            Color Color = Color.FromArgb(0, 0, 0);

            for (int y = 0; y < matrix.Height; ++y)
            {
                for (int x = 0; x < matrix.Width; ++x)
                {
                    Color pixelColor = img.GetPixel(x, y);

                    //Find the colour of the dot
                    if (matrix.get_Renamed(x, y) == -1)
                    {
                        img.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        img.SetPixel(x, y, Color.Black);
                    }
                }
            }


            img.Save(qrCodePath + qrFilename, imageFormat);*/
        }

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]", "", RegexOptions.Compiled);
        }

        public static string CheckPhoneValid(string phone, out string message)
        {
            message = "";
            //Regex regex = new Regex(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
            Regex regex = new Regex(@"^(\+?(1|63)-?)?(\([2-9]\d{2}\)|[2-9]\d{2})-?[2-9]\d{2}-?\d{4}$");
            Match match = regex.Match(phone);
            if (!match.Success)
            {
                message = "The phone number you entered is not valid.";

                if (phone.StartsWith("+63") && phone.Length == 13)
                {
                    message = "";
                }
            }
            else
            {
                //var startsWith = country_iso3_to_country_calling_code().Where(x => x.Any(y => x.StartsWith(y)));

                if (phone.StartsWith("+1"))
                {
                    if (phone.Length < 12 || phone.Length > 12)
                    {
                        message = "The phone number you entered is not valid. Must be 10 digit, with or without +1.";
                    }
                }
                else if (phone.StartsWith("+63"))
                {
                    if (phone.Length < 13 || phone.Length > 13)
                    {
                        message = "The phone number you entered is not valid. Must be 10 digit, with or without +63.";
                    }
                }
                else
                {
                    if (phone.Length < 10 || phone.Length > 10)
                    {
                        message = "The phone number you entered is not valid. Must be 10 digit, with or without +1.";
                    }

                    if (phone.Length == 10)
                    {
                        phone = $"+1{phone}";
                    }
                }
            }
            return phone;
        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

        /// <summary>
        /// Ensures the subscriber email or throw.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public static string EnsureSubscriberEmailOrThrow(string email)
        {
            string output = EnsureNotNull(email);
            output = output.Trim();
            output = EnsureMaximumLength(output, 255);

            if (!IsValidEmail(output))
            {
                throw new AppException("Email is not valid.");
            }

            return output;
        }

        /// <summary>
        /// Verifies that a string is in valid e-mail format
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
        public static bool IsValidEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                return false;

            email = email.Trim();
            var result = Regex.IsMatch(email, "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.IgnoreCase);
            return result;
        }

        /// <summary>
        /// Generate random digit code
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns>Result string</returns>
        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            string str = string.Empty;
            for (int i = 0; i < length; i++)
                str = String.Concat(str, random.Next(10).ToString());
            return str;
        }

        /// <summary>
        /// Returns an random interger number within a specified rage
        /// </summary>
        /// <param name="min">Minimum number</param>
        /// <param name="max">Maximum number</param>
        /// <returns>Result</returns>
        public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue)
        {
            var randomNumberBuffer = new byte[10];
            new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
            return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
        }

        /// <summary>
        /// Ensure that a string doesn't exceed maximum allowed length
        /// </summary>
        /// <param name="str">Input string</param>
        /// <param name="maxLength">Maximum length</param>
        /// <param name="postfix">A string to add to the end if the original string was shorten</param>
        /// <returns>Input string if its lengh is OK; otherwise, truncated input string</returns>
        public static string EnsureMaximumLength(string str, int maxLength, string postfix = null)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            if (str.Length > maxLength)
            {
                var result = str.Substring(0, maxLength);
                if (!String.IsNullOrEmpty(postfix))
                {
                    result += postfix;
                }
                return result;
            }

            return str;
        }

        /// <summary>
        /// Ensures that a string only contains numeric values
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Input string with only numeric values, empty string if input is null/empty</returns>
        public static string EnsureNumericOnly(string str)
        {
            if (String.IsNullOrEmpty(str))
                return string.Empty;

            var result = new StringBuilder();
            foreach (char c in str)
            {
                if (Char.IsDigit(c))
                    result.Append(c);
            }
            return result.ToString();
        }

        /// <summary>
        /// Ensure that a string is not null
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Result</returns>
        public static string EnsureNotNull(string str)
        {
            if (str == null)
                return string.Empty;

            return str;
        }

        /// <summary>
        /// Indicates whether the specified strings are null or empty strings
        /// </summary>
        /// <param name="stringsToValidate">Array of strings to validate</param>
        /// <returns>Boolean</returns>
        public static bool AreNullOrEmpty(params string[] stringsToValidate)
        {
            bool result = false;
            Array.ForEach(stringsToValidate, str =>
            {
                if (string.IsNullOrEmpty(str)) result = true;
            });
            return result;
        }

        /// <summary>
        /// Compare two arrasy
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="a1">Array 1</param>
        /// <param name="a2">Array 2</param>
        /// <returns>Result</returns>
        public static bool ArraysEqual<T>(T[] a1, T[] a2)
        {
            //also see Enumerable.SequenceEqual(a1, a2);
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i])) return false;
            }
            return true;
        }

        private static AspNetHostingPermissionLevel? _trustLevel;
        /// <summary>
        /// Finds the trust level of the running application (http://blogs.msdn.com/dmitryr/archive/2007/01/23/finding-out-the-current-trust-level-in-asp-net.aspx)
        /// </summary>
        /// <returns>The current trust level.</returns>
        public static AspNetHostingPermissionLevel GetTrustLevel()
        {
            if (!_trustLevel.HasValue)
            {
                //set minimum
                _trustLevel = AspNetHostingPermissionLevel.None;

                //determine maximum
                foreach (AspNetHostingPermissionLevel trustLevel in new[] {
                                AspNetHostingPermissionLevel.Unrestricted,
                                AspNetHostingPermissionLevel.High,
                                AspNetHostingPermissionLevel.Medium,
                                AspNetHostingPermissionLevel.Low,
                                AspNetHostingPermissionLevel.Minimal 
                            })
                {
                    try
                    {
                        new AspNetHostingPermission(trustLevel).Demand();
                        _trustLevel = trustLevel;
                        break; //we've set the highest permission we can
                    }
                    catch (System.Security.SecurityException)
                    {
                        continue;
                    }
                }
            }
            return _trustLevel.Value;
        }

        /// <summary>
        /// Sets a property on an object to a valuae.
        /// </summary>
        /// <param name="instance">The object whose property to set.</param>
        /// <param name="propertyName">The name of the property to set.</param>
        /// <param name="value">The value to set the property to.</param>
        public static void SetProperty(object instance, string propertyName, object value)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            if (propertyName == null) throw new ArgumentNullException("propertyName");

            Type instanceType = instance.GetType();
            PropertyInfo pi = instanceType.GetProperty(propertyName);
            if (pi == null)
                throw new AppException("No property '{0}' found on the instance of type '{1}'.", propertyName, instanceType);
            if (!pi.CanWrite)
                throw new AppException("The property '{0}' on the instance of type '{1}' does not have a setter.", propertyName, instanceType);
            if (value != null && !value.GetType().IsAssignableFrom(pi.PropertyType))
                value = To(value, pi.PropertyType);
            pi.SetValue(instance, value, new object[0]);
        }

        public static TypeConverter GetUHackCustomTypeConverter(Type type)
        {
            //we can't use the following code in order to register our custom type descriptors
            //TypeDescriptor.AddAttributes(typeof(List<int>), new TypeConverterAttribute(typeof(GenericListTypeConverter<int>)));
            //so we do it manually here

            if (type == typeof(List<int>))
                return new GenericListTypeConverter<int>();
            if (type == typeof(List<decimal>))
                return new GenericListTypeConverter<decimal>();
            if (type == typeof(List<string>))
                return new GenericListTypeConverter<string>();

            return TypeDescriptor.GetConverter(type);
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType)
        {
            return To(value, destinationType, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <param name="culture">Culture</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType, CultureInfo culture)
        {
            if (value != null)
            {
                var sourceType = value.GetType();

                TypeConverter destinationConverter = GetUHackCustomTypeConverter(destinationType);
                TypeConverter sourceConverter = GetUHackCustomTypeConverter(sourceType);
                if (destinationConverter != null && destinationConverter.CanConvertFrom(value.GetType()))
                    return destinationConverter.ConvertFrom(null, culture, value);
                if (sourceConverter != null && sourceConverter.CanConvertTo(destinationType))
                    return sourceConverter.ConvertTo(null, culture, value, destinationType);
                if (destinationType.IsEnum && value is int)
                    return Enum.ToObject(destinationType, (int)value);
                if (!destinationType.IsInstanceOfType(value))
                    return Convert.ChangeType(value, destinationType, culture);
            }
            return value;
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <returns>The converted value.</returns>
        public static T To<T>(object value)
        {
            //return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            return (T)To(value, typeof(T));
        }

        /// <summary>
        /// Convert enum for front-end
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Converted string</returns>
        public static string ConvertEnum(string str)
        {
            string result = string.Empty;
            char[] letters = str.ToCharArray();
            foreach (char c in letters)
                if (c.ToString() != c.ToString().ToLower())
                    result += " " + c.ToString();
                else
                    result += c.ToString();
            return result;
        }

        /// <summary>
        /// Set Telerik (Kendo UI) culture
        /// </summary>
        public static void SetTelerikCulture()
        {
            //little hack here
            //always set culture to 'en-US' (Kendo UI has a bug related to editing decimal values in other cultures). Like currently it's done for admin area in Global.asax.cs

            var culture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        public static List<List<T>> ChunkBy<T>(List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
