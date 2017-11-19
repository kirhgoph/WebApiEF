using System;
using Microsoft.Owin.Hosting;
using System.ServiceModel;
using System.ServiceModel.Description;
using Serilog;
using StructureMap;

namespace WebApiEF
{
    public class Program : System.Web.Http.ApiController
    {
        static void Main()
        {
            ConfigureContainer();
            ConfigureLogging();
            StartImportService();
            _serviceHost = StartWcfService();
            Console.ReadLine();
            StopWcfService();
        }

        private static void ConfigureContainer()
        {
            _container = new Container(_ =>
            {
                _.Scan(x =>
                {
                    x.TheCallingAssembly();
                    x.WithDefaultConventions();
                });
                _.For<IUserInfoService>().Use<UserInfoService>().Singleton();
            });
        }

        public static void StopWcfService()
        {
            _serviceHost.Close();
        }

        public static void ConfigureLogging()
        {
            var log = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.LiterateConsole()
                .CreateLogger();
            Log.Logger = log;
            Log.Information("Logging started");
        }

        public static ServiceHost StartWcfService()
        {
            try
            {
                _wcfAddress = new Uri("http://localhost:10000/");
                var wcfServiceSingleton = _container.GetInstance<IUserInfoService>();
                ServiceHost selfHost = new ServiceHost(wcfServiceSingleton, _wcfAddress);
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior()
                {
                    HttpGetEnabled = true
                };
                selfHost.Description.Behaviors.Add(smb);
                selfHost.Open();
                Log.Information($"The WCF service is listening at {_wcfAddress}");
                return selfHost;
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Wcf service could not be started");
                throw exception;
            }
        }

        public static void StartImportService()
        {
            try
            {
                _importAddress = "http://localhost:9000/";
                WebApp.Start<ApiControllerStartup>(url: _importAddress);
                Log.Information($"Import service is listening at {_importAddress}import.json");
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Import service could not be started");
                throw exception;
            }
        }
        static public Uri _wcfAddress;
        static public string _importAddress;
        static public ServiceHost _serviceHost;
        static private Container _container;
    }
}