namespace Isban.MapsMB.DataAccess
{
    using Constantes;
    using DataAccess;
    using IDataAccess;
    using Isban.Common.Data;
    using Isban.MapsMB.Common.Entity.Extensions;
    using Isban.MapsMB.Common.Entity.Request;
    using Isban.MapsMB.Common.Entity.Response;
    using Isban.MapsMB.DataAccess.IATXRequest;
    using Isban.MapsMB.Entity.Constantes.Estructuras;
    using Isban.MapsMB.Entity.Request;
    using Isban.MapsMB.Entity.Response;
    using Isban.MapsMB.IDataAccesss;
    using Isban.Mercados.LogTrace;
    using Isban.PDC.Middleware.DataAccess.Requests;
    using Isban.PDC.Middleware.DataAccess.Response;
    using Isban.PDC.Middleware.Entity;
    using Mercados;
    using Mercados.UnityInject;
    using System;
    using System.Data;

    public class CanalesIatxDA : ServiceProxy, ICanalesIatxDA
    {
        protected override string ProviderName => Constantes.ConstantesIATX.Iatx;

        private ConsultaMAPSRequest ConfiguraIatx(CabeceraConsulta cabecera)
        {
            ConsultaMAPSRequest entity = new ConsultaMAPSRequest();
            entity.CanalId = cabecera.H_CanalId;
            entity.SubCanalId = cabecera.H_SubCanalId;
            entity.CanalVer = cabecera.H_CanalVer;
            entity.SubcanalTipo = cabecera.H_SubCanalTipo;
            entity.UsuarioTipo = cabecera.H_UsuarioTipo;
            entity.UsuarioId = cabecera.H_UsuarioID;
            entity.UsuarioAttr = cabecera.H_UsuarioAttr;
            entity.UsuarioPwd = cabecera.H_UsuarioPwd;
            entity.IdusConc = cabecera.H_IdusConc;
            entity.NumSec = cabecera.H_NumSec;
            entity.TipoCliente = cabecera.H_TipoCliente;
            entity.TipoIdCliente = cabecera.H_TipoIDCliente;
            entity.NroIdCliente = cabecera.H_NroIDCliente;
            entity.FechaNac = cabecera.H_FechaNac;
            entity.CanalTipo = cabecera.H_CanalTipo;
            entity.IndSincro = cabecera.H_IndSincro;

            return entity;
        }


        public virtual DatosCuentaIATXResponse ConsultaDatosCuenta(CabeceraConsulta cabecera, CargaSolicitudOrden soli)
        {
            ConsultaDatosCuentaRequest request = cabecera.MapperClass<ConsultaDatosCuentaRequest>(TypeMapper.IgnoreCaseSensitive,
                ModeExcludeWord.All, new string[] { "H", "_" });

            request.Tipo_Cuenta = soli.TipoCtaOper.ToString().PadLeft(2, '0');
            request.Sucursal_Cuenta = soli.SucuCtaOper.ToString().PadLeft(3, '0');
            request.Nro_Cuenta = soli.NroCtaOper.ToString().PadLeft(10, '0');
            request.H_Nup = soli.Nup;

            DatosCuentaResponse responseIatx = null;
            DatosCuentaIATXResponse result = null;

            responseIatx = this.Provider.GetFirst<DatosCuentaResponse>(ConstantesIATX.CONSULTADATOSCUENTA, CommandType.Text, request);

            result = responseIatx.MapperClass<DatosCuentaIATXResponse>(TypeMapper.IgnoreCaseSensitive);

            return result;
        }


        public virtual SuscripcionFCIIATXResponse SuscripcionFCI(CabeceraConsulta cabecera, string nup, int codigoFondo, string cuentaTitulo, decimal importeBruto, int tipoCuenta, int sucursalCuenta, string nroCuenta, int moneda)
        {
            var usuarioRacf = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioRacf();
            ConsultaMAPSRequest entity = ConfiguraIatx(cabecera);
            entity.UsuarioId = usuarioRacf.Usuario;
            entity.UsuarioPwd = usuarioRacf.Password;      
            entity.Usuario = cabecera.Usuario;
            entity.Ip = cabecera.Ip;
            entity.Nup = nup;

            SuscripcionFCIRequest request = entity.MapperClass<SuscripcionFCIRequest>(
                TypeMapper.IgnoreCaseSensitive,
                ModeExcludeWord.All,
                new string[] { "H", "_" }
                );

            request.H_Func_Autor = cabecera.H_Func_Autor;
                
            //configuracion t-banco, mail viernes 16/11/2018 12:14 p.m.
            //request.Terminal_safe = "    ";
            request.Codigo_Fondo = codigoFondo.ToString().PadLeft(3, '0');
            request.Codigo_Cliente = "001" + cuentaTitulo.PadLeft(8, '0');
            request.Codigo_Agente = "001";
            request.Codigo_Canal = !string.IsNullOrEmpty(cabecera.CodigoCanal) ? cabecera.CodigoCanal :  "975";
            //request.Importe_Bruto = (importeBruto * 100).ToString().PadLeft(14, '0');
            request.Importe_Bruto = importeBruto.FormatIaxt(2).PadLeft(14, '0');
            request.Porcentaje_Comis = "0000";
            request.Forma_Pago = "02";
            //request.Nombre_Banco = "          ";
            //request.Numero_Cheque = "          ";
            request.Tipo_Cuenta = tipoCuenta.ToString().PadLeft(2, '0');
            request.Suc_Cuenta = sucursalCuenta.ToString().PadLeft(3, '0');
            request.Nro_Cuenta = nroCuenta.PadLeft(7, '0');
            request.Impre_solicitud = "N";
            //request.Cotizac_Cambio = "00000000000000";
            //request.Fecha_Rescate_futuro = "          ";
            request.Codigo_Custodia_Actual = "4";
            request.Dia_Clearing_Cheques = "0";
            request.Moneda = moneda.ToString();       //0:pesos       2:dolar
            //request.Nro_Certific_a_Reversar = "0000000000";
            //request.Monto_a_Reversar_KU = "00000000000000";

            SuscripcionFCIResponse responseIatx = null;

            responseIatx = this.Provider.GetFirst<SuscripcionFCIResponse>(Constantes.ConstantesIATX.SuscripcionFondoComunInversion, CommandType.Text, request);

            return responseIatx.MapperClass<SuscripcionFCIIATXResponse>(TypeMapper.IgnoreCaseSensitive);
        }


        public virtual SuscripcionFCIIATXResponse SuscripcionFCIOBE(CabeceraConsulta cabecera, string nup, int codigoFondo, string cuentaTitulo, decimal importeBruto, int tipoCuenta, int sucursalCuenta, string nroCuenta, int moneda)
        {
            var usuarioRacf = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioRacf();
            ConsultaMAPSRequest entity = ConfiguraIatx(cabecera);
            entity.UsuarioId = usuarioRacf.Usuario;
            entity.UsuarioPwd = usuarioRacf.Password;

            entity.Usuario = cabecera.Usuario;
            entity.Ip = cabecera.Ip;
            entity.Nup = nup;

            SuscripcionFCIRequest request = entity.MapperClass<SuscripcionFCIRequest>(
                TypeMapper.IgnoreCaseSensitive,
                ModeExcludeWord.All,
                new string[] { "H", "_" }
                );

            request.Terminal_safe = "114";

            request.H_Func_Autor = cabecera.H_Func_Autor;
            request.Codigo_Fondo = codigoFondo.ToString().PadLeft(3, '0');
            request.Codigo_Cliente = "001" + cuentaTitulo.PadLeft(8, '0');
            request.Codigo_Agente = "001";
            request.Codigo_Canal = !string.IsNullOrEmpty(cabecera.CodigoCanal) ? cabecera.CodigoCanal : "975";
            request.Importe_Bruto = importeBruto.FormatIaxt(2).PadLeft(14, '0');
            request.Porcentaje_Comis = "0000";
            request.Forma_Pago = "02";
            request.Tipo_Cuenta = tipoCuenta.ToString().PadLeft(2, '0');
            request.Suc_Cuenta = sucursalCuenta.ToString().PadLeft(3, '0');
            request.Nro_Cuenta = nroCuenta.PadLeft(7, '0');
            request.Impre_solicitud = "N";      
            request.Codigo_Custodia_Actual = "4";
            request.Dia_Clearing_Cheques = "0";
            request.Moneda = moneda.ToString();       //0:pesos       2:dolar

            SuscripcionFCIOBEResponse responseIatx = null;

            try
            {
                responseIatx = this.Provider.GetFirst<SuscripcionFCIOBEResponse>(Constantes.ConstantesIATX.SuscripcionFondoComunInversion, CommandType.Text, request);

            }
            catch (Exception ex)
            {
                if(!ex.Message.Contains("Index"))
                {
                    throw;
                }
               
            }
       
            return responseIatx.MapperClass<SuscripcionFCIIATXResponse>(TypeMapper.IgnoreCaseSensitive);
        }
        public virtual void ConsultarFondoComunInversion(CargaSolicitudOrden entity)
        {
            var requestFCI = entity.MapperClass<ConsultaPdcRequest>(TypeMapper.IgnoreCaseSensitive, ModeExcludeWord.Target, "_");//validar todo lo que no llena

            var request = new ConsultaFCIRequest();
            request.H_Canal_Id = "0000";
            request.H_SubCanal_Id = "HTML";
            request.H_Canal_Ver = "000";
            request.H_SubCanal_Tipo = "99";
            request.H_Usuario_Tipo = "03";
            request.H_Usuario_Id = "ONLINEBP";
            request.H_Usuario_Attr = "";
            request.H_Usuario_Pwd = "DV09SA10";
            request.H_Idus_Conc = "00000000";
            request.H_NumSec = "00000002";
            //request.TipoCliente = "I";
            //request.TipoIdCliente = "N";
            //request.NroIdCliente = "00018065858";
            request.H_Fecha_Nac = "19380805";
            request.H_Canal_Tipo = "04";
            request.H_Usuario_Id = "B039514";
            //request.Ip = "::1";
            request.H_Ind_Sincro = "0";

            //ConsultaFCIRequest request = request.MapperClass<ConsultaFCIRequest>(TypeMapper.IgnoreCaseSensitive,
            //    ModeExcludeWord.All, new string[] { "H", "_" });

            request.Tipo_Cuenta = "SU";
            request.Nro_Cuenta = "7003523508";
            request.Moneda = "ARG";
            request.Codigo_Agente = "";

            var responseIatx = this.Provider.GetFirst<ConsultaPdcIatxResponse>(Constantes.ConstantesIATX.ConsultaSuscripcionFondoComunInversion, CommandType.Text, requestFCI);

        }

        public virtual void CompraVtaBonos()
        {
            CompraVtaAccionesBonosRequest entity = new CompraVtaAccionesBonosRequest
            {
                CanalId = "0000",
                SubCanalId = "HTML",
                CanalVer = "000",
                SubcanalTipo = "99",

                UsuarioTipo = "03",
                UsuarioId = "USRDA4",
                UsuarioAttr = "",
                UsuarioPwd = "RSUDESA4",

                TipoCtaOper = 09,
                SucursalCuenta = 000,
                NumeroCuenta = "3590902",
                TipoOperacion = (long)1,//ConstantesIATX.CodigoCompra, // Compra
                TipoEspecie = "BON",//ConstantesIATX.CodigoBono, // Bonos
                CantidadTitulo = 1000,
                CuentaTitulo = 17950417,
                EspecieCodigo = 48564,
                ImporteDebitoCredito = 0,
                Cotizacion = 15.77M,
                CotizacionLimite = 16,
                CanalCodigo = "HB",
                Plazo = "3",
                OperadorAlta = "HB",
                TipoAccion = "I",//ConstantesIATX.TipoAccionSimulacion, // Simulación
                PrecioClean = 0

            };

            var request = entity.MapperClass<DirectaCompraVtaBonosIatxRequest>(TypeMapper.IgnoreCaseSensitive,
                ModeExcludeWord.All, new string[] { "H", "_" });
            request.CantidadTitulo = entity.CantidadTitulo.FormatIaxt(4);
            request.CotizacionLimite = entity.CotizacionLimite.FormatIaxt(8);
            request.Cotizacion = entity.Cotizacion.FormatIaxt(8);
            request.PrecioClean = entity.PrecioClean.FormatIaxt(8);
            request.ImporteDebitoCredito = entity.ImporteDebitoCredito.FormatIaxt(2);

            request.Format();

            var responseIatx = new DirectaCompraVtaAccionesIatxResponse();

            try
            {
                responseIatx = this.Provider.GetFirst<DirectaCompraVtaAccionesIatxResponse>(
                        Constantes.ConstantesIATX.DirectaCompraVtaAccionesNombreServicio, CommandType.Text, request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void ConsultaPoderDeCompra()
        {
            ConsultaPdcRequest entity = new ConsultaPdcRequest();
            entity.CanalId = "0000";
            entity.SubCanalId = "HTML";
            entity.CanalVer = "000";
            entity.SubcanalTipo = "99";
            entity.UsuarioTipo = "03";
            entity.UsuarioId = "USRDA4";
            entity.UsuarioAttr = "";
            entity.UsuarioPwd = "RSUDESA4";
            entity.IdusConc = "00000000";
            entity.NumSec = "00000002";
            entity.TipoCliente = "I";
            entity.TipoIdCliente = "N";
            entity.NroIdCliente = "00018065858";
            entity.FechaNac = "19380805";
            entity.CanalTipo = "04";
            entity.Usuario = "B039514";
            entity.Ip = "::1";
            entity.IndSincro = "0";



            var request = entity.MapperClass<ConsultaPdcIatxRequest>(TypeMapper.IgnoreCaseSensitive,
                    ModeExcludeWord.All, new string[] { "H", "_" });
            request.Operatoria = "C/V";
            request.TipoCuenta = "OP";//request.TipoCtaOper.ToString();
            request.Sucursal = "00";// request.SucCtaOper;
            //input
            request.Format();

            var responseIatx = Provider.GetFirst<ConsultaPdcIatxResponse>(Constantes.ConstantesIATX.ConsultaPdcNombreServicio, CommandType.Text, request);

        }

        public virtual RescateFCIIATXResponse RescateFCIRTL(CabeceraConsulta cabecera, CargaSolicitudOrden solicitud)
        {
            var usuarioRacf = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioRacf();
            var entity = ConfiguraIatx(cabecera);
            entity.UsuarioId = usuarioRacf.Usuario;
            entity.UsuarioPwd = usuarioRacf.Password;
            entity.Usuario = cabecera.Usuario;
            entity.Ip = cabecera.Ip;
            entity.Nup = solicitud.Nup;

            var request = entity.MapperClass<RescateFCIRequest>(
                TypeMapper.IgnoreCaseSensitive,
                ModeExcludeWord.All,
                new string[] { "H", "_" }
                );

            request.Codigo_Fondo = solicitud.CodigoFondo.Ceros(3);
            request.Codigo_Cliente = $"001{solicitud.CuentaTitulos.Ceros(8)}";
            request.Cantidad_Cuotas_partes = ((decimal)0).FormatIaxt(4).Ceros(14); 
            request.Importe_Bruto = solicitud.SaldoEnviado.Value.FormatIaxt(2).Ceros(14);
            request.Importe_Rescate_Comision = 0.Ceros(14); 
            request.Importe_Rescate_Neto = solicitud.SaldoEnviado.Value.FormatIaxt(2).Ceros(14); 

            request.Tipo_Cuenta = solicitud.TipoCtaOper.Ceros(2);
            request.Suc_Cuenta = solicitud.SucuCtaOper.Ceros(3);
            request.Nro_Cuenta = solicitud.NroCtaOper.Ceros(10);
            request.Moneda = solicitud.CodMoneda == TipoMoneda.PesosAR ? "0":"2";       //0:pesos       2:dolar

            request.Terminal_safe = new string(' ', 4);
            request.Codigo_Agente = "001";
            request.Codigo_Canal = "975";
            request.Forma_Pago = "02";  //Débito/Crédito en cuenta
            request.Impre_solicitud = SiNo.No;
            request.Nro_Certif_a_Reversar = new string('0', 10);
            request.Monto_a_Reversar_KU = new string('0', 14);

            var responseIatx = this.Provider.GetFirst<RescateFCIResponse>(ConstantesIATX.RescateFCIRTL, CommandType.Text, request);

            var result =  responseIatx.MapperClass<RescateFCIIATXResponse>(TypeMapper.IgnoreCaseSensitive);
            result.Importe_Rescate_Neto = responseIatx.Importe_Rescate_Neto.FormatIaxt(2);
            result.Importe_Rescate_Comision = responseIatx.Importe_Rescate_Comision.FormatIaxt(2);
            result.Importe_Rescate_Bruto = responseIatx.Importe_Rescate_Bruto.FormatIaxt(2);
            result.Total_cuotas_partes = responseIatx.Total_cuotas_partes.FormatIaxt(4);
            result.Monto_Cambio = responseIatx.Monto_Cambio.FormatIaxt(2);
            result.Cotacao_pact = responseIatx.Cotacao_pact.FormatIaxt(2);

            return result;
        }


        public virtual RescateFCIBPResponse RescateFCIBP(CabeceraConsulta cabecera, CargaSolicitudOrden solicitud)
        {
            var usuarioRacf = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioRacf();
            var entity = ConfiguraIatx(cabecera);
            entity.UsuarioId = usuarioRacf.Usuario;
            entity.UsuarioPwd = usuarioRacf.Password;
            entity.Usuario = cabecera.Usuario;
            entity.Ip = cabecera.Ip;
            entity.Nup = solicitud.Nup;

            var request = entity.MapperClass<RescateFCIBPRequest>(
                TypeMapper.IgnoreCaseSensitive,
                ModeExcludeWord.All,
                new string[] { "H", "_" }
                );

            request.Codigo_Fondo = solicitud.CodigoFondo.Ceros(3);
            request.Codigo_Cliente = $"001{solicitud.CuentaTitulos.Ceros(8)}";
            request.Importe_Neto = solicitud.SaldoEnviado.Value.FormatIaxt(2).Ceros(14);
            request.Cantidad_Cuotas_partes = 0.Ceros(14);
            request.Porcentaje_comision = 0.Ceros(4);

            request.Tipo_transaccion = "08";    //Rescate por monto
            request.Codigo_Agente = "001";
            request.Codigo_Canal = "991";
            request.Forma_Pago = "09"; 
            request.Nro_Certificado = 0.Ceros(10);
            request.Impre_Solicitud = SiNo.No;
            request.Nro_Certif_a_Reversar = new string('0', 10);

            var responseIatx = this.Provider.GetFirst<RescateFCIBPIATXResponse>(ConstantesIATX.RescateFCIBP, CommandType.Text, request);

            var result = responseIatx.MapperClass<RescateFCIBPResponse>(TypeMapper.IgnoreCaseSensitive);
            result.Importe_rescate = responseIatx.Importe_rescate.FormatIaxt(2);
            result.Total_cuotas_Partes = responseIatx.Total_cuotas_Partes.FormatIaxt(4);
            result.Monto_Cambio = responseIatx.Monto_Cambio.FormatIaxt(2);
            result.Cotacao_pact = responseIatx.Cotacao_pact.FormatIaxt(8);

            return result;
        }

        public virtual AltaCuentaResponse AltaCuentaIATX(CabeceraConsulta cabecera,string nup , string numeroCuenta, string codigoMoneda)
        {
            AltaCuentaRequest request = cabecera.MapperClass<AltaCuentaRequest>(TypeMapper.IgnoreCaseSensitive,
                ModeExcludeWord.All, new string[] { "H", "_" });

            request.Nro_Cuenta = numeroCuenta;
            request.Producto = "60";
            request.Sucursal = "0000";
            request.SubProducto = "0000";
            request.H_Nup = nup;
            request.Cónyuge_Indicador_cta_mancomunado = "N";
            request.Adhiere_paquete = "N";
            request.Tipo_intervención = "TI";
            request.Firmante = "1";
            request.Código_moneda = codigoMoneda;
            request.Sucursal_operación = "0000";
            request.Canal_venta = "BM";
            request.Tipo_uso = "1";
            request.Cod_condicion = "1";
            request.Forma_Operar = "1";
            request.ADHIERE_RIOLINE_RIOSELF = "N";
            request.DOMICILIO_CORRESPONDENCIA = "P";
            request.SEC_DOM_LABORAL = "1";
            request.CajaAhorro = "0";
            request.CuentaCorriente = "0";


            AltaCuentaDbResp responseIatx = null;
            AltaCuentaResponse result = null;

            responseIatx = this.Provider.GetFirst<AltaCuentaDbResp>(ConstantesIATX.AltaCuenta, CommandType.Text, request);

            //LoggingHelper.Instance.Debug($"Llamado a servicio IATX: AltaCuentaIATX correctamente, Response: {responseIatx.Cod_retorno} CodRetorno: {responseIatx.Nup}", "BusinessHelper", "CreacionCuentaTitulosRepatriacion");

            result = responseIatx.MapperClass<AltaCuentaResponse>(TypeMapper.IgnoreCaseSensitive);

            return result;
        }

        public virtual AltaCuentaResponse GeneracionCuentaIATX(CabeceraConsulta cabecera, string nup)
        {
            GeneracionCuentaRequest request = cabecera.MapperClass<GeneracionCuentaRequest>(TypeMapper.IgnoreCaseSensitive,
                ModeExcludeWord.All, new string[] { "H", "_" });

            request.Producto = "60";
            request.Sucursal = "0000";
            request.Subproducto = "0000";
            request.H_Nup = nup;

            GeneracionCuentaDbResp responseIatx = null;

            responseIatx = this.Provider.GetFirst<GeneracionCuentaDbResp>(ConstantesIATX.GeneracionCuenta, CommandType.Text, request);

            LoggingHelper.Instance.Information($"Llamado a servicio IATX: GENNROCTA correctamente, Response: {responseIatx.Código_retorno_extendido} ", "BusinessHelper", "CreacionCuentaTitulosRepatriacion");

            var result = responseIatx.MapperClass<AltaCuentaResponse>(TypeMapper.IgnoreCaseSensitive);

            return result;
        }


        public virtual AltaCuentaResponse ServicioINIATX(CabeceraConsulta cabecera, string nup)
        {
            AltaCuentaRequest request = cabecera.MapperClass<AltaCuentaRequest>(TypeMapper.IgnoreCaseSensitive,
                ModeExcludeWord.All, new string[] { "H", "_" });

            request.Cónyuge_Indicador_cta_mancomunado = "F";
            request.Adhiere_paquete = "1";
            request.Tipo_intervención = "00004506879";
            request.H_Nup = nup;

            GeneracionCuentaDbResp responseIatx = null;

            responseIatx = this.Provider.GetFirst<GeneracionCuentaDbResp>(ConstantesIATX.servicioIN, CommandType.Text, request);

            LoggingHelper.Instance.Information($"Llamado a servicio IATX: ServicioINIATX correctamente, Response: {responseIatx.Código_retorno_extendido} ", "BusinessHelper", "CreacionCuentaTitulosRepatriacion");

            var result = responseIatx.MapperClass<AltaCuentaResponse>(TypeMapper.IgnoreCaseSensitive);

            return result;
        }

        public virtual string AsociarCuentasOperativasIATX(CabeceraConsulta cabecera, WorkflowCTRReq opcion,AsociarCtaOperativaReq asociarCtaOperativaReq)
        {
            ConsultaCuentaRequest request = cabecera.MapperClass<ConsultaCuentaRequest>(TypeMapper.IgnoreCaseSensitive,
                ModeExcludeWord.All, new string[] { "H", "_" });

            request.Nro_Cuenta = asociarCtaOperativaReq.NumeroCuenta;
            request.UsuarioV = asociarCtaOperativaReq.UsuarioVerificacion;
            request.CuentaC = asociarCtaOperativaReq.CuentaCorriente;
            request.CajaAhorro = asociarCtaOperativaReq.CajaAhorro;
            request.CuentaDolares = asociarCtaOperativaReq.CuentaDolares;
            request.Sucursal = asociarCtaOperativaReq.Sucursal;

            ConsultaCuentaDbResp responseIatx = null;

            responseIatx = this.Provider.GetFirst<ConsultaCuentaDbResp>(ConstantesIATX.AsociarCuentasOperativas, CommandType.Text, request);

            //LoggingHelper.Instance.Debug($"Llamado a servicio IATX: CNSCTATITU correctamente, Response: {responseIatx.Código_retorno_extendido}  ", "BusinessHelper", "CreacionCuentaTitulosRepatriacion");

            //var result = responseIatx.MapperClass<AltaCuentaResponse>(TypeMapper.IgnoreCaseSensitive);

            return responseIatx.Código_retorno_extendido;
        }


        public virtual AltaCuentaResponse CMBRELCLICIATX(CabeceraConsulta cabecera, string nup, string numeroCuenta,string ordenCliente)
        {
            CmbreclicRequest request = cabecera.MapperClass<CmbreclicRequest>(TypeMapper.IgnoreCaseSensitive,
                ModeExcludeWord.All, new string[] { "H", "_" });

            request.Nro_Cuenta = numeroCuenta;
            request.Oficina_Contrato = "0000";
            request.Aplicacion = "ATIT";
            request.MotBaja = "00";
            request.H_Nup = nup;
            request.FechaBaja = "00000000";
            request.Opcion = "M";
            request.DatosCliente = string.Format("2"+nup+"CTN");
            request.OrdenCliente = ordenCliente;
            request.Responsabilidad = "00000";
            request.FormaOperar = "04";
            request.ADHIERE_RIOLINE_RIOSELF = "N";

            CMBRECLCIResp responseIatx = null;
            AltaCuentaResponse result = null;

            responseIatx = this.Provider.GetFirst<CMBRECLCIResp>(ConstantesIATX.CMBRELCLIC, CommandType.Text, request);
       
            result = responseIatx.MapperClass<AltaCuentaResponse>(TypeMapper.IgnoreCaseSensitive);

            return result;
        }

        public virtual AltaCuentaResponse ModificarRelacionClienteContratoIATX(CabeceraConsulta cabecera, RelacionClienteContrato relacion)
        {
            CmbreclicRequest request = cabecera.MapperClass<CmbreclicRequest>(TypeMapper.IgnoreCaseSensitive,
                ModeExcludeWord.All, new string[] { "H", "_" });

            request.Nro_Cuenta = relacion.NumeroCuenta;
            request.Oficina_Contrato = "0000";
            request.Aplicacion = "ATIT";
            request.MotBaja = relacion.MotBaja;
            request.H_Nup = relacion.Nup;
            request.FechaBaja = relacion.FechaBaja;
            request.Opcion = relacion.Opcion;
            request.DatosCliente = relacion.DatosCliente;
            request.OrdenCliente = relacion.OrdenCliente;
            request.Responsabilidad = "00000";
            request.FormaOperar = "04";
            request.ADHIERE_RIOLINE_RIOSELF = "N";

            CMBRECLCIResp responseIatx = null;
            AltaCuentaResponse result = null;

            responseIatx = this.Provider.GetFirst<CMBRECLCIResp>(ConstantesIATX.CMBRELCLIC, CommandType.Text, request);

            result = responseIatx.MapperClass<AltaCuentaResponse>(TypeMapper.IgnoreCaseSensitive);

            return result;
        }

    }
}
