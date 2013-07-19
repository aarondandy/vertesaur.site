using System;
using System.Web;
using MarkdownSharp;
using ServiceStack.Razor;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.WebHost.Endpoints.Formats;

namespace Vertesaur.Site
{
    public class Global : HttpApplication
    {

        public class AppHost : AppHostBase
        {
            public AppHost() : base("Vertesaur Site", typeof(AppHost).Assembly) { }

            public override void Configure(Funq.Container container) {
                Plugins.Add(new RazorFormat());

                JsConfig.EmitCamelCaseNames = true;

                container.Register(c => new CodeDocRepositories());
            }
        }

        protected void Application_Start(object sender, EventArgs e) {
            new AppHost().Init();
        }

    }
}