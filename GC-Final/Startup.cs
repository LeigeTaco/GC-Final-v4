using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GC_Final.Startup))]
namespace GC_Final
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
