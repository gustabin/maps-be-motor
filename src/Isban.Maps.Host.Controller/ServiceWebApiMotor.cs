namespace Isban.MapsMB.Host.Controller
{
    using System;
    using Common.Entity;
    using Isban.MapsMB.Configuration.Backend.Interception;
    using Mercados;
    using Mercados.Service;
    using Mercados.Service.InOut;
    using Isban.Mercados.Security.Adsec.Entity;


    public class ServiceWebApiMotor : ServiceWebApiBase
    {

        public ServiceWebApiMotor()
        {
            this.OnBeforeCheckSecurity += ServiceWebApiMotor_OnBeforeCheckSecurity;
        }

        private void ServiceWebApiMotor_OnBeforeCheckSecurity(BeforeCheckSecurityEvent e)
        {
            if (e.Request == null)
            {
                throw new ArgumentNullException("", "Json mal formateado o no envia datos");
            }
            var request = e.Request as IDatoFirma;
            var request2 = e.Request as IRequest;
            if (request != null)
            {
                if (string.IsNullOrWhiteSpace(request.Firma))
                    throw new ArgumentNullException("Es Obligatorio", "Firma");

                if (string.IsNullOrWhiteSpace(request.Dato))
                    throw new ArgumentNullException("Es Obligatorio", "Dato");
            }
            if (request2 != null)
            {
                if (string.IsNullOrWhiteSpace(request2.Canal))
                    throw new ArgumentNullException("Es Obligatorio", "Canal");
                else if (request2.Canal.Length != 2)
                    throw new ArgumentException("El parámetro debe tener 2 caracteres", "Canal");

                if (string.IsNullOrWhiteSpace(request2.SubCanal))
                    throw new ArgumentNullException("Es Obligatorio", "SubCanal");
                else if (request2.SubCanal.Length != 4)
                    throw new ArgumentException("El parámetro debe tener 4 caracteres", "SubCanal");
            }
        }

        public override void FillException<TRp>(Exception ex, Response<TRp> response)
        {

            if (typeof(TRp).IsClass)
            {
                response.Datos = (TRp)Activator.CreateInstance(typeof(TRp));

                if (ex is Exception && response.Datos is EntityBase )
                {
                    var argException = ex;
                    response.Codigo = 0;
                    response.Mensaje = string.Format("El campo {0} es obligatorio", argException.Message);
                    response.MensajeTecnico = ex.ToString();                      
                   
                    return;
                }
                
                response.Mensaje = ex.Message;

                response.Codigo = 2;
                if (ex.InnerException != null)
                {
                    response.MensajeTecnico = ex.InnerException.ToString();
                }
                else
                {
                    response.MensajeTecnico = ex.ToString();
                }
                if (ex is ICodeException)
                {
                    response.Codigo = 2;//((ICodeException)ex).Code;
                    if (ex.InnerException != null)
                    {
                        response.MensajeTecnico = ex.InnerException.ToString();
                    }
                }
            }

        }
    }

}