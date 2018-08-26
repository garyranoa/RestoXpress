using System;
using RestSharp;

namespace UHack.Core
{
    public class ApiTokenHelper
    {

        public static TokenResult GetAccessToken(string username, string password)
        {
            var api = new UHackWebApi(true);
            var request = new RestRequest("", Method.POST);
            request.AddParameter("grant_type", @"password");
            request.AddParameter("username", username);
            request.AddParameter("password", password);

            return api.ExecuteRequestToken<TokenResult>(request);
        }

        public class TokenResult
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public DateTime expires { get; set; }
            public DateTime issued { get; set; }
            public string userName { get; set; }
            public string userGuid { get; set; }
            public string userId { get; set; }
            public string clubId { get; set; }
        }
    }
}
