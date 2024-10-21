
namespace Isban.MapsMB.Host.Webapi
{
    using Common.Entity;
    using Isban.MapsMB.Host.Controller;
    using Isban.Mercados.Service.InOut;
    using System;
    using System.Linq;
    using System.Web.UI;

    public partial class Estado : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
              
                hEstado.InnerText = "OK";
            }
            catch (Exception)
            {
                hEstado.InnerText = "KO";
            }
        }
    }
}