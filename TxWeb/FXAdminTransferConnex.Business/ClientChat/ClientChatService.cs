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

namespace FXAdminTransferConnex.Business.ClientChat
{
    public class ClientChatService : IClientChatService
    {
        #region Constants

        /// <summary>
        /// Declare The logger object for perform operation on Logger
        /// </summary>
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Fields

        /// <summary>
        /// Declare The provider repository object for perform operation on Provider repository
        /// </summary>
        private readonly IRepository<SignalRModel> _signalrRepository;

        #endregion

        #region ctor

        public ClientChatService(IRepository<SignalRModel> providerRepository)
        {
            _signalrRepository = providerRepository;
        }

        #endregion

        public bool AddUpdateGroup(long SignalRGroupId, string GroupName, long UserId, DataTable SignalRUserId)
        {
            SqlParameter paraSignalRGroupId = new SqlParameter
            {
                ParameterName = "SignalRGroupId",
                DbType = DbType.Int64,
                Value = (object)SignalRGroupId ?? DBNull.Value
            };

            SqlParameter paraGroupName = new SqlParameter
            {
                ParameterName = "GroupName",
                DbType = DbType.String,
                Value = (object)GroupName ?? DBNull.Value
            };

            SqlParameter paraCreatedBy = new SqlParameter
            {
                ParameterName = "CreatedBy",
                DbType = DbType.Int64,
                Value = (object)UserId ?? DBNull.Value
            };
            SqlParameter paramSignalRUserIds = new SqlParameter
            {
                ParameterName = "SignalRUser",
                SqlDbType = SqlDbType.Structured,
                Value = SignalRUserId,
                TypeName = "dbo.tt_SignalRUserIds"
            };

            string result = _signalrRepository.ExecuteStoredProcedure<string>("usp_CreateGroupAndAddUpdateMembers", paraSignalRGroupId, paraGroupName, paraCreatedBy, paramSignalRUserIds).FirstOrDefault();

            if (string.IsNullOrEmpty(result))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public long AddUpdateSignalRUser(string SignalRId, long UserId)
        {
            SqlParameter paraSignalRId = new SqlParameter
            {
                ParameterName = "SignalRId",
                DbType = DbType.String,
                Value = (object)SignalRId ?? DBNull.Value
            };

            SqlParameter paraUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = (object)UserId ?? DBNull.Value
            };

            long result = _signalrRepository.ExecuteStoredProcedure<long>("usp_AddUpdateSignalRUser", paraSignalRId, paraUserId).FirstOrDefault();
            return result;
        }

        public bool ChangeUserOnlineStatus(long UserId)
        {
            SqlParameter paraUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.String,
                Value = (object)UserId ?? DBNull.Value
            };

            string result = _signalrRepository.ExecuteStoredProcedure<string>("usp_SignalRUsers_UpdateOnlineStatus", paraUserId).FirstOrDefault();

            if (string.IsNullOrEmpty(result))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteSignalRGroup(long SignalRGroupId)
        {
            SqlParameter paraSignalRGroupId = new SqlParameter
            {
                ParameterName = "SignalRGroupId",
                DbType = DbType.Int64,
                Value = (object)SignalRGroupId ?? DBNull.Value
            };

            string result = _signalrRepository.ExecuteStoredProcedure<string>("usp_SignalRGroup_Delete", paraSignalRGroupId).FirstOrDefault();

            if (string.IsNullOrEmpty(result))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<SignalRContactModel> GetContactNGroups(long UserId)
        {
            SqlParameter paraUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = (object)UserId ?? DBNull.Value
            };

            List<SignalRContactModel> result = _signalrRepository.ExecuteStoredProcedure<SignalRContactModel>("usp_GetAllSignalRContactAndGroupList", paraUserId).ToList();
            return result.ToList();
        }

        public List<GroupConversationModel> GetGroupConversation(long SignalRGroupId, long SignalRUserId)
        {
            SqlParameter paraSignalRGroupId = new SqlParameter
            {
                ParameterName = "SignalRGroupId",
                DbType = DbType.Int64,
                Value = (object)SignalRGroupId ?? DBNull.Value
            };

            SqlParameter paraSignalRUserId = new SqlParameter
            {
                ParameterName = "SignalRUserId",
                DbType = DbType.Int64,
                Value = (object)SignalRUserId ?? DBNull.Value
            };

            List<GroupConversationModel> result = _signalrRepository.ExecuteStoredProcedure<GroupConversationModel>("usp_GetGroupConversation", paraSignalRGroupId, paraSignalRUserId).ToList();
            return result;
        }

        public List<PrivateConversationModel> GetPrivateConversation(long MsgFrom, long MsgTo)
        {
            SqlParameter paraMsgFrom = new SqlParameter
            {
                ParameterName = "MsgFrom",
                DbType = DbType.Int64,
                Value = (object)MsgFrom ?? DBNull.Value
            };

            SqlParameter paraMsgTo = new SqlParameter
            {
                ParameterName = "MsgTo",
                DbType = DbType.Int64,
                Value = (object)MsgTo ?? DBNull.Value
            };

            List<PrivateConversationModel> result = _signalrRepository.ExecuteStoredProcedure<PrivateConversationModel>("usp_GetPrivateConversation", paraMsgFrom, paraMsgTo).ToList();
            return result;
        }

        public SignalRGroupModel GetSignalRGroupData(long SignalRGroupId)
        {
            SqlParameter paraSignalRGroupId = new SqlParameter
            {
                ParameterName = "SignalRGroupId",
                DbType = DbType.Int64,
                Value = (object)SignalRGroupId ?? DBNull.Value
            };

            SignalRGroupModel result = _signalrRepository.ExecuteStoredProcedure<SignalRGroupModel>("usp_getSignalRGroupName", paraSignalRGroupId).FirstOrDefault();
            return result;
        }

        public List<SignalRUsersModel> GetSignalRUsers(long SignalRGroupId)
        {
            SqlParameter paraSignalRGroupId = new SqlParameter
            {
                ParameterName = "SignalRGroupId",
                DbType = DbType.Int64,
                Value = (object)SignalRGroupId ?? DBNull.Value
            };

            List<SignalRUsersModel> result = _signalrRepository.ExecuteStoredProcedure<SignalRUsersModel>("usp_GetAllSignalRUsers", paraSignalRGroupId).ToList();
            return result;
        }

        public bool SaveGroupConversation(long MsgFrom, long SignalRGroupId, string Msg, long GroupChatId)
        {
            SqlParameter paraMsgFrom = new SqlParameter
            {
                ParameterName = "MsgFrom",
                DbType = DbType.Int64,
                Value = (object)MsgFrom ?? DBNull.Value
            };

            SqlParameter paraSignalRGroupId = new SqlParameter
            {
                ParameterName = "SignalRGroupId",
                DbType = DbType.Int64,
                Value = (object)SignalRGroupId ?? DBNull.Value
            };

            SqlParameter paraMsg = new SqlParameter
            {
                ParameterName = "Msg",
                DbType = DbType.String,
                Value = (object)Msg ?? DBNull.Value
            };

            SqlParameter paraGroupChatId = new SqlParameter
            {
                ParameterName = "GroupChatId",
                DbType = DbType.Int64,
                Value = (object)GroupChatId ?? DBNull.Value
            };

            string result = _signalrRepository.ExecuteStoredProcedure<string>("usp_SaveGroupConversation", paraMsgFrom, paraSignalRGroupId, paraMsg, paraGroupChatId).FirstOrDefault();

            if (string.IsNullOrEmpty(result))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public long SavePrivateConversation(long MsgFrom, long MsgTo, string Msg, long chatId)
        {
            SqlParameter paraMsgFrom = new SqlParameter
            {
                ParameterName = "MsgFrom",
                DbType = DbType.Int64,
                Value = (object)MsgFrom ?? DBNull.Value
            };

            SqlParameter paraMsgTo = new SqlParameter
            {
                ParameterName = "MsgTo",
                DbType = DbType.Int64,
                Value = (object)MsgTo ?? DBNull.Value
            };

            SqlParameter paraMsg = new SqlParameter
            {
                ParameterName = "Msg",
                DbType = DbType.String,
                Value = (object)Msg ?? DBNull.Value
            };

            SqlParameter paraChatId = new SqlParameter
            {
                ParameterName = "ChatId",
                DbType = DbType.Int64,
                Value = (object)chatId ?? DBNull.Value
            };

            long result = _signalrRepository.ExecuteStoredProcedure<long>("usp_SavePrivateConversation", paraMsgFrom, paraMsgTo, paraMsg, paraChatId).FirstOrDefault();

            if (result != 0)
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        public List<SignalRContactModel> DisplayUserSearchByName(long UserId, string Name)
        {
            SqlParameter paraUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = (object)UserId ?? DBNull.Value
            };
            SqlParameter paraName = new SqlParameter
            {
                ParameterName = "Name",
                DbType = DbType.String,
                Value = (object)Name ?? DBNull.Value
            };
            List<SignalRContactModel> result = _signalrRepository.ExecuteStoredProcedure<SignalRContactModel>("usp_GetUserNameSearchByName", paraUserId, paraName).ToList();
            return result;
        }

        //added
        public List<SignalRContactModel> DisplayMSG_UserSearchByName(long UserId, string Name)
        {
            SqlParameter paraUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = (object)UserId ?? DBNull.Value
            };
            SqlParameter paraName = new SqlParameter
            {
                ParameterName = "Name",
                DbType = DbType.String,
                Value = (object)Name ?? DBNull.Value
            };
            List<SignalRContactModel> result = _signalrRepository.ExecuteStoredProcedure<SignalRContactModel>("usp_GetUserNameSearchByName_MSG", paraUserId, paraName).ToList();
            return result;
        }

        public SignalRUsersStatus UpdateUserStatus(long UserId, long UserStatus)
        {
            SqlParameter paraUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.String,
                Value = (object)UserId ?? DBNull.Value
            };
            SqlParameter paraUserStatus = new SqlParameter
            {
                ParameterName = "UserStatus",
                DbType = DbType.String,
                Value = (object)UserStatus ?? DBNull.Value
            };
            SignalRUsersStatus result = _signalrRepository.ExecuteStoredProcedure<SignalRUsersStatus>("usp_UpdateUserStatus", paraUserId, paraUserStatus).FirstOrDefault();
            return result;
        }
        public PrivateConversationModel LoadMessageFromDB(long PersonalChatId)
        {
            SqlParameter paraPersonalChatId = new SqlParameter
            {
                ParameterName = "PersonalChatId",
                DbType = DbType.Int64,
                Value = (object)PersonalChatId ?? DBNull.Value
            };
            PrivateConversationModel result = _signalrRepository.ExecuteStoredProcedure<PrivateConversationModel>("usp_GetChatMessage", paraPersonalChatId).FirstOrDefault();
            return result;
        }

        public bool RemovePrivateMsg(long PersonalChatId)
        {
            SqlParameter paraPersonalChatId = new SqlParameter
            {
                ParameterName = "PersonalChatId",
                DbType = DbType.Int64,
                Value = (object)PersonalChatId ?? DBNull.Value
            };

            string result = _signalrRepository.ExecuteStoredProcedure<string>("usp_DeletePrivateMsg", paraPersonalChatId).FirstOrDefault();

            if (string.IsNullOrEmpty(result))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool updateMessage(long PersonalChatId, string Message)
        {
            SqlParameter paraPersonalChatId = new SqlParameter
            {
                ParameterName = "PersonalChatId",
                DbType = DbType.Int64,
                Value = (object)PersonalChatId ?? DBNull.Value
            };
            SqlParameter paraMessage = new SqlParameter
            {
                ParameterName = "Message",
                DbType = DbType.String,
                Value = (object)Message ?? DBNull.Value
            };
            string result = _signalrRepository.ExecuteStoredProcedure<string>("usp_EditPrivateMsg", paraPersonalChatId, paraMessage).FirstOrDefault();

            if (string.IsNullOrEmpty(result))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public long GetUserStatus(long UserId)
        {
            SqlParameter userId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = (object)UserId ?? DBNull.Value
            };
            long result = _signalrRepository.ExecuteStoredProcedure<long>("usp_GetUserStatus", userId).FirstOrDefault();
            return result;
        }

        public bool changeStickedUser(long SignalRUserId, long UserSignalRUserId, bool IsStick)
        {
            SqlParameter paraUserId = new SqlParameter
            {
                ParameterName = "UserId",
                DbType = DbType.Int64,
                Value = (object)SignalRUserId ?? DBNull.Value
            };
            SqlParameter paraStickedUserId = new SqlParameter
            {
                ParameterName = "StickedUserId",
                DbType = DbType.Int64,
                Value = (object)UserSignalRUserId ?? DBNull.Value
            };
            SqlParameter paraIsStick = new SqlParameter
            {
                ParameterName = "IsStick",
                DbType = DbType.Boolean,
                Value = (object)IsStick ?? DBNull.Value
            };
            string result = _signalrRepository.ExecuteStoredProcedure<string>("usp_SignalRStickedUser_AddUpdate", paraUserId, paraStickedUserId, paraIsStick).FirstOrDefault();

            if (!string.IsNullOrEmpty(result))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveGroupMsg(long GroupChatId)
        {
            SqlParameter paraGroupChatId = new SqlParameter
            {
                ParameterName = "GroupChatId",
                DbType = DbType.Int64,
                Value = (object)GroupChatId ?? DBNull.Value
            };

            string result = _signalrRepository.ExecuteStoredProcedure<string>("usp_SignalRGroupConversation_Delete", paraGroupChatId).FirstOrDefault();

            if (string.IsNullOrEmpty(result))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
