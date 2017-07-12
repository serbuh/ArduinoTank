using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(bTankService.Startup))]

namespace bTankService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}