using UHack.Core.Caching;
using UHack.Core.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UHack.Core.ConstantClasses
{
    public partial class LibraryConstants
    {
        public static EnvironmentHosting GetCurrentEnvironmentHosting(ICacheManager _cacheManager, string xml_file)
        {
            string keyFormat = "UHack.HostingEnvironment.id-{0}";
            string key = string.Format(keyFormat, "active");
            return _cacheManager.Get(key, () =>
            {
                XDocument xml = XDocument.Load(xml_file);
                var result = (from s in xml.Descendants("hosting")
                              where s.Element("active").Value == "1"
                              select s.Element("name").Value).FirstOrDefault();

                EnvironmentHosting eh;
                switch (result)
                {
                    case "PRODUCTION":
                        eh = EnvironmentHosting.PRODUCTION;
                        break;
                    case "DEVELOPMENT":
                        eh = EnvironmentHosting.DEVELOPMENT;
                        break;
                    case "LOCAL":
                        eh = EnvironmentHosting.LOCAL;
                        break;
                    default:
                        eh = EnvironmentHosting.PRODUCTION;
                        break;
                }

                return eh;

            });
        }

        public static Image ResizePhoto(FileInfo sourceImage, int desiredWidth, int desiredHeight)
        {
            //throw error if bouning box is to small
            if (desiredWidth < 4 || desiredHeight < 4)
                throw new InvalidOperationException("Bounding Box of Resize Photo must be larger than 4X4 pixels.");
            var original = Bitmap.FromFile(sourceImage.FullName);

            //store image widths in variable for easier use
            var oW = (decimal)original.Width;
            var oH = (decimal)original.Height;
            var dW = (decimal)desiredWidth;
            var dH = (decimal)desiredHeight;

            // check if image size is below desired size
            if (oW <= dW && oH <= dH)
            {
                original.Dispose();
                return null;
            }

            //check if image already fits
            if (oW < dW && oH < dH)
                return original; //image fits in bounding box, keep size (center with css) If we made it biger it would stretch the image resulting in loss of quality.

            //check for double squares
            if (oW == oH && dW == dH)
            {
                //image and bounding box are square, no need to calculate aspects, just downsize it with the bounding box
                Bitmap square = new Bitmap(original, (int)dW, (int)dH);
                original.Dispose();
                return square;
            }

            //check original image is square
            if (oW == oH)
            {
                //image is square, bounding box isn't.  Get smallest side of bounding box and resize to a square of that center the image vertically and horizonatally with Css there will be space on one side.
                int smallSide = (int)Math.Min(dW, dH);
                Bitmap square = new Bitmap(original, smallSide, smallSide);
                original.Dispose();
                return square;
            }

            //not dealing with squares, figure out resizing within aspect ratios            
            if (oW > dW && oH > dH) //image is wider and taller than bounding box
            {
                var r = Math.Min(dW, dH) / Math.Min(oW, oH); //two demensions so figure out which bounding box demension is the smallest and which original image demension is the smallest, already know original image is larger than bounding box
                var nH = Math.Round(oW * r); //will downscale the original image by an aspect ratio to fit in the bounding box at the maximum size within aspect ratio.
                var nW = Math.Round(oW * r);
                var resized = new Bitmap(original, (int)nW, (int)nH);
                original.Dispose();
                return resized;
            }
            else
            {
                if (oW > dW) //image is wider than bounding box
                {
                    var r = dW / oW; //one demension (width) so calculate the aspect ratio between the bounding box width and original image width
                    var nW = oW * r; //downscale image by r to fit in the bounding box...
                    var nH = oW * r;
                    var resized = new Bitmap(original, (int)nW, (int)nH);
                    original.Dispose();
                    return resized;
                }
                else
                {
                    //original image is taller than bounding box
                    var r = dH / oH;
                    var nH = oH * r;
                    var nW = oW * r;
                    var resized = new Bitmap(original, (int)nW, (int)nH);
                    original.Dispose();
                    return resized;
                }
            }
        }
    }
}
