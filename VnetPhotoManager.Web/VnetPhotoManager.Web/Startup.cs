using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VnetPhotoManager.Web.Startup))]
namespace VnetPhotoManager.Web
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
