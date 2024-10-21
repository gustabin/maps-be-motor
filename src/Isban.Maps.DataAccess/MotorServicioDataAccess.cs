using Isban.MapsMB.Common.Entity;
using Isban.MapsMB.DataAccess.DBRequest;
using Isban.MapsMB.DataAccess.DBResponse;
using Isban.MapsMB.Entity.Request;
using Isban.MapsMB.Entity.Response;
using Isban.MapsMB.IDataAccess;
using Isban.Mercados;
using Isban.Mercados.DataAccess;
using Isban.Mercados.DataAccess.OracleClient;
using Isban.Mercados.LogTrace;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Isban.MapsMB.Common.Entity.Request;
using Isban.MapsMB.Common.Entity.Response;
using Isban.Maps.DataAccess.Base;

namespace Isban.MapsMB.DataAccess
{
    [ProxyProvider("DBMAPS", Owner = "MAPS")]
    public class MotorServicioDataAccess : BaseProxy, IMotorServicioDataAccess
    {
        public virtual long? ActualizaSolicitud(ModificarSolicitudOrdenReq entity)
        {
            var request = entity.MapperClass<ModificarSolicitudOrdenDbReq>(TypeMapper.IgnoreCaseSensitive);

            var a = this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();

            return request.CantidadIntentos;

        }

        public virtual void EnviarMensaje(EnviarMensajesReq entity)
        {
            var request = entity.MapperClass<EnviarMensajesDbReq>(TypeMapper.IgnoreCaseSensitive);

            var a = this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();
        }

        public virtual void EnviarMensajeNET(EnviarMensajesReq entity)
        {
            var request = entity.MapperClass<EnviarMensajesNetDbReq>(TypeMapper.IgnoreCaseSensitive);

            var a = this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();
        }

        public virtual void ActualizaObservacionSolicitudesVencida(ModificarSolicitudOrdenReq entity)
        {
            var request = entity.MapperClass<ActualizaObservacionOrdenesDbReq>(TypeMapper.IgnoreCaseSensitive);

            var a = this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();

        }

        public ChequeoAcceso Chequeo(EntityBase entity)
        {
            var request = entity.MapperClass<ChequeoAccesoPlReq>(TypeMapper.IgnoreCaseSensitive);
            var li = this.Provider.GetCollection<ChequeoAccesoResp>(CommandType.StoredProcedure, request);

            if (li.Any())
                return li.First().MapperClass<ChequeoAcceso>(TypeMapper.IgnoreCaseSensitive);

            throw new DBCodeException(-1, "Error MBMAPS");
        }

        public string GetInfoDB(long id)
        {
            BaseProxySeguridad seg = new BaseProxySeguridad();
            UsuarioPasswordBaseClaves usuario = seg.ObtenerUsuarioBaseDeClaves(id);

            return Encrypt.EncryptToBase64String($"{usuario.Usuario}||{usuario.Password}");
        }

        public List<CargaSolicitudOrden> ConsultaAdhesionesActivas(WorkflowSAFReq entity)
        {
            var request = entity.MapperClass<CargaAdhesionesSolicitudDbReq>(TypeMapper.IgnoreCaseSensitive);

            var list = Provider.GetCollection<CargaSolicitudOrdenDbResp>(CommandType.StoredProcedure, request);

            request.CheckError();

            var result = list.MapperEnumerable<CargaSolicitudOrden>(TypeMapper.IgnoreCaseSensitive).ToList();

            return result;
        }

        public List<CuentasActivasRTF> ConsultaAdhesionesResumenRTF(WorkflowSAFReq entity)
        {
            var request = entity.MapperClass<ObtenerCuentasActivasRTFDbReq>(TypeMapper.IgnoreCaseSensitive);

            var list = Provider.GetCollection<CuentasActivasRTFDbResp>(CommandType.StoredProcedure, request);

            request.CheckError();

            var result = list.MapperEnumerable<CuentasActivasRTF>(TypeMapper.IgnoreCaseSensitive).ToList();

            return result;
        }

        public List<AdheVigenciaVencida> ConsultaAdhesionesPorVigenciaVencida(WorkflowSAFReq entity)
        {
            var request = entity.MapperClass<AdheVigenciaVencidaDbReq>(TypeMapper.IgnoreCaseSensitive);

            var list = Provider.GetCollection<AdheVigenciaVencidaDbResp>(CommandType.StoredProcedure, request);

            request.CheckError();

            var result = list.MapperEnumerable<AdheVigenciaVencida>(TypeMapper.IgnoreCaseSensitive).ToList();

            return result;
        }

        public List<NupSolicitudes> InformacionClientePorNup(WorkflowSAFReq entity)
        {
            var request = entity.MapperClass<NupSolicitudesDbReq>(TypeMapper.IgnoreCaseSensitive);

            var list = Provider.GetCollection<NupSolicitudesDbResp>(CommandType.StoredProcedure, request);

            request.CheckError();

            var result = list.MapperEnumerable<NupSolicitudes>(TypeMapper.IgnoreCaseSensitive).ToList();

            return result;
        }

        public List<CargaSolicitudOrden> ConsultaAdhesionesPorFallaTecnica(WorkflowSAFReq entity)
        {
            var request = entity.MapperClass<ConsultaOrdenesFallaTecnicaDbReq>(TypeMapper.IgnoreCaseSensitive);

            var list = Provider.GetCollection<CargaSolicitudOrdenDbResp>(CommandType.StoredProcedure, request);

            request.CheckError();

            var result = list.MapperEnumerable<CargaSolicitudOrden>(TypeMapper.IgnoreCaseSensitive).ToList();

            return result;
        }

        /// <summary>
        /// Obteners the usuario racf.
        /// </summary>
        /// <returns></returns>
        public virtual UsuarioRacf ObtenerUsuarioRacf()
        {
            var request = new ObtenerParametroRequest { Parametro = "ID_RACF_USER", Sistema = "MAPS", Ip = KnownParameters.IpDefault, Usuario = KnownParameters.UsuarioDefault };
            var listResult = Provider.GetFirst<ParametroResponse>(CommandType.StoredProcedure, request);
            request.CheckError();
            var id = listResult.MapperClass<Parametro>();

            BaseProxySeguridad dbseg = new BaseProxySeguridad();
            var usuarioBaseDeClaves = dbseg.ObtenerUsuarioBaseDeClaves(Convert.ToInt64(id.Valor));
            return usuarioBaseDeClaves.MapperClass<UsuarioRacf>();
        }

        public virtual string ObtenerUsuarioAsesor()
        {
            var request = new ObtenerParametroRequest { Parametro = "ID_USER_4", Sistema = "MAPS", Ip = KnownParameters.IpDefault, Usuario = KnownParameters.UsuarioDefault };
            var listResult = Provider.GetFirst<ParametroResponse>(CommandType.StoredProcedure, request);
            request.CheckError();
            var user = listResult.MapperClass<Parametro>();

            return user.Valor;
        }

        public virtual string ObtenerParametro(string parametro)
        {
            Parametro user = null;

            try
            {

                var request = new ObtenerParametroRequest { Parametro = parametro, Sistema = "MAPS", Ip = KnownParameters.IpDefault, Usuario = KnownParameters.UsuarioDefault };
                var listResult = Provider.GetFirst<ParametroResponse>(CommandType.StoredProcedure, request);
                request.CheckError();
                user = listResult.MapperClass<Parametro>();
            }
            catch (Exception ex)
            {
                user = new Parametro();
                LoggingHelper.Instance.Error(ex, $"ObtenerParametro {parametro} Exception: {ex.Message}");
            }

            return user.Valor;
        }

        public virtual List<Mensaje> ObtenerMailsAEnviar(ObtenerMensajes req)
        {
            var request = req.MapperClass<ObtenerMensajesDbReq>(TypeMapper.IgnoreCaseSensitive);
            var list = Provider.GetCollection<MensajeDbResp>(CommandType.StoredProcedure, request);
            request.CheckError();
            var result = list.MapperEnumerable<Mensaje>(TypeMapper.IgnoreCaseSensitive).ToList();

            return result;
        }

        public void CargarMensajesDelcliente(EnviarMensajesNetReq entity)
        {
            var request = entity.MapperClass<EnviarMensajesDbReq>(TypeMapper.IgnoreCaseSensitive);
            var a = this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);
            request.CheckError();
        }

        public virtual string ObtenerModoEjecucionMyA()
        {
            var request = new ObtenerParametroRequest { Parametro = "MODO_EJECUCION_MYA", Sistema = "MAPS", Ip = KnownParameters.IpDefault, Usuario = KnownParameters.UsuarioDefault };
            var listResult = Provider.GetFirst<ParametroResponse>(CommandType.StoredProcedure, request);
            request.CheckError();
            var user = listResult.MapperClass<Parametro>();

            return user.Valor;
        }

        public virtual Texto ObtenerTexto(ObtenerTexto req)
        {
            try
            {
                var request = req.MapperClass<ObtenerTextoDbReq>(TypeMapper.IgnoreCaseSensitive);
                var texto = Provider.GetFirst<TextoDbResp>(CommandType.StoredProcedure, request);
                request.CheckError();
                return texto.MapperClass<Texto>(TypeMapper.IgnoreCaseSensitive);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void ActualizarMensaje(ActualizarMensaje req)
        {
            var request = req.MapperClass<ActualizarMensajeNetDbReq>(TypeMapper.IgnoreCaseSensitive);
            var a = this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);
            request.CheckError();
        }

        public void ActualizarMensajeRTF(ActualizarMensajeRTF req)
        {
            var request = req.MapperClass<ActualizarMensajeRTFReq>(TypeMapper.IgnoreCaseSensitive);
            var a = this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);
            request.CheckError();
        }

        public dynamic ActualizarMapsParametros(ActualizarMapsParametro datos)
        {
            var request = datos.MapperClass<ActualizarMapsParametrosDbReq>(TypeMapper.IgnoreCaseSensitive);
            var a = this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);
            request.CheckError();

            return $"Se actualizo correctamente con el valor: {datos.Valor}";
        }

        public void CargarMensajesDelclienteNET(EnviarMensajesNetReq entity)
        {
            var request = entity.MapperClass<EnviarMensajesNetDbReq>(TypeMapper.IgnoreCaseSensitive);
            var a = this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);
            request.CheckError();
        }

        public virtual List<OperacionesResp> ObtenerOperaciones()
        {
            var request = new OperacionesDbReq { Ip = KnownParameters.IpDefault, Usuario = KnownParameters.UsuarioDefault };
            var list = Provider.GetCollection<OperacionesDbResp>(CommandType.StoredProcedure, request);
            request.CheckError();
            return list.MapperEnumerable<OperacionesResp>(TypeMapper.IgnoreCaseSensitive).ToList();
        }

        public virtual List<SaldoCuentaResp> ObtenerCuentasOperativas(ConsultaCuentaReq entity)
        {
            var request = entity.MapperClass<ConsultaCuentaDbReq>(TypeMapper.IgnoreCaseSensitive);
            var dbResul = Provider.GetCollection<SaldoCuentaDbResp>(CommandType.StoredProcedure, request);
            request.CheckError();

            return dbResul.MapperEnumerable<SaldoCuentaResp>(TypeMapper.IgnoreCaseSensitive).ToList();
        }

        public virtual List<SaldoCuentaResp> GuardarEstadoYSaldoCuentaOperativa(ConsultaCuentaReq entity)
        {
            var request = entity.MapperClass<ConsultaCuentaDbReq>(TypeMapper.IgnoreCaseSensitive);
            var dbResul = Provider.GetCollection<SaldoCuentaDbResp>(CommandType.StoredProcedure, request);
            request.CheckError();

            return dbResul.MapperEnumerable<SaldoCuentaResp>(TypeMapper.IgnoreCaseSensitive).ToList();
        }
        
        public virtual List<CuentaAdheridaRTF> ConsultaArchivosRTF(RTFWorkflowOnDemandReq entity)
        {
            var request = entity.MapperClass<ConsultaArchivoRTFDbReq>(TypeMapper.IgnoreCaseSensitive);
            var dbResul = Provider.GetCollection<ConsultaArchivoRTFDbResp>(CommandType.StoredProcedure, request);
            request.CheckError();

            return dbResul.MapperEnumerable<CuentaAdheridaRTF>(TypeMapper.IgnoreCaseSensitive).ToList();
        }

        public virtual List<ArchivoRTF> ObtenerArchivoRTF(RTFWorkflowOnDemandReq entity)
        {
            var request = entity.MapperClass<ConsultaArchivoRTFDbReq>(TypeMapper.IgnoreCaseSensitive);
            var dbResul = Provider.GetCollection<ConsultaArchivoRTFDbResp>(CommandType.StoredProcedure, request);
            request.CheckError();

            return dbResul.MapperEnumerable<ArchivoRTF>(TypeMapper.IgnoreCaseSensitive).ToList();
        }
        
        public void CargarArchivoRTF(ArchivoRTFReq entity)
        {
            var request = entity.MapperClass<InsertarArchivoRTFDbReq>(TypeMapper.IgnoreCaseSensitive);
            var a = this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);
            request.CheckError();
        }
        
        public virtual List<ObtenerCuentasRepaResp> ObtenerCuentasRepatriacion(ObtenerCuentasRepaReq entity)
        {
            var request = entity.MapperClass<ObtenerCuentasRepaDBReq>(TypeMapper.IgnoreCaseSensitive);
            var dbResul = Provider.GetCollection<ObtenerCuentasRepaDBResp>(CommandType.StoredProcedure, request);
            request.CheckError();

            return dbResul.MapperEnumerable<ObtenerCuentasRepaResp>(TypeMapper.IgnoreCaseSensitive).ToList();
        }

        public void CargarCuentasParticipantes(CargarCuentasParticipantesReq entity)
        {
            var request = entity.MapperClass<CargarCuentasParticipantesDbReq>(TypeMapper.IgnoreCaseSensitive);
            var a = this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);
            request.CheckError();
        }

        public void ActualizarAdhesionRepatriacion(ActualizarAdhesionRepatriacionReq entity)
        {
            var request = entity.MapperClass<ActualizarAdhesionRepatriacionDbReq>(TypeMapper.IgnoreCaseSensitive);
            var a = this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);
            request.CheckError();
        }

        public virtual List<AdhesionesMEPResp> ObtenerAdhesionesMEP(AdhesionesMEPReq entity)
        {
            var request = entity.MapperClass<AdhesionesMEPDbReq>(TypeMapper.IgnoreCaseSensitive);
            var dbResul = Provider.GetCollection<AdhesionesMepDbResp>(CommandType.StoredProcedure, request);
            request.CheckError();

            return dbResul.MapperEnumerable<AdhesionesMEPResp>(TypeMapper.IgnoreCaseSensitive).ToList();
        }

        public virtual AdhesionMEPResp ObtenerAdhesionMEP(AdhesionesMEPReq entity)
        {
            var request = entity.MapperClass<ObtenerAdhesionMEPDbReq>(TypeMapper.IgnoreCaseSensitive);
            var dbResul = Provider.GetCollection<AdhesionMepDbResp>(CommandType.StoredProcedure, request);
            request.CheckError();

            return dbResul.MapperEnumerable<AdhesionMEPResp>().FirstOrDefault(); 
        }

        public virtual void ActualizarAdhesionMEPCompra(AdhesionMEPCompraReq entity)
        {
            var request = entity.MapperClass<AdhesionMEPCompraDbReq>(TypeMapper.IgnoreCaseSensitive);
            var a = this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);
            request.CheckError();
        }

        public virtual void ActualizarAdhesionMEPError(AdhesionMEPErrorReq entity)
        {
            var request = entity.MapperClass<AdhesionMEPErrorDbReq>(TypeMapper.IgnoreCaseSensitive);
            var a = this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);
            request.CheckError();
        }

        public virtual void ActualizarAdhesionMEPVenta(AdhesionMEPVentaReq entity)
        {
            var request = entity.MapperClass<AdhesionMEPVentaDbReq>(TypeMapper.IgnoreCaseSensitive);
            var a = this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);
            request.CheckError();
        }

        public virtual List<AdhesionesMEPCompraResp> ObtenerAdhesionesMEPCompra(AdhesionesMEPReq entity)
        {
            var request = entity.MapperClass<AdhesionesMEPCompraDbReq>(TypeMapper.IgnoreCaseSensitive);
            var dbResul = Provider.GetCollection<AdhesionesMepCompraDbResp>(CommandType.StoredProcedure, request);
            request.CheckError();

            return dbResul.MapperEnumerable<AdhesionesMEPCompraResp>(TypeMapper.IgnoreCaseSensitive).ToList();
        }

        public virtual AdhesionesMEPCompraResp ObtenerAdhesionMEPCompra(AdhesionesMEPReq entity)
        {
            var request = entity.MapperClass<ObtenerAdhesionMEPCompraDbReq>(TypeMapper.IgnoreCaseSensitive);
            var dbResul = Provider.GetCollection<AdhesionesMepCompraDbResp>(CommandType.StoredProcedure, request);
            request.CheckError();

            return dbResul.MapperEnumerable<AdhesionesMEPCompraResp>().FirstOrDefault();
        }

        public virtual List<OrdenesMapsResp> ObtenerOrdenesMaps(OrdenesMapsReq entity)
        {
            var request = entity.MapperClass<OrdenesMapsDbReq>(TypeMapper.IgnoreCaseSensitive);
            var dbResul = Provider.GetCollection<OrdenesMapsDbResp>(CommandType.StoredProcedure, request);
            request.CheckError();

            return dbResul.MapperEnumerable<OrdenesMapsResp>(TypeMapper.IgnoreCaseSensitive).ToList();
        }

        public virtual void ActualizarOrden(ActualizarOrdenReq entity)
        {
            var request = entity.MapperClass<ActualizarOrdenDbReq>(TypeMapper.IgnoreCaseSensitive);
            var a = this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);
            request.CheckError();
        }

        public virtual List<AdhesionesMEPResp> ObtenerAdhesionesMEPInversa(AdhesionesMEPReq entity)
        {
            var request = entity.MapperClass<ObtenerAdhesionesMEPInversaDbReq>(TypeMapper.IgnoreCaseSensitive);
            var dbResul = Provider.GetCollection<AdhesionesMepDbResp>(CommandType.StoredProcedure, request);
            request.CheckError();

            return dbResul.MapperEnumerable<AdhesionesMEPResp>(TypeMapper.IgnoreCaseSensitive).ToList();
        }

        public virtual List<AdhesionesMEPCompraResp> ObtenerAdhesionesMEPInversaCompra(AdhesionesMEPReq entity)
        {
            var request = entity.MapperClass<ObtenerAdhesionesMEPInversaCompraDbReq>(TypeMapper.IgnoreCaseSensitive);
            var dbResul = Provider.GetCollection<AdhesionesMepCompraDbResp>(CommandType.StoredProcedure, request);
            request.CheckError();

            return dbResul.MapperEnumerable<AdhesionesMEPCompraResp>(TypeMapper.IgnoreCaseSensitive).ToList();
        }

        public virtual void ActualizarTarjeta(ActualizarTarjetaReq entity)
        {
            var request = entity.MapperClass<ActualizarTarjetaDbReq>(TypeMapper.IgnoreCaseSensitive);
            var a = this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);
            request.CheckError();
        }

        public virtual void InsertarDisclaimerEriMEPCompra(InsertarDisclaimerEriMEPCompraReq entity)
        {
            var request = entity.MapperClass<InsertarDisclaimerEriMEPCompraDbReq>(TypeMapper.IgnoreCaseSensitive);
            var a = this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);
            request.CheckError();
        }

        public virtual List<MensajeMEP> ObtenerMailsMepAEnviar(ObtenerMensajesMepReq req)
        {
            var request = req.MapperClass<ObtenerMensajesMEPDbReq>(TypeMapper.IgnoreCaseSensitive);
            var list = Provider.GetCollection<MensajeMEPDbResp>(CommandType.StoredProcedure, request);
            request.CheckError();
            var result = list.MapperEnumerable<MensajeMEP>(TypeMapper.IgnoreCaseSensitive).ToList();

            return result;
        }

        public virtual Texto ObtenerTextoMep(ObtenerTextoMepReq req)
        {
            var request = req.MapperClass<ObtenerTextoMepDbReq>(TypeMapper.IgnoreCaseSensitive);
            var texto = Provider.GetFirst<TextoDbResp>(CommandType.StoredProcedure, request);
            request.CheckError();
            return texto.MapperClass<Texto>(TypeMapper.IgnoreCaseSensitive);
        }

        public virtual void ActualizarMensajeMep(ActualizarMensajeMepReq req)
        {
            var request = req.MapperClass<ActualizarMensajeMepDbReq>(TypeMapper.IgnoreCaseSensitive);
            var a = this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);
            request.CheckError();
        }
    }
}
