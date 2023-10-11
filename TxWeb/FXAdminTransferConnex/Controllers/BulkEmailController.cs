using FXAdminTransferConnex.Business.BulkEmail;
using FXAdminTransferConnex.Business.Client;
using FXAdminTransferConnex.Business.Common;
using FXAdminTransferConnex.Business.Prospect;
using FXAdminTransferConnex.Common;
using FXAdminTransferConnex.Entities;
using FXAdminTransferConnexApp.LocalizationResource;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FXAdminTransferConnexApp.Controllers
{
    public class BulkEmailController : BaseAdminController
    {
        #region Initializations

        /// <summary>
        /// The usersService service
        /// </summary>
        private readonly IBulkEmailService _bulkemailService;
        private readonly IProspectService _prospectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientController"/> class.
        /// </summary>
        public BulkEmailController()
        {
            _bulkemailService = EngineContext.Resolve<IBulkEmailService>();
            _prospectService = EngineContext.Resolve<IProspectService>();
        }
        #endregion

        // GET: BulkEmail
        public ActionResult Index()
        {
            ViewBag.IsAwaitingAction = "2";
            ViewBag.IsMarketOrder = "2";

            if (!string.IsNullOrEmpty(Request.QueryString["IsAwaitingAction"]))
                ViewBag.IsAwaitingAction = Request.QueryString["IsAwaitingAction"].ToString();

            if (!string.IsNullOrEmpty(Request.QueryString["IsMarketOrder"]))
                ViewBag.IsMarketOrder = Request.QueryString["IsMarketOrder"].ToString();

            return View();
        }

        /// <summary>
        /// By Mayank
        /// 12 March 2018
        /// Get client list
        /// </summary>
        /// <param name="request"></param>
        /// <param name="FullName"></param>
        /// <param name="CompanyName"></param>
        /// <param name="AccountNo"></param>
        /// <param name="EmailAddress"></param>
        /// <param name="AwaitingAction"></param>
        /// <param name="MarketOrder"></param>
        /// <returns></returns>
        public ActionResult GetClientList([DataSourceRequest]DataSourceRequest request, string FullName, string EmailAddress, string CompanyName, string AccountNo, string AwaitingAction = "2", string MarketOrder = "2", string SearchType = null)
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

            List<ClientMasterModel> clientList = _bulkemailService.GetAllClientlist(pageNo, pageSize, sortColumn, sortDir, FullName, EmailAddress, CompanyName, AccountNo, AwaitingAction, MarketOrder, SearchType);

            if (clientList != null && clientList.Count > 0)
            {
                DataSourceResult obj = new DataSourceResult();
                obj.Data = clientList;
                if (clientList.Count > 0)
                {
                    obj.Total = clientList.FirstOrDefault().TotalCount;
                }

                return Json(obj, JsonRequestBehavior.AllowGet);
            }

            return Json(new List<ClientMasterModel>(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendMail(string[] clientEmail, string subject, string body)
        {
            return Json(null);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SendMail(string[] emaillist, string subject, string body, string Signature = "TransferConnex")
        {
            string emails = string.Join(",", emaillist);
            emails = emails.Replace('/', ',').Trim();

            List<List<string>> list = ListExtensions.ChunkBy(emaillist.ToList(), 100);

            for (int i = 0; i < list.Count; i++)
            {
                emails = string.Join(",", list[i]);
                bool result = _bulkemailService.SendMail(emails, subject, body);
                if (i == list.Count - 1)
                {
                    return Json(result
                            ? new { Message = FXSystemResource.SendEmailSuccess, IsError = Enums.NotifyType.Success }
                            : new { Message = FXSystemResource.SendEmailFailed, IsError = Enums.NotifyType.Error }
                            , JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Message = FXSystemResource.Failed, IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GenerateDrafts(string[] emailTo, string Subject, string emailType)
        {
            List<ServiceFile> serviceFile = new List<ServiceFile>();
            string filePath = "";
            string guid = Guid.NewGuid().ToString();
            string Body = string.Empty;
            string SenderName = ProjectSession.LoginUserDetails.FirstName + " " + ProjectSession.LoginUserDetails.LastName;

            try    /// GENERATING AN EMAIL DRAFT
            {
                if (emailType.ToUpper() == "EML1")
                {
                    string template1Path = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["EMLTemplate1"]);
                    string attachment1Path = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["EMLAttachments1"]);

                    string strbody = CommonService.ReadTextFile(template1Path);
                    strbody = strbody.Replace("{{Sender}}", SenderName);
                    Body = strbody;
                    string[] filePaths = Directory.GetFiles(attachment1Path);

                    if (filePaths != null)
                    {
                        serviceFile = new List<ServiceFile>();
                        foreach (string item in filePaths)
                        {
                            ServiceFile model = new ServiceFile
                            {
                                FileName = Path.GetFileName(item),
                                FileBytes = System.IO.File.ReadAllBytes(item)
                            };

                            serviceFile.Add(model);
                        }
                    }
                }
                else
                {
                    if (emailType.ToUpper() == "EML2")
                    {
                        string template2Path = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["EMLTemplate2"]);
                        string attachment2Path = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["EMLAttachments2"]);

                        string strbody = CommonService.ReadTextFile(template2Path);
                        strbody = strbody.Replace("{{Sender}}", SenderName);
                        Body = strbody;
                        string[] filePaths = Directory.GetFiles(attachment2Path);

                        if (filePaths != null)
                        {
                            serviceFile = new List<ServiceFile>();
                            foreach (string item in filePaths)
                            {
                                ServiceFile model = new ServiceFile
                                {
                                    FileName = Path.GetFileName(item),
                                    FileBytes = System.IO.File.ReadAllBytes(item)
                                };

                                serviceFile.Add(model);
                            }
                        }
                    }
                    else
                    {
                        if (emailType.ToUpper() == "NEWBLANK")
                        {
                            string blankTemplatePath = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["EMLNewBlank"]);
                            string strbody = CommonService.ReadTextFile(blankTemplatePath);
                            strbody = strbody.Replace("{{Sender}}", SenderName);
                            Body = strbody;
                        }
                    }
                }

                /// Default emails in case they are empty
                MailAddress defaultFrom = new MailAddress(ProjectSession.LoginUserDetails.Email);
                MailMessage mailMessage = new MailMessage();

                mailMessage.From = defaultFrom;
                if (emailTo != null)
                {
                    foreach (string item in emailTo)
                    {
                        mailMessage.To.Add(item);
                    }
                }

                mailMessage.Subject = Subject;
                mailMessage.Body = Body;
                mailMessage.IsBodyHtml = true;
                /// Mark Email as a DRAFT
                mailMessage.Headers.Add("X-Unsent", "1");
                /// ATTACHMENTS FOR EMAIL
                if (serviceFile != null)
                {
                    foreach (ServiceFile item in serviceFile)
                    {
                        if (item.FileBytes != null)
                        {
                            MemoryStream attachmentStream = new MemoryStream(item.FileBytes);
                            mailMessage.Attachments.Add(new System.Net.Mail.Attachment(attachmentStream, item.FileName));
                        }
                    }
                }

                // SETTING UP FILE LOCATION
                /// Creating folder to store email draft
                string directoryPath = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["EMLFilePath"]);
                filePath = directoryPath;
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string[] files = Directory.GetFiles(directoryPath);

                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);

                    if (fi.LastAccessTime < DateTime.Now.AddDays(-1))
                    {
                        fi.Delete();
                    }
                }

                using (SmtpClient client = new SmtpClient())
                {
                    client.UseDefaultCredentials = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    client.PickupDirectoryLocation = directoryPath;
                    client.Send(mailMessage);
                }
            }
            catch (System.Exception ex)
            {
                return Json(new { Message = ex.Message, Success = false }, JsonRequestBehavior.AllowGet);
            }

            /// Copy file and rename v2
            FileInfo emailFileNew = new DirectoryInfo(filePath).GetFiles().OrderByDescending(f => f.LastWriteTime).First();
            string emailFileCopy = Path.Combine(filePath, guid + ".eml");
            try
            {
                System.IO.File.Move(emailFileNew.FullName, emailFileCopy);
            }
            catch (System.IO.IOException)
            {
                System.IO.File.Delete(emailFileCopy);
                System.IO.File.Move(emailFileNew.FullName, emailFileCopy);
            }

            string emlDirectory = System.Configuration.ConfigurationManager.AppSettings["EMLFilePath"];
            emlDirectory = emlDirectory.Substring(1);
            return Json(new { Message = emlDirectory + guid + ".eml", Success = true }, JsonRequestBehavior.AllowGet);
        }

        private bool IsValidNetMailEmail(string email)
        {
            try
            {
                MailAddress netMailAddress = new MailAddress(email);
                return netMailAddress.Address == email;
            }
            catch { return false; }
        }

        private string SendOutLookAppointment(string schLocation, string schSubject, string schDescription, DateTime schBeginDate, DateTime schEndDate)
        {
            //INITIALIZING MEETING DETAILS

            schLocation = "Conference Room";
            schSubject = "Business visit discussion";
            schDescription = "Schedule description";
            schBeginDate = Convert.ToDateTime("7/9/2020 11:15:00 AM");
            schEndDate = Convert.ToDateTime("7/9/2020 11:30:00 AM");

            //PUTTING THE MEETING DETAILS INTO AN ARRAY OF STRING

            string[] contents = { "BEGIN:VCALENDAR",
                                "PRODID:-//Flo Inc.//FloSoft//EN",
                                "BEGIN:VEVENT",
                                "DTSTART:" + schBeginDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
                                "DTEND:" + schEndDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
                                "LOCATION:" + schLocation,
                                "DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + schDescription,
                                "SUMMARY:" + schSubject, "PRIORITY:3",
                                "END:VEVENT", "END:VCALENDAR" };

            /*THE METHOD 'WriteAllLines' CREATES A FILE IN THE SPECIFIED PATH WITH 
           THE SPECIFIED NAME,WRITES THE ARRAY OF CONTENTS INTO THE FILE AND CLOSES THE
            FILE.SUPPOSE THE FILE ALREADY EXISTS IN THE SPECIFIED LOCATION,THE CONTENTS 
           IN THE FILE ARE OVERWRITTEN*/

            System.IO.File.WriteAllLines(Server.MapPath("Sample.ics"), contents);

            return Server.MapPath("Sample.ics");
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SendOutlookMeeting(string[] emailTo, string Subject, string Body, string Location, DateTime meetingStartTime, DateTime meetingEndTime, long ClientId, long ProspectId)
        {
            try
            {
                if (meetingStartTime > meetingEndTime)
                {
                    return Json(new { Message = "Start Date Time can not be smaller then End Date Time", IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
                }
                string email = System.Configuration.ConfigurationManager.AppSettings["Email"];
                string password = System.Configuration.ConfigurationManager.AppSettings["passsword"];
                int PortNumber = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PortNumber"]);
                string HostName = System.Configuration.ConfigurationManager.AppSettings["HostName"];

                string startTime = TimeZoneInfo.ConvertTimeToUtc(meetingStartTime).ToString("yyyyMMddTHHmmssZ");
                string endTime = TimeZoneInfo.ConvertTimeToUtc(meetingEndTime).ToString("yyyyMMddTHHmmssZ");

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(email);
                if (emailTo != null)
                {
                    foreach (string item in emailTo)
                    {
                        msg.To.Add(new MailAddress(item));
                    }
                }
                msg.IsBodyHtml = true;
                msg.Subject = Subject;
                msg.Body = Body;

                StringBuilder str = new StringBuilder();
                str.AppendLine("BEGIN:VCALENDAR");

                //PRODID: identifier for the product that created the Calendar object
                str.AppendLine("PRODID:-//Transfer Connex//Outlook MIMEDIR//EN");
                str.AppendLine("VERSION:2.0");
                str.AppendLine("METHOD:REQUEST");

                str.AppendLine("BEGIN:VEVENT");

                str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", startTime));//TimeZoneInfo.ConvertTimeToUtc("BeginTime").ToString("yyyyMMddTHHmmssZ")));
                str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
                str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", endTime));//TimeZoneInfo.ConvertTimeToUtc("EndTime").ToString("yyyyMMddTHHmmssZ")));
                str.AppendLine(string.Format("LOCATION: {0}", Location));

                // UID should be unique.
                str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
                str.AppendLine(string.Format("DESCRIPTION:{0}", msg.Body));
                str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", msg.Body));
                str.AppendLine(string.Format("SUMMARY:{0}", msg.Subject));

                str.AppendLine("STATUS:CONFIRMED");
                str.AppendLine("BEGIN:VALARM");
                str.AppendLine("TRIGGER:-PT15M");
                str.AppendLine("ACTION:Accept");
                str.AppendLine("DESCRIPTION:Reminder");
                str.AppendLine("X-MICROSOFT-CDO-BUSYSTATUS:BUSY");
                str.AppendLine("END:VALARM");
                str.AppendLine("END:VEVENT");

                str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", msg.From.Address));

                StringBuilder DisplayName = new StringBuilder();
                StringBuilder Address = new StringBuilder();

                foreach (MailAddress item in msg.To)
                {
                    DisplayName.Append(item.DisplayName).Append(",");
                    Address.Append(item.Address).Append(",");
                }
                str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", DisplayName.ToString().TrimEnd(','), Address.ToString().TrimEnd(',')));

                str.AppendLine("END:VCALENDAR");
                System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("text/calendar");
                ct.Parameters.Add("method", "REQUEST");
                ct.Parameters.Add("name", "meeting.ics");
                AlternateView avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), ct);
                msg.AlternateViews.Add(avCal);

                SmtpClient c = new SmtpClient
                {
                    EnableSsl = true,
                    Host = HostName,
                    Port = PortNumber,
                    Credentials = new System.Net.NetworkCredential(email, password)
                };

                // Send message
                c.Send(msg);
                //Task task = new Task(() => c.Send(msg));
                //task.Start();
                TaskReminderModel obj = new TaskReminderModel
                {
                    AssignToId = ProjectSession.LoginUserDetails.UserId,
                    ClientId = ClientId,
                    ProspectId = ProspectId,
                    Subject = Subject,
                    TaskTypeId = 3,
                    TaskId = 0,
                    TaskReminderDatetime = meetingStartTime,
                    Notes = ""
                };
                _prospectService.SaveTaskReminder(obj);

                return Json(new { Message = "Meeting inviation sent successfully.", IsError = Enums.NotifyType.Success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message, IsError = Enums.NotifyType.Error }, JsonRequestBehavior.AllowGet);
            }
        }
    }

    public class ServiceFile
    {
        public string FileName { get; set; }

        public byte[] FileBytes { get; set; }
    }

    public static class ListExtensions
    {
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}