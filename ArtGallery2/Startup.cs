using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ArtGallery2.Startup))]
namespace ArtGallery2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
