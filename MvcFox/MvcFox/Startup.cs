using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcFox.Startup))]
namespace MvcFox
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
