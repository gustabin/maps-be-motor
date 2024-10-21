using Isban.MapsMB.Common.Entity;
using Isban.MapsMB.DataAccess.DBRequest;
using Isban.MapsMB.DataAccess.DBResponse;
using Isban.Mercados;
using Isban.Mercados.DataAccess;
using Isban.Mercados.DataAccess.OracleClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.MapsMB.DataAccess
{
    [ExcludeFromCodeCoverage]
    public class BaseGsaDa : BaseProxy
    {
        public ChequeoAcceso Chequeo(EntityBase entity)
        {
            var request = entity.MapperClass<ChequeoAccesoSmcRequest>(TypeMapper.IgnoreCaseSensitive);
            var li = this.Provider.GetCollection<ChequeoAccesoResp>(CommandType.StoredProcedure, request);
            if (li.Any())
                return li.First().MapperClass<ChequeoAcceso>(TypeMapper.IgnoreCaseSensitive);
            throw new DBCodeException(-1, $"Error {this.ProviderName}");
        }

        public string GetInfoDB(long id)
        {
            BaseProxySeguridad seg = new BaseProxySeguridad();
            UsuarioPasswordBaseClaves usuario = seg.ObtenerUsuarioBaseDeClaves(id);
            return Encrypt.EncryptToBase64String(string.Format("{0}||{1}", usuario.Usuario, usuario.Password));
        }
    }
}
