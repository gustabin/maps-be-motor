using Isban.MapsMB.Common.Entity;
using Isban.MapsMB.Common.Entity.Extensions;
using Isban.MapsMB.Common.Entity.Request;
using Isban.MapsMB.Common.Entity.Response;
using Isban.MapsMB.Entity.Constantes.Estructuras;
using Isban.MapsMB.Entity.Request;
using Isban.MapsMB.Entity.Response;
using Isban.MapsMB.IBusiness;
using Isban.MapsMB.IDataAccess;
using Isban.MapsMB.IDataAccesss;
using Isban.Mercados;
using Isban.Mercados.Service.InOut;
using Isban.Mercados.UnityInject;
using System;
using System.Collections.Generic;

namespace Isban.MapsMB.Business
{
    public class CuentasBusiness : ICuentasBusiness
    {
        public List<SaldoCuentaResp> ConsultarCuentasOperativas(RequestSecurity<EntityBase> entity)
        {

            DatosCuentaIATXResponse datosCuentaIATXResponse = null;

            //var daMaps = DependencyFactory.Resolve<IMotorServicioDataAccess>();
            var daMaps = DependencyFactory.Resolve<IOpicsDA>();
            var daIATX = DependencyFactory.Resolve<ICanalesIatxDA>();

            var reqCuentasOperativas = entity.Datos.MapperClass<ConsultaCuentaReq>(TypeMapper.IgnoreCaseSensitive);
            var cuentasOperativas = daMaps.ObtenerCuentasOperativas(reqCuentasOperativas);
            var usuarioRacf = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerUsuarioRacf();

            cuentasOperativas.ForEach(cuenta =>
            {
                try
                {
                    #region BancaPrivada
                    if (cuenta.Segmento.ToUpper() == TipoSegmento.BancaPrivada)
                    {
                        decimal sumSaldo = 0;

                        var atisResp = DependencyFactory.Resolve<IOpicsDA>().ObtenerAtis(new ConsultaLoadAtisRequest
                        {
                            Nup = cuenta.Nup.ParseGeneric<long?>(),
                            CuentaBp = 0
                        });

                        var cuentaBp = ValidarCuentas(atisResp, cuenta.NumeroCuentaOperativa, cuenta.CuentaTitulos);

                        var resLoadSaldos = DependencyFactory.Resolve<IOpicsDA>().EjecutarLoadSaldos(new LoadSaldosRequest
                        {
                            Canal = entity.Canal,// entity.Canal,
                            Cuenta = cuentaBp.Value.ParseGeneric<string>(),
                            FechaDesde = DateTime.Now.Date,
                            FechaHasta = DateTime.Now.Date,
                            Usuario = usuarioRacf.Usuario,
                            Password = usuarioRacf.Password
                        });

                        var resSaldoConcertado = DependencyFactory.Resolve<ISmcDA>().EjecutarSaldoConcertadoNoLiquidado(new SaldoConcertadoNoLiquidadoRequest
                        {
                            Fecha = DateTime.Now.Date,
                            Ip = entity.Datos.Ip,
                            Moneda = TraducirUsdAUsb(cuenta.CodMoneda),
                            NroCtaOper = cuenta.NumeroCuentaOperativa.ParseGeneric<string>(),
                            SucCtaOper = cuenta.SucuCtaOper.ParseGeneric<string>(),
                            TipoCtaOper = cuenta.TipoCtaOper,
                            Usuario = entity.Datos.Usuario
                        });


                        switch (cuenta.CodMoneda.ToLower())
                        {
                            case "ars":
                                foreach (var saldo in resLoadSaldos.ListaSaldos)
                                {

                                    sumSaldo += saldo.CAhorroPesos;
                                }

                                break;

                            case "usd":
                                foreach (var saldo in resLoadSaldos.ListaSaldos)
                                {
                                    sumSaldo += saldo.CAhorroDolares;
                                }

                                break;
                        }

                        var totalSaldo = sumSaldo - (resSaldoConcertado.Saldo.HasValue ? resSaldoConcertado.Saldo.Value : 0m);
                        cuenta.SaldoActual = totalSaldo;
                    }
                    #endregion
                    #region RTL
                    else
                    {
                        var reqSolicitudOrden = new CargaSolicitudOrden
                        {
                            TipoCtaOper = cuenta.TipoCtaOper,
                            SucuCtaOper = cuenta.SucuCtaOper,
                            NroCtaOper = cuenta.NumeroCuentaOperativa,
                            Nup = cuenta.Nup,
                            Canal = entity.Canal,
                            SubCanal = entity.SubCanal
                        };

                        var cabecera = GenerarCabecera(reqSolicitudOrden);
                        cabecera.H_UsuarioID = usuarioRacf.Usuario;
                        cabecera.H_UsuarioPwd = usuarioRacf.Password;

                        datosCuentaIATXResponse = daIATX.ConsultaDatosCuenta(cabecera, reqSolicitudOrden);
                        var entero = datosCuentaIATXResponse.Dispuesto_USD_cuenta.Substring(0, 12);
                        var dec = datosCuentaIATXResponse.Dispuesto_USD_cuenta.Substring(12, 2);
                        var signo = datosCuentaIATXResponse.Dispuesto_USD_cuenta.Substring(14, 1);
                        decimal numDec = 0;

                        if (decimal.TryParse($"{signo}{entero}.{dec}", out numDec))
                        {
                            cuenta.SaldoActual = numDec;
                        }
                    }
                    #endregion

                    BusinessHelper.GuardarSaldoCuentaOperativa(cuenta);
                }
                catch (Exception ex)
                {
                    cuenta.Observacion = string.Format("Error {0} | Día y Hora: {1}. | Exception: {2}", string.Empty,
      DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), ex);

                    BusinessHelper.GuardarFallaTecnicaConsultaSaldoCuentaOperativa(ex, cuenta);
                }

            });

            return cuentasOperativas;
        }

        private string TraducirUsdAUsb(string monedaOperacion)
        {
            switch (monedaOperacion.ToUpper())
            {
                case "USD": return "USB";
                default: return monedaOperacion;
            }
        }

        private long? ValidarCuentas(List<ConsultaLoadAtisResponse> atisResp, long nroCtaOperativa, long nroCtaTitulos)
        {
            bool cuentasValidas = false;

            foreach (var item in atisResp)
            {
                cuentasValidas = item.Validar(item.CuentaBp.Value.ToString().Substring(3, item.CuentaBp.Value.ToString().Length - 3), nroCtaOperativa.ToString());//sacar los 3 primeros.
                cuentasValidas = item.Validar(item.CuentaAtit.Value.ToString(), nroCtaTitulos.ToString());

                if (cuentasValidas)
                {
                    return item.CuentaBp;
                }
            }

            if (!cuentasValidas)
            {
                throw new BusinessException("La Cuenta Título y Número de Cuenta, no corresponden al cliente");
            }

            return -1;
        }
        private static CabeceraConsulta GenerarCabecera(CargaSolicitudOrden entity)
        {
            //TODO: se tiene que reemplazar por la forma correcta según dijo lucas.
            return new CabeceraConsulta()
            {
                H_CanalTipo = entity.Canal, // "22",
                H_SubCanalId = "HTML",
                H_CanalVer = "000",
                H_SubCanalTipo = "99",
                H_CanalId = entity.SubCanal,
                H_UsuarioTipo = "03",
                H_UsuarioID = "ONLINEBP",
                H_UsuarioAttr = " ",
                H_UsuarioPwd = "DV09SA10",
                H_IdusConc = "00000000",
                H_NumSec = "00000002",
                H_IndSincro = "0",
                H_TipoCliente = "I",
                H_TipoIDCliente = "N",
                H_NroIDCliente = "13488020",
                H_FechaNac = "19570812"
            };

        }
    }
}
