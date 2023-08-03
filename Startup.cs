using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DNyC.Startup))]
namespace DNyC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Versión correcta
            ConfigureAuth(app);
        }
    }
}
