

namespace Isban.Maps.IBussiness
{
    using Entity.Response;
    public interface IMapsServiciosBusiness
    {
        FormularioResponse AltaAdhesion(FormularioResponse entity);
        FormularioResponse BajaAdhesion(FormularioResponse entity);
        FormularioResponse ConsultaAdhesion(FormularioResponse entity);
        FormularioResponse ConsultaDeServicios(FormularioResponse formularioResponse);
    }
}
