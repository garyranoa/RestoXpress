using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Hosting;

namespace UHack.Web.Models
{
    internal class DiskFileStore : IFileStore
    {
        private string _uploadsFolder = HostingEnvironment.MapPath("~/content/images/deals");

        public Guid SaveUploadedFile(HttpPostedFileBase fileBase)
        {
            var identifier = Guid.NewGuid();
            fileBase.SaveAs(GetDiskLocation(identifier));
            return identifier;
        }

        private string GetDiskLocation(Guid identifier)
        {
            return Path.Combine(_uploadsFolder, identifier.ToString());
        }
    }
}