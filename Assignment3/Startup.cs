using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Assignment3.Startup))]
namespace Assignment3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
