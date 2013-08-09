using System.Web.Security;

namespace Infrastructure.Services
{
    public interface IUserAuthenticationService
    {
        void Authenticate(string userName);
    }

    public class UserAuthenticationService : IUserAuthenticationService
    {
        public void Authenticate(string userName)
        {
            FormsAuthentication.SetAuthCookie(userName, false);
        }
    }
}