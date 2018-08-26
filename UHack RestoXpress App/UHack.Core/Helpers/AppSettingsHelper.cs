using System;
using System.Collections.Generic;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace UHack.Core.Helpers
{
    public static class AppSettingsHelper
    {

        private static ISettings AppSettings => CrossSettings.Current;

        public static string AuthUsername
        {
            get => AppSettings.GetValueOrDefault(nameof(AuthUsername), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(AuthUsername), value);
        }

        public static string AuthPassPhrase
        {
            get => AppSettings.GetValueOrDefault(nameof(AuthPassPhrase), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(AuthPassPhrase), value);
        }

        public static void RemoveAuthUsername() => AppSettings.Remove(nameof(AuthUsername));

        public static bool IsAuthAccessTokenSet => AppSettings.Contains(nameof(AuthAccessToken));

        public static string AuthAccessToken
        {
            get => AppSettings.GetValueOrDefault(nameof(AuthAccessToken), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(AuthAccessToken), value);
        }

        public static void RemoveAuthAccessToken() => AppSettings.Remove(nameof(AuthAccessToken));


        public static int AuthUserId
        {
            get => AppSettings.GetValueOrDefault(nameof(AuthUserId), 0);
            set => AppSettings.AddOrUpdateValue(nameof(AuthUserId), value);
        }

        public static string AuthUserGuid
        {
            get => AppSettings.GetValueOrDefault(nameof(AuthUserGuid), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(AuthUserGuid), value);
        }

        public static DateTime AuthAccessTokenExpiration
        {
            get => AppSettings.GetValueOrDefault(nameof(AuthAccessTokenExpiration), DateTime.Now);
            set => AppSettings.AddOrUpdateValue(nameof(AuthAccessTokenExpiration), value);
        }

        public static bool IsAuthAccessTokenExpirationSet => AppSettings.Contains(nameof(AuthAccessTokenExpiration));

        public static bool IsSNSEndpointARNSet => AppSettings.Contains(nameof(SNSEndpointARN));
        public static string SNSEndpointARN
        {
            get => AppSettings.GetValueOrDefault(nameof(SNSEndpointARN), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(SNSEndpointARN), value);
        }


        public static bool IsLastLocationSet => AppSettings.Contains(nameof(LastLocation));
        public static string LastLocation
        {
            get => AppSettings.GetValueOrDefault(nameof(LastLocation), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(LastLocation), value);
        }


        public static bool IsDoneFirstTimeSync
        {
            get => AppSettings.GetValueOrDefault(nameof(IsDoneFirstTimeSync),false);
            set => AppSettings.AddOrUpdateValue(nameof(IsDoneFirstTimeSync), value);
        }

        public static void ClearAllSettings()
        {
            AppSettings.Clear();
        }

    }

}
