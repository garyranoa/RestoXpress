using System;
using System.Collections.Generic;
using UHack.Core;

namespace UHack.Core.Helpers
{
    public static class AuthHelper
    {

        public static bool SignIn(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;


            AppSettingsHelper.AuthAccessToken = "1";
            AppSettingsHelper.AuthUserId = 1;
            AppSettingsHelper.AuthUsername = username;
            AppSettingsHelper.AuthUserGuid = new Guid().ToString();
            AppSettingsHelper.AuthPassPhrase = password;
            AppSettingsHelper.AuthAccessTokenExpiration = DateTime.Now.AddDays(15);

            return true;
        


            var api = new UHackWebApi();
            var result = api.SignIn(username);
            if (result != null)
            {
                AppSettingsHelper.AuthAccessToken = result.Id.ToString();
                AppSettingsHelper.AuthUserId = Convert.ToInt32(result.Id);
                AppSettingsHelper.AuthUsername = result.Username;
                AppSettingsHelper.AuthUserGuid = result.UserGuid;
                AppSettingsHelper.AuthPassPhrase = password;
                AppSettingsHelper.AuthAccessTokenExpiration = DateTime.Now.AddDays(15);
        
                return true;
            }
            return false;
                
        }


        public static string ValidateAccessToken()
        {
            var currentDate = DateTime.Now;

            if (AppSettingsHelper.IsAuthAccessTokenExpirationSet)
            {
                var authAccessTokenExpiration = AppSettingsHelper.AuthAccessTokenExpiration;

                if (currentDate > authAccessTokenExpiration)
                {
                    var result = ApiTokenHelper.GetAccessToken(AppSettingsHelper.AuthUsername, AppSettingsHelper.AuthPassPhrase);
                    if (result != null && !string.IsNullOrEmpty(result.access_token))
                    {
                        AppSettingsHelper.AuthAccessToken = result.access_token;
                    }
                }
            }
            return AppSettingsHelper.AuthAccessToken;

        }

        public static void SignOut()
        {
            AppSettingsHelper.ClearAllSettings();
        }

    }
}
