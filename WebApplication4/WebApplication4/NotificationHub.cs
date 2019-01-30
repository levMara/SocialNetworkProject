using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace WebApplication4
{
    public class NotificationHub : Hub
    {

        //userId-to-connId map
        private static Dictionary<string, string> connIds = new Dictionary<string, string>();

        public void Register(string userId, string connId)
        {
            connIds[userId] = connId;
        }

        private static IHubContext _hubContext = null;

        private static IHubContext HubContext
        {
            get
            {
                if (_hubContext == null)
                    _hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                return _hubContext;
            }
        }

        public static void SendMessage(string message, string destUserId)
        {
            if (connIds.ContainsKey(destUserId))
                HubContext.Clients.Client(connIds[destUserId]).sendMessage(message);
            else
            {
                // drop message...
            }
            
        }
    }
}