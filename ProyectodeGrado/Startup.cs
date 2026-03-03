using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProyectodeGrado.Startup))]
namespace ProyectodeGrado
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
