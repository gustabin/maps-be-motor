
namespace Isban.MapsMB.Entity.Exceptions
{
    using Isban.Maps.Entity.Base;
    using Isban.MapsMB.Common.Entity;
    using System.Runtime.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Web.Hosting;
    using System.IO;
    using Isban.Mercados;
    using System.Linq;

    public static class ExceptionHandler
    {

        public static string GetExcepcionIatx(System.Exception ex)
        {
            var message = GetCodeMessage(ex.InnerException.Message);

            return message;
        }

       private static string GetCodeMessage(string s)
        {
            var message = string.Empty;

            string[] list = s.Replace("\r\n", "\n").Split('\n');
            if (list.Length > 3)
            {
                message = list[2].Trim();
            }

            return message;
        }
    }

    public class Excepcion
    {
        public string ErrorIatx { get; set; }
        public string CodigoIatx { get; set; }
        public long Error { get; set; }
        public string MensajeAmigable { get; set; }
        public string MensajeTecnico { get; set; }
    }

}
