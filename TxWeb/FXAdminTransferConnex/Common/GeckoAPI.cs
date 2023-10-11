using FXAdminTransferConnex.Business.Ringcentral;
using FXAdminTransferConnex.Data.Helper;
using FXAdminTransferConnex.Entities;
using FXAdminTransferConnexApp.Common.Gecko;
using FXAdminTransferConnexApp.Models.GeckoBoard;
using Microsoft.ApplicationBlocks.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace FXAdminTransferConnexApp.Common
{
    public class GeckoAPI
    {
        //private readonly IRingcentralService _ringcentralService;
        //static string strcon = System.Configuration.ConfigurationManager.ConnectionStrings["FXBackOfficeSystemContext"].ConnectionString;
        private static string encryptedConnectionString = ConfigurationManager.ConnectionStrings["FXBackOfficeSystemContext"].ConnectionString;
        private static string decryptedConnectionString = AESEncryptionDecryptionHelper.Decrypt(encryptedConnectionString);
        //static long traderId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.Trader.GetHashCode() ? ProjectSession.LoginUserDetails.TraderId : 0;
        //static long salespersonId = ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.SalesPerson.GetHashCode() ? ProjectSession.LoginUserDetails.UserId : 0;

        public void UpdateAllWidget()
        {
            //CreateRingCentralCallLogDataSet();
            //CreateSalesFigureDataSet();
            //CreateTopBillerDataSet();

            //Ring Central
            AddMissedCallTodayWidget();
            AddAcceptncePercentageTodayWidget();
            AddCompanyCommissionTodayWidget();
            AddTopServiceTodayWidget();
            AddIncomingCallOfLast5Days();
            AddOutgoingCallOfLast5Days();
            //AddInBoundCallTodayWidget();
            //AddOutBoundCallTodayWidget();

            AppendRingCentralData();

            //Sales Figure
            AddRNCTotalWidget();
            AddDingTotalWidget();
            AddLeadsTotalWidget();
            AddREGTotalWidget();
            AddMonthlyTotalWidget();
            AddTopREGWidget();
            AddTopRNCWidget();
            AddTopLeadsWidget();
            AddTopDingWidget();
            //AddTopCommsWidget();

            AppendSalesFigureData();
            AppendTopBillerData();

        }
        public PushResult AddMissedCallTodayWidget()
        {
            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_RingCentralCallLog_GetCallAcceptancePercentage");
                PushResult response = new PushResult();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);

                    List<RingCentralModel> result = JsonConvert.DeserializeObject<List<RingCentralModel>>(JSONString);

                    List<RingCentralModel> callPercentageData = result;
                    int MissedCallCount = 0;
                    if (callPercentageData != null)
                    {
                        MissedCallCount = result[0].MissedCallCount;
                    }
                    GeckoItems geckoItems = new GeckoItems();
                    DataItem[] arrayDataItems = new DataItem[1];

                    geckoItems.DataItems = arrayDataItems;
                    DataItem dataItem = new DataItem();
                    dataItem.Text = MissedCallCount.ToString();
                    dataItem.Type = 0;
                    arrayDataItems[0] = dataItem;
                    PushPayload<GeckoItems> payload = new PushPayload<GeckoItems>();
                    payload.Data = geckoItems;
                    payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
                    string widgetKey = ConfigurationManager.AppSettings["MissedCallTodayTextWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);
                    
                }
                return response;
            }
            catch(Exception ex)
            {
                con.Close();
                PushResult response = new PushResult();
                return response;
                
            }
            
        }
        public PushResult AddAcceptncePercentageTodayWidget()
        {
            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_RingCentralCallLog_GetCallAcceptancePercentage");
                PushResult response = new PushResult();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);

                    List<RingCentralModel> result = JsonConvert.DeserializeObject<List<RingCentralModel>>(JSONString);
                    double AcceptancePercentage = 0;
                    if (result != null)
                    {
                        AcceptancePercentage = result[0].AcceptancePercentage;
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
                    string widgetKey = ConfigurationManager.AppSettings["AcceptncePercentageTodayTextWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);
                   // return response;

                }
                return response;
            }
            catch(Exception ex)
            {
                con.Close();
                PushResult response = new PushResult();
                return response;
                
            }
           
        }
        public PushResult AddCompanyCommissionTodayWidget()
        {
            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "USP_GetTodaysCompanyCommission");
                PushResult response = new PushResult();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    List<RingCentralModel> result = JsonConvert.DeserializeObject<List<RingCentralModel>>(JSONString);
                    decimal TotalCommission = 0;
                    //decimal TotalCommission = _ringcentralService.GetTodaysCommission().FirstOrDefault().TotalCommission;
                    if (result != null)
                    {
                        TotalCommission = result[0].TotalCommission;
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
                    string widgetKey = ConfigurationManager.AppSettings["CompanyCommissionTodayTextWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);

                }
                return response;
            }
            catch(Exception ex)
            {
                con.Close();
                PushResult response = new PushResult();
                return response;
                
            }
           
        }
        public PushResult AddTopServiceTodayWidget()
        {
            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_RingCentralCallLog_GetTop3Caller");
                PushResult response = new PushResult();
                if (ds != null && ds.Tables.Count > 0 )/*&& ds.Tables[0].Rows.Count > 0*/
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);

                    List<RingCentralModel> result = JsonConvert.DeserializeObject<List<RingCentralModel>>(JSONString);
                    

                    List<RingCentralModel> Top3Caller = result;
                    GeckoLeaderboard leaderboard = new GeckoLeaderboard();
                    List<GeckoLeaderboardItem> Items = new List<GeckoLeaderboardItem>();
                    GeckoItems geckoItems = new GeckoItems();
                    int length = Top3Caller.Count;
                    DataItem[] arrayDataItems = new DataItem[length];
                    if(length > 0)
                    {
                        for (int i = 0; i < length; i++)
                        {
                            GeckoLeaderboardItem leaderboardItem = new GeckoLeaderboardItem();

                            leaderboardItem.Label = Top3Caller[i].CallFromName;
                            leaderboardItem.Value = Top3Caller[i].CallCount;
                            Items.Add(leaderboardItem);
                        }
                    }
                    else
                    {
                        leaderboard.Items = new List<GeckoLeaderboardItem>();
                    }
                    
                    leaderboard.Items = Items;
                    geckoItems.DataItems = arrayDataItems;
                    PushPayload<GeckoLeaderboard> payload = new PushPayload<GeckoLeaderboard>();
                    payload.Data = leaderboard;
                    payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
                    string widgetKey = ConfigurationManager.AppSettings["TopServiceTodayLeaderBoardWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);

                }
                return response;
            }
            catch(Exception ex)
            {
                con.Close();
                PushResult response = new PushResult();
                return response;
            }
            
        }
        public PushResult AddInBoundCallTodayWidget()
        {
            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_RingCentralCallLog_GetLast5DayInCallLog");
                PushResult response = new PushResult();
                if (ds != null && ds.Tables.Count > 0)/*&& ds.Tables[0].Rows.Count > 0*/
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    List<RingCentralModel> result = JsonConvert.DeserializeObject<List<RingCentralModel>>(JSONString);
                    GeckoItems geckoItems = new GeckoItems();
                    int length = result.Count;
                    DataItem[] arrayDataItems = new DataItem[length];
                    for (int i = 0; i < length; i++)
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
                    string widgetKey = ConfigurationManager.AppSettings["InBoundCallTodayPiWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);

                }
                return response;
            }
            catch(Exception ex)
            {
                con.Close();
                PushResult response = new PushResult();
                return response;
            }
           
        }
        public PushResult AddOutBoundCallTodayWidget()
        {
            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_RingCentralCallLog_GetEmployeeWiseCallCount");
                PushResult response = new PushResult();
                if (ds != null && ds.Tables.Count > 0) /*&& ds.Tables[0].Rows.Count > 0*/
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    List<RingCentralModel> result = JsonConvert.DeserializeObject<List<RingCentralModel>>(JSONString);
                    GeckoItems geckoItems = new GeckoItems();
                    int length = result.Count;
                    DataItem[] arrayDataItems = new DataItem[length];
                    for (int i = 0; i < length; i++)
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
                    string widgetKey = ConfigurationManager.AppSettings["OutBoundCallTodayPiWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);

                }
                return response;
            }
            catch(Exception ex)
            {
                con.Close();
                PushResult response = new PushResult();
                return response;
            }

        }
       
        //newly added widget
        public PushResult AddREGTotalWidget()
        {

            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_ReconciliationTeam_GetCurrentMonthReport");
                PushResult response = new PushResult();
                if (ds != null && ds.Tables.Count > 0) /*&& ds.Tables[0].Rows.Count > 0*/
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);

                    List<ReconciliationTeamMemberModel> result = JsonConvert.DeserializeObject<List<ReconciliationTeamMemberModel>>(JSONString);
                    int? REGTotal = result.Sum(x => x.REGCount);
                    GeckoMeterChart geckometer = new GeckoMeterChart();
                    DataItem dataItemMax = new DataItem();
                    DataItem dataItemMin = new DataItem();
                    geckometer.Item = Convert.ToDecimal(REGTotal);
                    dataItemMax.Text = "Max";
                    dataItemMax.Value = Convert.ToDecimal(1000);
                    geckometer.Max = dataItemMax;
                    dataItemMin.Text = "Min";
                    dataItemMin.Value = Convert.ToDecimal(0);
                    geckometer.Min = dataItemMin;

                    PushPayload<GeckoMeterChart> payload = new PushPayload<GeckoMeterChart>();
                    payload.Data = geckometer;
                    payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
                    string widgetKey = ConfigurationManager.AppSettings["REGTotalWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);

                }
                return response;
            }
            catch (Exception ex)
            {
                con.Close();
                PushResult response = new PushResult();
                return response;
            }
        }
        public PushResult AddDingTotalWidget()
        {

            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_ReconciliationTeam_GetCurrentMonthReport");
                PushResult response = new PushResult();
                if (ds != null && ds.Tables.Count > 0) /*&& ds.Tables[0].Rows.Count > 0*/
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);

                    List<ReconciliationTeamMemberModel> result = JsonConvert.DeserializeObject<List<ReconciliationTeamMemberModel>>(JSONString);
                    int? DingTotal = result.Sum(x => x.DingCount);
                    GeckoMeterChart geckometer = new GeckoMeterChart();
                    DataItem dataItemMax = new DataItem();
                    DataItem dataItemMin = new DataItem();
                    geckometer.Item = Convert.ToDecimal(DingTotal);
                    dataItemMax.Text = "Max";
                    dataItemMax.Value = Convert.ToDecimal(1000);
                    geckometer.Max = dataItemMax;
                    dataItemMin.Text = "Min";
                    dataItemMin.Value = Convert.ToDecimal(0);
                    geckometer.Min = dataItemMin;
                    PushPayload<GeckoMeterChart> payload = new PushPayload<GeckoMeterChart>();
                    payload.Data = geckometer;
                    payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
                    string widgetKey = ConfigurationManager.AppSettings["DingTotalWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);

                }
                return response;
            }
            catch (Exception ex)
            {
                con.Close();
                PushResult response = new PushResult();
                return response;
            }
        }
        public PushResult AddMonthlyTotalWidget()
        {

            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_ReconciliationTeam_GetCurrentMonthReport");
                PushResult response = new PushResult();
                if (ds != null && ds.Tables.Count > 0) /*&& ds.Tables[0].Rows.Count > 0*/
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    List<ReconciliationTeamMemberModel> result = JsonConvert.DeserializeObject<List<ReconciliationTeamMemberModel>>(JSONString);
                    decimal? MonthlyTotal = result.Sum(x => x.Commission);
                    GeckoMeterChart geckometer = new GeckoMeterChart();
                    DataItem dataItemMax = new DataItem();
                    DataItem dataItemMin = new DataItem();
                    geckometer.Item = Convert.ToDecimal(MonthlyTotal);
                    dataItemMax.Text = "Max";
                    dataItemMax.Value = Convert.ToDecimal(1000000);
                    dataItemMax.Prefix = "£";
                    geckometer.Max = dataItemMax;
                    dataItemMin.Text = "Min";
                    dataItemMin.Prefix = "£";
                    dataItemMin.Value = Convert.ToDecimal(0);
                    geckometer.Min = dataItemMin;
                    PushPayload<GeckoMeterChart> payload = new PushPayload<GeckoMeterChart>();
                    payload.Data = geckometer;
                    payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
                    string widgetKey = ConfigurationManager.AppSettings["MonthlyTotalWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);

                }
                return response;
            }
            catch (Exception ex)
            {
                con.Close();
                PushResult response = new PushResult();
                return response;
            }
        }
        public PushResult AddRNCTotalWidget()
        {
            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_ReconciliationTeam_GetCurrentMonthReport");
                PushResult response = new PushResult();
                if (ds != null && ds.Tables.Count > 0) /*&& ds.Tables[0].Rows.Count > 0*/
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    List<ReconciliationTeamMemberModel> result = JsonConvert.DeserializeObject<List<ReconciliationTeamMemberModel>>(JSONString);
                    int? RNCTotal = result.Sum(x => x.RNCCount);
                    GeckoMeterChart geckometer = new GeckoMeterChart();
                    DataItem dataItemMax = new DataItem();
                    DataItem dataItemMin = new DataItem();
                    geckometer.Item = Convert.ToDecimal(RNCTotal);
                    dataItemMax.Text = "Max";
                    dataItemMax.Value = Convert.ToDecimal(1000);
                    dataItemMax.Prefix = "£";
                    geckometer.Max = dataItemMax;
                    dataItemMin.Text = "Min";
                    dataItemMin.Prefix = "£";
                    dataItemMin.Value = Convert.ToDecimal(0);
                    geckometer.Min = dataItemMin;
                    PushPayload<GeckoMeterChart> payload = new PushPayload<GeckoMeterChart>();
                    payload.Data = geckometer;
                    payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
                    string widgetKey = ConfigurationManager.AppSettings["RNCTotalWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);

                }
                return response;
            }
            catch (Exception ex)
            {
                con.Close();
                PushResult response = new PushResult();
                return response;
            }
        }
        public PushResult AddLeadsTotalWidget()
        {
            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_ReconciliationTeam_GetCurrentMonthReport");
                PushResult response = new PushResult();
                if (ds != null && ds.Tables.Count > 0) /*&& ds.Tables[0].Rows.Count > 0*/
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    List<ReconciliationTeamMemberModel> result = JsonConvert.DeserializeObject<List<ReconciliationTeamMemberModel>>(JSONString);
                    int? LeadsTotal = result.Sum(x => x.LeadCount);
                    GeckoMeterChart geckometer = new GeckoMeterChart();
                    DataItem dataItemMax = new DataItem();
                    DataItem dataItemMin = new DataItem();
                    geckometer.Item = Convert.ToDecimal(LeadsTotal);
                    dataItemMax.Text = "Max";
                    dataItemMax.Value = Convert.ToDecimal(1000);
                    dataItemMax.Prefix = "£";
                    geckometer.Max = dataItemMax;
                    dataItemMin.Text = "Min";
                    dataItemMin.Prefix = "£";
                    dataItemMin.Value = Convert.ToDecimal(0);
                    geckometer.Min = dataItemMin;
                    PushPayload<GeckoMeterChart> payload = new PushPayload<GeckoMeterChart>();
                    payload.Data = geckometer;
                    payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
                    string widgetKey = ConfigurationManager.AppSettings["LeadTotalWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);

                }
                return response;
            }
            catch (Exception ex)
            {
                con.Close();
                PushResult response = new PushResult();
                return response;
            }
        }

        public PushResult AddTopCommsWidget()
        {
            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_ReconciliationTop_GetCurruntReport");
                PushResult response = new PushResult();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    List<ReconciliationTeamMemberModel> result = JsonConvert.DeserializeObject<List<ReconciliationTeamMemberModel>>(JSONString);
                    decimal? TopComms = 0;
                    string fullname = string.Empty;
                    string name = string.Empty;
                    if (result != null && result.Count > 0)
                    {
                        TopComms = result.Max(x => x.Commission);
                        name = result.Where(x => x.Commission == TopComms).Select(x => x.FullName).FirstOrDefault();
                        fullname = name + " ( " + TopComms.ToString() + " )";
                    }
                    
                    GeckoItems geckoItems = new GeckoItems();
                    DataItem[] arrayDataItems = new DataItem[1];

                    geckoItems.DataItems = arrayDataItems;
                    DataItem dataItem = new DataItem();
                    dataItem.Label = name;
                    dataItem.Text = fullname;
                    dataItem.Type = 0;
                    arrayDataItems[0] = dataItem;
                    PushPayload<GeckoItems> payload = new PushPayload<GeckoItems>();
                    payload.Data = geckoItems;
                    payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
                    string widgetKey = ConfigurationManager.AppSettings["TopCommsTextWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);

                }
                else
                {
                    DataItem[] arrayDataItems = new DataItem[1];
                    GeckoItems geckoItems = new GeckoItems();
                    geckoItems.DataItems = arrayDataItems;
                    DataItem dataItem = new DataItem();
                    dataItem.Label = string.Empty;
                    dataItem.Text = string.Empty;
                    dataItem.Type = 0;
                    arrayDataItems[0] = dataItem;

                    PushPayload<GeckoItems> payload = new PushPayload<GeckoItems>();
                    payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
                    payload.Data = geckoItems;
                    string widgetKey = ConfigurationManager.AppSettings["TopCommsTextWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);

                }
                return response;
            }
            catch (Exception ex)
            {
                con.Close();
                PushResult response = new PushResult();
                return response;
            }
        }
        public PushResult AddTopDingWidget()
        {
            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_ReconciliationTop_GetCurruntReport");
                PushResult response = new PushResult();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    List<ReconciliationTeamMemberModel> result = JsonConvert.DeserializeObject<List<ReconciliationTeamMemberModel>>(JSONString);
                    decimal? TopDing = 0;
                    string name = string.Empty;
                    string fullname = string.Empty;
                    if (result != null && result.Count > 0)
                    {
                         TopDing = result.Max(x => x.DingCount);
                         name = result.Where(x => x.DingCount == TopDing).Select(x => x.FullName).FirstOrDefault();
                         fullname = name + " ( " + TopDing.ToString() + " )";
                    }
                    

                    GeckoItems geckoItems = new GeckoItems();
                    DataItem[] arrayDataItems = new DataItem[1];

                    geckoItems.DataItems = arrayDataItems;
                    DataItem dataItem = new DataItem();
                    dataItem.Label = name;
                    dataItem.Text = fullname;
                    dataItem.Type = 0;
                    arrayDataItems[0] = dataItem;
                    PushPayload<GeckoItems> payload = new PushPayload<GeckoItems>();
                    payload.Data = geckoItems;
                    payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
                    string widgetKey = ConfigurationManager.AppSettings["TopDingTextWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);

                }
                else
                {
                    DataItem[] arrayDataItems = new DataItem[1];
                    GeckoItems geckoItems = new GeckoItems();
                    geckoItems.DataItems = arrayDataItems;
                    DataItem dataItem = new DataItem();
                    dataItem.Label = string.Empty;
                    dataItem.Text = string.Empty;
                    dataItem.Type = 0;
                    arrayDataItems[0] = dataItem;

                    PushPayload<GeckoItems> payload = new PushPayload<GeckoItems>();
                    payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
                    payload.Data = geckoItems;
                    string widgetKey = ConfigurationManager.AppSettings["TopDingTextWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);

                }
                return response;
            }
            catch (Exception ex)
            {
                con.Close();
                PushResult response = new PushResult();
                return response;
            }
        }
        public PushResult AddTopLeadsWidget()
        {
            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_ReconciliationTop_GetCurruntReport");
                PushResult response = new PushResult();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    List<ReconciliationTeamMemberModel> result = JsonConvert.DeserializeObject<List<ReconciliationTeamMemberModel>>(JSONString);
                    decimal? TopLeads = 0;
                    string name = string.Empty;
                    string fullname = string.Empty;
                    if (result != null && result.Count > 0)
                    {
                         TopLeads = result.Max(x => x.LeadCount);
                         name = result.Where(x => x.LeadCount == TopLeads).Select(x => x.FullName).FirstOrDefault();
                         fullname = name + " ( " + TopLeads.ToString() + " )"; 
                    }
                    

                    GeckoItems geckoItems = new GeckoItems();
                    DataItem[] arrayDataItems = new DataItem[1];

                    geckoItems.DataItems = arrayDataItems;
                    DataItem dataItem = new DataItem();
                    dataItem.Label = name;
                    dataItem.Text = fullname;
                    dataItem.Type = 0;
                    arrayDataItems[0] = dataItem;
                    PushPayload<GeckoItems> payload = new PushPayload<GeckoItems>();
                    payload.Data = geckoItems;
                    payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
                    string widgetKey = ConfigurationManager.AppSettings["TopLeadsTextWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);

                }
                else
                {
                    DataItem[] arrayDataItems = new DataItem[1];
                    GeckoItems geckoItems = new GeckoItems();
                    geckoItems.DataItems = arrayDataItems;
                    DataItem dataItem = new DataItem();
                    dataItem.Label = string.Empty;
                    dataItem.Text = string.Empty;
                    dataItem.Type = 0;
                    arrayDataItems[0] = dataItem;

                    PushPayload<GeckoItems> payload = new PushPayload<GeckoItems>();
                    payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
                    payload.Data = geckoItems;
                    string widgetKey = ConfigurationManager.AppSettings["TopLeadsTextWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);

                }
                return response;
            }
            catch (Exception ex)
            {
                con.Close();
                PushResult response = new PushResult();
                return response;
            }
        }

        public PushResult AddTopRNCWidget()
        {
            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_ReconciliationTop_GetCurruntReport");
                PushResult response = new PushResult();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    List<ReconciliationTeamMemberModel> result = JsonConvert.DeserializeObject<List<ReconciliationTeamMemberModel>>(JSONString);
                    decimal? TopRNC = 0;
                    string name = string.Empty;
                    string fullname = string.Empty;
                    if (result != null && result.Count > 0)
                    {
                         TopRNC = result.Max(x => x.RNCCount);
                         name = result.Where(x => x.RNCCount == TopRNC).Select(x => x.FullName).FirstOrDefault();
                         fullname = name + " ( " + TopRNC.ToString() + " )";
                    }
                    

                    GeckoItems geckoItems = new GeckoItems();
                    DataItem[] arrayDataItems = new DataItem[1];

                    geckoItems.DataItems = arrayDataItems;
                    DataItem dataItem = new DataItem();
                    dataItem.Label = name;
                    dataItem.Text = fullname;
                    dataItem.Type = 0;
                    arrayDataItems[0] = dataItem;
                    PushPayload<GeckoItems> payload = new PushPayload<GeckoItems>();
                    payload.Data = geckoItems;
                    payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
                    string widgetKey = ConfigurationManager.AppSettings["TopRNCTextWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);

                }
                else
                {
                    DataItem[] arrayDataItems = new DataItem[1];
                    GeckoItems geckoItems = new GeckoItems();
                    geckoItems.DataItems = arrayDataItems;
                    DataItem dataItem = new DataItem();
                    dataItem.Label = string.Empty;
                    dataItem.Text = string.Empty;
                    dataItem.Type = 0;
                    arrayDataItems[0] = dataItem;

                    PushPayload<GeckoItems> payload = new PushPayload<GeckoItems>();
                    payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
                    payload.Data = geckoItems;
                    string widgetKey = ConfigurationManager.AppSettings["TopRNCTextWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);

                }
                return response;
            }
            catch (Exception ex)
            {
                con.Close();
                PushResult response = new PushResult();
                return response;
            }
        }
        public PushResult AddTopREGWidget()
        {
            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_ReconciliationTop_GetCurruntReport");
                PushResult response = new PushResult();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    List<ReconciliationTeamMemberModel> result = JsonConvert.DeserializeObject<List<ReconciliationTeamMemberModel>>(JsonConvert.SerializeObject(ds.Tables[0]));
                    decimal? TopREG = 0;
                    string name = string.Empty;
                    string fullname = string.Empty;
                    if (result != null && result.Count > 0)
                    {
                         TopREG = result.Max(x => x.REGCount);
                         name = result.Where(x => x.REGCount == TopREG).Select(x => x.FullName).FirstOrDefault();
                         fullname = name + " ( " + TopREG.ToString() + " )";
                    }
                    

                    GeckoItems geckoItems = new GeckoItems();
                    DataItem[] arrayDataItems = new DataItem[1];

                    geckoItems.DataItems = arrayDataItems;
                    DataItem dataItem = new DataItem();
                    dataItem.Label = name;
                    dataItem.Text = fullname;
                    dataItem.Type = 0;
                    arrayDataItems[0] = dataItem;
                    PushPayload<GeckoItems> payload = new PushPayload<GeckoItems>();
                    payload.Data = geckoItems;
                    payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
                    string widgetKey = ConfigurationManager.AppSettings["TopREGTextWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);

                }
                else
                {
                    DataItem[] arrayDataItems = new DataItem[1];
                    GeckoItems geckoItems = new GeckoItems();
                    geckoItems.DataItems = arrayDataItems;
                    DataItem dataItem = new DataItem();
                    dataItem.Label = string.Empty;
                    dataItem.Text = string.Empty;
                    dataItem.Type = 0;
                    arrayDataItems[0] = dataItem;
                    
                    PushPayload<GeckoItems> payload = new PushPayload<GeckoItems>();
                    payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
                    payload.Data = geckoItems;
                    string widgetKey = ConfigurationManager.AppSettings["TopREGTextWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);
                   
                }
                return response;
            }
            catch (Exception ex)
            {
                con.Close();
                PushResult response = new PushResult();
                return response;
            }
        }


        


        public PushResult AddIncomingCallOfLast5Days()
        {
            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_RingCentralCallLog_GetLast5DayInCallLog");
                PushResult response = new PushResult();
                if (ds != null && ds.Tables.Count > 0)/*&& ds.Tables[0].Rows.Count > 0*/
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    List<RingCentralModel> result = JsonConvert.DeserializeObject<List<RingCentralModel>>(JSONString);
                    GeckoBarChart geckoBarChart = new GeckoBarChart();
                    List<GeckoBarChartSeries> listBarChartSeries = new List<GeckoBarChartSeries>();
                    GeckoBarChartSeries barchartSeries = new GeckoBarChartSeries();
                    barchartSeries.Data = result.Select(x => x.TotalCallDuration).ToList();
                    barchartSeries.Name = "Total Call duration";
                    listBarChartSeries.Add(barchartSeries);

                    GeckoBarChartXAxis barchartXAxis = new GeckoBarChartXAxis();
                    barchartXAxis.Labels = result.Select(x => x.CallToName).ToList();

                    geckoBarChart.Series = listBarChartSeries;
                    geckoBarChart.XAxis = barchartXAxis;
                    PushPayload<GeckoBarChart> payload = new PushPayload<GeckoBarChart>();
                    payload.Data = geckoBarChart;
                    payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
                    string widgetKey = ConfigurationManager.AppSettings["IncomingCallOfLast5DaysWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);

                }
                return response;
               
            }
            catch (Exception ex)
            {
                con.Close();
                PushResult response = new PushResult();
                return response;
            }
        }

        public PushResult AddOutgoingCallOfLast5Days()
        {
            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_RingCentralCallLog_GetLast5DayOutCallLog");
                PushResult response = new PushResult();
                if (ds != null && ds.Tables.Count > 0)/*&& ds.Tables[0].Rows.Count > 0*/
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    List<RingCentralModel> result = JsonConvert.DeserializeObject<List<RingCentralModel>>(JSONString);
                    GeckoBarChart geckoBarChart = new GeckoBarChart();
                    List<GeckoBarChartSeries> listBarChartSeries = new List<GeckoBarChartSeries>();
                    GeckoBarChartSeries barchartSeries = new GeckoBarChartSeries();
                    barchartSeries.Data = result.Select(x => x.TotalCallDuration).ToList();
                    barchartSeries.Name = "Total Call duration";
                    listBarChartSeries.Add(barchartSeries);

                    GeckoBarChartXAxis barchartXAxis = new GeckoBarChartXAxis();
                    barchartXAxis.Labels = result.Select(x => x.CallFromName).ToList();

                    geckoBarChart.Series = listBarChartSeries;
                    geckoBarChart.XAxis = barchartXAxis;
                    PushPayload<GeckoBarChart> payload = new PushPayload<GeckoBarChart>();
                    payload.Data = geckoBarChart;
                    payload.ApiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
                    string widgetKey = ConfigurationManager.AppSettings["OutgoingCallOfLast5DaysWidgetKey"].ToString();
                    GeckoConnect geckoConnect = new GeckoConnect();
                    response = geckoConnect.Push(payload, widgetKey);

                }
                return response;
            }
            catch (Exception ex)
            {
                con.Close();
                PushResult response = new PushResult();
                return response;
            }
        }

        public IList<IList<Object>> GenerateData(bool isRingCentralData, bool isSalesFigureData, bool isTopBillerData)
        {
            SqlConnection con = new SqlConnection(decryptedConnectionString);
            List<IList<Object>> objNewRecords = new List<IList<Object>>();

            if (isRingCentralData)
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_RingCentralCallLog_GetEmployeeWiseCallCount");
                if (ds != null && ds.Tables.Count > 0) /*&& ds.Tables[0].Rows.Count > 0*/
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    List<RingCentralModel> result = JsonConvert.DeserializeObject<List<RingCentralModel>>(JSONString);
                    foreach (RingCentralModel i in result)
                    {
                        IList<Object> obj = new List<Object>();
                        obj.Add(i.CallToName);
                        obj.Add(i.TotalInCount.ToString());
                        obj.Add(i.TotalOutCount.ToString());
                        obj.Add(i.CallDuration.ToString());
                        objNewRecords.Add(obj);

                    }
                }
                con.Close();

            }
            if (isSalesFigureData)
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_ReconciliationTeam_GetCurrentMonthReport");
                if (ds != null && ds.Tables.Count > 0) /*&& ds.Tables[0].Rows.Count > 0*/
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);

                    List<ReconciliationTeamMemberModel> result = JsonConvert.DeserializeObject<List<ReconciliationTeamMemberModel>>(JSONString);
                    foreach (ReconciliationTeamMemberModel i in result)
                    {
                        IList<Object> obj = new List<Object>();

                        obj.Add(i.FullName);
                        obj.Add(i.LeadCount.ToString());
                        obj.Add(i.RNCCount.ToString());
                        obj.Add(i.REGCount.ToString());
                        obj.Add(i.DingCount.ToString());

                        objNewRecords.Add(obj);
                    }
                }
                con.Close();
            }
            if (isTopBillerData)
            {
                string traderId = ConfigurationManager.AppSettings["TraderId"].ToString();
                string salesPersonId = ConfigurationManager.AppSettings["SalesPersonId"].ToString();

                SqlParameter paraDateFrom = new SqlParameter
                {
                    ParameterName = "DateFrom",
                    DbType = DbType.DateTime,
                    Value = DateTime.UtcNow.AddMonths(-12)
                };

                SqlParameter paraDateTo = new SqlParameter
                {
                    ParameterName = "DateTo",
                    DbType = DbType.DateTime,
                    Value = DateTime.UtcNow
                };

                SqlParameter paraTraderId = new SqlParameter
                {
                    ParameterName = "TraderId",
                    DbType = DbType.Int64,
                    Value = Convert.ToInt64(traderId)
                };

                SqlParameter paraSalesPersonId = new SqlParameter
                {
                    ParameterName = "SalesPersonId",
                    DbType = DbType.Int64,
                    Value = Convert.ToInt64(salesPersonId)
                };

                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_DashboardTop10ClientCommissions", paraDateFrom, paraDateTo, paraTraderId, paraSalesPersonId);

                if (ds != null && ds.Tables.Count > 0) /*&& ds.Tables[0].Rows.Count > 0*/
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);

                    List<Top10ClientCommissions> result = JsonConvert.DeserializeObject<List<Top10ClientCommissions>>(JSONString);
                    foreach (Top10ClientCommissions i in result)
                    {
                        IList<Object> obj = new List<Object>();
                        obj.Add(i.FullName);
                        obj.Add(i.Commisson.ToString());
                        objNewRecords.Add(obj);
                    }
                }
                con.Close();
            }
            return objNewRecords;
        }

        public GeckoDatasetResult CreateRingCentralCallLogDataSet()
        {
            GeckoDataset dataSet = new GeckoDataset();
            Dictionary<string, IDatasetField> fields = Getfield<RingCentralModel>(new RingCentralModel());
            List<KeyValuePair<string, IDatasetField>> list_fields = fields.ToList();
            Dictionary<string, IDatasetField> field = new Dictionary<string, IDatasetField>();
            foreach (KeyValuePair<string, IDatasetField> i in list_fields)
            {
               if(i.Key == "calltoname" || i.Key == "totalincount" || i.Key == "totaloutcount" || i.Key == "callduration")
               {
                    field.Add(i.Key, i.Value);
               }
            }
             dataSet.Fields = field;
            // Create Sample DataSet by API
            string dataSetName = "ringcentral_call_log";
            string apiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
            GeckoConnect geckoConnect = new GeckoConnect();
            GeckoDatasetResult response = geckoConnect.CreateDataset(dataSet, dataSetName, apiKey);
            return response;
           
        }

        public GeckoDatasetResult CreateSalesFigureDataSet()
        {
            GeckoDataset dataSet = new GeckoDataset();
            Dictionary<string, IDatasetField> fields = Getfield<ReconciliationTeamMemberModel>(new ReconciliationTeamMemberModel());
            List<KeyValuePair<string, IDatasetField>> list_fields = fields.ToList();
            Dictionary<string, IDatasetField> field = new Dictionary<string, IDatasetField>();
            foreach (KeyValuePair<string, IDatasetField> i in list_fields)
            {
                if (i.Key == "fullname" || i.Key == "leadcount" || i.Key == "rnccount" || i.Key == "regcount" || i.Key == "dingcount")
                {
                    field.Add(i.Key, i.Value);
                }
            }
            dataSet.Fields = field;
            // Create Sample DataSet by API
            string dataSetName = "board_stats";
            string apiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
            GeckoConnect geckoConnect = new GeckoConnect();
            GeckoDatasetResult response = geckoConnect.CreateDataset(dataSet, dataSetName, apiKey);
            return response;

        }

        public GeckoDatasetResult CreateTopBillerDataSet()
        {
            GeckoDataset dataSet = new GeckoDataset();
            Dictionary<string, IDatasetField> fields = Getfield<Top10ClientCommissions>(new Top10ClientCommissions());
            List<KeyValuePair<string, IDatasetField>> list_fields = fields.ToList();
            Dictionary<string, IDatasetField> field = new Dictionary<string, IDatasetField>();
            foreach (KeyValuePair<string, IDatasetField> i in list_fields)
            {
                if (i.Key == "fullname" || i.Key == "commisson")
                {
                    field.Add(i.Key, i.Value);
                }
            }
            dataSet.Fields = field;
            // Create Sample DataSet by API
            string dataSetName = "top_biller";
            string apiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
            GeckoConnect geckoConnect = new GeckoConnect();
            GeckoDatasetResult response = geckoConnect.CreateDataset(dataSet, dataSetName, apiKey);
            return response;

        }

        public bool AppendRingCentralData()
        {

            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_RingCentralCallLog_GetEmployeeWiseCallCount");
                bool response = false;
                if (ds != null && ds.Tables.Count > 0)/*&& ds.Tables[0].Rows.Count > 0*/
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    List<RingCentralModel> result = JsonConvert.DeserializeObject<List<RingCentralModel>>(JSONString);
                    string dataSetName = "ringcentral_call_log";
                    string apiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
                    RingCentralModel dataModel = new RingCentralModel();
                    Type RingCentralModel = dataModel.GetType();
                    PropertyInfo[] properties = RingCentralModel.GetProperties();
                    dynamic ListDyObj = new List<ExpandoObject>();
                    RequestModel<ExpandoObject> requestModel = new RequestModel<ExpandoObject>();

                    foreach (RingCentralModel data in result)
                    {
                        dynamic DyObj = new ExpandoObject();
                        foreach (PropertyInfo prop in properties)
                        {
                            string name = prop.Name.ToLower();
                            object value = data.GetType().GetProperty(prop.Name).GetValue(data, null);
                            if (name == "calltoname" || name == "totalincount" || name == "totaloutcount" || name == "callduration")
                            {
                                ((IDictionary<String, object>)DyObj)[name] = value;
                            }
                        }
                        ListDyObj.Add(DyObj);
                    }
                    requestModel.data = ListDyObj;
                    GeckoConnect geckoConnect = new GeckoConnect();
                   // response = geckoConnect.AppendDataset(requestModel, dataSetName, apiKey);
                    response = geckoConnect.ReplaceDataset(requestModel, dataSetName, apiKey);
                }
                con.Close();
                return response;
            }
            catch (Exception ex)
            {
                con.Close();
                return false;
            }

           
        }
        public bool AppendSalesFigureData()
        {

            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_ReconciliationTeam_GetCurrentMonthReport");
                bool response = false;
                if (ds != null && ds.Tables.Count > 0)/*&& ds.Tables[0].Rows.Count > 0*/
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    List<ReconciliationTeamMemberModel> result = JsonConvert.DeserializeObject<List<ReconciliationTeamMemberModel>>(JSONString);
                    string dataSetName = "board_stats";
                    string apiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();
                    ReconciliationTeamMemberModel dataModel = new ReconciliationTeamMemberModel();
                    Type ReconciliationModel = dataModel.GetType();
                    PropertyInfo[] properties = ReconciliationModel.GetProperties();
                    dynamic ListDyObj = new List<ExpandoObject>();
                    RequestModel<ExpandoObject> requestModel = new RequestModel<ExpandoObject>();

                    foreach (ReconciliationTeamMemberModel data in result)
                    {
                        dynamic DyObj = new ExpandoObject();
                        foreach (PropertyInfo prop in properties)
                        {
                            string name = prop.Name.ToLower();
                            
                            if (name == "fullname" || name == "leadcount" || name == "rnccount" || name == "regcount" || name == "dingcount")
                            {
                                object value = data.GetType().GetProperty(prop.Name).GetValue(data, null);
                                ((IDictionary<String, object>)DyObj)[name] = value;
                            }
                        }
                        ListDyObj.Add(DyObj);
                    }
                   
                    requestModel.data = ListDyObj;
                    GeckoConnect geckoConnect = new GeckoConnect();
                   // response = geckoConnect.AppendDataset(requestModel, dataSetName, apiKey);
                    response = geckoConnect.ReplaceDataset(requestModel, dataSetName, apiKey);
                }
                con.Close();
                return response;
            }
            catch (Exception ex)
            {
                con.Close();
                return false;
            }

        }
        public bool AppendTopBillerData()
        {

            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                string traderId = ConfigurationManager.AppSettings["TraderId"].ToString();
                string salesPersonId = ConfigurationManager.AppSettings["SalesPersonId"].ToString();
                SqlParameter paraDateFrom = new SqlParameter
                {
                    ParameterName = "DateFrom",
                    DbType = DbType.DateTime,
                    Value = DateTime.UtcNow.AddMonths(-12)
                };

                SqlParameter paraDateTo = new SqlParameter
                {
                    ParameterName = "DateTo",
                    DbType = DbType.DateTime,
                    Value = DateTime.UtcNow
                };

                SqlParameter paraTraderId = new SqlParameter
                {
                    ParameterName = "TraderId",
                    DbType = DbType.Int64,
                    Value = Convert.ToInt64(traderId)
                };

                SqlParameter paraSalesPersonId = new SqlParameter
                {
                    ParameterName = "SalesPersonId",
                    DbType = DbType.Int64,
                    Value = Convert.ToInt64(salesPersonId)
                };

                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_DashboardTop10ClientCommissions", paraDateFrom, paraDateTo, paraTraderId, paraSalesPersonId);

                bool response = false;
                if (ds != null && ds.Tables.Count > 0)/*&& ds.Tables[0].Rows.Count > 0*/
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    List<Top10ClientCommissions> result = JsonConvert.DeserializeObject<List<Top10ClientCommissions>>(JSONString);
                    string dataSetName = "top_biller";
                    string apiKey = ConfigurationManager.AppSettings["GeckoAPIkey"].ToString();

                    Top10ClientCommissions dataModel = new Top10ClientCommissions();
                    Type clientCommisionModel = dataModel.GetType();
                    PropertyInfo[] properties = clientCommisionModel.GetProperties();
                    dynamic ListDyObj = new List<ExpandoObject>();
                    RequestModel<ExpandoObject> requestModel = new RequestModel<ExpandoObject>();

                    foreach (Top10ClientCommissions data in result)
                    {
                        dynamic DyObj = new ExpandoObject();
                        foreach (PropertyInfo prop in properties)
                        {
                            string name = prop.Name.ToLower();

                            if (name == "fullname" || name == "commisson")
                            {
                                object value = data.GetType().GetProperty(prop.Name).GetValue(data, null);
                                ((IDictionary<String, object>)DyObj)[name] = value;
                            }
                        }
                        ListDyObj.Add(DyObj);
                    }

                    requestModel.data = ListDyObj;
                    GeckoConnect geckoConnect = new GeckoConnect();
                    //response = geckoConnect.AppendDataset(requestModel, dataSetName, apiKey);
                    response = geckoConnect.ReplaceDataset(requestModel, dataSetName, apiKey);
                }
                con.Close();
                return response;
            }
            catch (Exception ex)
            {
                con.Close();
                return false;
            }

        }
        public Dictionary<string, IDatasetField> Getfield<T>(T rc)
        {
            Type RingCentralModel = rc.GetType();
            PropertyInfo[] properties = RingCentralModel.GetProperties();
            List<IDatasetField> DataFieldList = new List<IDatasetField>();
            foreach (PropertyInfo i in properties)
            {
                IDatasetField dataField = new IDatasetField();
                dataField.Name = i.Name.ToLower();
                string type = i.PropertyType.Name.ToLower();
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
                    dataField.Optional = false;
                }
                dataField.CurrencyCode = null;
                DataFieldList.Add(dataField);



            }
            Dictionary<string, IDatasetField> field = new Dictionary<string, IDatasetField>();
            foreach (IDatasetField i in DataFieldList)
            {

                field.Add(i.Name, i);
            }

            return field;

        }

    }
}