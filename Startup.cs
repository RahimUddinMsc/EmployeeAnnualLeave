using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AnnualLeave.Startup))]
namespace AnnualLeave
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
