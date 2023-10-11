using FXAdminTransferConnex.Business.Ringcentral;
using FXAdminTransferConnex.Entities;
using FXAdminTransferConnexApp.Common.Gecko;
using FXAdminTransferConnexApp.Models.GeckoBoard;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace FXAdminTransferConnexApp.Controllers
{
    public class GeckoBoardController : ApiController
    {
        private readonly IRingcentralService _ringcentralService;
        public GeckoBoardController()
        {
            _ringcentralService = EngineContext.Resolve<IRingcentralService>();
        }
        [HttpPost]
        public GeckoDatasetResult CreateSampleDataSet()
        {
            GeckoDataset dataSet = new GeckoDataset();
            //IDatasetField callToName = new IDatasetField();

            //callToName.Name = "calltoname";
            //callToName.Type = "string";
            //callToName.CurrencyCode = null;
            //IDatasetField TotalInCount = new IDatasetField();
            //TotalInCount.Name = "totalincount";
            //TotalInCount.Type = "number";
            //TotalInCount.CurrencyCode = null;
            //IDatasetField TotalOutCount = new IDatasetField();
            //TotalOutCount.Name = "totaloutcount";
            //TotalOutCount.Type = "number";
            //TotalOutCount.CurrencyCode = null;
            //IDatasetField MissedCallCount = new IDatasetField();
            //MissedCallCount.Name = "missedcallcount";
            //MissedCallCount.Type = "number";
            //MissedCallCount.CurrencyCode = null;
            //IDatasetField AcceptancePercentage = new IDatasetField();
            //AcceptancePercentage.Name = "acceptancepercentage";
            //AcceptancePercentage.Type = "number";
            //AcceptancePercentage.CurrencyCode = null;
            //IDatasetField AverageCallDuration = new IDatasetField();
            //AverageCallDuration.Name = "averagecallduration";
            //AverageCallDuration.Type = "number";
            //AverageCallDuration.CurrencyCode = null;
            //IDatasetField CallDuration = new IDatasetField();
            //CallDuration.Name = "callduration";
            //CallDuration.Type = "number";
            //CallDuration.CurrencyCode = null;
            //Dictionary<string, IDatasetField> field = new Dictionary<string, IDatasetField>()
            //{
            //    { "calltoname", callToName },
            //    { "totalincount", TotalInCount },
            //    { "totaloutcount", TotalOutCount },
            //    { "missedcallcount", MissedCallCount },
            //    { "acceptancepercentage", AcceptancePercentage }, 
            //    { "averagecallduration", AverageCallDuration }, 
            //    { "callduration", CallDuration }

            //};
            var f = Getfield(new RingCentralModel());
            dataSet.Fields = f;//f


            // Create Sample DataSet by API
            string dataSetName = "ringcentral_dynamic_dataset";
            string apiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();

            GeckoConnect geckoConnect = new GeckoConnect();
            GeckoDatasetResult response = geckoConnect.CreateDataset(dataSet, dataSetName, apiKey);

            return response;    
        }
        [HttpPost]
        //[Route("api/Geckoboard/AddDatatoDataSet")]
        public bool AddDatatoDataSet()
        {
            string dataSetName = "ringcentral_dynamic_dataset";
            string apiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
            var result = _ringcentralService.GetRingcentralCallList(null, null, null, null, null, 1, 10);
          
            //var result = _ringcentralService.GetEmployeeWiseCallCount();
            RingCentralModel data1 = new RingCentralModel();
            data1.RingCentralCallId = "Cl01";
            data1.RingCentralSessionId = 01;
            data1.CallStartTime = DateTime.Now;
            data1.CallDuration = 30;
            data1.TotalCallDuration = 45;
            data1.AverageCallDuration = 37.5;
            data1.CallType = "Voice";
            data1.CallDirection = "Inbound";
            data1.CallAction = "Phone Call";
            data1.CallResult = "Accepted";
            data1.CallToName = "John";
            data1.CallToNumber = "+442038616587";
            data1.CallFromName = "David";
            data1.CallFromNumber = "+442038001996";
            data1.CallCount = 2;
            data1.TotalInCount = 2;
            data1.TotalOutCount = 2;
            data1.MissedCallCount = 2;
            data1.AcceptancePercentage = 2;
            data1.StrCallStartTime = "2";
            data1.LastMonthCount = 2;
            data1.ThisMonthCount = 2;
            data1.ClientCompanyName = "Company 1";
            data1.ProspectCompanyName = "Company 2";
            data1.TotalCount = 2;
            data1.ImageName = "Img 1";
            data1.ImageURL = null;
            data1.ClientId = 111;
            data1.ProspectId = 123;


            RingCentralModel data2 = new RingCentralModel();
            data2.RingCentralCallId = "Cl02";
            data2.RingCentralSessionId = 02;
            data2.CallStartTime = DateTime.Now;
            data2.CallDuration = 30;
            data2.TotalCallDuration = 45;
            data2.AverageCallDuration = 37.5;
            data2.CallType = "Voice";
            data2.CallDirection = "Inbound";
            data2.CallAction = "Phone Call";
            data2.CallResult = "Accepted";
            data2.CallToName = "David";
            data2.CallToNumber = "+442038616587";
            data2.CallFromName = "John";
            data2.CallFromNumber = "+442038001996";
            data2.CallCount = 2;
            data2.TotalInCount = 2;
            data2.TotalOutCount = 2;
            data2.MissedCallCount = 2;
            data2.AcceptancePercentage = 2;
            data2.StrCallStartTime = "2";
            data2.LastMonthCount = 2;
            data2.ThisMonthCount = 2;
            data2.ClientCompanyName = "Company 1";
            data2.ProspectCompanyName = "Company 2";
            data2.TotalCount = 2;
            data2.ImageName = "Img 1";
            data2.ImageURL = null;
            data2.ClientId = 123;
            data2.ProspectId = 111;
            List<RingCentralModel> ListData = new List<RingCentralModel>();
            RequestModel<ExpandoObject> requestModel = new RequestModel<ExpandoObject>();

            ListData.Add(data1);
            ListData.Add(data2);

            RingCentralModel dataModel = new RingCentralModel();
            Type RingCentralModel = dataModel.GetType();
            PropertyInfo[] properties = RingCentralModel.GetProperties();
            dynamic ListDyObj = new List<ExpandoObject>();

            foreach (var data in result)
            {
                dynamic DyObj = new ExpandoObject();
                foreach (var prop in properties)
                {
                    var name = prop.Name.ToLower();
                    var value = data.GetType().GetProperty(prop.Name).GetValue(data, null);
                    ((IDictionary<String, object>)DyObj)[name] = value;

                }
                ListDyObj.Add(DyObj);
            }

            requestModel.data = ListDyObj;
            GeckoConnect geckoConnect = new GeckoConnect();
            var response = geckoConnect.AppendDataset(requestModel, dataSetName, apiKey);

            return response;
        }

        [HttpPost]
        public PushResult AddListWidget()
        {
            PushPayload<ExpandoObject> payload = new PushPayload<ExpandoObject>();
            PushPayload<List<DataListItem>> p1 = new PushPayload<List<DataListItem>>();
            List<DataListItem> p1d = new List<DataListItem>();
            p1.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
            payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
            var result = _ringcentralService.GetEmployeeWiseCallCount();
            if(result.Count == 0)
            {
                result = _ringcentralService.GetRingcentralCallList(null, null, null, null, null, 1, 10).ToList();
            }

            RingCentralModel dataModel = new RingCentralModel();
            Type RingCentralModel = dataModel.GetType();
            PropertyInfo[] properties = RingCentralModel.GetProperties();
            dynamic ListDyObj = new List<ExpandoObject>();

            foreach (var data in result)
            {
                dynamic DyObj = new ExpandoObject();
                foreach (var prop in properties)
                {
                    var name = prop.Name.ToLower();
                    var value = data.GetType().GetProperty(prop.Name).GetValue(data);
                        ((IDictionary<String, object>)DyObj)[name] = value;
                 
                }
                ListDyObj.Add(DyObj);
            }

            foreach (var data in result)
            {
                
                foreach (var prop in properties)
                {
                    DataListItem d1 = new DataListItem();
                    DataListItemTitle dlt = new DataListItemTitle();
                    DataListItemLabel dll = new DataListItemLabel();
                    dlt.Text = prop.Name.ToLower();
                    dll.Name = prop.Name.ToLower(); 
                    d1.Title = dlt;
                    d1.Label = dll;
                    var value = data.GetType().GetProperty(prop.Name).GetValue(data);
                    d1.Description = Convert.ToString(value);
                    p1d.Add(d1);
                }
            }
            p1.Data = p1d;
            GeckoConnect geckoConnect = new GeckoConnect();
            PushResult response = geckoConnect.Push(p1, "1148484-a9e05420-9667-013b-084a-0ad2523158a1");
            return response;
        }

        public Dictionary<string, IDatasetField> Getfield(RingCentralModel rc)
        {
            Type RingCentralModel = rc.GetType();
            PropertyInfo[] properties = RingCentralModel.GetProperties();
            List<IDatasetField> DataFieldList = new List<IDatasetField>();
            foreach (var i in properties)
            {
                IDatasetField dataField = new IDatasetField();
                dataField.Name = i.Name.ToLower();
                var type = i.PropertyType.Name.ToLower();
                if (type == "int32" || type == "double" || type == "int64" || type == "decimal" || type == "nullable`1")
                {
                    if (type == "nullable`1")
                    {
                        dataField.Optional = true;
                    }
                    else
                    {
                        dataField.Optional = false;
                    }
                    dataField.Type = "number";
                }
                else
                {
                    dataField.Type = type;
                    dataField.Optional = true;
                }
                dataField.CurrencyCode = null;
                DataFieldList.Add(dataField);
               
              

            }
            Dictionary<string, IDatasetField> field = new Dictionary<string, IDatasetField>();
            foreach (var i in DataFieldList)
            {

                field.Add(i.Name, i);
            }

            return field;

        }
        [HttpPost]
        public PushResult AddMissedCallTodayWidget()
        {
            var callPercentageData = _ringcentralService.GetCallAcceptancePercentage().FirstOrDefault();
            int MissedCallCount = 0;
            if (callPercentageData != null)
            {
                MissedCallCount = callPercentageData.MissedCallCount;
            }
            GeckoItems geckoItems = new GeckoItems();
            DataItem[] arrayDataItems = new DataItem[1];

            geckoItems.DataItems = arrayDataItems;
            DataItem dataItem = new DataItem();
            dataItem.Text = MissedCallCount.ToString();
            dataItem.Type = 0;
            arrayDataItems[0] = dataItem;
            PushPayload <GeckoItems> payload = new PushPayload<GeckoItems>();
            payload.Data = geckoItems;
            payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
            GeckoConnect geckoConnect = new GeckoConnect();
            PushResult response = geckoConnect.Push(payload, "1148484-b88e5360-98a7-013b-c90f-0242ef0d6b6d");
            return response;
        }
        [HttpPost]
        public PushResult AddAcceptncePercentageTodayWidget()
        {
            var callPercentageData = _ringcentralService.GetCallAcceptancePercentage().FirstOrDefault();
            double AcceptancePercentage = 0;
            if (callPercentageData != null)
            {
                AcceptancePercentage = callPercentageData.AcceptancePercentage;
            }
            GeckoItems geckoItems = new GeckoItems();
            DataItem[] arrayDataItems = new DataItem[1];

            geckoItems.DataItems = arrayDataItems;
            DataItem dataItem = new DataItem();
            dataItem.Text = AcceptancePercentage.ToString();
            dataItem.Type = 0;
            arrayDataItems[0] = dataItem;
            PushPayload<GeckoItems> payload = new PushPayload<GeckoItems>();
            payload.Data = geckoItems;
            payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
            GeckoConnect geckoConnect = new GeckoConnect();
            PushResult response = geckoConnect.Push(payload, "1148484-7505ed20-98ab-013b-d02a-02d029c9f5cb");
            return response;
        }
        [HttpPost]
        public PushResult AddCompanyCommissionTodayWidget()
        {
            decimal TotalCommission = 0;
            List<RingCentralModel> RingCentralList = _ringcentralService.GetTodaysCommission();
            if(RingCentralList != null && RingCentralList.Count > 0)
            {
                RingCentralModel todayCommission = _ringcentralService.GetTodaysCommission().FirstOrDefault();
                if(todayCommission != null)
                {
                    TotalCommission = todayCommission.TotalCommission;
                }

            }
           
            GeckoItems geckoItems = new GeckoItems();
            DataItem[] arrayDataItems = new DataItem[1];

            geckoItems.DataItems = arrayDataItems;
            DataItem dataItem = new DataItem();
            dataItem.Text = TotalCommission.ToString();
            dataItem.Type = 0;
            dataItem.Prefix = "£";
            arrayDataItems[0] = dataItem;
            PushPayload<GeckoItems> payload = new PushPayload<GeckoItems>();
            payload.Data = geckoItems;
            payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
            GeckoConnect geckoConnect = new GeckoConnect();
            PushResult response = geckoConnect.Push(payload, "1148484-7dad7670-98ad-013b-26a9-0ecaedc1ab03");
            return response;
        }
        [HttpPost]
        public PushResult AddTopServiceTodayWidget()
        {
            List<RingCentralModel> Top3Caller = _ringcentralService.GetTop3Caller();
            GeckoLeaderboard leaderboard = new GeckoLeaderboard();
            List<GeckoLeaderboardItem> Items = new List<GeckoLeaderboardItem>();
            GeckoItems geckoItems = new GeckoItems();
             int length = Top3Caller.Count;
            DataItem[] arrayDataItems = new DataItem[length];
            for(var i = 0; i < length; i++)
            {
                GeckoLeaderboardItem leaderboardItem = new GeckoLeaderboardItem();
                
                leaderboardItem.Label = Top3Caller[i].CallFromName;
                leaderboardItem.Value = Top3Caller[i].CallCount;
                Items.Add(leaderboardItem);
            }
            leaderboard.Items = Items;
            geckoItems.DataItems = arrayDataItems;
            PushPayload<GeckoLeaderboard> payload = new PushPayload<GeckoLeaderboard>();
            payload.Data = leaderboard;
            payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
            GeckoConnect geckoConnect = new GeckoConnect();
            PushResult response = geckoConnect.Push(payload, "1148484-0818a510-98b0-013b-ea84-0e3ba7e805b1");
            return response;
        }

        [HttpPost]
        public PushResult AddInBoundCallTodayWidget()
        {
            var result = _ringcentralService.GetEmployeeWiseCallCount();
            

            GeckoItems geckoItems = new GeckoItems();
            int length = result.Count;
            DataItem[] arrayDataItems = new DataItem[length];
            for (var i = 0; i < length; i++)
            {
                DataItem dataItem = new DataItem();
                dataItem.Label = result[i].CallToName;
                dataItem.Value = result[i].TotalInCount;
                arrayDataItems[i] = dataItem;
            }
            geckoItems.DataItems = arrayDataItems;
            PushPayload<GeckoItems> payload = new PushPayload<GeckoItems>();
            payload.Data = geckoItems;
            payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
            GeckoConnect geckoConnect = new GeckoConnect();
            PushResult response = geckoConnect.Push(payload, "1148484-4e7e5480-98b8-013b-a6fe-0ae1efb5e4bd");
            return response;
        }

        [HttpPost]
        public PushResult AddOutBoundCallTodayWidget()
        {
            var result = _ringcentralService.GetEmployeeWiseCallCount();


            GeckoItems geckoItems = new GeckoItems();
            int length = result.Count;
            DataItem[] arrayDataItems = new DataItem[length];
            for (var i = 0; i < length; i++)
            {
                DataItem dataItem = new DataItem();
                dataItem.Label = result[i].CallToName;
                dataItem.Value = result[i].TotalOutCount;
                arrayDataItems[i] = dataItem;
            }
            geckoItems.DataItems = arrayDataItems;
            PushPayload<GeckoItems> payload = new PushPayload<GeckoItems>();
            payload.Data = geckoItems;
            payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
            GeckoConnect geckoConnect = new GeckoConnect();
            PushResult response = geckoConnect.Push(payload, "1148484-c0ce6b00-98b9-013b-d03a-02d029c9f5cb");
            return response;
        }
    }
}
