using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Common
{
    /// <summary>
    /// Class Web API Helper.
    /// </summary>
    public static class WebApiHelper
    {
        #region WebAPi Common Method

        /// <summary>
        /// HTTPs the client request response.
        /// </summary>
        /// <typeparam name="T">T object</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="uri">The URI.</param>
        /// <returns>Return T object</returns>
        public static async Task<T> HttpClientRequestResponse<T>(T value, string uri, string token)
        {
            try
            {
                WriteLogger("Currency Cloud");
                HttpClient client = new HttpClient { BaseAddress = new Uri(ConfigItems.CurrencyCloudAPIServer) };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-Auth-Token", token);
                HttpResponseMessage response = await client.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                    return default(T);

                T result = await response.Content.ReadAsAsync<T>();
                return (T)Convert.ChangeType(result, typeof(T));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// HTTPs the client request response synchronize.
        /// </summary>
        /// <typeparam name="T">T object</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="uri">The URI.</param>
        /// <returns>return T object</returns>
        public static T HttpClientRequestResponseSync<T>(T value, string uri, string token)
        {
            WriteLogger("Currency Cloud");
            HttpClient client = new HttpClient { BaseAddress = new Uri(ConfigItems.CurrencyCloudAPIServer) };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-Auth-Token", token);

            HttpResponseMessage httpresult = client.GetAsync(uri).Result;

            if (!httpresult.IsSuccessStatusCode)
                return default(T);

            Task<T> result = httpresult.Content.ReadAsAsync<T>();
            return (T)Convert.ChangeType(result.Result, typeof(T));
        }

        public static async Task<T> HttpClientPostCurrencyCloudToken<T>(T value, string uri, Dictionary<string, string> parameters = null)
        {
            uri = Uri.EscapeUriString(uri);
            WriteLogger("Currency Cloud");
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigItems.CurrencyCloudAPIServer);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("utf-8"));
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                HttpResponseMessage response = await httpClient.PostAsync(ConfigItems.CurrencyCloudAPIServer + uri, new FormUrlEncodedContent(parameters));
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    T result = JsonConvert.DeserializeObject<T>(jsonResponse);
                    return (T)Convert.ChangeType(result, typeof(T));
                }
            }
            return default(T);
        }

        public static async Task<T> HttpClientPostRingCentralToken<T>(T value, string uri, Dictionary<string, string> parameters = null)
        {
            uri = Uri.EscapeUriString(uri);
            WriteLogger("Currency Cloud");
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigItems.RingCentralAPIServer);
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic c29KVF9nd0FRSHFBdW0zVHdZNEZfUToxam9NVUVZVVE0Q0FUMzBLRlFlRWdRWjcxaGh4X0lUYjJXWmR1bjYtRU9Rdw==");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                HttpResponseMessage response = await httpClient.PostAsync(uri, new FormUrlEncodedContent(parameters));
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    T result = JsonConvert.DeserializeObject<T>(jsonResponse);
                    return (T)Convert.ChangeType(result, typeof(T));
                }
            }
            return default(T);
        }

        public static async Task<T> HttpClientRequestResponseRingcentral<T>(T value, string uri, string token)
        {
            try
            {
                WriteLogger("Currency Cloud");
                HttpClient client = new HttpClient { BaseAddress = new Uri(ConfigItems.RingCentralAPIServer) };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", token);
                HttpResponseMessage response = await client.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                    return default(T);

                string jsonResponse = await response.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(jsonResponse);
                return (T)Convert.ChangeType(result, typeof(T));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// HTTPs the client request response.
        /// </summary>
        /// <typeparam name="T">T object</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="uri">The URI.</param>
        /// <returns>Return T object</returns>
        public static async Task<object> HttpClientRequestResponseString(string uri, string token)
        {
            try
            {
                WriteLogger("Currency Cloud");
                HttpClient client = new HttpClient { BaseAddress = new Uri(ConfigItems.CurrencyCloudAPIServer) };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-Auth-Token", token);
                HttpResponseMessage response = await client.GetAsync(ConfigItems.CurrencyCloudAPIServer + uri);
                if (!response.IsSuccessStatusCode)
                    return default(object);

                object result = await response.Content.ReadAsAsync<object>();

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion
        #region GCPartner WebAPI
        //public static async Task<T> GCPartnerHttpClientRequestResponse<T>(T value, string uri, string token)
        //{
        //    try
        //    {
        //        var client = new HttpClient { BaseAddress = new Uri(ConfigItems.GCPartnerAPIServer) };
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        client.DefaultRequestHeaders.Add("Authentication", token);
        //        var response = await client.PostAsync(uri);
        //        if (!response.IsSuccessStatusCode)
        //            return default(T);

        //        var result = await response.Content.ReadAsAsync<T>();
        //        return (T)Convert.ChangeType(result, typeof(T));
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}

        public static async Task<T> HttpClientPostGCPartnerToken<T>(T value, string uri, Dictionary<string, string> parameters = null)
        {
            uri = Uri.EscapeUriString(uri);
            try { 
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigItems.GCPartnerAPIServer);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("utf-8"));
                httpClient.DefaultRequestHeaders.Add("Authentication", ConfigItems.GCPartnerAPIToken);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                var response = await httpClient.PostAsync(ConfigItems.GCPartnerAPIServer + uri, new FormUrlEncodedContent(parameters));
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<T>(jsonResponse);
                    return (T)Convert.ChangeType(result, typeof(T));
                }
            }
            return default(T);
            }catch(Exception ex)
            {
                return default(T);
            }
        }
        #endregion

        public static void WriteLogger(string message)
        {
            try
            {
                string path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/LoggerInfo/"), Path.GetFileName("Request.txt"));

                if (!Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/LoggerInfo/")))
                {
                    Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~/LoggerInfo/"));
                }

                if (!File.Exists(path))
                {
                    FileStream myFile = System.IO.File.Create(path);
                    myFile.Close();
                }

                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("=================" + message + "->" + DateTime.Now + "=============================");
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void WriteCallLogger(string message)
        {
            try
            {
                string path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/LoggerInfo/"), Path.GetFileName("ImportCallLog.txt"));

                if (!Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/LoggerInfo/")))
                {
                    Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~/LoggerInfo/"));
                }

                if (!File.Exists(path))
                {
                    FileStream myFile = System.IO.File.Create(path);
                    myFile.Close();
                }

                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("=================" + message + "->" + DateTime.Now + "=============================");
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
