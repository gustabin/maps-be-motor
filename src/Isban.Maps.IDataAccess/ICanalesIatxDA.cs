using Isban.MapsMB.Common.Entity.Request;
using Isban.MapsMB.Common.Entity.Response;
using Isban.MapsMB.Entity.Request;
using Isban.MapsMB.Entity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.IDataAccesss
{
    public interface ICanalesIatxDA
    {
        DatosCuentaIATXResponse ConsultaDatosCuenta(CabeceraConsulta cabecera, CargaSolicitudOrden soli);

        void CompraVtaBonos();

        void ConsultaPoderDeCompra();

        void ConsultarFondoComunInversion(CargaSolicitudOrden entity);

        SuscripcionFCIIATXResponse SuscripcionFCI(CabeceraConsulta cabecera, string nup, int codigoFondo, string cuentaTitulo, decimal importeBruto, int tipoCuenta, int sucursalCuenta, string nroCuenta, int moneda);

        SuscripcionFCIIATXResponse SuscripcionFCIOBE(CabeceraConsulta cabecera, string nup, int codigoFondo, string cuentaTitulo, decimal importeBruto, int tipoCuenta, int sucursalCuenta, string nroCuenta, int moneda);


        RescateFCIIATXResponse RescateFCIRTL(CabeceraConsulta cabecera, CargaSolicitudOrden solicitud);

        RescateFCIBPResponse RescateFCIBP(CabeceraConsulta cabecera, CargaSolicitudOrden solicitud);

        AltaCuentaResponse AltaCuentaIATX(CabeceraConsulta cabecera, string nup, string numeroCuenta, string codigoMoneda);

        AltaCuentaResponse CMBRELCLICIATX(CabeceraConsulta cabecera, string nup, string numeroCuenta, string ordenCliente);

        AltaCuentaResponse ModificarRelacionClienteContratoIATX(CabeceraConsulta cabecera, RelacionClienteContrato relacion);

        AltaCuentaResponse GeneracionCuentaIATX(CabeceraConsulta cabecera, string nup);

        AltaCuentaResponse ServicioINIATX(CabeceraConsulta cabecera, string nup);

        string AsociarCuentasOperativasIATX(CabeceraConsulta cabecera, WorkflowCTRReq opcion, AsociarCtaOperativaReq asociarCtaOperativaReq);
    }
}
