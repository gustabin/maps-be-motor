
namespace Isban.Maps.IBussiness
{
    using Entity.Controles.Customizados;
    using Isban.Maps.Entity.Controles;
    using Newtonsoft.Json.Linq;

    public interface IControlesBusiness
    {
        JObject ObtenerControlInputNumber(InputNumber<int> datos);
        JObject ObtenerControlFondos(Lista<ItemFondo<string>> datos);
    }
}
