using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCValidateDemo.Startup))]
namespace MVCValidateDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
