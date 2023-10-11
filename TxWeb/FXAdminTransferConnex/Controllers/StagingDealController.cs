
using FXAdminTransferConnex.Business.StagingDeal;
using FXAdminTransferConnex.Entities;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
namespace FXAdminTransferConnexApp.Controllers
{
    public class StagingDealController : BaseAdminController
    {
        #region Initializations

        /// <summary>
        /// The usersService service
        /// </summary>
        private readonly IStagingDealService _stagingDealService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StagingDealController"/> class.
        /// </summary>
        public StagingDealController()
        {
            _stagingDealService = EngineContext.Resolve<IStagingDealService>();
        }
        #endregion

        // GET: StagingDeal
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// By Mayank
        /// 15 March 2018
        /// Get staging deal list
        /// </summary>
        /// <param name="request"></param>
        /// <param name="DealNo"></param>
        /// <param name="CompanyName"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public ActionResult GetStagingDealList([DataSourceRequest]DataSourceRequest request, string DealNo, string CompanyName, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            int pageNo = request.Page;
            int pageSize = request.PageSize;
            string sortColumn = string.Empty;
            string sortDir = string.Empty;

            foreach (SortDescriptor sort in request.Sorts)
            {
                sortColumn = sort.Member;

                if (sort.SortDirection == System.ComponentModel.ListSortDirection.Ascending)
                {
                    sortDir = "ASC";
                }
                else
                {
                    sortDir = "DESC";
                }
            }

            List<StagingDealModel> userList = _stagingDealService.GetStagingDeallist(pageNo, pageSize, sortColumn, sortDir, DealNo, CompanyName, FromDate, ToDate);
            if (userList != null && userList.Count > 0)
            {
                DataSourceResult obj = new DataSourceResult();
                obj.Data = userList;
                if (userList.Count > 0)
                {
                    StagingDealModel data = userList.FirstOrDefault();
                    if (data != null)
                    {
                        obj.Total = data.TotalCount;
                    }
                    
                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<StagingDealModel>(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// By Mayank
        /// 15 March 2018
        /// Get Staging Deal details by StagingDealId
        /// </summary>
        /// <param name="StagingDealId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageStagingDeal(long StagingDealId = 0)
        {
            if (StagingDealId == 0)
                return RedirectToAction("Index", "StagingDeal");

            StagingDealModel model = new StagingDealModel();
            StagingDealModel stagingDealModel = _stagingDealService.GetStagingDealDetailsByStagingDealId(StagingDealId);
            if (stagingDealModel != null)
            {
                model = stagingDealModel;
            }
            return View(model);
        }

        /// <summary>
        /// By Mayank
        /// 15 March 2018
        /// Save Staging deal Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ManageStagingDeal(StagingDealModel model)
        {
            int result = _stagingDealService.SaveStagingDeal(model);
            if (result > 0)
            {
                SuccessNotification("Record Saved Successfully");
            }
            else
            {
                ErrorNotification("Error Occur record not saved successfully");
            }

            return RedirectToAction("Index", "StagingDeal");
        }


        /// <summary>
        /// By Mayank
        /// 22 March 2018
        /// Reload Staging deal Details
        /// </summary>
        /// <param name="StagingDealId"></param>
        /// <returns></returns>
        public ActionResult ReloadStagingDeal(long StagingDealId = 0)
        {
            bool result = _stagingDealService.ReloadStagingDeal(StagingDealId);
            if (result)
                SuccessNotification("Record Reload Successfully");
            else
                ErrorNotification("Client is unavailable for reload staging deal. please check client records.");

            return RedirectToAction("ManageStagingDeal", "StagingDeal", new { StagingDealId = StagingDealId });
        }


        /// <summary>
        /// By Mayank
        /// 22 March 2018
        /// Proceed Staging deal Details
        /// </summary>
        /// <param name="StagingDealId"></param>
        /// <returns></returns>
        public ActionResult ProceedStagingDeal(long StagingDealId = 0)
        {
            bool result = _stagingDealService.ProceedStagingDeal(StagingDealId);
            if (result)
                SuccessNotification("Staging Deal Proceed Successfully");
            else
                ErrorNotification("Error Occur record not saved successfully");

            return RedirectToAction("Index", "StagingDeal");
        }

        /// <summary>
        /// By Mayank
        /// 23 March 2018
        /// Discard Staging deal Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult DiscardStagingDeal(long StagingDealId = 0)
        {
            bool result = _stagingDealService.DiscardStagingDeal(StagingDealId);
            if (result)
                SuccessNotification("Staging Deal Discard Successfully");
            else
                ErrorNotification("Error Occur record not discard successfully");

            return RedirectToAction("Index", "StagingDeal");
        }


        /// <summary>
        /// By Mayank
        /// 15 March 2018
        /// Return Import View
        /// </summary>
        /// <param name="ImportStagingDeal"></param>
        /// <returns></returns>
        public ActionResult ImportDealWindow()
        {
            return View("~/Views/StagingDeal/_Import.cshtml");
        }

        /// <summary>
        /// By Mayank
        /// 15 March 2018
        /// Import Staging Deal
        /// </summary>
        /// <param name="ImportStagingDeal"></param>
        /// <returns></returns>
        public ActionResult UploadStagingDeal(HttpPostedFileBase ImportStagingDeal)
        {
            string errorMessage = string.Empty;
            int importRecordCount = 0;
            string physicalPath = "";
            int Increment_ColumnNo = -1;

            try
            {
                if (ImportStagingDeal != null)
                {
                    string fileName = Path.GetFileName(ImportStagingDeal.FileName);
                    physicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName.ToString().Replace(" ", ""));
                    ImportStagingDeal.SaveAs(physicalPath);
                    if (System.IO.File.Exists(physicalPath))
                    {
                        #region DataTable
                        DataTable dtStagingDeal = new DataTable("ImportStagingDataCDTable");

                        dtStagingDeal.Columns.Add("DealNo");
                        dtStagingDeal.Columns.Add("ContactName");
                        dtStagingDeal.Columns.Add("CompanyName");
                        dtStagingDeal.Columns.Add("ClientId");
                        dtStagingDeal.Columns.Add("TradeDate");
                        dtStagingDeal.Columns.Add("ValueDate");
                        dtStagingDeal.Columns.Add("ClientSoldAmt");
                        dtStagingDeal.Columns.Add("ClientSoldCCY");
                        dtStagingDeal.Columns.Add("ClientSoldGBP");
                        dtStagingDeal.Columns.Add("ClientBoughtAmt");
                        dtStagingDeal.Columns.Add("ClientBoughtCCY");
                        dtStagingDeal.Columns.Add("GrossCommGBP");
                        dtStagingDeal.Columns.Add("WLTotalCommGBP");
                        dtStagingDeal.Columns.Add("Comms");
                        dtStagingDeal.Columns.Add("TStatus");
                        dtStagingDeal.Columns.Add("ImportDate");
                        dtStagingDeal.Columns.Add("ImportFileName");
                        dtStagingDeal.Columns.Add("ImportBy");
                        #endregion
                        DataTable dt = new DataTable();
                        using (OleDbConnection conn = new OleDbConnection())
                        {
                            //Local string
                            conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + physicalPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                            //Live string
                            //conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + physicalPath + ";Extended Properties=\"Excel 12.0 xml;HDR=Yes;IMEX=2\"";                            
                            using (OleDbCommand comm = new OleDbCommand())
                            {
                                conn.Open();
                                DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string SheetName = string.Empty;
                                string SheetName1 = string.Empty;

                                if (dtSheet != null && dtSheet.Rows.Count > 0)
                                {
                                    SheetName = "Traded$"; //Convert.ToString(dtSheet.Rows[0].Field<string>("TABLE_NAME"));
                                }

                                {
                                    SheetName1 = "Settled$";
                                    //Convert.ToString(dtSheet.Rows[1].Field<string>("TABLE_NAME"));
                                }

                                comm.CommandText = "Select *,'" + SheetName.Replace(@"$", string.Empty) + "' as TStatus from [" + SheetName + "]";
                                if (!string.IsNullOrEmpty(SheetName1))
                                {
                                    comm.CommandText = "Select *,'" + SheetName.Replace(@"$", string.Empty) + "' as TStatus from [" + SheetName + "] UNION ALL Select *,'" + SheetName1.Replace(@"$", string.Empty) + "' as TStatus from [" + SheetName1 + "]";
                                }
                                comm.Connection = conn;
                                using (OleDbDataAdapter da = new OleDbDataAdapter())
                                {
                                    da.SelectCommand = comm;
                                    da.Fill(dt);
                                    conn.Close();
                                }
                            }
                        }

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_TStatus = Increment_ColumnNo;

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_DealNo = Increment_ColumnNo;

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_ClientName = Increment_ColumnNo;

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_TradeDate = Increment_ColumnNo;

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_ValueDate = Increment_ColumnNo;

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_ClientSoldAmt = Increment_ColumnNo;

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_ClientSoldCCY = Increment_ColumnNo;

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_ClientSoldGBP = Increment_ColumnNo;

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_ClientBoughtAmt = Increment_ColumnNo;

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_ClientBoughtCCY = Increment_ColumnNo;

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_GrossCommGBP = Increment_ColumnNo;

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_ContactName = Increment_ColumnNo;

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_WLTotalCommGBP = Increment_ColumnNo;

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_Comms = Increment_ColumnNo;



                        if (dt != null)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                DataRow dtrow = dtStagingDeal.NewRow();
                                if (!string.IsNullOrEmpty(dt.Rows[i][C_DealNo].ToString()))
                                {

                                    dtrow["DealNo"] = dt.Rows[i][C_DealNo].ToString();
                                    dtrow["ContactName"] = dt.Rows[i][C_ContactName].ToString();
                                    dtrow["CompanyName"] = dt.Rows[i][C_ClientName].ToString();
                                    dtrow["ClientId"] = 0;
                                    dtrow["TradeDate"] = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(dt.Rows[i][C_TradeDate].ToString()));
                                    dtrow["ValueDate"] = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(dt.Rows[i][C_ValueDate].ToString()));
                                    //dtrow["ClientSoldAmt"] = Regex.Replace(Convert.ToString(dt.Rows[i][C_ClientSoldAmt]), @"\([0-9]\)", "");
                                    dtrow["ClientSoldAmt"] = Convert.ToString(dt.Rows[i][C_ClientSoldAmt]);
                                    dtrow["ClientSoldCCY"] = dt.Rows[i][C_ClientSoldCCY].ToString();
                                    dtrow["ClientSoldGBP"] = dt.Rows[i][C_ClientSoldGBP].ToString();
                                    dtrow["ClientBoughtAmt"] = Convert.ToString(dt.Rows[i][C_ClientBoughtAmt]);
                                    dtrow["ClientBoughtCCY"] = dt.Rows[i][C_ClientBoughtCCY].ToString();
                                    dtrow["GrossCommGBP"] = Convert.ToString(dt.Rows[i][C_GrossCommGBP]);
                                    dtrow["WLTotalCommGBP"] = Convert.ToString(dt.Rows[i][C_WLTotalCommGBP]);
                                    // For Handle Exponetial Value
                                    dtrow["Comms"] = Convert.ToDouble(Convert.ToString(dt.Rows[i][C_GrossCommGBP])) + Convert.ToDouble(Convert.ToString(dt.Rows[i][C_WLTotalCommGBP]));
                                    dtrow["TStatus"] = dt.Rows[i][C_TStatus].ToString();
                                    dtrow["ImportDate"] = String.Format("{0:MM/dd/yyyy}", System.DateTime.UtcNow);
                                    dtrow["ImportFileName"] = fileName;
                                    dtrow["ImportBy"] = ProjectSession.LoginUserDetails.UserId;

                                    dtStagingDeal.Rows.Add(dtrow);
                                }
                            }
                        }

                        if (dtStagingDeal != null && dtStagingDeal.Rows.Count > 0)
                        {
                            importRecordCount = _stagingDealService.ImportStagingDeal(dtStagingDeal);

                            importRecordCount = dtStagingDeal.Rows.Count;
                            if (importRecordCount <= 0)
                            {
                                errorMessage = "Error Occurs (File Foramat is not Proper)";
                            }
                        }
                        if (dtStagingDeal == null || dtStagingDeal.Rows.Count <= 0)
                        {
                            errorMessage = "Error Occurs (File Foramat is not Proper)";

                        }

                        if (System.IO.File.Exists(physicalPath))
                        {
                            System.IO.File.Delete(physicalPath);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }
            }
            return Json(new { ErrorMessage = errorMessage, ImportRecordCount = importRecordCount }, JsonRequestBehavior.AllowGet);

        }
    }
}