namespace Infrastructure.Services
{
    public interface IUserAuthenticationService
    {
        bool Authenticate();
    }

    public class UserAuthenticationService : IUserAuthenticationService
    {
        public bool Authenticate()
        {
            throw new System.NotImplementedException();
        }
    }
}