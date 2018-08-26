using System;
namespace UHack.Core
{
    public static class UHackApiEndpoints
    {
        public static string BASE_URL { get { return "http://192.168.190.1/uhack/"; } }
        public static string API_BASE_URL { get { return BASE_URL+ "api/"; } }

        public static string Register { get { return "service/register"; } }
        public static string UserInfo { get { return "service/user-info"; } }
        public static string SaveProducts { get { return "service/save-products"; } }
        public static string SaveSales { get { return "service/user-info"; } }
    }
}
