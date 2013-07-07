﻿using System;
using System.Web;
using ServiceStack.Razor;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;

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

                // these are some supporting repositories that will help with any references to System.* or Mono.Cecil
                //container.Register(c => new CodeDocRepositories());
            }
        }

        protected void Application_Start(object sender, EventArgs e) {
            new AppHost().Init();
        }

    }
}