namespace Isban.PDC.Middleware.DataAccess.Requests
{
    public class DirectaCompraVtaBonosIatxRequest : CompraVtaIatxRequestBase
    {
        public string CanalCodigo { get; set; }
        public string Plazo { get; set; }
        public string OperadorAlta { get; set; }
        public string Instancia { get; set; }    
        public string PrecioClean { get; set; }

        public override void Format()
        {
            base.Format();
            CanalCodigo = CanalCodigo.PadRight(3, ' ');
            Plazo = Plazo.PadLeft(3, '0');
            OperadorAlta = OperadorAlta.PadLeft(4, ' ');
        }
    }
}
