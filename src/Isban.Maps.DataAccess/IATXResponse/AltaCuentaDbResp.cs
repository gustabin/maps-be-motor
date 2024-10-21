using Isban.Common.Data;
using Isban.Common.Data.Providers.IATX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.Common.Entity.Response
{

    [IATXCollectionDefinition(TableName = "Output")]
    public class AltaCuentaDbResp : IContract
    {
        //[DBFieldDefinition(Name = "Código_retorno_extendido")]
        public string Cod_retorno { get; set; }

        //[DBFieldDefinition(Name = "Cod_retorno")]
        public string Nup { get; set; }

    }


    [IATXCollectionDefinition(TableName = "Output")]
    public class GeneracionCuentaDbResp : IContract
    {
        [DBFieldDefinition(Name = "Código_retorno_extendido")]
        public string Código_retorno_extendido { get; set; }

    }

    [DataContract]
    [IATXCollectionDefinition(TableName = "Output")]
    public class ConsultaCuentaDbResp : IContract
    {
        [DBFieldDefinition(Name = "Ceros", ValueConverter = typeof(string))]
        [DataMember]
        public string Código_retorno_extendido { get; set; }
        //[DBFieldDefinition(Name = "UsuarioCarga", ValueConverter = typeof(string))]
        //[DataMember]
        //public string UsuarioCarga { get; set; }
        //[DBFieldDefinition(Name = "UsuarioV", ValueConverter = typeof(string))]
        //[DataMember]
        //public string UsuarioV { get; set; }
        //[DBFieldDefinition(Name = "SucursalOrigen", ValueConverter = typeof(string))]
        //[DataMember]
        //public string SucursalOrigen { get; set; }
        //[DBFieldDefinition(Name = "CT", ValueConverter = typeof(string))]
        //[DataMember]
        //public string CT { get; set; }
        //[DBFieldDefinition(Name = "CC", ValueConverter = typeof(string))]
        //[DataMember]
        //public string CC { get; set; }
        //[DBFieldDefinition(Name = "CA", ValueConverter = typeof(string))]
        //[DataMember]
        //public string CA { get; set; }
        //[DBFieldDefinition(Name = "CD", ValueConverter = typeof(string))]
        //[DataMember]
        //public string CD { get; set; }
        //[DBFieldDefinition(Name = "ST", ValueConverter = typeof(string))]
        //[DataMember]
        //public string ST { get; set; }

    }


    [IATXCollectionDefinition(TableName = "Output")]
    public class CMBRECLCIResp : IContract
    {
        public string Cod_retorno { get; set; }

    }

}
