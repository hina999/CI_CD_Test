using FXAdminTransferConnex.Common;
using FXAdminTransferConnexApp.Models.GeckoBoard;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace FXAdminTransferConnexApp.Common.Gecko
{
    public class GeckoConnect
    {
        private static string s_userAgent;

        private static string UserAgent => s_userAgent ??
                                   (s_userAgent =
                                       string.Format("Geckonet .NET RestSharp Client v" +
                                                     System.Reflection.Assembly.GetExecutingAssembly().GetName().Version));

        /// <summary>
        /// Appending data to a dataset
        /// </summary>
        /// <param name="payload">The dataset payload</param>
        /// <param name="name">The name of the dataset</param>
        /// <param name="apiKey">The api key</param>
        /// <returns>True if successful</returns>
        /// <exception cref="GeckoException">Thrown if response status is not 200 OK</exception>
        public bool AppendDataset<T>(RequestModel<T> payload, string name, string apiKey)
        {
            RestClient client = new RestClient(ConfigItems.GeckoboardBaseServer)
            {
                Authenticator = new HttpBasicAuthenticator(apiKey, string.Empty)
            };
            client.AddDefaultHeader("User-Agent", UserAgent);

            RestRequest request = new RestRequest($"datasets/{name}/data", Method.POST) { RequestFormat = DataFormat.Json };
            string json = JsonConvert.SerializeObject(payload);
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new GeckoException(response.StatusDescription, response.Content);
            }

            return true;
        }

        /// <summary>
        /// Clears the dataset
        /// </summary>
        /// <param name="name">The name of the dataset</param>
        /// <param name="apiKey">The api key</param>
        /// <returns>True if successful, throws GeckoException otherwise.</returns>
        public bool ClearDataset(string name, string apiKey)
        {
            RestClient client = new RestClient(ConfigItems.GeckoboardBaseServer)
            {
                Authenticator = new HttpBasicAuthenticator(apiKey, string.Empty)
            };
            client.AddDefaultHeader("User-Agent", UserAgent);

            RestRequest request = new RestRequest($"datasets/{name}/data", Method.PUT)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new { Data = new List<int>() });

            IRestResponse response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                //throw new GeckoException(response.StatusDescription, response.Content);
            }

            return true;
        }

        /// <summary>
        /// Create a dataset
        /// </summary>
        /// <param name="payload">The dataset payload</param>
        /// <param name="name">The name of the dataset</param>
        /// <param name="apiKey">The api key</param>
        /// <returns>The dataset result, if applicable.</returns>
        /// <exception cref="GeckoException">Thrown if response status is not 201 CREATED</exception>
        public GeckoDatasetResult CreateDataset(GeckoDataset payload, string name, string apiKey)
        {
            RestClient client = new RestClient(ConfigItems.GeckoboardBaseServer)
            {
                Authenticator = new HttpBasicAuthenticator(apiKey, string.Empty)
            };
            client.AddDefaultHeader("User-Agent", UserAgent);

            RestRequest request = new RestRequest($"datasets/{name}", Method.PUT)
            {
                RequestFormat = DataFormat.Json
            };

            string json = JsonConvert.SerializeObject(payload);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse<GeckoDatasetResult> response = client.Execute<GeckoDatasetResult>(request);
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                throw new GeckoException(response.StatusDescription, response.Content);
            }

            return response.Data;
        }

        /// <summary>
        /// Deletes a dataset
        /// </summary>
        /// <param name="name">The name of the dataset to delete</param>
        /// <param name="apiKey">The api key</param>
        /// <returns>True if successful, </returns>
        public bool DeleteDataset(string name, string apiKey)
        {
            RestClient client = new RestClient(ConfigItems.GeckoboardBaseServer)
            {
                Authenticator = new HttpBasicAuthenticator(apiKey, string.Empty),
            };
            client.AddDefaultHeader("User-Agent", UserAgent);

            RestRequest request = new RestRequest($"datasets/{name}", Method.DELETE)
            {
                RequestFormat = DataFormat.Json
            };

            IRestResponse response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                //throw new GeckoException(response.StatusDescription, response.Content);
            }

            return true;
        }

        /// <summary>
        /// Push it!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="payload"></param>
        /// <param name="widgetKey"></param>
        /// <returns></returns>
        public PushResult Push<T>(PushPayload<T> payload, string widgetKey)
        {
            RestClient client = new RestClient("https://push.geckoboard.com");
            client.AddDefaultHeader("User-Agent", UserAgent);

            RestRequest request = new RestRequest($"v1/send/{widgetKey}", Method.POST) { RequestFormat = DataFormat.Json };
            string json = JsonConvert.SerializeObject(payload);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse<PushResult> response = client.Execute<PushResult>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                //throw new GeckoException(response.StatusDescription, response.Content);
            }

            return response.Data;
        }

        /// <summary>
        /// Allows the replacement of data in a dataset
        /// </summary>
        /// <param name="payload">The dataset to send</param>
        /// <param name="name">The name of the dataset</param>
        /// <param name="apiKey">The api key</param>
        /// <returns>True if successful, throws GeckoException otherwise.</returns>
        public bool UpdateDataset(GeckoDataset payload, string name, string apiKey)
        {
            RestClient client = new RestClient(ConfigItems.GeckoboardBaseServer)
            {
                Authenticator = new HttpBasicAuthenticator(apiKey, string.Empty),
            };
            client.AddDefaultHeader("User-Agent", UserAgent);

            RestRequest request = new RestRequest($"datasets/{name}/data", Method.PUT)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddJsonBody(payload);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                //throw new GeckoException(response.StatusDescription, response.Content);
            }

            return true;
        }
     

        public bool ReplaceDataset<T>(RequestModel<T> payload, string name, string apiKey)
        {
            RestClient client = new RestClient(ConfigItems.GeckoboardBaseServer)
            {
                Authenticator = new HttpBasicAuthenticator(apiKey, string.Empty)
            };
            client.AddDefaultHeader("User-Agent", UserAgent);

            RestRequest request = new RestRequest($"datasets/{name}/data", Method.PUT) { RequestFormat = DataFormat.Json };
            string json = JsonConvert.SerializeObject(payload);
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new GeckoException(response.StatusDescription, response.Content);
            }

            return true;
        }

    }
}