namespace Isban.MapsMB.Entity.Response
{
    using Common.Entity;
    using Isban.MapsMB.Entity.Constantes.Estructuras;
    using System;
    using System.Runtime.Serialization;

    [System.Diagnostics.DebuggerDisplay("IdSol = {IdSolicitudOrdenes}")]
    [DataContract]
    public class CargaSolicitudOrden : EntityBase
    {
        [DataMember]
        public long IdSolicitudOrdenes { get; set; }

        [DataMember]
        public long IdAdhesion { get; set; }

        [DataMember]
        public long? CodAltaAdhesion { get; set; }

        [DataMember]
        public string IdServicio { get; set; }

        [DataMember]
        public long CuentaTitulos { get; set; }

        [DataMember]
        public long SucuCtaOper { get; set; }

        [DataMember]
        public long NroCtaOper { get; set; }

        [DataMember]
        public long TipoCtaOper { get; set; }

        [DataMember]
        public decimal? SaldoCuentaAntes { get; set; }

        [DataMember]
        public string CodMoneda { get; set; }

        [DataMember]
        public DateTime FechaProceso { get; set; }

        [DataMember]
        public long InReproceso { get; set; }

        [DataMember]
        public string CodEstadoProceso { get; set; }

        [DataMember]
        public string Observacion { get; set; }

        [DataMember]
        public DateTime FecVigenciaDesde { get; set; }

        [DataMember]
        public DateTime FecVigenciaHasta { get; set; }

        [DataMember]
        public decimal? SaldoMinOperacion { get; set; }

        [DataMember]
        public decimal? SaldoMaxOperacion { get; set; }

        [DataMember]
        public decimal? SaldoEnviado { get; set; }

        [DataMember]
        public string CodigoFondo { get; set; }

        [DataMember]
        public string CodEspecie { get; set; }

        [DataMember]
        public long SaldoMinPorFondo { get; set; }

        [DataMember]
        public decimal? SaldoRescatePorFondo { get; set; }

        [DataMember]
        public long NumOrdenOrigen { get; set; }

        [DataMember]
        public long IdEvaluacion { get; set; }

        [DataMember]
        public long TipoDisclaimer { get; set; }

        [DataMember]
        public string TextoDisclaimer { get; set; }

        [DataMember]
        public decimal SaldoMinDejarCta { get; set; }

        [DataMember]
        public string CodProducto { get; set; }

        [DataMember]
        public string CodCanal { get; set; }

        [DataMember]
        public string Producto { get; set; }

        [DataMember]
        public string TipoOperacion { get; set; }

        [DataMember]
        public int CompraVenta { get; set; }

        [DataMember]
        public DateTime FechaBaja { get; set; }

        [DataMember]
        public DateTime FechaDeEjecucion { get; set; }

        [DataMember]
        public string Operacion { get; set; }

        [DataMember]
        public bool BajaTercerIntento { get; set; }

        [DataMember]
        public string Documento { get; set; }

        [DataMember]
        public string TipoDocumento { get; set; }

        [DataMember]
        public string NroCtaOperativaFormateada
        {
            get
            {
                if (Segmento == TipoSegmento.BancaPrivada)
                    return CodProducto + NroCtaOper.ToString();
                else
                    return NroCtaOper.ToString();
            }
        }


    }

    [DataContract]
    public class SubscripcionFondoResponse : EntityBase
    {
        [DataMember]
        public string Estado { get; set; }

        [DataMember]
        public long CuentaTitulos { get; set; }

        [DataMember]
        public long SucuCtaOper { get; set; }

        [DataMember]
        public long NroCtaOper { get; set; }

        [DataMember]
        public long TipoCtaOper { get; set; }


        [DataMember]
        public string CodMoneda { get; set; }

        [DataMember]
        public DateTime FechaProceso { get; set; }

        [DataMember]
        public string CodEstadoProceso { get; set; }


        [DataMember]
        public decimal? SaldoEnviado { get; set; }

        [DataMember]
        public string CodigoFondo { get; set; }

        [DataMember]
        public string CodEspecie { get; set; }


        [DataMember]
        public decimal? SaldoRescatePorFondo { get; set; }
    
        [DataMember]
        public string CodProducto { get; set; }

        [DataMember]
        public string CodCanal { get; set; }

        [DataMember]
        public string Producto { get; set; }

        [DataMember]
        public string TipoOperacion { get; set; }

        [DataMember]
        public int CompraVenta { get; set; }

        [DataMember]
        public string Documento { get; set; }

        [DataMember]
        public string NroCtaOperativaFormateada
        {
            get
            {
                if (Segmento == TipoSegmento.BancaPrivada)
                    return CodProducto + NroCtaOper.ToString();
                else
                    return NroCtaOper.ToString();
            }
        }


    }
}
