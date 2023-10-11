using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Business.ClientChat
{
    public interface IClientChatService
    {
        long AddUpdateSignalRUser(string SignalRId, long UserId);

        bool AddUpdateGroup(long SignalRGroupId, string GroupName, long UserId, DataTable SignalRUserId);

        bool SaveGroupConversation(long MsgFrom, long SignalRGroupId, string Msg, long GroupChatId);

        //bool SavePrivateConversation(long MsgFrom, long MsgTo, string Msg, long chatId);
        long SavePrivateConversation(long MsgFrom, long MsgTo, string Msg, long chatId); //added

        List<PrivateConversationModel> GetPrivateConversation(long MsgFrom, long MsgTo);

        List<GroupConversationModel> GetGroupConversation(long SignalRGroupId, long SignalRUserId);

        List<SignalRContactModel> GetContactNGroups(long UserId);

        List<SignalRUsersModel> GetSignalRUsers(long SignalRGroupId);

        SignalRGroupModel GetSignalRGroupData(long SignalRGroupId);

        bool DeleteSignalRGroup(long SignalRGroupId);

        bool ChangeUserOnlineStatus(long UserId);

        List<SignalRContactModel> DisplayUserSearchByName(long UserId, string Name);
        //added
        List<SignalRContactModel> DisplayMSG_UserSearchByName(long UserId, string Name);

        SignalRUsersStatus UpdateUserStatus(long UserId, long UserStatus);
        PrivateConversationModel LoadMessageFromDB(long PersonalChatId);
        bool RemovePrivateMsg(long PersonalChatId);
        bool updateMessage(long PersonalChatId, string Message);
        long GetUserStatus(long UserId);
        bool changeStickedUser(long SignalRUserId, long UserSignalRUserId, bool IsStick);
        bool RemoveGroupMsg(long GroupChatId);
    }
}
