namespace Isban.Maps.Entity.Interfaces
{
    public interface IFormulario
    {
        string IdServicio { get; set; }

        long? IdSimulacion { get; set; }

        string Comprobante { get; set; }

        string Estado { get; set; }

        string FormAnterior { get; set; }

        long? IdAdhesion { get; set; }

        string Titulo { get; set; }

        string Nup { get; set; }

        string Segmento { get; set; }

        string Canal { get; set; }

        string SubCanal { get; set; }

        string PerfilInversor { get; set; }

        decimal? FormularioId { get; set; }

        string Error_Desc { get; set; }
    }
}
