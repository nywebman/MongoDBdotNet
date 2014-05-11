using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MongoDBdotNet.Startup))]
namespace MongoDBdotNet
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
