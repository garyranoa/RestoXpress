using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UHack.Web.Models
{
    public partial interface IFileStore
    {
        Guid SaveUploadedFile(HttpPostedFileBase fileBase);
    }
}