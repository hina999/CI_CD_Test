using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace FXAdminTransferConnexApp.SignalRHub
{
    public class ChatHub : Hub
    {
        public void LetsChat(string Cl_Name, string Cl_Message)
        {
            Clients.All.PublicMessage(Cl_Name, Cl_Message);
        }

        public void NewJoinAlert(string Cl_ClientId)
        {
            string[] Exceptional = new string[1];
            Exceptional[0] = Cl_ClientId;
            Clients.AllExcept(Exceptional).PublicMessage("NewJoin", Cl_ClientId);
        }

        public void PrivateChat(string Cl_To_Id, string Cl_From_Id, string Cl_Name, string Cl_Message, string ChatId, bool IsEdit)
        {
            Clients.Client(Cl_To_Id).PrivateMessage(Cl_From_Id, Cl_Name, Cl_Message, ChatId, IsEdit);
        }

        public void StatusChangeDisplay(string Cl_To_Id,string Cl_Name)
        {
            string[] Exceptional = new string[1];
            Exceptional[0] = Cl_To_Id;
            Clients.AllExcept(Exceptional).PublicMessage("NewJoin", Cl_To_Id);
        }

        public void BroadCastInGroup(string msgFrom, string msg, string GroupName, string ChatId, bool IsEdit)
        {
           // var id = Context.ConnectionId;
            string[] Exceptional = new string[0];
            Clients.Group(GroupName, Exceptional).GroupMessage(msgFrom, msg, GroupName, ChatId, IsEdit);
        }

        [HubMethodName("groupconnect")]
        public void Get_Connect(string username, string GroupName)
        {
            string id = Context.ConnectionId;
            Groups.Add(id, GroupName);

        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            string clientId = Context.ConnectionId;
            string[] Exceptional = new string[1];
            Exceptional[0] = clientId;
            Clients.AllExcept(Exceptional).PublicMessage("Disconnect", clientId);

            return base.OnDisconnected(stopCalled);
        }

        public void IsTyping(string html)
        {
            // do stuff with the html
            SayWhoIsTyping(html); //call the function to send the html to the other clients
        }

        public void SayWhoIsTyping(string html)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.All.sayWhoIsTyping(html);
        }

    }
}