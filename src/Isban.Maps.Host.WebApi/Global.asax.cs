using Isban.Mercados.Configuration;
using System;
using System.Web;
using System.Web.Http;
using Isban.Mercados.LogTrace;

namespace Isban.MapsMB.Host.Webapi
{

    public class Global : HttpApplication
    {
        public Global()
        {
            start = start ?? new ConfigurationStart();
        }

        #region atributo
        private ConfigurationStart start;
        #endregion
        void Application_Start(object sender, EventArgs e)
        {
            using (var trace = PerformanceHelper.Instance.BeginTrace("Host", this.GetType().Name, "Application_Start"))
            {
                try
                {
                    // Code that runs on application startup
                    GlobalConfiguration.Configure(WebApiConfig.Register);
                    if (start == null)
                        start = new ConfigurationStart();
                    start.Start();
                }
                catch (Exception ex)
                {
                    PerformanceHelper.Instance.EndTrace(trace, ex);
                    throw;
                }

            }
        }
    }
}