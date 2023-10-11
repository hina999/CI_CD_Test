using FXAdminTransferConnex.Business.ClientChat;
using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace TransferConnexChatApp.Controllers
{
    public class SignalRHelperController : BaseAdminController
    {
        private readonly IClientChatService _signalrService;
        public SignalRHelperController()
        {
            _signalrService = EngineContext.Resolve<IClientChatService>();
        }
        [HttpPost]
        public ActionResult SaveUserNGetContacts(string SignalRId)
        {
            long result = _signalrService.AddUpdateSignalRUser(SignalRId, ProjectSession.LoginChatUserDetails.UserId);
            //var data = new List<SignalRContactModel>();
            if (result > 0)
            {
                var LoginUserDetails = ProjectSession.LoginChatUserDetails;
                LoginUserDetails.SignalRUserId = result;
                ProjectSession.LoginChatUserDetails = LoginUserDetails;
                //data = _signalrService.GetContactNGroups(ProjectSession.LoginUserDetails.UserId);
            }
            //return Json(new { Message = "", Success = result, Data = data }, JsonRequestBehavior.AllowGet);
            return Json(new { Message = "", Success = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveSignalRGroupNGetContacts(long SignalRGroupId, string GroupName, long[] GroupMembers)
        {
            DataTable dtGroupMember = new DataTable("tt_SignalRUserIds");
            dtGroupMember.Columns.Add("SignalRUserId");
            DataRow dtRow;
            foreach (var item in GroupMembers)
            {
                dtRow = dtGroupMember.NewRow();
                dtRow["SignalRUserId"] = item;
                dtGroupMember.Rows.Add(dtRow);
            }
            //int GroupCount = GroupMembers.Length;
            bool result = _signalrService.AddUpdateGroup(SignalRGroupId, GroupName, ProjectSession.LoginChatUserDetails.UserId, dtGroupMember);
            return Json(new { Message = "", Success = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetContactNGroupsList()
        {
            if (ProjectSession.LoginChatUserDetails != null)
            {
                var data = _signalrService.GetContactNGroups(ProjectSession.LoginChatUserDetails.UserId);
                return Json(new { Message = "", Success = true, Data = data }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Message = "", Success = true, Data = new List<SignalRContactModel>() }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadcontactsListToslack()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SavePrivateConversations()
        {
            bool data = false;
            //bool isEdited = false;
            //string Message = "";
            //long data;
            long MsgTo = Convert.ToInt64(Request.Form["MsgTo"]);
            string Msg = Request.Form["Msg"].Trim();
            long chatId = Convert.ToInt64(Request.Form["ChatId"].Trim());
            string scheme = Request.Url.Scheme.ToString();
            string authority = Request.Url.Authority.ToString();
            string url = scheme + "://" + authority;
            string ChatAttachmentFilePath = ConfigurationManager.AppSettings["ChatAttachmentFilePath"];
            string DataList = "";

            var savePath = System.Web.HttpContext.Current.Server.MapPath(ChatAttachmentFilePath);
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase item = Request.Files[file];
                string filename = Path.GetFileName(item.FileName);
                string newfilename = Guid.NewGuid().ToString() + "_" + Regex.Replace(HttpUtility.UrlDecode(filename), @"\s+", String.Empty);
                item.SaveAs(Path.Combine(savePath, newfilename));
                string AttachmentFilePath = ChatAttachmentFilePath.Substring(1);
                string finalpath = url + AttachmentFilePath + newfilename;
                //string anchortag = "<a href='" + finalpath + "' target='_blank' style='color:#337ab7;'>" + filename + "</a>";
                double byteCount = Request.Files[file].ContentLength / 1024.0;
                string anchortag = "<div>" /*< div style = 'margin-bottom:35px;' >*/
                      + "<p style='margin:0;'>" + filename + "</p>"
                    + "<p style='margin:0;font-size:14px;'>" + byteCount.ToString("n2") + " KB"
                    + "<i class='fa fa-file' aria-hidden='true' style='font-size:14px;'></i>"
                    + "</p>"
                    + "<a class='btn btn-default' title='Download' href='" + finalpath + "' target='_blank' style='position: relative;left: 0px;right: 0;bottom: 0px;width: auto;font-size: 10px;padding: 3px 10px;margin: 5px;border-radius:1em;'>Download</a>"/*position: absolute*/
                    + "</div>";
                //data = _signalrService.SavePrivateConversation(ProjectSession.LoginUserDetails.SignalRUserId, MsgTo, anchortag, chatId);
                //if (data)
                //{
                //    DataList = DataList + anchortag + ",";
                //}
                var result = _signalrService.SavePrivateConversation(ProjectSession.LoginChatUserDetails.SignalRUserId, MsgTo, anchortag, chatId);
                if (result != 0)
                {
                    DataList = DataList + anchortag + ",";
                    data = true;
                    chatId = result;
                }

            }

            if (!string.IsNullOrEmpty(Msg))
            {
                // data = _signalrService.SavePrivateConversation(ProjectSession.LoginUserDetails.SignalRUserId, MsgTo, Msg, chatId);
                var result = _signalrService.SavePrivateConversation(ProjectSession.LoginChatUserDetails.SignalRUserId, MsgTo, Msg, chatId);
                if (result != 0)
                {
                    chatId = result;
                    data = true;
                }
                //chatId = data1;
                DataList = Msg;


            }
            return Json(new { Message = chatId.ToString(), Success = data, Data = DataList.TrimEnd(',') }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveGroupConversations()
        {
            bool data = false;
            long SignalRGroupId = Convert.ToInt64(Request.Form["SignalRGroupId"]);
            string Msg = Request.Form["Msg"].Trim();
            long GroupChatId = Convert.ToInt64(Request.Form["GroupChatId"].Trim());
            string scheme = Request.Url.Scheme.ToString();
            string authority = Request.Url.Authority.ToString();
            string url = scheme + "://" + authority;
            string ChatAttachmentFilePath = ConfigurationManager.AppSettings["ChatAttachmentFilePath"];
            string DataList = "";
            var savePath = System.Web.HttpContext.Current.Server.MapPath(ChatAttachmentFilePath);
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase item = Request.Files[file];
                string filename = Path.GetFileName(item.FileName);
                string newfilename = Guid.NewGuid().ToString() + "_" + Regex.Replace(HttpUtility.UrlDecode(filename), @"\s+", String.Empty);
                item.SaveAs(Path.Combine(savePath, newfilename));
                string AttachmentFilePath = ChatAttachmentFilePath.Substring(1);
                string finalpath = url + AttachmentFilePath + newfilename;
                //string anchortag = "<a href='" + finalpath + "' target='_blank' style='color:#337ab7;'>" + filename + "</a>";
                double byteCount = Request.Files[file].ContentLength / 1024.0;
                string anchortag = "<div>" /*< div style = 'margin-bottom:35px;' >*/
                      + "<p style='margin:0;font-size:14px;'>" + filename + "</p>"
                    + "<p style='margin:0;font-size:14px;'>" + byteCount.ToString("n2") + " KB"
                    + "<i class='fa fa-file' aria-hidden='true' style='font-size:14px;'></i>"
                    + "</p>"
                    + "<a class='btn btn-default' title='Download' href='" + finalpath + "' target='_blank' style='position: relative;left: 0px;right: 0;bottom: 0px;width: auto;font-size: 10px;padding: 3px 10px;margin: 5px;border-radius:1em;'>Download</a>"/*position: absolute*/
                    + "</div>";
                data = _signalrService.SaveGroupConversation(ProjectSession.LoginChatUserDetails.SignalRUserId, SignalRGroupId, anchortag, GroupChatId);
                if (data)
                {
                    DataList = DataList + anchortag + ",";
                }
                //var result = _signalrService.SaveGroupConversation(ProjectSession.LoginUserDetails.SignalRUserId, SignalRGroupId, anchortag, GroupChatId);
                //if (result != 0)
                //{
                //    DataList = DataList + anchortag + ",";
                //    data = true;
                //}

            }

            if (!string.IsNullOrEmpty(Msg))
            {
                data = _signalrService.SaveGroupConversation(ProjectSession.LoginChatUserDetails.SignalRUserId, SignalRGroupId, Msg, GroupChatId);
                // var result = _signalrService.SaveGroupConversation(ProjectSession.LoginUserDetails.SignalRUserId, SignalRGroupId, Msg, GroupChatId);
                //if (result != 0)
                //{
                //    GroupChatId = result;
                //    data = true;
                //}

                DataList = Msg;
            }
            return Json(new { Message = "", Success = data, Data = DataList.TrimEnd(',') }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPrivateConversations(long MsgTo)
        {
            var data = _signalrService.GetPrivateConversation(ProjectSession.LoginChatUserDetails.SignalRUserId, MsgTo);
            return Json(new { Message = "", Success = true, Data = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetGroupConversations(long SignalRGroupId)
        {
            var data = _signalrService.GetGroupConversation(SignalRGroupId, ProjectSession.LoginChatUserDetails.SignalRUserId);
            return Json(new { Message = "", Success = true, Data = data, MySignalRUserId = ProjectSession.LoginChatUserDetails.SignalRUserId }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetSignalRContactNGroups()
        {
            var data = _signalrService.GetContactNGroups(ProjectSession.LoginChatUserDetails.UserId);
            return Json(new { Message = "", Success = true, Data = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetSignalRGroupName(long SignalRGroupId)
        {
            SignalRGroupModel model = new SignalRGroupModel();
            if (SignalRGroupId > 0)
            {
                model = _signalrService.GetSignalRGroupData(SignalRGroupId);
            }
            model.UserList = _signalrService.GetSignalRUsers(SignalRGroupId);
            return PartialView("~/Views/Shared/_AddEditSignalRGroup.cshtml", model);
        }
        [HttpPost]
        public JsonResult DeleteSignalRGroup(long SignalRGroupId)
        {
            bool result = _signalrService.DeleteSignalRGroup(SignalRGroupId);
            return Json(new { Message = "", Success = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ChangeUserOnlineStatus()
        {
            bool result = false;
            if (ProjectSession.LoginChatUserDetails != null)
            {
                result = _signalrService.ChangeUserOnlineStatus(ProjectSession.LoginChatUserDetails.UserId);
            }
            return Json(new { Message = "", Success = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DisplayUserSearchByName(string Name)
        {

            if (Name != null && Name != "")
            {
                var results = _signalrService.DisplayUserSearchByName(ProjectSession.LoginChatUserDetails.UserId, Name);
                var res = _signalrService.DisplayMSG_UserSearchByName(ProjectSession.LoginChatUserDetails.UserId, Name);
                var r = results.Concat(res);
                return Json(new { Message = "", Success = true, Data = r }, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        [HttpPost]
        public JsonResult UpdateUserStatus(long UserStatus)
        {
            if (UserStatus > 0)
            {
                var results = _signalrService.UpdateUserStatus(ProjectSession.LoginChatUserDetails.UserId, UserStatus);
                return Json(new { Message = "", Success = true, Data = results }, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        [HttpPost]
        public JsonResult LoadMessageFromDB(long PersonalChatId)
        {
            if (PersonalChatId > 0)
            {
                var results = _signalrService.LoadMessageFromDB(PersonalChatId);
                return Json(new { Message = "", Success = true, Data = results }, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        [HttpPost]
        public JsonResult RemovePrivateMsg(long PersonalChatId)
        {
            if (PersonalChatId > 0)
            {
                var results = _signalrService.RemovePrivateMsg(PersonalChatId);
                return Json(new { Message = "", Success = true, Data = results }, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        public JsonResult updateMessage(long PersonalChatId, string Message)
        {
            if (PersonalChatId > 0)
            {
                var results = _signalrService.updateMessage(PersonalChatId, Message);
                return Json(new { Message = "", Success = true, Data = results }, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        [HttpPost]
        public JsonResult GetUserStatus()
        {
            long result = _signalrService.GetUserStatus(ProjectSession.LoginChatUserDetails.UserId);
            return Json(new { Message = "", Success = true, UserStatus = result }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult changeStickedUser(long SignalRUserId, bool IsStick)
        {
            if (SignalRUserId > 0)
            {
                var results = _signalrService.changeStickedUser(ProjectSession.LoginChatUserDetails.SignalRUserId, SignalRUserId, IsStick);
                return Json(new { Message = "", Success = true, Data = results }, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        public JsonResult RemoveGroupMsg(long GroupChatId)
        {
            if (GroupChatId > 0)
            {
                var results = _signalrService.RemoveGroupMsg(GroupChatId);
                return Json(new { Message = "", Success = true, Data = results }, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
    }
}