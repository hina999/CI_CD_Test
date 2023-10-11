using FXAdminTransferConnex.Business.Client;
using FXAdminTransferConnex.Common;
using FXAdminTransferConnex.Data.Helper;
using FXAdminTransferConnex.Entities;
using Microsoft.ApplicationBlocks.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace TransferConnexChatApp.Common   
{
    public class MarketValueNotification
    {
        private static string encryptedConnectionString = ConfigurationManager.ConnectionStrings["FXBackOfficeSystemContext"].ConnectionString;
        private static string decryptedConnectionString = AESEncryptionDecryptionHelper.Decrypt(encryptedConnectionString);
        Dictionary<string, decimal> exchangeRates = new Dictionary<string, decimal>();
        string CurrencyCloudToken = "";
        string RingCentralToken = "";

        public static List<MarketOrderSettingModel> GetMarketOrderSettingList()
        {
            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                //SqlCommand com = new SqlCommand("usp_MarketOrderSettingList", con);
                //com.CommandType = CommandType.StoredProcedure;
                //com.Parameters.Add(new SqlParameter("@ClientId", 0));
                //SqlDataAdapter da = new SqlDataAdapter(com);
                //DataSet ds = new DataSet();
                //con.Open();
                //da.Fill(ds);

                var paraClientId = new SqlParameter
                {
                    ParameterName = "ClientId",
                    DbType = DbType.Int64,
                    Value = 0
                };

                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_MarketOrderSettingList", paraClientId);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var JSONString = JsonConvert.SerializeObject(ds.Tables[0]);

                    var result = JsonConvert.DeserializeObject<List<MarketOrderSettingModel>>(JSONString);
                    return result;
                }
                else
                {
                    return new List<MarketOrderSettingModel>();
                }
            }
            catch (Exception ex)
            {
                return new List<MarketOrderSettingModel>();
            }
            finally
            {
                con.Close();
            }
        }

        public static void MarketOrderTriggerWithGCPartner()
        {
            try
            {
                var clientMOList = GetMarketOrderSettingList();
                if (clientMOList != null && clientMOList.Count > 0)
                {
                    string[] Currencies = clientMOList.Select(x => x.FromCurrencyName).Distinct().ToArray();
                    foreach (var Currency in Currencies)
                    {
                        try
                        {
                            var model = new GCPartnerMarketOrderRateModel();

                            string url = "GetInterbankRates";

                            Dictionary<string, string> dic = new Dictionary<string, string>();
                            dic.Add("Currency", Currency);

                            var objMarketRate = WebApiHelper.HttpClientPostGCPartnerToken(model, url, dic);
                            if (objMarketRate != null && objMarketRate.Result != null && objMarketRate.Result.ReturnObject != null)
                            {
                                Type myType = objMarketRate.Result.ReturnObject.GetType();
                                IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                                decimal currentMarketRate = 0;
                                foreach (PropertyInfo prop in props)
                                {
                                    if (prop.Name.ToUpper() == Currency.ToUpper())
                                    {
                                        currentMarketRate = Convert.ToDecimal(prop.GetValue(objMarketRate.Result.ReturnObject, null));
                                        break;
                                    }
                                }

                                if (currentMarketRate > 0)
                                {
                                    var Clients = clientMOList.Where(x => x.FromCurrencyName == Currency).ToArray();

                                    foreach (var item in Clients)
                                    {
                                        switch (item.Operator)
                                        {
                                            case "<":
                                                if (item.Filter.ToUpper() == "BEFORE" &&
                                                    item.StartDate > DateTime.Now &&
                                                    (currentMarketRate < item.MarketRate))
                                                {

                                                    SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);
                                                }
                                                else if (item.Filter.ToUpper() == "AFTER" &&
                                                         item.StartDate < DateTime.Now &&
                                                         (currentMarketRate < item.MarketRate))
                                                {
                                                    SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);
                                                }
                                                else if (item.Filter.ToUpper() == "RANGE" &&
                                                        (item.StartDate <= DateTime.Now &&
                                                          item.EndDate >= DateTime.Now) &&
                                                        currentMarketRate < item.MarketRate)
                                                {
                                                    SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);
                                                }

                                                break;
                                            case ">":
                                                if (item.Filter.ToUpper() == "BEFORE" &&
                                                    item.StartDate > DateTime.Now &&
                                                    (currentMarketRate > item.MarketRate))
                                                {
                                                    SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);
                                                }
                                                else if (item.Filter.ToUpper() == "AFTER" &&
                                                         item.StartDate < DateTime.Now &&
                                                         (currentMarketRate > item.MarketRate))
                                                {
                                                    SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);
                                                }
                                                else if (item.Filter.ToUpper() == "RANGE" &&
                                                         (item.StartDate <= DateTime.Now &&
                                                          item.EndDate >= DateTime.Now) &&
                                                         currentMarketRate > item.MarketRate)
                                                {
                                                    SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);

                                                }
                                                break;
                                            case "<=":
                                                if (item.Filter.ToUpper() == "BEFORE" &&
                                                    item.StartDate > DateTime.Now &&
                                                    (currentMarketRate <= item.MarketRate))
                                                {
                                                    SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);

                                                }
                                                else if (item.Filter.ToUpper() == "AFTER" &&
                                                         item.StartDate < DateTime.Now &&
                                                         (currentMarketRate <= item.MarketRate))
                                                {
                                                    SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);

                                                }
                                                else if (item.Filter.ToUpper() == "RANGE" &&
                                                         (item.StartDate <= DateTime.Now &&
                                                          item.EndDate >= DateTime.Now) &&
                                                         currentMarketRate <= item.MarketRate)
                                                {
                                                    SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);

                                                }
                                                break;
                                            case ">=":
                                                if (item.Filter.ToUpper() == "BEFORE" &&
                                                    item.StartDate > DateTime.Now &&
                                                    (currentMarketRate >= item.MarketRate))
                                                {
                                                    SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);

                                                }
                                                else if (item.Filter.ToUpper() == "AFTER" &&
                                                         item.StartDate < DateTime.Now &&
                                                         (currentMarketRate >= item.MarketRate))
                                                {
                                                    SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);

                                                }
                                                else if (item.Filter.ToUpper() == "RANGE" &&
                                                         (item.StartDate <= DateTime.Now &&
                                                          item.EndDate >= DateTime.Now) &&
                                                         currentMarketRate >= item.MarketRate)
                                                {
                                                    SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {

                            throw;
                        }
                    }


                    //foreach (var item in clientMOList)
                    //{
                    //    try
                    //    {
                    //        var model = new GCPartnerMarketOrderRateModel();

                    //        string url = "GetInterbankRates";

                    //        Dictionary<string, string> dic = new Dictionary<string, string>();
                    //        dic.Add("Currency", item.FromCurrencyName);

                    //        var objMarketRate = WebApiHelper.HttpClientPostGCPartnerToken(model, url, dic);
                    //        if (objMarketRate != null && objMarketRate.Result != null && objMarketRate.Result.ReturnObject != null)
                    //        {
                    //            Type myType = objMarketRate.Result.ReturnObject.GetType();
                    //            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                    //            decimal currentMarketRate = 0;
                    //            foreach (PropertyInfo prop in props)
                    //            {
                    //                if (prop.Name.ToUpper() == item.ToCurrencyName.ToUpper())
                    //                {
                    //                    currentMarketRate = Convert.ToDecimal(prop.GetValue(objMarketRate.Result.ReturnObject, null));
                    //                    break;
                    //                }
                    //            }

                    //            if (currentMarketRate > 0)
                    //            {
                    //                switch (item.Operator)
                    //                {
                    //                    case "<":
                    //                        if (item.Filter.ToUpper() == "BEFORE" &&
                    //                            item.StartDate > DateTime.Now &&
                    //                            (currentMarketRate < item.MarketRate))
                    //                        {

                    //                            SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);
                    //                        }
                    //                        else if (item.Filter.ToUpper() == "AFTER" &&
                    //                                 item.StartDate < DateTime.Now &&
                    //                                 (currentMarketRate < item.MarketRate))
                    //                        {
                    //                            SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);
                    //                        }
                    //                        else if (item.Filter.ToUpper() == "RANGE" &&
                    //                                (item.StartDate <= DateTime.Now &&
                    //                                  item.EndDate >= DateTime.Now) &&
                    //                                currentMarketRate < item.MarketRate)
                    //                        {
                    //                            SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);
                    //                        }

                    //                        break;
                    //                    case ">":
                    //                        if (item.Filter.ToUpper() == "BEFORE" &&
                    //                            item.StartDate > DateTime.Now &&
                    //                            (currentMarketRate > item.MarketRate))
                    //                        {
                    //                            SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);
                    //                        }
                    //                        else if (item.Filter.ToUpper() == "AFTER" &&
                    //                                 item.StartDate < DateTime.Now &&
                    //                                 (currentMarketRate > item.MarketRate))
                    //                        {
                    //                            SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);
                    //                        }
                    //                        else if (item.Filter.ToUpper() == "RANGE" &&
                    //                                 (item.StartDate <= DateTime.Now &&
                    //                                  item.EndDate >= DateTime.Now) &&
                    //                                 currentMarketRate > item.MarketRate)
                    //                        {
                    //                            SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);

                    //                        }
                    //                        break;
                    //                    case "<=":
                    //                        if (item.Filter.ToUpper() == "BEFORE" &&
                    //                            item.StartDate > DateTime.Now &&
                    //                            (currentMarketRate <= item.MarketRate))
                    //                        {
                    //                            SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);

                    //                        }
                    //                        else if (item.Filter.ToUpper() == "AFTER" &&
                    //                                 item.StartDate < DateTime.Now &&
                    //                                 (currentMarketRate <= item.MarketRate))
                    //                        {
                    //                            SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);

                    //                        }
                    //                        else if (item.Filter.ToUpper() == "RANGE" &&
                    //                                 (item.StartDate <= DateTime.Now &&
                    //                                  item.EndDate >= DateTime.Now) &&
                    //                                 currentMarketRate <= item.MarketRate)
                    //                        {
                    //                            SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);

                    //                        }
                    //                        break;
                    //                    case ">=":
                    //                        if (item.Filter.ToUpper() == "BEFORE" &&
                    //                            item.StartDate > DateTime.Now &&
                    //                            (currentMarketRate >= item.MarketRate))
                    //                        {
                    //                            SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);

                    //                        }
                    //                        else if (item.Filter.ToUpper() == "AFTER" &&
                    //                                 item.StartDate < DateTime.Now &&
                    //                                 (currentMarketRate >= item.MarketRate))
                    //                        {
                    //                            SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);

                    //                        }
                    //                        else if (item.Filter.ToUpper() == "RANGE" &&
                    //                                 (item.StartDate <= DateTime.Now &&
                    //                                  item.EndDate >= DateTime.Now) &&
                    //                                 currentMarketRate >= item.MarketRate)
                    //                        {
                    //                            SaveMarketOrderClientNotification(item.ClientId, item.MarketOrderId);
                    //                        }
                    //                        break;
                    //                    default:
                    //                        break;
                    //                }
                    //            }
                    //        }
                    //    }
                    //    catch (Exception e)
                    //    {

                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void SaveMarketOrderClientNotification(long clientId, long marketOrderId)
        {
            try
            {
                SqlConnection con = new SqlConnection(decryptedConnectionString);
                try
                {
                    //SqlCommand com = new SqlCommand("usp_MarketValueNotification_Insert", con);
                    //com.CommandType = CommandType.StoredProcedure;
                    //com.Parameters.Add(new SqlParameter("@ClientId", clientId));
                    //com.Parameters.Add(new SqlParameter("@MarketOrderId", marketOrderId));
                    //com.Connection = con;
                    //con.Open();
                    //com.ExecuteNonQuery();

                    var paraClientId = new SqlParameter
                    {
                        ParameterName = "ClientId",
                        DbType = DbType.Int64,
                        Value = clientId
                    };
                    var paraMarketOrderId = new SqlParameter
                    {
                        ParameterName = "MarketOrderId",
                        DbType = DbType.Int64,
                        Value = marketOrderId
                    };

                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "usp_MarketValueNotification_Insert", paraClientId, paraMarketOrderId);
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    con.Close();
                }
            }
            catch (Exception e)
            {

            }
        }

        public List<ClientMasterModel> GetCC_Clientlist()
        {
            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                //SqlCommand com = new SqlCommand("usp_ClientMaster_GetCC_ClientList", con);
                //com.CommandType = CommandType.StoredProcedure;
                //SqlDataAdapter da = new SqlDataAdapter(com);
                //DataSet ds = new DataSet();

                //con.Open();
                //da.Fill(ds);
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_ClientMaster_GetCC_ClientList");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var JSONString = JsonConvert.SerializeObject(ds.Tables[0]);

                    var result = JsonConvert.DeserializeObject<List<ClientMasterModel>>(JSONString);
                    return result;
                }
                else
                {
                    return new List<ClientMasterModel>();
                }
            }
            catch (Exception ex)
            {
                return new List<ClientMasterModel>();
            }
            finally
            {
                con.Close();
            }
        }

        public long ImportClient(ContactModel contact, AccountModel account, string ClientSource)
        {
            try
            {
                SqlConnection con = new SqlConnection(decryptedConnectionString);
                try
                {
                    //SqlCommand com = new SqlCommand("usp_ClientMaster_ImportClient", con);
                    //com.CommandType = CommandType.StoredProcedure;
                    //com.Parameters.Add(new SqlParameter("@FullName", (object)(contact.first_name + ' ' + contact.last_name) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@CompanyName", (object)(contact.account_name) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@AddressLine1", (object)(account.street) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@AddressLine2", (object)(account.postal_code) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@City_Town", (object)(account.city) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@Country", (object)(account.country) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@MobileNo", (object)(contact.mobile_phone_number) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@AltMobileNo", (object)(contact.phone_number) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@EmailAddress", (object)(contact.email_address) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@AccountNo", (object)(account.short_reference) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@CCAccount_id", (object)(contact.account_id) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@RegiterDate", (object)(Convert.ToDateTime(contact.created_at)) ?? DBNull.Value));
                    //int IsActive = 0;
                    //if (contact.status == "enabled")
                    //{
                    //    IsActive = 1;
                    //}
                    //com.Parameters.Add(new SqlParameter("@IsActive", IsActive));
                    //com.Parameters.Add(new SqlParameter("@CreatedDate", (object)(Convert.ToDateTime(contact.created_at)) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@UpdatedDate", (object)(Convert.ToDateTime(contact.updated_at)) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@UserId", 0));
                    //com.Parameters.Add(new SqlParameter("@ClientSource", (object)(ClientSource) ?? DBNull.Value));
                    //SqlDataAdapter da = new SqlDataAdapter(com);
                    //DataSet ds = new DataSet();

                    //con.Open();
                    //da.Fill(ds);
                    var paraFullName = new SqlParameter
                    {
                        ParameterName = "FullName",
                        DbType = DbType.String,
                        Value = (object)(contact.first_name + ' ' + contact.last_name) ?? DBNull.Value
                    };
                    var paraCompanyName = new SqlParameter
                    {
                        ParameterName = "CompanyName",
                        DbType = DbType.String,
                        Value = (object)(contact.account_name) ?? DBNull.Value
                    };
                    var paraAddressLine1 = new SqlParameter
                    {
                        ParameterName = "AddressLine1",
                        DbType = DbType.String,
                        Value = (object)(account.street) ?? DBNull.Value
                    };
                    var paraAddressLine2 = new SqlParameter
                    {
                        ParameterName = "AddressLine2",
                        DbType = DbType.String,
                        Value = (object)(account.postal_code) ?? DBNull.Value
                    };
                    var paraCityTown = new SqlParameter
                    {
                        ParameterName = "City_Town",
                        DbType = DbType.String,
                        Value = (object)(account.city) ?? DBNull.Value
                    };
                    var paraCountry = new SqlParameter
                    {
                        ParameterName = "Country",
                        DbType = DbType.String,
                        Value = (object)(account.country) ?? DBNull.Value
                    };
                    var paraMobileNo = new SqlParameter
                    {
                        ParameterName = "MobileNo",
                        DbType = DbType.String,
                        Value = (object)(contact.mobile_phone_number) ?? DBNull.Value
                    };
                    var paraAltMobileNo = new SqlParameter
                    {
                        ParameterName = "AltMobileNo",
                        DbType = DbType.String,
                        Value = (object)(contact.phone_number) ?? DBNull.Value
                    };
                    var paraEmailAddress = new SqlParameter
                    {
                        ParameterName = "EmailAddress",
                        DbType = DbType.String,
                        Value = (object)(contact.email_address) ?? DBNull.Value
                    };
                    var paraAccountNo = new SqlParameter
                    {
                        ParameterName = "AccountNo",
                        DbType = DbType.String,
                        Value = (object)(account.short_reference) ?? DBNull.Value
                    };
                    var paraCCAccount_id = new SqlParameter
                    {
                        ParameterName = "CCAccount_id",
                        DbType = DbType.String,
                        Value = (object)(contact.account_id) ?? DBNull.Value
                    };
                    var paraRegiterDate = new SqlParameter
                    {
                        ParameterName = "RegiterDate",
                        DbType = DbType.DateTime,
                        Value = (object)(Convert.ToDateTime(contact.created_at)) ?? DBNull.Value
                    };
                    int IsActive = 0;
                    if (contact.status == "enabled")
                    {
                        IsActive = 1;
                    }
                    var paraIsActive = new SqlParameter
                    {
                        ParameterName = "IsActive",
                        DbType = DbType.Int32,
                        Value = IsActive
                    };
                    var paraCreatedDate = new SqlParameter
                    {
                        ParameterName = "CreatedDate",
                        DbType = DbType.DateTime,
                        Value = (object)(Convert.ToDateTime(contact.created_at)) ?? DBNull.Value
                    };
                    var paraUpdatedDate = new SqlParameter
                    {
                        ParameterName = "UpdatedDate",
                        DbType = DbType.DateTime,
                        Value = (object)(Convert.ToDateTime(contact.updated_at)) ?? DBNull.Value
                    };
                    var paraUserId = new SqlParameter
                    {
                        ParameterName = "UserId",
                        DbType = DbType.Int32,
                        Value = 0
                    };
                    var paraClientSource = new SqlParameter
                    {
                        ParameterName = "ClientSource",
                        DbType = DbType.String,
                        Value = (object)(ClientSource) ?? DBNull.Value
                    };
                    DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_ClientMaster_ImportClient", paraFullName, paraCompanyName, paraAddressLine1, paraAddressLine2, paraCityTown, paraCountry, paraMobileNo, paraAltMobileNo, paraEmailAddress, paraAccountNo, paraCCAccount_id, paraRegiterDate, paraIsActive, paraCreatedDate, paraUpdatedDate, paraUserId, paraClientSource);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        //var JSONString = JsonConvert.SerializeObject(ds.Tables[0]);

                        //var result = JsonConvert.DeserializeObject<List<string>>(JSONString);
                        return ds.Tables[0].Rows[0].Field<long>(0);
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    return 0;
                }
                finally
                {
                    con.Close();
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int ImportDeal(ConversionModel conversion, string TStatus, string DealSource)
        {
            try
            {
                SqlConnection con = new SqlConnection(decryptedConnectionString);
                try
                {
                    //SqlCommand com = new SqlCommand("usp_Deal_ImportDeal", con);
                    //com.CommandType = CommandType.StoredProcedure;
                    //com.Parameters.Add(new SqlParameter("@DealNo", (object)(conversion.short_reference) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@TradeDate", (object)(Convert.ToDateTime(conversion.created_at)) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@ValueDate", (object)(Convert.ToDateTime(conversion.settlement_date)) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@ClientSoldAmt", (object)(conversion.client_sell_amount) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@ClientSoldCCY", (object)(conversion.sell_currency) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@ClientSoldGBP", (object)(conversion.client_sell_amount_In_GBP) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@ClientBoughtAmt", (object)(conversion.client_buy_amount) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@ClientBoughtCCY", (object)(conversion.buy_currency) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@TradeType", (object)("Spot") ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@TStatus", (object)TStatus ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@CCAccount_id", (object)(conversion.account_id) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@GrossCommCurrency", (object)conversion.GrossCommCurrency ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@GrossCommGBP", (object)(conversion.GrossCommGBP) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@GrossComm", (object)(conversion.GrossComm) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@CreatedDate", (object)(Convert.ToDateTime(conversion.created_at)) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@UpdatedDate", (object)(Convert.ToDateTime(conversion.updated_at)) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@UserId", 0));
                    //com.Parameters.Add(new SqlParameter("@Status", (object)conversion.status ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@DealSource", (object)DealSource ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@MarketConversionRate", (object)conversion.mid_market_rate ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@GBPConversationRate", (object)conversion.GBPConversationRate ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@ProfitOrLoss", (object)conversion.amount_profit_loss ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@EventDate", (object)(Convert.ToDateTime(conversion.event_date)) ?? DBNull.Value));
                    //SqlDataAdapter da = new SqlDataAdapter(com);
                    //DataSet ds = new DataSet();

                    //con.Open();
                    //da.Fill(ds);

                    var paraDealNo = new SqlParameter
                    {
                        ParameterName = "DealNo",
                        DbType = DbType.String,
                        Value = (object)(conversion.short_reference) ?? DBNull.Value
                    };
                    var paraTradeDate = new SqlParameter
                    {
                        ParameterName = "TradeDate",
                        DbType = DbType.DateTime,
                        Value = (object)(Convert.ToDateTime(conversion.created_at)) ?? DBNull.Value
                    };
                    var paraValueDate = new SqlParameter
                    {
                        ParameterName = "ValueDate",
                        DbType = DbType.DateTime,
                        Value = (object)(Convert.ToDateTime(conversion.settlement_date)) ?? DBNull.Value
                    };
                    var paraClientSoldAmt = new SqlParameter
                    {
                        ParameterName = "ClientSoldAmt",
                        DbType = DbType.String,
                        Value = (object)(conversion.client_sell_amount) ?? DBNull.Value
                    };
                    var paraClientSoldCCY = new SqlParameter
                    {
                        ParameterName = "ClientSoldCCY",
                        DbType = DbType.String,
                        Value = (object)(conversion.sell_currency) ?? DBNull.Value
                    };
                    var paraClientSoldGBP = new SqlParameter
                    {
                        ParameterName = "ClientSoldGBP",
                        DbType = DbType.String,
                        Value = (object)(conversion.client_sell_amount_In_GBP) ?? DBNull.Value
                    };
                    var paraClientBoughtAmt = new SqlParameter
                    {
                        ParameterName = "ClientBoughtAmt",
                        DbType = DbType.String,
                        Value = (object)(conversion.client_buy_amount) ?? DBNull.Value
                    };
                    var paraClientBoughtCCY = new SqlParameter
                    {
                        ParameterName = "ClientBoughtCCY",
                        DbType = DbType.String,
                        Value = (object)(conversion.buy_currency) ?? DBNull.Value
                    };
                    var paraTradeType = new SqlParameter
                    {
                        ParameterName = "TradeType",
                        DbType = DbType.String,
                        Value = (object)("Spot") ?? DBNull.Value
                    };
                    var paraTStatus = new SqlParameter
                    {
                        ParameterName = "TStatus",
                        DbType = DbType.String,
                        Value = (object)(TStatus) ?? DBNull.Value
                    };
                    var paraCCAccount_id = new SqlParameter
                    {
                        ParameterName = "CCAccount_id",
                        DbType = DbType.String,
                        Value = (object)(conversion.account_id) ?? DBNull.Value
                    };
                    var paraGrossCommCurrency = new SqlParameter
                    {
                        ParameterName = "GrossCommCurrency",
                        DbType = DbType.String,
                        Value = (object)(conversion.GrossCommCurrency) ?? DBNull.Value
                    };
                    var paraGrossCommGBP = new SqlParameter
                    {
                        ParameterName = "GrossCommGBP",
                        DbType = DbType.String,
                        Value = (object)(conversion.GrossCommGBP) ?? DBNull.Value
                    };
                    var paraGrossComm = new SqlParameter
                    {
                        ParameterName = "GrossComm",
                        DbType = DbType.String,
                        Value = (object)(conversion.GrossComm) ?? DBNull.Value
                    };
                    var paraCreatedDate = new SqlParameter
                    {
                        ParameterName = "CreatedDate",
                        DbType = DbType.String,
                        Value = (object)(Convert.ToDateTime(conversion.created_at)) ?? DBNull.Value
                    };
                    var paraUpdatedDate = new SqlParameter
                    {
                        ParameterName = "UpdatedDate",
                        DbType = DbType.String,
                        Value = (object)(Convert.ToDateTime(conversion.updated_at)) ?? DBNull.Value
                    };
                    var paraUserId = new SqlParameter
                    {
                        ParameterName = "UserId",
                        DbType = DbType.Int32,
                        Value = 0
                    };
                    var paraStatus = new SqlParameter
                    {
                        ParameterName = "Status",
                        DbType = DbType.String,
                        Value = (object)conversion.status ?? DBNull.Value
                    };
                    var paraDealSource = new SqlParameter
                    {
                        ParameterName = "DealSource",
                        DbType = DbType.String,
                        Value = (object)DealSource ?? DBNull.Value
                    };
                    var paraMarketConversionRate = new SqlParameter
                    {
                        ParameterName = "MarketConversionRate",
                        DbType = DbType.Decimal,
                        Value = (object)conversion.mid_market_rate ?? DBNull.Value
                    };
                    var paraGBPConversationRate = new SqlParameter
                    {
                        ParameterName = "GBPConversationRate",
                        DbType = DbType.Decimal,
                        Value = (object)conversion.GBPConversationRate ?? DBNull.Value
                    };
                    var paraProfitOrLoss = new SqlParameter
                    {
                        ParameterName = "ProfitOrLoss",
                        DbType = DbType.Decimal,
                        Value = (object)(conversion.amount_profit_loss) ?? DBNull.Value
                    };
                    var paraEventDate = new SqlParameter
                    {
                        ParameterName = "EventDate",
                        DbType = DbType.DateTime,
                        Value = (object)(Convert.ToDateTime(conversion.event_date)) ?? DBNull.Value
                    };

                    DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_Deal_ImportDeal", paraDealNo, paraTradeDate, paraValueDate, paraClientSoldAmt, paraClientSoldCCY, paraClientSoldGBP, paraClientBoughtAmt, paraClientBoughtCCY, paraTradeType, paraTStatus, paraCCAccount_id, paraGrossCommCurrency, paraGrossCommGBP, paraGrossComm, paraCreatedDate, paraUpdatedDate, paraUserId, paraStatus, paraDealSource, paraMarketConversionRate, paraGBPConversationRate, paraProfitOrLoss, paraEventDate);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds.Tables[0].Rows[0].Field<int>(0);
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    return 0;
                }
                finally
                {
                    con.Close();
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public void ImportGCPartnerDealJSON(DealJSONData obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(decryptedConnectionString);
                try
                {
                    //SqlCommand com = new SqlCommand("usp_ImportDealOriginalJson", con);
                    //com.CommandType = CommandType.StoredProcedure;
                    //com.Parameters.Add(new SqlParameter("@DealId", (object)(obj.DealId) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@JsonStr", (object)(obj.DealJsonStr) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@CreatedBy", (object)(obj.UserId) ?? DBNull.Value));
                    //com.Connection = con;
                    //con.Open();
                    //com.ExecuteNonQuery();
                    var paraDealId = new SqlParameter
                    {
                        ParameterName = "DealId",
                        DbType = DbType.Int64,
                        Value = (object)(obj.DealId) ?? DBNull.Value
                    };
                    var paraJsonStr = new SqlParameter
                    {
                        ParameterName = "JsonStr",
                        DbType = DbType.String,
                        Value = (object)(obj.DealJsonStr) ?? DBNull.Value
                    };
                    var paraCreatedBy = new SqlParameter
                    {
                        ParameterName = "CreatedBy",
                        DbType = DbType.Int64,
                        Value = (object)(obj.UserId) ?? DBNull.Value
                    };
                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "usp_ImportDealOriginalJson", paraDealId, paraJsonStr, paraCreatedBy);
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    con.Close();
                }
            }
            catch (Exception e)
            {

            }
        }

        public async Task ImportDeal()
        {
            int ImportDealCount = 0;
            int ExistDealCount = 0;

            int ImportClientCount = 0;
            int ExistClientCount = 0;

            string short_reference = "";

            string importDealTimer = System.Configuration.ConfigurationManager.AppSettings["CCImportDealTime"];
            double time = Convert.ToDouble(importDealTimer) * -1;
            DateTime dtFromDate = DateTime.Now.AddMinutes(time);
            DateTime dtToDate = DateTime.Now;

            string FromDate = dtFromDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string ToDate = dtToDate.ToString("yyyy-MM-ddTHH:mm:ssZ");

            //DateTime FromDate1;
            //DateTime ToDate1;
            //string format = "yyyy-MM-dd";

            //if (DateTime.TryParseExact(FromDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate1))
            //{

            //}
            //if (DateTime.TryParseExact(ToDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out ToDate1))
            //{

            //}

            try
            {
                //if (ImportBy.ToLower().Contains("auto"))
                //{
                //    //string path = Server.MapPath("~/ImportDealLog.txt");

                //    //if (!System.IO.File.Exists(path))
                //    //{
                //    //    System.IO.File.Create(path).Dispose();
                //    //}
                //    //System.IO.File.AppendAllText(Server.MapPath("~/ImportDealLog.txt"), "---------------------" + Environment.NewLine);
                //    //System.IO.File.AppendAllText(Server.MapPath("~/ImportDealLog.txt"), "FromDate: " + FromDate + Environment.NewLine);
                //    //System.IO.File.AppendAllText(Server.MapPath("~/ImportDealLog.txt"), "ToDate: " + ToDate + Environment.NewLine);
                //    //System.IO.File.AppendAllText(Server.MapPath("~/ImportDealLog.txt"), "RequestDate: " + DateTime.Now.ToString() + Environment.NewLine);
                //    //System.IO.File.AppendAllText(Server.MapPath("~/ImportDealLog.txt"), "---------------------" + Environment.NewLine);
                //}

                //GC Partner Client and Deal Import
                //var loggedUserId = ProjectSession.LoginUserDetails.UserId;
                //var GCPartnerDealCount = await GCPartnerImportClient(FromDate, ToDate, ImportBy, loggedUserId); //GC Partner API

                //ImportDealCount = ImportDealCount + GCPartnerDealCount;
                //Generate Token
                await GetCurrencyCloudToken();

                exchangeRates.Clear();

                var resultTransactionAPI = new CurrencyCloudConversionModel();
                var listContacts = GetCC_Clientlist();

                for (int page = 1; page < 100000000; page++)
                {

                    //string uriTransaction = "conversions/find?short_reference=20190802-RCKZCP&scope=all";
                    //string uriTransaction = "conversions/find?short_reference=20190729-PKTHKP&scope=all";
                    //string uriTransaction = "conversions/find?short_reference=20190722-RGCHVR&scope=all";
                    //string uriTransaction = "conversions/find?page=" + page + "&conversion_date_from=" + FromDate + "&conversion_date_to=" + ToDate + "&scope=all";
                    string uriTransaction = "conversions/find?page=" + page + "&created_at_from=" + FromDate + "&created_at_to=" + ToDate + "&scope=all";
                    var resultTransactions = await WebApiHelper.HttpClientRequestResponse(resultTransactionAPI, uriTransaction, CurrencyCloudToken);

                    if (resultTransactions == null)
                    {
                        System.Threading.Thread.Sleep(5000);
                        resultTransactions = await WebApiHelper.HttpClientRequestResponse(resultTransactionAPI, uriTransaction, CurrencyCloudToken);
                    }

                    if (resultTransactions != null)
                    {
                        foreach (var conversion in resultTransactions.conversions)
                        {
                            var originalDealResponse = conversion;
                            short_reference = conversion.short_reference;
                            //var resultContactAPI = new CurrencyCloudContactModel();
                            //string uricontact = "contacts/find?account_id=" + conversion.account_id;

                            var singlecontact = listContacts.Where(t => t.CCAccount_id == conversion.account_id).FirstOrDefault();
                            if (singlecontact == null)
                            {
                                var resultContactsAPI = new CurrencyCloudContactModel();
                                string uriContact = "contacts/find?account_id=" + conversion.account_id;

                                var resultContacts = await WebApiHelper.HttpClientRequestResponse(resultContactsAPI, uriContact, CurrencyCloudToken);

                                if (resultContacts != null)
                                {
                                    foreach (var contacts in resultContacts.contacts)
                                    {
                                        var resultAccountAPI = new AccountModel();
                                        string uriAccountClient = "accounts/" + Convert.ToString(contacts.account_id);

                                        var account = await WebApiHelper.HttpClientRequestResponse(resultAccountAPI, uriAccountClient, CurrencyCloudToken);

                                        if (account != null)
                                        {
                                            try
                                            {
                                                var resultImportClient = ImportClient(contacts, account, "CurrencyCloud");

                                                if (resultImportClient > 0)
                                                {
                                                    ClientMasterModel contactModel = new ClientMasterModel();

                                                    contactModel.ClientId = resultImportClient;
                                                    contactModel.FullName = contacts.first_name + ' ' + contacts.last_name;
                                                    contactModel.CCAccount_id = contacts.account_id;
                                                    contactModel.CompanyName = contacts.account_name;
                                                    contactModel.EmailAddress = contacts.email_address;

                                                    listContacts.Add(contactModel);

                                                    ImportClientCount = ImportClientCount + 1;
                                                }
                                                else if (resultImportClient == -2)
                                                {
                                                    ExistClientCount = ExistClientCount + 1;
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                    }
                                }
                            }

                            string TradeType = string.Empty;
                            string ConverstatinStatus = Convert.ToString(conversion.status);

                            if (ConverstatinStatus == "awaiting_funds" || ConverstatinStatus == "funds_sent")
                                TradeType = "Traded";
                            if (ConverstatinStatus == "funds_arrived" || ConverstatinStatus == "trade_settled")
                                TradeType = "Settled";

                            // ConverstatinStatus == "closed" Delete deal

                            decimal clientsellamount = 0; decimal partnersellamount = 0; decimal clientbuyamount = 0; decimal partnerbuyamount = 0;

                            if (!string.IsNullOrEmpty(conversion.client_sell_amount))
                                clientsellamount = Convert.ToDecimal(conversion.client_sell_amount);

                            if (!string.IsNullOrEmpty(conversion.partner_sell_amount) && conversion.partner_sell_amount != "0.00")
                            {
                                partnersellamount = Convert.ToDecimal(conversion.partner_sell_amount);
                            }
                            else
                            {
                                if (conversion.fixed_side == "sell")
                                    partnersellamount = Convert.ToDecimal(conversion.client_sell_amount);
                                else
                                    partnersellamount = Convert.ToDecimal(conversion.client_buy_amount) * Convert.ToDecimal(conversion.mid_market_rate);
                            }

                            if (clientsellamount != partnersellamount)
                            {
                                if ((clientsellamount - partnersellamount) < 0)
                                    conversion.GrossComm = 0;
                                else
                                    conversion.GrossComm = clientsellamount - partnersellamount;

                                conversion.GrossCommCurrency = conversion.sell_currency;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(conversion.client_buy_amount))
                                    clientbuyamount = Convert.ToDecimal(conversion.client_buy_amount);

                                if (!string.IsNullOrEmpty(conversion.partner_buy_amount) && conversion.partner_buy_amount != "0.00")
                                    partnerbuyamount = Convert.ToDecimal(conversion.partner_buy_amount);
                                else
                                {
                                    if (conversion.fixed_side == "buy")
                                        partnerbuyamount = Convert.ToDecimal(conversion.client_buy_amount);
                                    else
                                        partnerbuyamount = Convert.ToDecimal(conversion.client_sell_amount) * Convert.ToDecimal(conversion.mid_market_rate);
                                }

                                //                                conversion.GrossComm = partnerbuyamount - clientbuyamount;
                                if ((partnerbuyamount - clientbuyamount) < 0)
                                    conversion.GrossComm = 0;
                                else
                                    conversion.GrossComm = partnerbuyamount - clientbuyamount;


                                conversion.GrossCommCurrency = conversion.buy_currency;
                            }

                            //Gross Comm GBP
                            if (conversion.GrossCommCurrency.ToUpper() == "GBP")
                                conversion.GrossCommGBP = conversion.GrossComm;
                            else if (conversion.buy_currency == "GBP") //This means the profit currency is sell currency
                            {
                                conversion.GrossCommGBP = conversion.GrossComm * Convert.ToDecimal(conversion.mid_market_rate); // TODO check - may be we will have to do 1/MarketRate
                            }
                            else if (conversion.sell_currency == "GBP") //This means the profit currency is buy currency
                            {
                                conversion.GrossCommGBP = conversion.GrossComm * (1 / Convert.ToDecimal(conversion.mid_market_rate)); // TODO check - may be we will have to do MarketRate
                            }
                            else
                            {
                                //Get GBP Rate via Currency Cloud API
                                var resultRate = await GetRates(conversion.GrossCommCurrency, "GBP");
                                if (resultRate > 0)
                                    conversion.GrossCommGBP = conversion.GrossComm * resultRate;
                            }

                            int resultImportDeal = 0;
                            if (conversion.sell_currency != "GBP")
                            {
                                var resultRate = await GetRates(conversion.sell_currency, "GBP");
                                var Rate = Math.Round(resultRate, 4);
                                if (resultRate > 0)
                                {
                                    var x = Rate * Convert.ToDecimal(conversion.client_sell_amount);
                                    conversion.client_sell_amount_In_GBP = Convert.ToString(x);
                                    conversion.GBPConversationRate = Rate.ToString();
                                }

                                //conversion.GBPConversationRate = resultRate.ToString();

                                // var x = Convert.ToDecimal(conversion.mid_market_rate) * Convert.ToDecimal(conversion.client_sell_amount);
                                //conversion.client_sell_amount_In_GBP = (Convert.ToDecimal(conversion.mid_market_rate) * Convert.ToInt64(conversion.client_sell_amount)).ToString();

                            }
                            else
                            {
                                conversion.client_sell_amount_In_GBP = conversion.client_sell_amount;
                            }
                            var resultProfitLossAPI = new CurrencyCloudProfitLossModel();
                            string uriProfitLoss = "conversions/profit_and_loss?account_id=" + conversion.account_id + "&conversion_id=" + conversion.id;

                            var resultProfitLoss = await WebApiHelper.HttpClientRequestResponse(resultProfitLossAPI, uriProfitLoss, ProjectSession.CurrencyCloudToken);
                            var ProfitLoss = resultProfitLoss.conversion_profit_and_losses;
                            if (ProfitLoss.Count > 0)
                            {
                                foreach (var profit_loss in resultProfitLoss.conversion_profit_and_losses)
                                {

                                    if (profit_loss.event_type == "back_office_drawdown")
                                    {
                                        conversion.amount_profit_loss = (profit_loss.amount == null || profit_loss.amount == "") ? 0 : Convert.ToDecimal(profit_loss.amount);
                                        conversion.event_date = (profit_loss.event_date_time == null || profit_loss.ToString() == "01/01/0001 00:00:00") ? DateTime.Now : profit_loss.event_date_time;
                                    }
                                }
                            }
                            else
                            {
                                conversion.amount_profit_loss = 0;
                                conversion.event_date = DateTime.Now;
                            }

                            //conversion.amount_profit_loss = Convert.ToDecimal(resultProfitLoss.amount);
                            //conversion.event_date = resultProfitLoss.event_date_time == null ? DateTime.Now : resultProfitLoss.event_date_time;
                            try
                            {
                                resultImportDeal = ImportDeal(conversion, TradeType, "CurrencyCloud");
                                if (resultImportDeal > 0)
                                {
                                    var dealjsonModel = new DealJSONData();
                                    dealjsonModel.DealId = resultImportDeal;
                                    dealjsonModel.DealJsonStr = JsonConvert.SerializeObject(originalDealResponse);
                                    dealjsonModel.UserId = 0;
                                    ImportGCPartnerDealJSON(dealjsonModel);
                                    resultImportDeal = 1;
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                            switch (resultImportDeal)
                            {
                                case 0:
                                    break;
                                case 1:
                                    ImportDealCount = ImportDealCount + 1;
                                    break;
                                case -2:
                                    ExistDealCount = ExistDealCount + 1;
                                    break;
                            }
                        }

                        if (resultTransactions.pagination.next_page == -1)
                            break;

                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task GetCurrencyCloudToken()
        {
            var resultAPI = new AuthTokenModel();

            Dictionary<string, string> mydict = new Dictionary<string, string>();

            mydict.Add("login_id", ConfigItems.CurrencyCloudAPIEmail);
            mydict.Add("api_key", ConfigItems.CurrencyCloudAPIKey);

            const string uri = "authenticate/api";
            var result = await WebApiHelper.HttpClientPostCurrencyCloudToken(resultAPI, uri, mydict);

            if (result != null)
            {
                CurrencyCloudToken = result.auth_token;
            }
        }

        public async Task<decimal> GetRates(string fromCurrency, string toCurrency)
        {
            //await CommonController.GetCurrencyCloudToken();

            string pair = fromCurrency + toCurrency;
            if (exchangeRates.ContainsKey(pair))
                return exchangeRates[pair];

            var resultTransactionAPI = new Rootobject();
            string uriRates = "rates/find?currency_pair=" + pair;

            var resultTransactions = await WebApiHelper.HttpClientRequestResponseString(uriRates, CurrencyCloudToken);

            if (resultTransactions != null)
            {
                dynamic data = JObject.Parse(resultTransactions.ToString());
                var Value1 = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)data).First).First).First).First).First).Value;
                var Value2 = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)data).First).First).First).First).Last).Value;

                exchangeRates.Add(pair, (Convert.ToDecimal(Value1) + Convert.ToDecimal(Value2)) / 2);

                return (Convert.ToDecimal(Value1) + Convert.ToDecimal(Value2)) / 2;
            }
            return 0;
        }

        public class Rootobject
        {
            public Rates rates { get; set; }
            public object[] unavailable { get; set; }
        }

        public class Rates
        {
            public dynamic[] USDGBP { get; set; }
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public async Task GetRingCentralToken()
        {
            var resultAPI = new RingCentralTokenModel();
            const string uri = "oauth/token";
            Dictionary<string, string> mydict = new Dictionary<string, string>
            {
                { "username", ConfigItems.RingCentralUserName },
                { "password", ConfigItems.RingCentralPassword },
                //{ "extension", ConfigItems.RingCentralExtension },
                { "grant_type", ConfigItems.RingCentralGrantType }
            };

            var result = await WebApiHelper.HttpClientPostRingCentralToken(resultAPI, uri, mydict);

            if (result != null)
            {
                RingCentralToken = result.access_token;
            }
        }

        public async Task GetRingCentralCallLog(bool everyminute, int time)
        {
            string dtFrom = "2019-01-01";
            //string dtTo = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string dtTo = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd");
            if (everyminute)
            {
                //DateTime from = DateTime.Now.AddMinutes((5 + time) * -1);
               
                DateTime from = DateTime.UtcNow.AddMinutes((5 + time) * -1);
                dtFrom = from.ToString("yyyy-MM-ddTHH:mm:ssZ");
             

                //dtTo = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");

                dtTo = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                
            }

            await GetRingCentralToken();

            var resultAPI = new RingCentralResponceModel();
            if (!everyminute)
            {
                var data = GetRingCentralDataList();
                if (data != null)
                {
                    if (data.Count > 0)
                    {
                        //dtFrom = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");
                        dtFrom = DateTime.UtcNow.AddDays(-2).ToString("yyyy-MM-dd");
                    }
                }
            }

            string token = "Bearer " + RingCentralToken;

            for (int i = 1; i < 1000000000; i++)
            {
                string uri = "v1.0/account/~/call-log?view=Detailed&dateFrom=" + dtFrom + "&dateTo=" + dtTo + "&page=" + i + "&perPage=10000";
                var result = await WebApiHelper.HttpClientRequestResponseRingcentral(resultAPI, uri, token);
                if (result == null)
                {
                    System.Threading.Thread.Sleep(5000);
                    result = await WebApiHelper.HttpClientRequestResponseRingcentral(resultAPI, uri, token);
                }
                if (result != null)
                {
                    if (result.records.Count > 0)
                    {
                        foreach (var item in result.records)
                        {
                            ImportRingCentralCallLog(item);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }


        public async Task GetRingCentralCallLogCustom(bool everyminute, int time, DateTime fromDate, DateTime toDate)
        {
            string dtFrom = "2019-01-01";
            //string dtTo = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string dtTo = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd");
            if (everyminute)
            {
                //DateTime from = DateTime.Now.AddMinutes((5 + time) * -1);

                DateTime from = DateTime.UtcNow.AddMinutes((5 + time) * -1);
                //dtFrom = from.ToString("yyyy-MM-ddTHH:mm:ssZ");
                dtFrom = fromDate.ToString("yyyy-MM-ddTHH:mm:ssZ"); //changes

                //dtTo = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");

                //dtTo = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                dtTo = toDate.ToString("yyyy-MM-ddTHH:mm:ssZ");  //changes
            }

            await GetRingCentralToken();

            var resultAPI = new RingCentralResponceModel();
            if (!everyminute)
            {
                var data = GetRingCentralDataList();
                if (data != null)
                {
                    if (data.Count > 0)
                    {
                        //dtFrom = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");
                        dtFrom = DateTime.UtcNow.AddDays(-2).ToString("yyyy-MM-dd");
                    }
                } 
            }

            string token = "Bearer " + RingCentralToken;

            for (int i = 1; i < 1000000000; i++)
            {
                string uri = "v1.0/account/~/call-log?view=Detailed&dateFrom=" + dtFrom + "&dateTo=" + dtTo + "&page=" + i + "&perPage=10000";
                var result = await WebApiHelper.HttpClientRequestResponseRingcentral(resultAPI, uri, token);
                if (result == null)
                {
                    System.Threading.Thread.Sleep(5000);
                    result = await WebApiHelper.HttpClientRequestResponseRingcentral(resultAPI, uri, token);
                }
                if (result != null)
                {
                    if (result.records.Count > 0)
                    {
                        foreach (var item in result.records)
                        {
                            ImportRingCentralCallLog(item);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        public string ImportRingCentralCallLog(RingCentralRecordModel record)
        {
            try
            {
                SqlConnection con = new SqlConnection(decryptedConnectionString);
                try
                {
                    //SqlCommand com = new SqlCommand("usp_RingCentralCallLog_ImportLog", con);
                    //com.CommandType = CommandType.StoredProcedure;
                    //com.Parameters.Add(new SqlParameter("@RingCentralCallId", (object)(record.id) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@RingCentralSessionId", (object)(record.sessionId) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@CallStartTime", (object)(record.startTime) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@CallDuration", (object)(record.duration) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@CallType", (object)(record.type) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@CallDirection", (object)(record.direction) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@CallAction", (object)(record.action) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@CallResult", (object)(record.result) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@CallToName", (object)(record.to.name) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@CallToNumber", (object)(record.to.phoneNumber) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@CallFromName", (object)(record.from.name) ?? DBNull.Value));
                    //com.Parameters.Add(new SqlParameter("@CallFromNumber", (object)(record.from.phoneNumber) ?? DBNull.Value));
                    //com.CommandTimeout = 3600;
                    //SqlDataAdapter da = new SqlDataAdapter(com);
                    //DataSet ds = new DataSet();

                    //con.Open();
                    //da.Fill(ds);

                    var paraRingCentralCallId = new SqlParameter
                    {
                        ParameterName = "RingCentralCallId",
                        DbType = DbType.String,
                        Value = (object)(record.id) ?? DBNull.Value
                    };
                    var paraRingCentralSessionId = new SqlParameter
                    {
                        ParameterName = "RingCentralSessionId",
                        DbType = DbType.Int64,
                        Value = (object)(record.sessionId) ?? DBNull.Value
                    };
                    var paraCallStartTime = new SqlParameter
                    {
                        ParameterName = "CallStartTime",
                        DbType = DbType.DateTime,
                        Value = (object)(record.startTime) ?? DBNull.Value
                    };
                    var paraCallDuration = new SqlParameter
                    {
                        ParameterName = "CallDuration",
                        DbType = DbType.Int64,
                        Value = (object)(record.duration) ?? DBNull.Value
                    };
                    var paraCallType = new SqlParameter
                    {
                        ParameterName = "CallType",
                        DbType = DbType.String,
                        Value = (object)(record.type) ?? DBNull.Value
                    };
                    var paraCallDirection = new SqlParameter
                    {
                        ParameterName = "CallDirection",
                        DbType = DbType.String,
                        Value = (object)(record.direction) ?? DBNull.Value
                    };
                    var paraCallAction = new SqlParameter
                    {
                        ParameterName = "CallAction",
                        DbType = DbType.String,
                        Value = (object)(record.action) ?? DBNull.Value
                    };
                    var paraCallResult = new SqlParameter
                    {
                        ParameterName = "CallResult",
                        DbType = DbType.String,
                        Value = (object)(record.result) ?? DBNull.Value
                    };
                    var paraCallToName = new SqlParameter
                    {
                        ParameterName = "CallToName",
                        DbType = DbType.String,
                        Value = (object)(record.to.name) ?? DBNull.Value
                    };
                    var paraCallToNumber = new SqlParameter
                    {
                        ParameterName = "CallToNumber",
                        DbType = DbType.String,
                        Value = (object)(record.to.phoneNumber) ?? DBNull.Value
                    };
                    var paraCallFromName = new SqlParameter
                    {
                        ParameterName = "CallFromName",
                        DbType = DbType.String,
                        Value = (object)(record.from.name) ?? DBNull.Value
                    };
                    var paraCallFromNumber = new SqlParameter
                    {
                        ParameterName = "CallFromNumber",
                        DbType = DbType.String,
                        Value = (object)(record.from.phoneNumber) ?? DBNull.Value
                    };

                    DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_RingCentralCallLog_ImportLog", paraRingCentralCallId, paraRingCentralSessionId, paraCallStartTime, paraCallDuration, paraCallType, paraCallDirection, paraCallAction, paraCallResult, paraCallToName, paraCallToNumber, paraCallFromName, paraCallFromNumber);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds.Tables[0].Rows[0].Field<string>(0);
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception ex)
                {
                    return "";
                }
                finally
                {
                    con.Close();
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static List<RingCentralModel> GetRingCentralDataList()
        {
            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                //SqlCommand com = new SqlCommand("usp_RingCentralCallLog_GetList", con);
                //com.CommandType = CommandType.StoredProcedure;
                //com.Parameters.Add(new SqlParameter("@PageNumber", (object)(1) ?? DBNull.Value));
                //com.Parameters.Add(new SqlParameter("@PageSize", (object)(10) ?? DBNull.Value));
                //SqlDataAdapter da = new SqlDataAdapter(com);
                //DataSet ds = new DataSet();

                //con.Open();
                //da.Fill(ds);

                var paraPageNumber = new SqlParameter
                {
                    ParameterName = "PageNumber",
                    DbType = DbType.Int64,
                    Value = (object)(1) ?? DBNull.Value
                };
                var paraPageSize = new SqlParameter
                {
                    ParameterName = "PageSize",
                    DbType = DbType.Int64,
                    Value = (object)(10) ?? DBNull.Value
                };
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_RingCentralCallLog_ImportLog", null, null, null, null, null, paraPageNumber, paraPageSize);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    var result = JsonConvert.DeserializeObject<List<RingCentralModel>>(JSONString);
                    return result;
                }
                else
                {
                    return new List<RingCentralModel>();
                }
            }
            catch (Exception ex)
            {
                return new List<RingCentralModel>();
            }
            finally
            {
                con.Close();
            }
        }
    }
}