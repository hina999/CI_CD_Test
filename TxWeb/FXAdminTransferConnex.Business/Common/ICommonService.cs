using FXAdminTransferConnex.Data.Databases;
using FXAdminTransferConnex.Entities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FXAdminTransferConnex.Business.Common
{
    /// <summary>
    /// Interface IUserService
    /// </summary>
    public interface ICommonService
    {
        ///// <summary>
        ///// Prototype for getting email settings
        ///// </summary>
        ///// <returns></returns>
        //EmailNotification.EmailSetting GetEmailSettings();
        List<CommunicatinTypeModel> GetCommunicationTypelist();

        List<UserModel> GetUserByUserType(int UserTypeId);

        List<CurrencyModel> GetCurrencyList();

        List<CategoryModel> GetCategoryList();

        List<DealModel> GetDealTradeStatus();

        List<MonthModel> GetMonthList();

        List<YearModel> GetYearList();

        List<TaskTypeModel> GetTaskTypeList();

        List<UserDDModel> GetUserList();

        List<LeadCategoryModel> GetLeadCategoryList();

        List<ClientMasterModel> GetClientListForDropdown();

        List<ProspectModel> GetProspectListForDropdown();
        List<SelectListItem> GetCompanyList();
        List<SelectListItem> GetTraderList();
        List<SelectListItem> GetSalesPersonList();
        List<SectorModel> GetSectorList(long CategoryId);
        List<SectorModel> GetBusinessSectorList(int CategoryId);
        List<SelectListItem> GetSalespersonListByTradeId(int UserId);  // ------


    }
}
