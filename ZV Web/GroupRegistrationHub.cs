using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ZV_Web
{
    [HubName("groupRegistration")]
    public class GroupRegistrationHub : Hub
    {
        public void Hello(string user)
        {
            Clients.Others.hello(user);
        }
    }
}