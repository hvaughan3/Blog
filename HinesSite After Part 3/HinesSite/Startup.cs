using Microsoft.Owin;
using Owin;
// ReSharper disable MissingXmlDoc

[assembly: OwinStartupAttribute(typeof(HinesSite.Startup))]
namespace HinesSite {

    public partial class Startup {

        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
