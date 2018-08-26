using System;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UHack.Core.Data.Domain;
using System.Net;
using UIKit;
using Foundation;
using Xamarin.Forms;
using System.Threading;

//https://www.learnhowtoprogram.com/net/net-core-and-apis/apis-with-restsharp-and-twilio
namespace UHack.Core
{
    public class UHackWebApi
    {
        string BaseUrl = UHackApiEndpoints.API_BASE_URL;
        string TokenUrl = UHackApiEndpoints.BASE_URL;
        bool _isRequestToken = false;
        public string access_token = "";

        public UHackWebApi(bool isRequestToken = false)
        {
            _isRequestToken = isRequestToken;
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            string uri = BaseUrl;

            if (_isRequestToken)
                uri = TokenUrl;

            var client = new RestClient();
            client.BaseUrl = new System.Uri(uri);

            var response = new RestResponse();

            Task.Run(async () => { response = await GetResponseContentAsync(client, request) as RestResponse; }).Wait();

            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    const string message = "Error retrieving response.  Check inner details for more info.";
                    var cashClubException = new ApplicationException(message, response.ErrorException);
                    throw cashClubException;
                }
            }

            var data = JsonConvert.DeserializeObject<T>(response.Content);
            return data;
        }


        public T GetRequest<T>(string endpoint) where T : new()
        {
            var request = new RestRequest(endpoint);
            if (!string.IsNullOrEmpty(access_token))
                request.AddHeader("Authorization", string.Format("Bearer {0}", access_token));

            var result = this.Execute<T>(request);
            return result;
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => { tcs.SetResult(response); });
            return tcs.Task;
        }

        public virtual bool POSTRequest(dynamic objData, string endpoint)
        {
            bool result = false;
            string uri = BaseUrl;

            var client = new RestClient();
            client.BaseUrl = new System.Uri(uri);

            var request = new RestRequest(endpoint, Method.POST);
            if (!string.IsNullOrEmpty(access_token))
                request.AddHeader("Authorization", string.Format("Bearer {0}", access_token));

            string postData = JsonConvert.SerializeObject(objData, Formatting.None);
            request.AddParameter("application/json; charset=utf-8", postData, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;


            var response = new RestResponse();

            Task.Run(async () => { response = await GetResponseContentAsync(client, request) as RestResponse; }).Wait();

            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                    result = true;
                else
                {
                    const string message = "Error retrieving response.  Check inner details for more info.";
                    var cashClubException = new ApplicationException(message, response.ErrorException);
                    throw cashClubException;

                }
            }

            return result;

        }

        public virtual dynamic POSTRequestWithData(dynamic objData, string endpoint)
        {
            dynamic result = null;
            string uri = BaseUrl;

            var client = new RestClient();
            client.BaseUrl = new System.Uri(uri);

            var request = new RestRequest(endpoint, Method.POST);
            if (!string.IsNullOrEmpty(access_token))
                request.AddHeader("Authorization", string.Format("Bearer {0}", access_token));

            string postData = JsonConvert.SerializeObject(objData, Formatting.None);
            request.AddParameter("application/json; charset=utf-8", postData, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;


            var response = new RestResponse();

            Task.Run(async () => { response = await GetResponseContentAsync(client, request) as RestResponse; }).Wait();

            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = JsonConvert.DeserializeObject(response.Content);
                    result = data;
                }

                else
                {
                    const string message = "Error retrieving response.  Check inner details for more info.";
                    var cashClubException = new ApplicationException(message, response.ErrorException);
                    throw cashClubException;

                }
            }

            return result;

        }

        // TODO : work with specific data to be return and dont use dynamic data
        public virtual T POSTRequestWithDataGeneric<T>(T objData, string endpoint)
        {
            dynamic result = null;
            string uri = BaseUrl;

            var client = new RestClient();
            client.BaseUrl = new System.Uri(uri);

            var request = new RestRequest(endpoint, Method.POST);
            if (!string.IsNullOrEmpty(access_token))
                request.AddHeader("Authorization", string.Format("Bearer {0}", access_token));

            string postData = JsonConvert.SerializeObject(objData, Formatting.None);
            request.AddParameter("application/json; charset=utf-8", postData, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;


            var response = new RestResponse();

            Task.Run(async () => { response = await GetResponseContentAsync(client, request) as RestResponse; }).Wait();

            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                if (response.StatusCode == HttpStatusCode.OK && !response.ContentType.Contains("html"))
                {
                    var data = JsonConvert.DeserializeObject<T>(response.Content);
                    result = data;
                }

                else
                {
                    Console.WriteLine($"response.Content {response.Content}");
                    const string message = "Error retrieving response.  Check inner details for more info.";
                    var cashClubException = new ApplicationException(message, response.ErrorException);
                    throw cashClubException;

                }
            }

            return result;

        }

        public virtual T POSTRequestWithDataGeneric<T>(dynamic objData, string endpoint)
        {
            dynamic result = null;
            string uri = BaseUrl;

            var client = new RestClient();
            client.BaseUrl = new System.Uri(uri);

            var request = new RestRequest(endpoint, Method.POST);
            if (!string.IsNullOrEmpty(access_token))
                request.AddHeader("Authorization", string.Format("Bearer {0}", access_token));

            string postData = JsonConvert.SerializeObject(objData, Formatting.None);
            request.AddParameter("application/json; charset=utf-8", postData, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;


            var response = new RestResponse();

            Task.Run(async () => { response = await GetResponseContentAsync(client, request) as RestResponse; }).Wait();

            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = JsonConvert.DeserializeObject<T>(response.Content);
                    result = data;
                }

                else
                {
                    const string message = "Error retrieving response.  Check inner details for more info.";
                    var cashClubException = new ApplicationException(message, response.ErrorException);
                    throw cashClubException;

                }
            }

            return result;

        }

        public T ExecuteRequestToken<T>(RestRequest request) where T : new()
        {
            string uri = BaseUrl;

            if (_isRequestToken)
                uri = TokenUrl;

            var client = new RestClient();
            client.BaseUrl = new System.Uri(uri);

            var response = new RestResponse();

            Task.Run(async () => { response = await GetResponseContentAsync(client, request) as RestResponse; }).Wait();

            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    const string message = "Error retrieving response.  Check inner details for more info.";
                    //var cashClubException = new ApplicationException(message, response.ErrorException);
                    //throw cashClubException;
                }
            }

            var data = JsonConvert.DeserializeObject<T>(response.Content);
            return data;
        }

        public virtual User SignIn(string username)
        {
            var jsonObj = new { Username = username };
            var result = this.POSTRequestWithDataGeneric<User>(jsonObj, UHackApiEndpoints.UserInfo);
            return result;

        }

        public virtual User Register(string username, string firstname, string lastname, string password, string phone, int roleId)
        {
            var jsonObj = new { Username = username, Firstname = firstname, Lastname = lastname, Phone = phone, RoleId = roleId, Password = password };
            var result = this.POSTRequestWithDataGeneric<User>(jsonObj, UHackApiEndpoints.Register);
            return result;

        }

        public virtual Product SaveProduct(string name, string description, string latitude, string longitude, string location, string imageUrl, string costPrice, int userId)
        {
            var jsonObj = new { Name = name, Description = description, UserId = userId, CostPrice = costPrice, Latitude = latitude, Longitude = longitude, Location = location, ImageUrl = imageUrl};
            var result = this.POSTRequestWithDataGeneric<Product>(jsonObj, UHackApiEndpoints.Register);
            return result;

        }






        public static string Serialize<T>(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, obj);
            string retVal = Encoding.UTF8.GetString(ms.ToArray());
            return retVal;
        }

        public static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            return obj;
        }

      
    }
}
