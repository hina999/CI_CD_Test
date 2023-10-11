using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Business.TaskReminder
{
    public interface ITaskReminderService
    {
       List<TaskReminderReportModel> GetTaskReminderReportList(string Search, long salespersonId);
       List<TaskSchedulerModel> GetTaskReminderList(long assignToId);
       int AddTaskReminder(TaskSchedulerModel obj);
       int UpdateTaskReminder(TaskSchedulerModel obj);
       bool DismissReminder(long TaskId);
       bool CompleteReminder(long TaskId);

    }
}
