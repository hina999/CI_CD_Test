using FXAdminTransferConnex.Common;
using FXAdminTransferConnex.Data.Helper;
using FXAdminTransferConnex.Entities;
using log4net;
using Microsoft.ApplicationBlocks.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FXAdminTransferConnexApp.Common
{
    public class MarketValueNotification
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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
                SqlParameter paraClientId = new SqlParameter
                {
                    ParameterName = "ClientId",
                    DbType = DbType.Int64,
                    Value = 0
                };

                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_MarketOrderSettingList", paraClientId);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);

                    List<MarketOrderSettingModel> result = JsonConvert.DeserializeObject<List<MarketOrderSettingModel>>(JSONString);
                    return result;
                }
                else
                {
                    return new List<MarketOrderSettingModel>();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
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
                List<MarketOrderSettingModel> clientMOList = GetMarketOrderSettingList();
                if (clientMOList != null && clientMOList.Count > 0)
                {
                    string[] Currencies = clientMOList.Select(x => x.FromCurrencyName).Distinct().ToArray();
                    foreach (string Currency in Currencies)
                    {
                        try
                        {
                            GCPartnerMarketOrderRateModel model = new GCPartnerMarketOrderRateModel();

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
                                    MarketOrderSettingModel[] Clients = clientMOList.Where(x => x.FromCurrencyName == Currency).ToArray();

                                    foreach (MarketOrderSettingModel item in Clients)
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
                            _logger.Error(e.Message);
                            throw;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        public static void SaveMarketOrderClientNotification(long clientId, long marketOrderId)
        {
            try
            {
                SqlConnection con = new SqlConnection(decryptedConnectionString);
                try
                {
                    SqlParameter paraClientId = new SqlParameter
                    {
                        ParameterName = "ClientId",
                        DbType = DbType.Int64,
                        Value = clientId
                    };
                    SqlParameter paraMarketOrderId = new SqlParameter
                    {
                        ParameterName = "MarketOrderId",
                        DbType = DbType.Int64,
                        Value = marketOrderId
                    };

                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "usp_MarketValueNotification_Insert", paraClientId,paraMarketOrderId);

                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
            }
        }

        public List<ClientMasterModel> GetCC_Clientlist()
        {
            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_ClientMaster_GetCC_ClientList");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    List<ClientMasterModel> result = JsonConvert.DeserializeObject<List<ClientMasterModel>>(JSONString);
                    return result;
                }
                else
                {
                    return new List<ClientMasterModel>();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
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
                    SqlParameter paraFullName = new SqlParameter
                    {
                        ParameterName = "FullName",
                        DbType = DbType.String,
                        Value = (object)(contact.first_name + ' ' + contact.last_name) ?? DBNull.Value
                    };
                    SqlParameter paraCompanyName = new SqlParameter
                    {
                        ParameterName = "CompanyName",
                        DbType = DbType.String,
                        Value = (object)(contact.account_name) ?? DBNull.Value
                    };
                    SqlParameter paraAddressLine1 = new SqlParameter
                    {
                        ParameterName = "AddressLine1",
                        DbType = DbType.String,
                        Value = (object)(account.street) ?? DBNull.Value
                    };
                    SqlParameter paraAddressLine2 = new SqlParameter
                    {
                        ParameterName = "AddressLine2",
                        DbType = DbType.String,
                        Value = (object)(account.postal_code) ?? DBNull.Value
                    };
                    SqlParameter paraCityTown = new SqlParameter
                    {
                        ParameterName = "City_Town",
                        DbType = DbType.String,
                        Value = (object)(account.city) ?? DBNull.Value
                    };
                    SqlParameter paraCountry = new SqlParameter
                    {
                        ParameterName = "Country",
                        DbType = DbType.String,
                        Value = (object)(account.country) ?? DBNull.Value
                    };
                    SqlParameter paraMobileNo = new SqlParameter
                    {
                        ParameterName = "MobileNo",
                        DbType = DbType.String,
                        Value = (object)(contact.mobile_phone_number) ?? DBNull.Value
                    };
                    SqlParameter paraAltMobileNo = new SqlParameter
                    {
                        ParameterName = "AltMobileNo",
                        DbType = DbType.String,
                        Value = (object)(contact.phone_number) ?? DBNull.Value
                    };
                    SqlParameter paraEmailAddress = new SqlParameter
                    {
                        ParameterName = "EmailAddress",
                        DbType = DbType.String,
                        Value = (object)(contact.email_address) ?? DBNull.Value
                    };
                    SqlParameter paraAccountNo = new SqlParameter
                    {
                        ParameterName = "AccountNo",
                        DbType = DbType.String,
                        Value = (object)(account.short_reference) ?? DBNull.Value
                    };
                    SqlParameter paraCCAccount_id = new SqlParameter
                    {
                        ParameterName = "CCAccount_id",
                        DbType = DbType.String,
                        Value = (object)(contact.account_id) ?? DBNull.Value
                    };
                    SqlParameter paraRegiterDate = new SqlParameter
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
                    SqlParameter paraIsActive = new SqlParameter
                    {
                        ParameterName = "IsActive",
                        DbType = DbType.Int32,
                        Value = IsActive
                    };
                    SqlParameter paraCreatedDate = new SqlParameter
                    {
                        ParameterName = "CreatedDate",
                        DbType = DbType.DateTime,
                        Value = (object)(Convert.ToDateTime(contact.created_at)) ?? DBNull.Value
                    };
                    SqlParameter paraUpdatedDate = new SqlParameter
                    {
                        ParameterName = "UpdatedDate",
                        DbType = DbType.DateTime,
                        Value = (object)(Convert.ToDateTime(contact.updated_at)) ?? DBNull.Value
                    };
                    SqlParameter paraUserId = new SqlParameter
                    {
                        ParameterName = "UserId",
                        DbType = DbType.Int32,
                        Value = 0
                    };
                    SqlParameter paraClientSource = new SqlParameter
                    {
                        ParameterName = "ClientSource",
                        DbType = DbType.String,
                        Value = (object)(ClientSource) ?? DBNull.Value
                    };
                    DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_ClientMaster_ImportClient", paraFullName,paraCompanyName,paraAddressLine1,paraAddressLine2,paraCityTown,paraCountry,paraMobileNo,paraAltMobileNo,paraEmailAddress,paraAccountNo,paraCCAccount_id,paraRegiterDate,paraIsActive,paraCreatedDate,paraUpdatedDate,paraUserId,paraClientSource);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds.Tables[0].Rows[0].Field<long>(0);
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    return 0;
                }
                finally
                {
                    con.Close();
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
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
                    SqlParameter paraDealNo = new SqlParameter
                    {
                        ParameterName = "DealNo",
                        DbType = DbType.String,
                        Value = (object)(conversion.short_reference) ?? DBNull.Value
                    };
                    SqlParameter paraTradeDate = new SqlParameter
                    {
                        ParameterName = "TradeDate",
                        DbType = DbType.DateTime,
                        Value = (object)(Convert.ToDateTime(conversion.created_at)) ?? DBNull.Value
                    };
                    SqlParameter paraValueDate = new SqlParameter
                    {
                        ParameterName = "ValueDate",
                        DbType = DbType.DateTime,
                        Value = (object)(Convert.ToDateTime(conversion.settlement_date)) ?? DBNull.Value
                    };
                    SqlParameter paraClientSoldAmt = new SqlParameter
                    {
                        ParameterName = "ClientSoldAmt",
                        DbType = DbType.String,
                        Value = (object)(conversion.client_sell_amount) ?? DBNull.Value
                    };
                    SqlParameter paraClientSoldCCY = new SqlParameter
                    {
                        ParameterName = "ClientSoldCCY",
                        DbType = DbType.String,
                        Value = (object)(conversion.sell_currency) ?? DBNull.Value
                    };
                    SqlParameter paraClientSoldGBP = new SqlParameter
                    {
                        ParameterName = "ClientSoldGBP",
                        DbType = DbType.String,
                        Value = (object)(conversion.client_sell_amount_In_GBP) ?? DBNull.Value
                    };
                    SqlParameter paraClientBoughtAmt = new SqlParameter
                    {
                        ParameterName = "ClientBoughtAmt",
                        DbType = DbType.String,
                        Value = (object)(conversion.client_buy_amount) ?? DBNull.Value
                    };
                    SqlParameter paraClientBoughtCCY = new SqlParameter
                    {
                        ParameterName = "ClientBoughtCCY",
                        DbType = DbType.String,
                        Value = (object)(conversion.buy_currency) ?? DBNull.Value
                    };
                    SqlParameter paraTradeType = new SqlParameter
                    {
                        ParameterName = "TradeType",
                        DbType = DbType.String,
                        Value = (object)("Spot") ?? DBNull.Value
                    };
                    SqlParameter paraTStatus = new SqlParameter
                    {
                        ParameterName = "TStatus",
                        DbType = DbType.String,
                        Value = (object)(TStatus) ?? DBNull.Value
                    };
                    SqlParameter paraCCAccount_id = new SqlParameter
                    {
                        ParameterName = "CCAccount_id",
                        DbType = DbType.String,
                        Value = (object)(conversion.account_id) ?? DBNull.Value
                    };
                    SqlParameter paraGrossCommCurrency = new SqlParameter
                    {
                        ParameterName = "GrossCommCurrency",
                        DbType = DbType.String,
                        Value = (object)(conversion.GrossCommCurrency) ?? DBNull.Value
                    };
                    SqlParameter paraGrossCommGBP = new SqlParameter
                    {
                        ParameterName = "GrossCommGBP",
                        DbType = DbType.String,
                        Value = (object)(conversion.GrossCommGBP) ?? DBNull.Value
                    };
                    SqlParameter paraGrossComm = new SqlParameter
                    {
                        ParameterName = "GrossComm",
                        DbType = DbType.String,
                        Value = (object)(conversion.GrossComm) ?? DBNull.Value
                    };
                    SqlParameter paraCreatedDate = new SqlParameter
                    {
                        ParameterName = "CreatedDate",
                        DbType = DbType.String,
                        Value = (object)(Convert.ToDateTime(conversion.created_at)) ?? DBNull.Value
                    };
                    SqlParameter paraUpdatedDate = new SqlParameter
                    {
                        ParameterName = "UpdatedDate",
                        DbType = DbType.String,
                        Value = (object)(Convert.ToDateTime(conversion.updated_at)) ?? DBNull.Value
                    };
                    SqlParameter paraUserId = new SqlParameter
                    {
                        ParameterName = "UserId",
                        DbType = DbType.Int32,
                        Value = 0
                    };
                    SqlParameter paraStatus = new SqlParameter
                    {
                        ParameterName = "Status",
                        DbType = DbType.String,
                        Value = (object)conversion.status ?? DBNull.Value
                    };
                    SqlParameter paraDealSource = new SqlParameter
                    {
                        ParameterName = "DealSource",
                        DbType = DbType.String,
                        Value = (object)DealSource ?? DBNull.Value
                    };
                    SqlParameter paraMarketConversionRate = new SqlParameter
                    {
                        ParameterName = "MarketConversionRate",
                        DbType = DbType.Decimal,
                        Value = (object)conversion.mid_market_rate ?? DBNull.Value
                    };
                    SqlParameter paraGBPConversationRate = new SqlParameter
                    {
                        ParameterName = "GBPConversationRate",
                        DbType = DbType.Decimal,
                        Value = (object)conversion.GBPConversationRate ?? DBNull.Value
                    };
                    SqlParameter paraProfitOrLoss = new SqlParameter
                    {
                        ParameterName = "ProfitOrLoss",
                        DbType = DbType.Decimal,
                        Value = (object)(conversion.amount_profit_loss) ?? DBNull.Value
                    };
                    SqlParameter paraEventDate = new SqlParameter
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
                    _logger.Error(ex.Message);
                    return 0;
                }
                finally
                {
                    con.Close();
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
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
                    SqlParameter paraDealId = new SqlParameter
                    {
                        ParameterName = "DealId",
                        DbType = DbType.Int64,
                        Value = (object)(obj.DealId) ?? DBNull.Value
                    };
                    SqlParameter paraJsonStr = new SqlParameter
                    {
                        ParameterName = "JsonStr",
                        DbType = DbType.String,
                        Value = (object)(obj.DealJsonStr) ?? DBNull.Value
                    };
                    SqlParameter paraCreatedBy = new SqlParameter
                    {
                        ParameterName = "CreatedBy",
                        DbType = DbType.Int64,
                        Value = (object)(obj.UserId) ?? DBNull.Value
                    };
                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "usp_ImportDealOriginalJson", paraDealId, paraJsonStr, paraCreatedBy);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
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

            try
            {
               
                //GC Partner Client and Deal Import
                
                //Generate Token
                await GetCurrencyCloudToken();

                exchangeRates.Clear();

                CurrencyCloudConversionModel resultTransactionAPI = new CurrencyCloudConversionModel();
                List<ClientMasterModel> listContacts = GetCC_Clientlist();

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
                        foreach (ConversionModel conversion in resultTransactions.conversions)
                        {
                            ConversionModel originalDealResponse = conversion;
                            short_reference = conversion.short_reference;

                            ClientMasterModel singlecontact = listContacts.Where(t => t.CCAccount_id == conversion.account_id).FirstOrDefault();
                            if (singlecontact == null)
                            {
                                CurrencyCloudContactModel resultContactsAPI = new CurrencyCloudContactModel();
                                string uriContact = "contacts/find?account_id=" + conversion.account_id;

                                var resultContacts = await WebApiHelper.HttpClientRequestResponse(resultContactsAPI, uriContact, CurrencyCloudToken);

                                if (resultContacts != null)
                                {
                                    foreach (ContactModel contacts in resultContacts.contacts)
                                    {
                                        AccountModel resultAccountAPI = new AccountModel();
                                        string uriAccountClient = "accounts/" + Convert.ToString(contacts.account_id);

                                        var account = await WebApiHelper.HttpClientRequestResponse(resultAccountAPI, uriAccountClient, CurrencyCloudToken);

                                        if (account != null)
                                        {
                                            try
                                            {
                                                long resultImportClient = ImportClient(contacts, account, "CurrencyCloud");

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
                                                _logger.Error(ex.Message);
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
                                decimal resultRate = await GetRates(conversion.GrossCommCurrency, "GBP");
                                if (resultRate > 0)
                                    conversion.GrossCommGBP = conversion.GrossComm * resultRate;
                            }

                            int resultImportDeal = 0;
                            if (conversion.sell_currency != "GBP")
                            {
                                decimal resultRate = await GetRates(conversion.sell_currency, "GBP");
                                decimal Rate = Math.Round(resultRate, 4);
                                if (resultRate > 0)
                                {
                                    decimal x = Rate * Convert.ToDecimal(conversion.client_sell_amount);
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
                            CurrencyCloudProfitLossModel resultProfitLossAPI = new CurrencyCloudProfitLossModel();
                            string uriProfitLoss = "conversions/profit_and_loss?account_id=" + conversion.account_id + "&conversion_id=" + conversion.id;

                            var resultProfitLoss = await WebApiHelper.HttpClientRequestResponse(resultProfitLossAPI, uriProfitLoss, ProjectSession.CurrencyCloudToken);
                            List<ProfitLossModel> ProfitLoss = resultProfitLoss.conversion_profit_and_losses;
                            if (ProfitLoss.Count > 0)
                            {
                                foreach (ProfitLossModel profit_loss in resultProfitLoss.conversion_profit_and_losses)
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
                                    DealJSONData dealjsonModel = new DealJSONData();
                                    dealjsonModel.DealId = resultImportDeal;
                                    dealjsonModel.DealJsonStr = JsonConvert.SerializeObject(originalDealResponse);
                                    dealjsonModel.UserId = 0;
                                    ImportGCPartnerDealJSON(dealjsonModel);
                                    resultImportDeal = 1;
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.Error(ex.Message);
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
                _logger.Error(ex.Message);
            }
        }

        public async Task GetCurrencyCloudToken()
        {
            AuthTokenModel resultAPI = new AuthTokenModel();

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

            string uriRates = "rates/find?currency_pair=" + pair;

            var resultTransactions = await WebApiHelper.HttpClientRequestResponseString(uriRates, CurrencyCloudToken);

            if (resultTransactions != null)
            {
                dynamic data = JObject.Parse(resultTransactions.ToString());
                object Value1 = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)data).First).First).First).First).First).Value;
                object Value2 = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)data).First).First).First).First).Last).Value;

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
            RingCentralTokenModel resultAPI = new RingCentralTokenModel();
            const string uri = "oauth/token";
            Dictionary<string, string> mydict = new Dictionary<string, string>
            {
                { "username", ConfigItems.RingCentralUserName },
                { "password", ConfigItems.RingCentralPassword },
                //{ "extension", ConfigItems.RingCentralExtension },
                { "grant_type", ConfigItems.RingCentralGrantType }
            };

            RingCentralTokenModel result = await WebApiHelper.HttpClientPostRingCentralToken(resultAPI, uri, mydict);

            if (result != null)
            {
                RingCentralToken = result.access_token;

                WebApiHelper.WriteCallLogger("GetRingCentralToken -> RingCentralToken: " + RingCentralToken );

            }


        }

        public async Task GetRingCentralCallLog(bool everyminute, int time)
        {

            string path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/RingCentralCallLog_HangFire/"), Path.GetFileName("RingCentralImportCallLog.txt"));

            if (!Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/RingCentralCallLog_HangFire/")))
            {
                Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~/RingCentralCallLog_HangFire/"));
            }

            if (!File.Exists(path))
            {
                FileStream myFile = System.IO.File.Create(path);
                myFile.Close();
            }

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine("=================" + "GetRingCentralCallLog is called from Hangfire" + "->" + DateTime.Now + "=============================");
            }

            string dtFrom = "2019-01-01";
            string dtTo = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd");
            if (everyminute)
            {
                DateTime from = DateTime.UtcNow.AddMinutes((5 + time) * -1);
                dtFrom = from.ToString("yyyy-MM-ddTHH:mm:ssZ");
                dtTo = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                
            }

            await GetRingCentralToken();

            RingCentralResponceModel resultAPI = new RingCentralResponceModel();
            if (!everyminute)
            {
                List<RingCentralModel> data = GetRingCentralDataList();
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
                        foreach (RingCentralRecordModel item in result.records)
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
            try
            {
                string dtFrom = "2019-01-01";
                string dtTo = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd");
                if (everyminute)
                {
                    DateTime from = DateTime.UtcNow.AddMinutes((5 + time) * -1);
                    dtFrom = fromDate.ToString("yyyy-MM-ddTHH:mm:ssZ"); //changes
                    dtTo = toDate.ToString("yyyy-MM-ddTHH:mm:ssZ");  //changes

                    WebApiHelper.WriteCallLogger("GetRingCentralCallLogCustom -> Converted From Date: " + dtFrom + "Converted To Date: " + dtTo);



                }

                await GetRingCentralToken();

                RingCentralResponceModel resultAPI = new RingCentralResponceModel();
                if (!everyminute)
                {
                    List<RingCentralModel> data = GetRingCentralDataList();
                    if (data != null)
                    {
                        if (data.Count > 0)
                        {
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
                        WebApiHelper.WriteCallLogger("GetRingCentralCallLogCustom -> RingCentralCallApi: Response null");

                        System.Threading.Thread.Sleep(5000);
                        result = await WebApiHelper.HttpClientRequestResponseRingcentral(resultAPI, uri, token);
                    }
                    if (result != null)
                    {
                        WebApiHelper.WriteCallLogger("GetRingCentralCallLogCustom -> RingCentralCallApi: Geting response");

                        if (result.records.Count > 0)
                        {
                            foreach (RingCentralRecordModel item in result.records)
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
            catch(Exception ex)
            {
                WebApiHelper.WriteCallLogger("GetRingCentralCallLogCustom -> Exception: " + ex.Message);
            }
            
        }

        public string ImportRingCentralCallLog(RingCentralRecordModel record)
        {
            try
            {
                SqlConnection con = new SqlConnection(decryptedConnectionString);
                try
                {
                    SqlParameter paraRingCentralCallId = new SqlParameter
                    {
                        ParameterName = "RingCentralCallId",
                        DbType = DbType.String,
                        Value = (object)(record.id) ?? DBNull.Value
                    };
                    SqlParameter paraRingCentralSessionId = new SqlParameter
                    {
                        ParameterName = "RingCentralSessionId",
                        DbType = DbType.Int64,
                        Value = (object)(record.sessionId) ?? DBNull.Value
                    };
                    SqlParameter paraCallStartTime = new SqlParameter
                    {
                        ParameterName = "CallStartTime",
                        DbType = DbType.DateTime,
                        Value = (object)(record.startTime) ?? DBNull.Value
                    };
                    SqlParameter paraCallDuration = new SqlParameter
                    {
                        ParameterName = "CallDuration",
                        DbType = DbType.Int64,
                        Value = (object)(record.duration) ?? DBNull.Value
                    };
                    SqlParameter paraCallType = new SqlParameter
                    {
                        ParameterName = "CallType",
                        DbType = DbType.String,
                        Value = (object)(record.type) ?? DBNull.Value
                    };
                    SqlParameter paraCallDirection = new SqlParameter
                    {
                        ParameterName = "CallDirection",
                        DbType = DbType.String,
                        Value = (object)(record.direction) ?? DBNull.Value
                    };
                    SqlParameter paraCallAction = new SqlParameter
                    {
                        ParameterName = "CallAction",
                        DbType = DbType.String,
                        Value = (object)(record.action) ?? DBNull.Value
                    };
                    SqlParameter paraCallResult = new SqlParameter
                    {
                        ParameterName = "CallResult",
                        DbType = DbType.String,
                        Value = (object)(record.result) ?? DBNull.Value
                    };
                    SqlParameter paraCallToName = new SqlParameter
                    {
                        ParameterName = "CallToName",
                        DbType = DbType.String,
                        Value = (object)(record.to.name) ?? DBNull.Value
                    };
                    SqlParameter paraCallToNumber = new SqlParameter
                    {
                        ParameterName = "CallToNumber",
                        DbType = DbType.String,
                        Value = (object)(record.to.phoneNumber) ?? DBNull.Value
                    };
                    SqlParameter paraCallFromName = new SqlParameter
                    {
                        ParameterName = "CallFromName",
                        DbType = DbType.String,
                        Value = (object)(record.from.name) ?? DBNull.Value
                    };
                    SqlParameter paraCallFromNumber = new SqlParameter
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
                    _logger.Error(ex.Message);
                    return "";
                }
                finally
                {
                    con.Close();
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return e.Message;
            }
        }

        public static List<RingCentralModel> GetRingCentralDataList()
        {
            SqlConnection con = new SqlConnection(decryptedConnectionString);
            try
            {
                SqlParameter paraPageNumber = new SqlParameter
                {
                    ParameterName = "PageNumber",
                    DbType = DbType.Int64,
                    Value = (object)(1) ?? DBNull.Value
                };
                SqlParameter paraPageSize = new SqlParameter
                {
                    ParameterName = "PageSize",
                    DbType = DbType.Int64,
                    Value = (object)(10) ?? DBNull.Value
                };
                DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_RingCentralCallLog_ImportLog",null,null,null,null,null,paraPageNumber,paraPageSize);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
                    List<RingCentralModel> result = JsonConvert.DeserializeObject<List<RingCentralModel>>(JSONString);
                    return result;
                }
                else
                {
                    return new List<RingCentralModel>();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new List<RingCentralModel>();
            }
            finally
            {
                con.Close();
            }
        }
    }
}