using FXAdminTransferConnex.Data.Models;
using FXAdminTransferConnex.Data.Repository;
using FXAdminTransferConnex.Entities;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Business.TaskReminder
{
    public class TaskReminderService : ITaskReminderService
    {
        #region Fields

        /// <summary>
        /// Declare The provider repository object for perform operation on Provider repository
        /// </summary>
        private readonly IRepository<UserMaster> _usersRepository;

        #endregion

        #region ctor

        public TaskReminderService(IRepository<UserMaster> providerRepository)
        {
            _usersRepository = providerRepository;
        }

        #endregion

        #region Methods

        public List<TaskReminderReportModel> GetTaskReminderReportList(string Search, long salespersonId)
        {
            SqlParameter ParaSearch = new SqlParameter
            {
                ParameterName = "Search",
                DbType = DbType.String,
                Value = Search
            };

            SqlParameter paramSalesPersonID = new SqlParameter
            {
                ParameterName = "SalesPersonId",
                DbType = DbType.Int64,
                Value = salespersonId
            };

            List<TaskReminderReportModel> result = _usersRepository.ExecuteStoredProcedureList<TaskReminderReportModel>("usp_TaskReminder_GetTaskReminderReportList", ParaSearch, paramSalesPersonID).ToList();
            return result;
        }


        public List<TaskSchedulerModel> GetTaskReminderList(long assignToId)
        {
            SqlParameter paramAssignToID = new SqlParameter
            {
                ParameterName = "AssignToId",
                DbType = DbType.Int64,
                Value = assignToId
            };

            List<TaskSchedulerModel> result = _usersRepository.ExecuteStoredProcedureList<TaskSchedulerModel>("usp_TaskReminder_GetTaskReminderList_Schedular", paramAssignToID).ToList();
            return result;
        }

        public int AddTaskReminder(TaskSchedulerModel obj)
        {
            SqlParameter TaskId = new SqlParameter
            {
                ParameterName = "TaskId",
                DbType = DbType.Int64,
                Value = 0
            };
            SqlParameter Subject = new SqlParameter
            {
                ParameterName = "Subject",
                DbType = DbType.String,
                Value = obj.Title
            };
            SqlParameter ClientId = new SqlParameter
            {
                ParameterName = "ClientId",
                DbType = DbType.Int64,
                Value = 1
            };
            SqlParameter AssignToId = new SqlParameter
            {
                ParameterName = "AssignToId",
                DbType = DbType.Int64,
                Value = 2
            };
            SqlParameter TaskTypeId = new SqlParameter
            {
                ParameterName = "TaskTypeId",
                DbType = DbType.Int64,
                Value = obj.TaskID
            };
            SqlParameter TaskReminderDatetime = new SqlParameter
            {
                ParameterName = "TaskReminderDatetime",
                DbType = DbType.DateTime,
                Value = obj.Start
            };
            SqlParameter Notes = new SqlParameter
            {
                ParameterName = "Notes",
                DbType = DbType.String,
                Value = obj.Description
            };
            SqlParameter UserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int32,
                Value = obj.UserID
            };
            IList<TaskSchedulerModel> result = _usersRepository.ExecuteStoredProcedureList<TaskSchedulerModel>("usp_TaskReminder_Save", TaskId, Subject, ClientId, AssignToId, TaskTypeId, TaskReminderDatetime, Notes, UserId);
            if (result.Count >= 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public int UpdateTaskReminder(TaskSchedulerModel obj)
        {
            SqlParameter TaskId = new SqlParameter
            {
                ParameterName = "TaskId",
                DbType = DbType.Int64,
                Value = obj.TaskID
            };

            SqlParameter TaskTypeId = new SqlParameter
            {
                ParameterName = "TaskTypeId",
                DbType = DbType.Int64,
                Value = obj.TaskTypeId
            };

            SqlParameter TaskReminderDatetime = new SqlParameter
            {
                ParameterName = "TaskReminderDatetime",
                DbType = DbType.DateTime,
                Value = obj.Start
            };
            SqlParameter Notes = new SqlParameter
            {
                ParameterName = "Notes",
                DbType = DbType.String,
                Value = (object)obj.Notes ?? DBNull.Value
            };

            SqlParameter UserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int32,
                Value = obj.UserID
            };

            SqlParameter AssignToId = new SqlParameter
            {
                ParameterName = "AssignToId",
                DbType = DbType.Int64,
                Value = obj.AssignToId
            };

            SqlParameter Subject = new SqlParameter
            {
                ParameterName = "Subject",
                DbType = DbType.String,
                Value = obj.Subject
            };
            IList<TaskSchedulerModel> result = _usersRepository.ExecuteStoredProcedureList<TaskSchedulerModel>("usp_TaskReminder_Update", TaskId, TaskTypeId, TaskReminderDatetime, Notes, UserId, AssignToId, Subject);
            if (result.Count >= 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public bool DismissReminder(long TaskId)
        {
            SqlParameter paraTaskId = new SqlParameter
            {
                ParameterName = "TaskId",
                DbType = DbType.Int64,
                Value = (object)TaskId ?? DBNull.Value
            };

            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_TaskReminder_Dismiss", paraTaskId).FirstOrDefault();
            return result > 0;
        }

        public bool CompleteReminder(long TaskId)
        {
            SqlParameter paraTaskId = new SqlParameter
            {
                ParameterName = "TaskId",
                DbType = DbType.Int64,
                Value = (object)TaskId ?? DBNull.Value
            };

            int result = _usersRepository.ExecuteStoredProcedure<int>("usp_TaskReminder_Complete", paraTaskId).FirstOrDefault();
            return result > 0;
        }
    }
    #endregion
}
