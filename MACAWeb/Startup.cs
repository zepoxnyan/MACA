using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MACAWeb.Startup))]
namespace MACAWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
