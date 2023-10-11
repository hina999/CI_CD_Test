using FXAdminTransferConnex.Business.Client;
using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FXAdminTransferConnexApp.Controllers
{
    public class ImportClientController : BaseAdminController
    {
        #region Initializations
        /// <summary>
        /// The usersService service
        /// </summary>
        private readonly IClientService _clientService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportClientController"/> class.
        /// </summary>
        public ImportClientController()
        {
            _clientService = EngineContext.Resolve<IClientService>();
        }
        #endregion


        // GET: ImportClient
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// By Mayank
        /// 30 May 2018
        /// Import Client
        /// </summary>
        /// <param name="ImportClient"></param>
        /// <returns></returns>
        public ActionResult UploadClient(HttpPostedFileBase ImportClient)
        {
            string errorMessage = string.Empty;
            int importRecordCount = 0;
            var physicalPath = "";
            int Increment_ColumnNo = -1;

            try
            {
                if (ImportClient != null)
                {
                    var fileName = Path.GetFileName(ImportClient.FileName);
                    physicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName.ToString().Replace(" ", ""));
                    ImportClient.SaveAs(physicalPath);
                    if (System.IO.File.Exists(physicalPath))
                    {
                        #region DataTable
                        DataTable dtClient = new DataTable("ImportClientTable");

                        dtClient.Columns.Add("Trader");
                        dtClient.Columns.Add("SalesPerson");
                        dtClient.Columns.Add("rfx_ref");
                        dtClient.Columns.Add("EmailAddress");
                        dtClient.Columns.Add("FullName");
                        dtClient.Columns.Add("CompanyName");
                        dtClient.Columns.Add("MobileNo");
                        dtClient.Columns.Add("AltMobileNo");
                        dtClient.Columns.Add("ImportDate");
                        dtClient.Columns.Add("ImportFileName");
                        dtClient.Columns.Add("ImportBy");
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
                                    SheetName = Convert.ToString(dtSheet.Rows[0].Field<string>("TABLE_NAME"));
                                }

                                comm.CommandText = "Select *,'" + SheetName.Replace(@"$", string.Empty) + "' as TStatus from [" + SheetName + "]";

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
                        int C_Trader = Increment_ColumnNo;

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_SalesPerson = Increment_ColumnNo;

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_CompanyName = Increment_ColumnNo;

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_rfx_ref = Increment_ColumnNo;

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_EmailAddress = Increment_ColumnNo;

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_FirstName = Increment_ColumnNo;

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_LastName = Increment_ColumnNo;

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_PhoneNumber = Increment_ColumnNo;

                        Increment_ColumnNo = Increment_ColumnNo + 1;
                        int C_OtherPhoneNumber = Increment_ColumnNo;


                        if (dt != null)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                DataRow dtrow = dtClient.NewRow();
                                if (!string.IsNullOrEmpty(dt.Rows[i][C_EmailAddress].ToString()))
                                {

                                    dtrow["Trader"] = dt.Rows[i][C_Trader].ToString();
                                    dtrow["SalesPerson"] = dt.Rows[i][C_SalesPerson].ToString();
                                    dtrow["rfx_ref"] = dt.Rows[i][C_rfx_ref].ToString();
                                    dtrow["EmailAddress"] = dt.Rows[i][C_EmailAddress].ToString();
                                    dtrow["FullName"] = dt.Rows[i][C_FirstName].ToString() + ' ' + dt.Rows[i][C_LastName].ToString();
                                    dtrow["CompanyName"] = dt.Rows[i][C_CompanyName].ToString();
                                    dtrow["MobileNo"] = dt.Rows[i][C_PhoneNumber].ToString();
                                    dtrow["AltMobileNo"] = dt.Rows[i][C_OtherPhoneNumber].ToString();
                                    dtrow["ImportFileName"] = fileName;
                                    dtrow["ImportBy"] = ProjectSession.LoginUserDetails.UserId;

                                    dtClient.Rows.Add(dtrow);
                                }
                            }
                        }

                        if (dtClient != null && dtClient.Rows.Count > 0)
                        {
                            importRecordCount = _clientService.ImportClient(dtClient);

                            importRecordCount = dtClient.Rows.Count;
                            if (importRecordCount <= 0)
                            {
                                errorMessage = "Error Occurs (File Foramat is not Proper)";
                            }
                        }
                        if (dtClient == null || dtClient.Rows.Count <= 0)
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