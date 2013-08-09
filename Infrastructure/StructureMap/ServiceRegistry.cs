using Infrastructure.Services;
using StructureMap.Configuration.DSL;

namespace ZV_Web.App_Start
{
    public class ServiceRegistry : Registry
    {
        public ServiceRegistry()
        {
            Scan(x =>
                {
                    x.AssemblyContainingType<IUserAuthenticationService>();
                    x.WithDefaultConventions();
                });
        }
    }
}