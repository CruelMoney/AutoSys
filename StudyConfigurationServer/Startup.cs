using System.Linq;
using Owin;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using StudyConfigurationServer;
using Swashbuckle.Application;

namespace OwinSelfhostSample
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder app)
        {
            // Configure Web API for self-host. 
            var thisAssembly = typeof(SwaggerConfig).Assembly;
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "StudyConfigurationServer");
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            })
                .EnableSwaggerUi(c =>
                {
                    c.InjectJavaScript(thisAssembly, "Swashbuckle.Dummy.SwaggerExtensions.testScript1.js");

                });
        
    
        app.UseWebApi(config);

    

       
        }
    }
}