using FXAdminTransferConnex.Business.Common;
using FXAdminTransferConnex.Common;
using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace FXAdminTransferConnexApp.Controllers
{
    public class CommonController : Controller
    {
        #region Initializations

        /// <summary>
        /// The usersService service
        /// </summary>
        private readonly ICommonService _commonService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonController"/> class.
        /// </summary>
        public CommonController()
        {
            _commonService = EngineContext.Resolve<ICommonService>();
        }
        #endregion
        // GET: Common
        public JsonResult SetLayOutPageUIToggleFlag()
        {
            try
            {
                ProjectSession.IsMenuSidebarStrip = !ProjectSession.IsMenuSidebarStrip;

                return new JsonResult { Data = true };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex.Message };
            }
        }


        public ActionResult GetUserByRole(int UserTypeId)
        {

            var userList = _commonService.GetUserByUserType(UserTypeId).Select(s => new { Text = s.FirstName, Value = s.UserId }).ToList();
            return Json(userList, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetCategoryList()
        {
            List<CategoryModel> categoryList = _commonService.GetCategoryList().ToList();
            return Json(categoryList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCurrencyList()
        {
            List<CurrencyModel> currencyList = _commonService.GetCurrencyList().ToList();
            return Json(currencyList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMonthList()
        {
            List<MonthModel> MonthList = _commonService.GetMonthList().ToList();
            return Json(MonthList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetYearList()
        {
            List<YearModel> YearList = _commonService.GetYearList().ToList();
            return Json(YearList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTaskTypeList()
        {
            List<TaskTypeModel> List = _commonService.GetTaskTypeList().ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUserList()
        {
            List<UserDDModel> List = _commonService.GetUserList().OrderBy(x => x.FullName).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// By Mayank
        /// 15 March 2018
        /// Gets the list of trade type
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTradeType()
        {
            IEnumerable<SelectListItem> roles = Enum.GetValues(typeof(SystemEnum.TradeType))
                .Cast<SystemEnum.TradeType>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.GetDescription().ToString()
                });
            return new JsonResult
            {
                Data = roles,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /// <summary>
        /// By Mayank
        /// 15 March 2018
        /// Gets the list of Transaction Status
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTransactionStatus()
        {
            IEnumerable<SelectListItem> roles = Enum.GetValues(typeof(SystemEnum.TransactionStatus))
                .Cast<SystemEnum.TransactionStatus>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.GetDescription().ToString()
                });
            return new JsonResult
            {
                Data = roles,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult GetDealTradeStatus()
        {
            var dealList = _commonService.GetDealTradeStatus().Select(s => new { Text = s.TStatus, Value = s.TStatus }).ToList();
            return Json(dealList, JsonRequestBehavior.AllowGet);
        }

        public static async Task GetCurrencyCloudToken()
        {
            AuthTokenModel resultAPI = new AuthTokenModel();

            Dictionary<string, string> mydict = new Dictionary<string, string>();

            mydict.Add("login_id", ConfigItems.CurrencyCloudAPIEmail);
            mydict.Add("api_key", ConfigItems.CurrencyCloudAPIKey);

            const string uri = "authenticate/api";
            AuthTokenModel result = await WebApiHelper.HttpClientPostCurrencyCloudToken(resultAPI, uri, mydict);

            if (result != null)
            {
                ProjectSession.CurrencyCloudToken = result.auth_token;
            }
        }

        public static async Task GetCurrencyCloudTokenLogout()
        {
            AuthTokenModel resultAPI = new AuthTokenModel();

            const string uri = "authenticate/close_session";
            var result = await WebApiHelper.HttpClientPostCurrencyCloudToken(resultAPI, uri);
            ProjectSession.CurrencyCloudToken = null;
        }

        public JsonResult GetSalesPersonByTraderId(int traderId)
        {
            int userTypeId = SystemEnum.UserType.SalesPerson.GetHashCode();
            List<UserModel> userList = _commonService.GetUserByUserType(userTypeId).Where(x => x.TraderId == traderId).ToList();
            var salespesronList = userList.Select(s => new { Text = s.FirstName, Value = s.UserId }).ToList();
            return Json(salespesronList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLeadCategoryList()
        {
            List<LeadCategoryModel> leadCategoryList = _commonService.GetLeadCategoryList().ToList();
            return Json(leadCategoryList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTenYearList()
        {
            List<YearModel> model = new List<YearModel>();
            for (int i = 0; i < 10; i++)
            {
                YearModel yearModel = new YearModel();
                int year = DateTime.Now.Year;
                year = year - i;
                yearModel.Year = year;
                model.Add(yearModel);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PermissionDenied()
        {
            return View("~/Views/Shared/permissiondenied.cshtml");

        }

        public ActionResult GetClientListForDropdown()
        {
            List<ClientMasterModel> leadCategoryList = _commonService.GetClientListForDropdown().ToList();
            JsonResult jsonResult = Json(leadCategoryList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult GetProspectListForDropdown()
        {
            List<ProspectModel> leadCategoryList = _commonService.GetProspectListForDropdown().ToList();
            JsonResult jsonResult = Json(leadCategoryList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult GetCompanyList()
        {
            List<SelectListItem> companyList = _commonService.GetCompanyList().ToList();
            return Json(companyList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTraderList()
        {
            List<SelectListItem> traderList = _commonService.GetTraderList().ToList();
            return Json(traderList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSalesPersonList()
        {
            List<SelectListItem> salesPersonList = _commonService.GetSalesPersonList().ToList();
            return Json(salesPersonList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSalesPersonListByTrader()
        {
            long traderId = 0;
            if (ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.Trader.GetHashCode())
            {
                traderId = ProjectSession.LoginUserDetails.TraderId;
            }
            int salespersonuserTypeId = SystemEnum.UserType.SalesPerson.GetHashCode();
            List<UserModel> userList = _commonService.GetUserByUserType(salespersonuserTypeId).Where(x => x.TraderId == traderId).ToList();
            var salesPersonList = userList.Select(s => new { Text = s.FirstName+" "+s.LastName, Value = s.UserId }).ToList();
            return Json(salesPersonList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSectorList(long CategoryId)
        {
            List<SectorModel> SectorList = _commonService.GetSectorList(CategoryId).ToList();
            return Json(SectorList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBusinessSectorList(int CategoryId)
        {
            List<SectorModel> SectorList = _commonService.GetBusinessSectorList(CategoryId).ToList();
            return Json(SectorList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSalespersonListByTradeId(int UserId)
        {
            List<SelectListItem> Salesperson = _commonService.GetSalespersonListByTradeId(UserId).ToList();
            return Json(Salesperson, JsonRequestBehavior.AllowGet);
        }
    }
}