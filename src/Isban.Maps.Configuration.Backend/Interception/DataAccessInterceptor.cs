namespace Isban.MapsMB.Configuration.Backend.Interception
{
    using Isban.Common.Data.IATX;
    using Mercados;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using System;
    using Isban.Mercados.LogTrace;
    using Isban.Mercados.UnityInject;
    using Oracle.ManagedDataAccess.Client;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class DataAccessInterceptor : InterceptorBase
    {
        public override IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            IMethodReturn methodReturn = getNext()(input, getNext);
            InterceptExceptions(methodReturn, input);

            return methodReturn;
        }

        private void InterceptExceptions(IMethodReturn result, IMethodInvocation input)
        {
            if (result.Exception != null && !(result.Exception is IsbanException))
            {
                OracleException ex = null;
                if (result.Exception.InnerException != null)
                {
                    ex = result.Exception.InnerException as OracleException;
                }
                if (ex != null && Math.Abs(ex.Number) == 20499)
                {
                    result.Exception = new DBCodeException(ex.Number, ex.Message);
                }

                if (!(result.Exception is IsbanException))
                {
                    var message = string.Format("Error {0} | Día y Hora: {1}. | Exception: {2}", this.GetType().Name,
                    DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), result.Exception);
                    LoggingHelper.Instance.Error(result.Exception, message);
                    result.Exception = new IatxException("Error no controlado. Verificar InnerException.",
                        result.Exception);
                }
            }
        }

        private string FillMessage(IATXException iatxExc)
        {
            string[] list = iatxExc.Message.Split('õ');
            if (list.Length > 5)
            {
                return list[4].Trim();
            }
            else if (list.Length == 1)
            {
                return list[0].Trim();
            }
            return string.Empty;
        }

        private long FillCode(IATXException iatxExc)
        {

            string[] list = iatxExc.Message.Split('õ');
            if (list.Length > 2)
            {
                var co = list[1].Trim();
                return long.Parse(co.Substring(co.Length - 8));
            }
            return -90;
        }
    }
}
