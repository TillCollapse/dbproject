using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(projectDB.Startup))]
namespace projectDB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
