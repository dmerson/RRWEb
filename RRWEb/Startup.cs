using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RRWEb.Startup))]
namespace RRWEb
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
