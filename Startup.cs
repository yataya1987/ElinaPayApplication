
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BnetApplication.Startup))]
namespace BnetApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
        
    }
}
