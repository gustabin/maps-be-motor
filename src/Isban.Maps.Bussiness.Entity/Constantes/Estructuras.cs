namespace Isban.MapsMB.Entity.Constantes.Estructuras
{
    public struct Constantes
    {
        public const string CtaNoBloqueada = "n";
        public const string CtaBloqueada = "s";
    }

    public struct EstadoFormularioMaps
    {
        public const string Carga = "carga";
    }

    public struct TipoCuenta
    {
        public const string CuentaTitulo = "60";
        public const string CuentaOperativa = "07";
    }

    public struct EstadoSolicitudDeOrden
    {
        public const string FallaTecnica = "FT";
        public const string SinSaldo = "SS";
        public const string Bloqueado = "BL";
        public const string Procesado = "PR";
        public const string Otro = "OT";
        public const string Inicial = "IN";
        public const string SimulacionOrigen = "SI";
        public const string ConfirmacionOrigen = "CO";
        public const string ConfirmacionDeRiesgo = "CR";
        public const string EvaluacionDeRiesgo = "ER";
        public const string BajaDeAdhesion = "BA";
        public const string BajaPorVigencia = "BV";
    }
    public struct Servicio
    {
        public const string SAF = "SAF";
        public const string PoderDeCompra = "PDC";
        public const string Agendamiento = "AGD";
        public const string AgendamientoFH = "AGDFH";
        public const string RTF = "RTF";
        public const string ACT = "ACT";
    }
    public struct TipoSegmento
    {
        public const string BancaPrivada = "BP";
        public const string Retail = "RTL";
    }

    public struct TipoSolicitud
    {
        public const string Rescate = "RE";
        public const string Suscripcion = "SU";
        public const string Transferencia = "TR";
    }

    public struct TipoMoneda
    {
        public const string DolaresUSA = "USD";
        public const string PesosAR = "ARS";
        public const string PesosARG = "ARG";
    }

    public struct ModoEjecucion
    {
        public const string DB = "0";
        public const string NET = "1";
    }

    public struct SiNo
    {
        public const string Si = "S";
        public const string No = "N";
    }

    public struct CodigosOperacion
    {
        public const string Rescate = "RES";
        public const string Suscripcion = "SUS";
    }
}
