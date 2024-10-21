using Newtonsoft.Json;

namespace Isban.Maps.Entity.Base
{
    public interface IError
    {       
        decimal Error { get; set; }
               
        string Error_Desc { get; set; }
       
        string Error_Tecnico { get; set; }     

    }
}
