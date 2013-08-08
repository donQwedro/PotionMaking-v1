using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ZV_Web
{
    public class GroupRegistrationHub : Hub
    {
        public void Hello(string user)
        {
            Clients.All.hello(user);
        }
    }
}