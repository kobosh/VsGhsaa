using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ghsaa.Startup))]
namespace Ghsaa
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
