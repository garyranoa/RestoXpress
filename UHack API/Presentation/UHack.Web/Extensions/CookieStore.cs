using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UHack.Web.Extensions
{
    public partial class CookieStore
    {
        //public static void SetCookie(string key, string value, TimeSpan expires)
        //{
        //    HttpCookie encodedCookie = HttpSecureCookie.Encode(new HttpCookie(key, value));

        //    if (HttpContext.Current.Request.Cookies[key] != null)
        //    {
        //        var cookieOld = HttpContext.Current.Request.Cookies[key];
        //        cookieOld.Expires = DateTime.Now.Add(expires);
        //        cookieOld.Value = encodedCookie.Value;
        //        HttpContext.Current.Response.Cookies.Add(cookieOld);
        //    }
        //    else
        //    {
        //        encodedCookie.Expires = DateTime.Now.Add(expires);
        //        HttpContext.Current.Response.Cookies.Add(encodedCookie);
        //    }
        //}
        //public static string GetCookie(string key)
        //{
        //    string value = string.Empty;
        //    HttpCookie cookie = HttpContext.Current.Request.Cookies[key];

        //    if (cookie != null)
        //    {
        //        // For security purpose, we need to encrypt the value.
        //        HttpCookie decodedCookie = HttpSecureCookie.Decode(cookie);
        //        value = decodedCookie.Value;
        //    }
        //    return value;
        //}

        public static string GetCookie(string key)
        {
            return HttpContext.Current.Request?.Cookies[key]?.Value;
        }

        public static void SetCookie(string key, string value)
        {
            var cookie = new HttpCookie(key);
            cookie.HttpOnly = true;
            cookie.Value = value;

            HttpContext.Current.Response.Cookies.Remove(key);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

    }
}