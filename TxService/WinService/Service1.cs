using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public void ImportDeals()
        {

            using (var client = new HttpClient())
            {
                var parameters = new Dictionary<string, string> { { "FromDate", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") }, { "ToDate", DateTime.Now.ToString("yyyy-MM-dd") }, { "ImportBy", "Auto" } };
                var encodedContent = new FormUrlEncodedContent(parameters);

                // Live 
                client.BaseAddress = new Uri("https://backoffice.transferconnex.com/");

                //Local 
                //client.BaseAddress = new Uri("http://localhost/FXAdminTransferConnexApp/");

                var response = client.PostAsync("Deal/ImportDeal", encodedContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    Console.Write("Success");
                }
                else
                {
                    Console.Write("Error");
                }
            }

            //var parameters = new Dictionary<string, string> { { "FromDate", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") }, { "ToDate", DateTime.Now.ToString("yyyy-MM-dd") }, { "ImportBy", "Auto" } };
            //var encodedContent = new FormUrlEncodedContent(parameters);
            //try
            //{
            //    using (var client = new System.Net.Http.HttpClient())
            //    {
            //        client.BaseAddress = new Uri("http://localhost/FXAdminTransferConnexApp/");
            //        client.DefaultRequestHeaders.Accept.Clear();

            //        var response = await client.PostAsync("Deal/ImportDeal", encodedContent).ConfigureAwait(false);

            //        if (response.IsSuccessStatusCode)
            //        {
            //            Console.WriteLine("Send mail");
            //        }
            //        else
            //        {
            //            Console.WriteLine("Internal server Error");
            //        }
            //    }
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }
    }
}
