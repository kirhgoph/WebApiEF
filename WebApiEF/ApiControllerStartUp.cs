using Owin;
using System.Web.Http;
using Serilog;
using System;
using WebApi.StructureMap;

namespace WebApiEF
{
    public class ApiControllerStartup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            try
            {
                HttpConfiguration config = new HttpConfiguration();
                config.MapHttpAttributeRoutes();
                config.UseStructureMap<ApiControllerRegistry>();
                config.MessageHandlers.Add(new LogRequestAndResponseHandler());
                appBuilder.UseWebApi(config);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "ApiController configuration failed");
                throw exception;
            }
        }
    }
}