namespace Isban.MapsMB.DataAccess.Constantes
{
    public struct Package
    {
        public const string MapsFormularios = "PKG_MAPS_CTRL_FORMULARIO";
        public const string MapsServicios = "PKG_MAPS_CTRL_SERVICIO";
        public const string MapsHelp = "PKG_MAPS_HELP";
        public const string MapsParametros = "PKG_MAPS_PARAMETROS";
        public const string BpServiciosA3 = "PKG_BP_SERVICIOS_A_3ROS";
        public const string BpOnlineBanking = "PKG_BP_ONLINE_BANKING";
        public const string ORDENES = "PKG_SMC_ORDENES";
        public const string MapsMyA = "PKG_MAPS_MYA";
        public const string MapsMyAMEP = "PKG_MAPS_MYA_MEP";
        public const string Transferencia = "PKG_TRANSFERENCIA_FONDOS";
        public const string MapsTools = "PKG_MAPS_TOOLS";
        public const string MapsCommon = "PKG_MAPS_COMMON ";
        public const string OpicsMaps = "PKG_OPICS_MAPS";
        public const string MapsRtf = "PKG_MAPS_RTF";

        #region Chequeo
        public const string ChequeoSmc = "PKG_SMC_MIDDLEWARE";
        public const string ChequeoDdc = "PKG_DDC_COMMON";
        public const string ChequeoPdc = "PKG_PDC_GENERAL";
        public const string ChequeoPl = "PKG_PL_COMMON";
        #endregion
    }

    public struct Owner
    {
        public const string Maps = "MAPS";
        public const string BCAINV = "BCAINV";
        public const string SMC = "SMC";
        public const string Opics = "OPICS";
        public const string PL = "PL";
    }

    public struct ConstantesIATX
    {
        public const string Iatx = "IATX";

        public const string ConsultaPdcNombreServicio = "CNSEXPPCPA";

        public const string CompraVtaAccionesBonosNombreServicio = "SIMCVTACBO";

        public const string CompraVtaSimulacionDirectaBonosNombreServicio = "CVTAACCBON";

        public const string DirectaCompraVtaAccionesNombreServicio = "CVTAEXTBUR";

        public const string ConsultaSuscripcionFondoComunInversion = "CNSSUSFCI";

        public const string SuscripcionFondoComunInversion = "SUSFCI";

        public const string CONSULTADATOSCUENTA = "CNSCTADATO";

        public const string RescateFCIBP = "RESFCIBCAP";

        public const string RescateFCIRTL = "RESFCI";

        public const string AltaCuenta = "ALTCTATITU100";

        public const string CMBRELCLIC = "CMBRELCLIC";

        public const string GeneracionCuenta = "GENNROCTA_100";

        public const string ConsultaCuentaTitulo = "CNSCTATITU";

        public const string AsociarCuentasOperativas = "CMBCTATITU";

        public const string servicioIN = "CNSINHABIL";
    }
}
