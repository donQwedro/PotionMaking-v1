using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ZV_Web.Code.Hubs
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