using FXAdminTransferConnex.Business.Client;
using FXAdminTransferConnex.Business.Prospect;
using FXAdminTransferConnex.Common;
using FXAdminTransferConnex.Entities;
using FXAdminTransferConnex.Entities.LocalizationResource;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace FXAdminTransferConnexApp.Controllers
{
    public class ProspectController : BaseAdminController
    {
        private readonly IProspectService _prospectService;
        private readonly IClientService _clientService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ProspectController()
        {
            _prospectService = EngineContext.Resolve<IProspectService>();
            _clientService = EngineContext.Resolve<IClientService>();
        }

        // GET: Prospect
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetProspectList([DataSourceRequest]DataSourceRequest request, string FullName, string CompanyName, string MobileNo, string Email, string boughtCurrency, string soldCurrency, string leadCategory, long? SalesPersonId, long? JuniorSalesPersonId, long? TraderId, string CommunicationDetail,long? SectorCategoryId, long? BusinessCategoryId)
        {
            try
            {
                int pageNo = request.Page;
                int pageSize = request.PageSize;


                if (ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.SalesPerson.GetHashCode())
                {
                    SalesPersonId = ProjectSession.LoginUserDetails.UserId;
                }

                long? loggedinUserId = ProjectSession.LoginUserDetails.UserId;

                int usertype = ProjectSession.LoginUserDetails.UserTypeId;
                long? loggedintraderId = ProjectSession.LoginUserDetails.TraderId;
                List<ProspectModel> result = _prospectService.GetProspectlist(pageNo, pageSize, FullName, CompanyName, MobileNo, Email, SalesPersonId,JuniorSalesPersonId, TraderId, loggedinUserId, usertype, loggedintraderId, boughtCurrency, soldCurrency, leadCategory, CommunicationDetail,SectorCategoryId,BusinessCategoryId);

                if (result != null && result.Count > 0)
                {
                    DataSourceResult obj = new DataSourceResult();
                    obj.Data = result;
                    if (result.Count > 0)
                    {
                        obj.Total = result.FirstOrDefault().TotalCount;
                    }
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
                return Json(new List<ProspectModel>(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                List<ProspectModel> result = new List<ProspectModel>();
                return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
        }

        [ValidateInput(false)]
        public ActionResult SaveProspect(ProspectModel model, FormCollection formdata)
        {
            if (formdata["BoughtCurrencies"] != null)
            {
                model.BoughtCurrencies = formdata["BoughtCurrencies"].ToString();
            }

            if (formdata["SoldCurrencies"] != null)
            {
                model.SoldCurrencies = formdata["SoldCurrencies"].ToString();
            }

            //if (formdata["LeadCategories"] != null)
            //{
            //    model.LeadCategories = formdata["LeadCategories"].ToString();
            //}

            if (ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.SalesPerson.GetHashCode())
            {
                model.SalesPersonId = ProjectSession.LoginUserDetails.UserId;
                model.TraderId = ProjectSession.LoginUserDetails.TraderId;
            }
            if (ProjectSession.LoginUserDetails.UserTypeId == SystemEnum.UserType.Trader.GetHashCode())
            {
                model.TraderId = ProjectSession.LoginUserDetails.TraderId;
            }
            if (model.ProspectId > 0)
            {
                model.UpdatedBy = ProjectSession.LoginUserDetails.UserId;
            }
            else
            {
                model.CreatedBy = ProjectSession.LoginUserDetails.UserId;
            }
            long result = _prospectService.SaveProspect(model);

            switch (result)
            {
                case -2:
                    ErrorNotification("Mobile Number is already exist.");
                    break;
                case -3:
                    ErrorNotification("Email Address is already existt.");
                    break;
                case -4:
                    ErrorNotification("Company Name is already exist.");
                    break;
                case 0:
                    ErrorNotification("Error Occur record not saved successfully");
                    break;
                default:
                    if (model.ProspectId > 0)
                        SuccessNotification("Prospect Updated Successfully");
                    else
                        SuccessNotification("Prospect Saved Successfully");
                    break;
            }
            if (result > 0)
            {
                return RedirectToAction("ManageProspect", new { ProspectId = result });
            }
            else
            {
                if (model.ProspectId <= 0)
                {
                    return View("~/Views/Prospect/ManageProspect.cshtml", model);
                }
                else
                {
                    return RedirectToAction("ManageProspect", new { ProspectId = model.ProspectId });
                }
            }

        }

        public ActionResult DeleteProspect(long prospectId)
        {
            int result = _prospectService.DeleteProspect(prospectId);

            switch (result)
            {
                case 0:
                    return Json(new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
                default:
                    return Json(new { Message = FXAdminTransferConnexResource.DeleteSuccess, IsError = Enums.NotifyType.Success }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ManageProspect(long ProspectId = 0)
        {
            ProspectModel model = new ProspectModel();
            ProspectModel response = _prospectService.GetProspectById(ProspectId);
            if (response != null)
            {
                model = response;
                ViewBag.isViewClient = false;
            }
            return View(model);
        }

        public ActionResult GetTaskReminderList([DataSourceRequest]DataSourceRequest request, long ProspectId = 0)
        {
            List<TaskReminderModel> clientDealList = _prospectService.GetTaskReminderlist(ProspectId);
            return Json(clientDealList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveTaskReminder([DataSourceRequest]DataSourceRequest request, TaskReminderModel obj, string TaskDateTime)
        {
            string format = "dd/MM/yyyy HH:mm";
            DateTime dateTime1;
            if (DateTime.TryParseExact(TaskDateTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime1))
            {
                obj.TaskReminderDatetime = dateTime1;
                int result = _prospectService.SaveTaskReminder(obj);

                switch (result)
                {
                    case -1:
                        return Json(new { Message = "Record already exist", IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
                    case 0:
                        return Json(new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
                    default:
                        return Json(new { Message = FXAdminTransferConnexResource.SaveSuccess, IsError = Enums.NotifyType.Success }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteTaskReminder([DataSourceRequest]DataSourceRequest request, TaskReminderModel obj)
        {
            bool result = _prospectService.DeleteTaskReminder(obj.TaskId);
            return Json(result
                ? new { Message = FXAdminTransferConnexResource.DeleteSuccess, IsError = Enums.NotifyType.Success }
                : new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }
                , JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateCommunicationDetail(long ProspectId, string CommunicationDetail)
        {
            bool result = _prospectService.UpdateCommunicationDetail(ProspectId, CommunicationDetail);
            return Json(result
                ? new { Message = FXAdminTransferConnexResource.StatusSuccess, IsError = Enums.NotifyType.Success }
                : new { Message = FXAdminTransferConnexResource.Failed, IsError = Enums.NotifyType.Error }
                , JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateClientDetail(string CompanyName,long ProspectId)
        {
            try
            {
                List<ClientMasterModel> response = _prospectService.GetClientByCompany(CompanyName);
                if (response.Count > 0)
                {
                    ProspectModel prospect = _prospectService.GetProspectById(ProspectId);
                    foreach (ClientMasterModel item in response)
                    {
                        item.FullName = prospect.FullName;
                        item.CompanyName = prospect.CompanyName;
                        item.MobileNo = prospect.MobileNo;
                        item.EmailAddress = prospect.EmailId;
                        item.AddressLine1 = prospect.ProspectAddress;
                        item.SalesPersonId = prospect.SalesPersonId;
                        item.TraderId = prospect.TraderId;
                        item.PastCommDetail = prospect.CommunicationDetail;
                        item.LeadCategoryId = prospect.LeadCategoryId;
                        item.CategoryId = prospect.BusinessCategoryId;
                        item.SectorId = prospect.BusinessCategorySectorId;
                        item.AltMobileNo = prospect.LandlineNo;
                        item.SoldCurrencies = prospect.SoldCurrencies;
                        item.BoughtCurrencies = prospect.BoughtCurrencies;
                        //item.PastCommDetail = prospect.CommunicationDetail;

                        long result = _clientService.AddClient(item);


                        List<TaskReminderModel> clientDealList_prospect = _prospectService.GetTaskReminderlist(ProspectId);
                        List<TaskReminderModel> clientDealList_client = _clientService.GetTaskReminderlist(item.ClientId);

                       
                            if (clientDealList_client.Count > 0)
                            {
                                if(clientDealList_prospect.Count > 0)
                                {
                                   foreach (TaskReminderModel client in clientDealList_prospect)
                                   {
                                       foreach (TaskReminderModel list_item in clientDealList_client)
                                       {
                                           client.TaskId = list_item.TaskId;
                                           client.ClientId = list_item.ClientId;
                                           client.TaskReminderDatetime = Convert.ToDateTime(client.TaskReminderDatetimeString);
                                       }


                                    int task_reminder = _clientService.SaveTaskReminder(client);
                                   }
                                }
                               else
                               {
                                 foreach (TaskReminderModel client in clientDealList_prospect)
                                 {
                                     client.ClientId = item.ClientId;
                                     client.TaskReminderDatetime = Convert.ToDateTime(client.TaskReminderDatetimeString);
                                    int task_reminder = _clientService.SaveTaskReminder(client);
                                 }
                               }
                                
                            }
                            else
                            {
                               if(clientDealList_prospect.Count > 0)
                               {
                                  foreach (TaskReminderModel client in clientDealList_prospect)
                                  {
                                      client.ClientId = item.ClientId;
                                      client.TaskReminderDatetime = Convert.ToDateTime(client.TaskReminderDatetimeString);
                                    int task_reminder = _clientService.SaveTaskReminder(client);
                                  }
                               }
                                
                            }
                       
                       
                    }
                    

                  


                    
                    return Json(new { Message = "Client updated successfully.", IsError = Enums.NotifyType.Success }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //ErrorNotification("Client does not exists!");
                    return Json(new { Message = "Client does not exists!", IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
                }
                //return RedirectToAction("ManageProspect", new { ProspectId = ProspectId });
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}