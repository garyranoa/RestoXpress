using System;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Threading.Tasks;

//https://github.com/cloudinary/CloudinaryDotNet/issues/10
namespace UHack.Core.Helpers
{
    public static class CloudinaryHelper
    {


        public async static Task<ImageUploadResult> UploadImage(string filename, System.IO.Stream data, string folderPath, string imageFormat = "png")
        {
            var account = new Account("cashclub-dev", "477224968934576", "ag_2a_AiArnrslYzdbkQ8y0VWnI");
            var _cloudinary = new Cloudinary(account);

            var uploadParams = new ImageUploadParams()
            {
                PublicId = Guid.NewGuid().ToString().Replace("-", ""),
                File = new FileDescription(filename, data),
                Folder = folderPath,
                Format = imageFormat,
                ReturnDeleteToken = true
            };

            var result = await _cloudinary.UploadAsync(uploadParams);
            return result;
        }


    }
}
