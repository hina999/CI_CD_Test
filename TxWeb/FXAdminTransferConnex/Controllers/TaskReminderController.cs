using FXAdminTransferConnex.Business.TaskReminder;
using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FXAdminTransferConnexApp.Controllers
{
    public class TaskReminderController : BaseAdminController
    {
        #region Initializations

        /// <summary>
        /// The usersService service
        /// </summary>
        private readonly ITaskReminderService _taskReminderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskReminderController"/> class.
        /// </summary>
        public TaskReminderController()
        {
            _taskReminderService = EngineContext.Resolve<ITaskReminderService>();
        }
        #endregion

        #region Task Reminder
        // GET: TaskReminder
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// By Datt
        /// 23/10/2019
        /// Update task Reminder
        /// </summary>
        /// <param name="request"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        /// 
        public ActionResult Update(TaskSchedulerModel obj)
        {
            int result;
            result = _taskReminderService.UpdateTaskReminder(obj);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// By Datt
        /// 23/10/2019
        /// Delete task Reminder
        /// </summary>
        /// <param name="request"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        /// 
        public ActionResult Dismiss(long taskID)
        {
            bool result = _taskReminderService.DismissReminder(taskID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// By Naresh
        /// 23/10/2019
        /// Complete task Reminder
        /// </summary>
        /// <param name="request"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        /// 
        public ActionResult Complete(long taskID)
        {
            bool result = _taskReminderService.CompleteReminder(taskID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// By Datt
        /// 23/10/2019
        /// Get task Reminder List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        /// 
        public JsonResult GetTaskReminderList(long UserId = 0)
        {
            List<TaskSchedulerModel> list = new List<TaskSchedulerModel>();
            if (UserId > 0)
            {
                list = _taskReminderService.GetTaskReminderList(UserId);
            }
            else
            {
                list = _taskReminderService.GetTaskReminderList(ProjectSession.LoginUserDetails.UserId);
            }

            list.ForEach(x =>
            {
                //if (DateTime.Now.Date > x.Start.Date.Date)
                //{
                //    x.ThemeColor = "#e42307";
                //}
                //else
                //{
                //    x.ThemeColor = "green";
                //}
                if (x.ClientId > 0)
                {
                    // x.ThemeColor = "#E42307";
                    switch (x.ClientLeadCategoryId)
                    {
                        case 1:
                            // Lead
                            x.ThemeColor = "#0000FF";
                            break;
                        case 2:
                            // Hot Lead
                            x.ThemeColor = "#FFA500";
                            break;
                        case 3:
                            // Registered Client
                            x.ThemeColor = "#008000";
                            break;
                        case 4:
                            // Need Documents
                            x.ThemeColor = "#800080";
                            break;
                        case 5:
                            // DING
                            x.ThemeColor = "#E42307";
                            break;
                        case 6:
                            //Sales Lead
                            x.ThemeColor = "#fe2469";
                            break;
                        default:
                            // code block
                            x.ThemeColor = "#E42307";
                            break;
                    }
                }
                if (x.ProspectId > 0)
                {
                    switch (x.ProspectLeadCategoryId)
                    {
                        case 1:
                            // Lead
                            x.ThemeColor = "#0000FF";
                            break;
                        case 2:
                            // Hot Lead
                            x.ThemeColor = "#FFA500";
                            break;
                        case 3:
                            // Registered Client
                            x.ThemeColor = "#008000";
                            break;
                        case 4:
                            // Need Documents
                            x.ThemeColor = "#800080";
                            break;
                        case 5:
                            // DING
                            x.ThemeColor = "#E42307";
                            break;
                        case 6:
                            //Sales Lead
                            x.ThemeColor = "#fe2469";
                            break;
                        default:
                            // code block
                            if(x.ClientId > 0)
                            {
                                x.ThemeColor = "#E42307";
                            }
                            if(x.ProspectId > 0)
                            {
                                x.ThemeColor = "#008000";
                            }
                            break;
                    }
                }

            });
            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

       
        #endregion
    }
}