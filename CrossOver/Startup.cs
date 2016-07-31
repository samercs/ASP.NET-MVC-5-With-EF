using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CrossOver.Startup))]
namespace CrossOver
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
