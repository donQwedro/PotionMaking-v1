using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ZV_Web.Code.Hubs
{
    [Authorize]
    [HubName("groupRegistration")]
    public class GroupRegistrationHub : Hub
    {
        public void Hello(string user)
        {
            Clients.All.hello(user);
        }
    }
}