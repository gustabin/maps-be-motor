
namespace Isban.MapsMB.Business
{
    using Common.Entity;
    using Isban.MapsMB.Entity.Constantes.Estructuras;
    using Isban.MapsMB.Entity.Controles.Independientes;
    using Isban.MapsMB.Entity.Request;
    using Isban.MapsMB.Entity.Response;
    using Isban.MapsMB.IDataAccess;
    using Isban.Mercados;
    using Isban.Mercados.UnityInject;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using Common.Entity.Response;
    using Common.Entity.Request;

    /// <summary>
    /// Clase para métodos comunes del business
    /// </summary>
    public static class BusinessHelper
    {
        /// <summary>
        /// Método para guardar cuaquier tipo de falla técnica en el servicio
        /// </summary>
        /// <param name="ex">Excepción</param>
        /// <param name="soli">Solicitud de Orden que generó la Excepción</param>
        internal static void GuardarFallaTecnica(Exception ex, CargaSolicitudOrden soli, string servicio)
        {
            var modifOrd = soli.MapperClass<ModificarSolicitudOrdenReq>(TypeMapper.IgnoreCaseSensitive);
            modifOrd.Estado = EstadoSolicitudDeOrden.FallaTecnica;
            var mensaje = servicio;
            mensaje += ex;
            modifOrd.Observacion = mensaje.Substring(0, Math.Min(mensaje.Length, 1000));
            soli.Observacion = mensaje;
            ModificarSolicitudOrden(modifOrd,soli);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="soli"></param>
        /// <param name="usrRACF"></param>
        internal static void GuardarFallaTecnica(Exception ex, CargaSolicitudOrden soli, UsuarioRacf usrRACF)
        {
            var modifOrd = soli.MapperClass<ModificarSolicitudOrdenReq>(TypeMapper.IgnoreCaseSensitive);
            modifOrd.Estado = EstadoSolicitudDeOrden.FallaTecnica;
            var mensaje = $"usrRACF:{usrRACF.Usuario} pass:{usrRACF.Password} error: {ex}";
            modifOrd.Observacion = mensaje.Substring(0, Math.Min(mensaje.Length, 1000));
            soli.Observacion = mensaje;
            ModificarSolicitudOrden(modifOrd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datos"></param>
        internal static void ModificarSolicitudOrden(ModificarSolicitudOrdenReq datos)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
            da.ActualizaSolicitud(datos);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="datos"></param>
        internal static void ModificarSolicitudOrden(ModificarSolicitudOrdenReq datos, CargaSolicitudOrden soli)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
            var cantidadIntentos =  da.ActualizaSolicitud(datos);

            if (cantidadIntentos.HasValue && cantidadIntentos >= 3) soli.BajaTercerIntento = true;
        }

        /// <summary>
        /// Método para guardar el estado por baja si la cuenta esta bloqueada
        /// </summary>
        /// <param name="item">Solicitud de Orden que se quiere dar de baja</param>
        /// <param name="idSolicitudDeBaja">Id de confirmación de baja</param>
        internal static void GuardarBajaPorCuentaBloqueada(CargaSolicitudOrden item, long idSolicitudDeBaja)
        {
            var modifOrd = item.MapperClass<ModificarSolicitudOrdenReq>(TypeMapper.IgnoreCaseSensitive);
            modifOrd.Estado = EstadoSolicitudDeOrden.Bloqueado;
            modifOrd.Observacion = $"Cuenta bloqueada. Baja: ({idSolicitudDeBaja})";
            modifOrd.IdSolicitudOrdenes = item.IdSolicitudOrdenes;
            modifOrd.CodBajaAdhesion = idSolicitudDeBaja;
            modifOrd.TextoDisclaimer = string.Empty;
            modifOrd.TipoDisclaimer = null;

            ModificarSolicitudOrden(modifOrd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comprobantes"></param>
        /// <param name="estado"></param>
        internal static void ActualizaObservacionSolicitudesVencida(string comprobantes, string estado)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            var modifOrd = new ModificarSolicitudOrdenReq();
            modifOrd.Observacion = $"TIENE LA VIGENCIA VENCIDA";
            modifOrd.Comprobantes = comprobantes;
            modifOrd.Estado = estado;

            da.ActualizaObservacionSolicitudesVencida(modifOrd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comprobantes"></param>
        /// <param name="estado"></param>
        internal static void ActualizaObservacionBajaSolicitudes(string comprobantes, string estado)
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            var modifOrd = new ModificarSolicitudOrdenReq();
            modifOrd.Observacion = $"BAJA DE SOLICITUD";
            modifOrd.Comprobantes = comprobantes;
            modifOrd.Estado = estado;

            da.ActualizaObservacionSolicitudesVencida(modifOrd);
        }

        /// <summary>
        /// Guarda el estado de la adhesión cuando no posee saldo suficiente para operar.
        /// </summary>
        /// <param name="soli">Saldo que se pretende debitar</param>
        /// <param name="saldoReal">Saldo que le quedo disponible para operar</param>
        internal static void GuardarSinSaldo(CargaSolicitudOrden soli, decimal saldoReal)
        {
            var modifOrd = soli.MapperClass<ModificarSolicitudOrdenReq>(TypeMapper.IgnoreCaseSensitive);
            modifOrd.Estado = EstadoSolicitudDeOrden.SinSaldo;
            modifOrd.Observacion = $"No posee saldo sufiente para operar, Saldo enviado: $({soli.SaldoEnviado}), Saldo real: ({saldoReal})";

            ModificarSolicitudOrden(modifOrd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="soli"></param>
        /// <param name="saldoReal"></param>
        /// <param name="saldoMinFondo"></param>
        internal static void GuardarSinSaldo(CargaSolicitudOrden soli, decimal saldoReal, decimal saldoMinFondo)
        {
            var modifOrd = soli.MapperClass<ModificarSolicitudOrdenReq>(TypeMapper.IgnoreCaseSensitive);
            modifOrd.Estado = EstadoSolicitudDeOrden.SinSaldo;
            modifOrd.Observacion = $"No posee saldo sufiente para operar, Saldo real: ({saldoReal}), MinDelFondo: {saldoMinFondo}";

            ModificarSolicitudOrden(modifOrd);
        }

        internal static void GuardarSaldoCuentaOperativa(SaldoCuentaResp cuenta)
        {
            var da = DependencyFactory.Resolve<IOpicsDA>();
            var req = cuenta.MapperClass<ConsultaCuentaReq>(TypeMapper.IgnoreCaseSensitive);

            da.GuardarEstadoYSaldoCuentaOperativa(req);
        }

        internal static void GuardarFallaTecnicaConsultaSaldoCuentaOperativa(Exception ex, SaldoCuentaResp cuenta)
        {
            var da = DependencyFactory.Resolve<IOpicsDA>();
            var req = cuenta.MapperClass<ConsultaCuentaReq>(TypeMapper.IgnoreCaseSensitive);
            //var msg = string.Format("Error {0} | Día y Hora: {1}. | Exception: {2}", string.Empty,
            //     DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), ex);
            //req.Observacion = msg;

            da.GuardarEstadoYSaldoCuentaOperativa(req);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="solicitud"></param>
        /// <param name="dentroDePerfil"></param>
        internal static void GuardarProcesado(CargaSolicitudOrden solicitud, string dentroDePerfil)
        {
            var modifOrd = solicitud.MapperClass<ModificarSolicitudOrdenReq>(TypeMapper.IgnoreCaseSensitive);
            modifOrd.Estado = EstadoSolicitudDeOrden.Procesado;
            modifOrd.Observacion = "Procesado OK";
            ModificarSolicitudOrden(modifOrd);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="evaluacionRiesgoResult"></param>
        /// <param name="item"></param>
        internal static void GuardarEvaluacionERI(DisclaimerERI evaluacionRiesgoResult, CargaSolicitudOrden item)
        {
            var texto = string.Empty;
            var modifOrd = item.MapperClass<ModificarSolicitudOrdenReq>(TypeMapper.IgnoreCaseSensitive);

            modifOrd.Estado = EstadoSolicitudDeOrden.EvaluacionDeRiesgo;
            modifOrd.Observacion = $"Simulación correcta: {evaluacionRiesgoResult.IdEvaluacion}";
            modifOrd.NumOrdenOrigen = item.NumOrdenOrigen;
            modifOrd.IdSolicitudOrdenes = item.IdSolicitudOrdenes;
            modifOrd.IdEvaluacion = evaluacionRiesgoResult.IdEvaluacion;
            modifOrd.TextoDisclaimer = item.TextoDisclaimer;
            modifOrd.TipoDisclaimer = evaluacionRiesgoResult.TipoDisclaimer;
            ModificarSolicitudOrden(modifOrd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="soli"></param>
        internal static void GuardarSaldoCuentaAntes(CargaSolicitudOrden soli)
        {
            var modifOrd = soli.MapperClass<ModificarSolicitudOrdenReq>(TypeMapper.IgnoreCaseSensitive);
            modifOrd.Estado = EstadoSolicitudDeOrden.Inicial;
            modifOrd.Observacion = string.Empty;

            ModificarSolicitudOrden(modifOrd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        internal static void GuardarConfirmacionERI(CargaSolicitudOrden item)
        {
            var modifOrd = item.MapperClass<ModificarSolicitudOrdenReq>(TypeMapper.IgnoreCaseSensitive);
            modifOrd.Estado = EstadoSolicitudDeOrden.ConfirmacionDeRiesgo;
            modifOrd.Observacion = $"Confirmación correcta: {item.IdEvaluacion}";

            ModificarSolicitudOrden(modifOrd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="Observacion"></param>
        internal static void GuardarOtros(CargaSolicitudOrden item, string Observacion)
        {
            var modifOrd = item.MapperClass<ModificarSolicitudOrdenReq>(TypeMapper.IgnoreCaseSensitive);
            modifOrd.Observacion = Observacion.Substring(0, Math.Min(Observacion.Length, 1000));
            modifOrd.Estado = EstadoSolicitudDeOrden.Otro;

            ModificarSolicitudOrden(modifOrd);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="solis"></param>
        internal static void AsignarUsuarioIP(List<CargaSolicitudOrden> solis)
        {
            var usuario = ObtenerUsuarioIPLocal();

            solis.ForEach(s =>
            {
                s.Usuario = usuario.UserName;
                s.Ip = usuario.Ip;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="solis"></param>
        internal static void AsignarUsuarioIP(SolicitudOrden solis)
        {
            var usuario = ObtenerUsuarioIPLocal();

            solis.Solicitudes.ForEach(s =>
            {
                s.Usuario = usuario.UserName;
                s.Ip = usuario.Ip;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal static UsuarioLocal ObtenerUsuarioIPLocal()
        {
            var usuario = new UsuarioLocal();
            usuario.UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            var host = Dns.GetHostEntry(Dns.GetHostName());
            string ipLocal = string.Empty;

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipLocal = ip.ToString();
                }
            }

            if (string.IsNullOrWhiteSpace(ipLocal))
                throw new Exception("No network adapters with an IPv4 address in the system!");

            usuario.Ip = ipLocal;

            return usuario;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="soli"></param>
        internal static void GuardarSaldoRescatado(CargaSolicitudOrden soli)
        {
            var modifOrd = soli.MapperClass<ModificarSolicitudOrdenReq>(TypeMapper.IgnoreCaseSensitive);
            modifOrd.Estado = EstadoSolicitudDeOrden.Inicial;
            modifOrd.Observacion = string.Empty;

            ModificarSolicitudOrden(modifOrd);
        }

        /// <summary>
        /// Obtengo el tiempo en millisegundos para demorar el envío de mensajes.
        /// </summary>
        /// <returns></returns>
        internal static int ObtenerTiempoDeEsperaMilSec()
        {
            IMotorServicioDataAccess da = DependencyFactory.Resolve<IMotorServicioDataAccess>();
            int valor = 0;
            int.TryParse(da.ObtenerParametro("MYA_SLEEP"), out valor);

            return valor;
        }
    }
}
