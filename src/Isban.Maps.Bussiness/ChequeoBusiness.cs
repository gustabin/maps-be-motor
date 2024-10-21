
namespace Isban.MapsMB.Bussiness
{
    using MapsMB.Common.Entity;
    using MapsMB.IBussiness;
    using MapsMB.IDataAccess;
    using Mercados.UnityInject;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ChequeoBusiness: IChequeoBusiness
    {
        public virtual List<ChequeoAcceso> Chequeo(EntityBase entity)
        {

            List<ChequeoAcceso> lista = new List<ChequeoAcceso>();
            var daSmc = DependencyFactory.Resolve<ISmcDA>();
            var daOpics = DependencyFactory.Resolve<IOpicsDA>();           
            var daMaps = DependencyFactory.Resolve<IMotorServicioDataAccess>();

            try
            {
                var item = daOpics.Chequeo(entity);
                ValidData(lista, daOpics.GetInfoDB, daOpics.ConnectionString, item, "OPICS");
            }
            catch (Exception ex)
            {
                CatchConnEx(lista, daOpics.GetInfoDB, daOpics.ConnectionString, "OPICS", ex);
            }
            try
            {
                var item = daMaps.Chequeo(entity);
                ValidData(lista, daMaps.GetInfoDB, daMaps.ConnectionString, item, "MAPS");
            }
            catch (Exception ex)
            {
                CatchConnEx(lista, daMaps.GetInfoDB, daMaps.ConnectionString, "MAPS", ex);
            }
            try
            {
                var item = daSmc.Chequeo(entity);
                ValidData(lista, daSmc.GetInfoDB, daSmc.ConnectionString, item, "SMC");
            }
            catch (Exception ex)
            {
                CatchConnEx(lista, daSmc.GetInfoDB, daSmc.ConnectionString, "SMC", ex);
            }

            return lista;
        }


        private static void ValidData(List<ChequeoAcceso> lista, Func<long, string> getInfoDB, string connectionString, ChequeoAcceso item, string userDB)
        {
            item.ConnectionString = connectionString;
            try
            {
                if (!string.IsNullOrWhiteSpace(item.ConnectionString) &&
                item.ConnectionString.ToUpper().Contains("CREDENTIALID"))
                {
                    var id = item.ConnectionString.Substring(item.ConnectionString.ToUpper().LastIndexOf("CREDENTIALID")).Split('=')[1];
                    item.Hash = getInfoDB(Convert.ToInt64(id));
                }
                item.Ok = true;
            }
            catch (Exception ex)
            {
                item = new ChequeoAcceso { UsuarioDB = userDB, BasedeDatos = ex.Message, ServidorDB = ex.Message, ServidorWin = ex.Message, UsuarioWin = ex.Message };
            }
            lista.Add(item);
        }

        private static void CatchConnEx(List<ChequeoAcceso> lista, Func<long, string> getInfoDB, string connectionString, string userDB, Exception ex)
        {
            ChequeoAcceso item = new ChequeoAcceso { UsuarioDB = userDB, BasedeDatos = ex.Message, ServidorDB = ex.Message, ServidorWin = ex.Message, UsuarioWin = ex.Message };
            item.ConnectionString = connectionString;
            try
            {
                if (!string.IsNullOrWhiteSpace(item.ConnectionString) &&
                item.ConnectionString.ToUpper().Contains("CREDENTIALID"))
                {
                    var id = item.ConnectionString.Substring(item.ConnectionString.ToUpper().LastIndexOf("CREDENTIALID")).Split('=')[1];
                    item.Hash = getInfoDB(Convert.ToInt64(id));
                }
                item.Ok = true;
            }
            catch (Exception exc)
            {
                item = new ChequeoAcceso { UsuarioDB = userDB, BasedeDatos = exc.Message, ServidorDB = exc.Message, ServidorWin = exc.Message, UsuarioWin = exc.Message };
            }
            lista.Add(item);
        }
    }
}
