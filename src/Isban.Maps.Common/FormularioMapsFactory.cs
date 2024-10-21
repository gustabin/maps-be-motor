using Isban.Maps.DataAccess.DBResponse;
using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Controles.Customizados;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.Common
{
    public static class FormularioMapsFactory
    {
        private static BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

        public static FormularioResponse ConstruirFormulario(List<ValorCtrlFormularioDbResp> list)
        {
            var form = new FormularioResponse();

            if (list != null && list.Count > 0)
            {
                var frmAttr = list.Where(x => !x.ControlPadreId.HasValue && !x.ControlAtributoPadreId.HasValue);

                decimal frmPadreId = frmAttr.Select(x => x.ControlDefId).FirstOrDefault();
                decimal? frmCtrlID = frmAttr.Where(x => x.AtributoDesc.ToLower().Equals("items")).Select(x => x.ControlId).FirstOrDefault();

                frmAttr.ToList().ForEach(attr =>
                {
                    var propInfo = form.GetType().GetProperty(attr.AtributoDesc, bindFlags);

                    if (propInfo != null)
                        propInfo.SetValue(form, attr.AtributoValor);
                });

                var ctrlLista = list.Where(x => x.ControlPadreId.HasValue
                                             && x.ControlPadreId.Equals(frmPadreId)
                                             && x.ControlAtributoPadreId.HasValue
                                             && x.ControlAtributoPadreId.Equals(frmCtrlID)
                                             && !x.AtributoPadreId.HasValue
                                             );
                var listaItemsTipo = ctrlLista.Where(x => x.AtributoDesc.ToLower().Equals("tipo")).FirstOrDefault();
                var listaDataType = ctrlLista.Where(x => x.AtributoDesc.ToLower().Equals("tipo")).Select(y => y.AtributoDataType).FirstOrDefault().ToType();
                var listatype = typeof(Lista<>).MakeGenericType(listaItemsTipo.AtributoValor.ToControlMaps(listaDataType));  //  typeof(ItemServicio<>).MakeGenericType(listaDataType));
                var listaInstace = Activator.CreateInstance(listatype);

                //setear las lista.

                ctrlLista.ToList().ForEach(atr =>
                {
                    var propInfo = listaInstace.GetType().GetProperty(atr.AtributoDesc, bindFlags);
                    propInfo.SetValue(listaInstace, atr.AtributoValor);

                });

                //TODO: Obtener los items de la lista de cualquier lugar.
                //obtener los items
                decimal? listAttrItemId = ctrlLista.Where(x => x.AtributoDesc.ToLower().Equals("items")
                                             ).Select(y => y.ControlId).FirstOrDefault();

                var itemsLista = list.Where(x => x.ControlPadreId.HasValue
                                             && x.ControlPadreId.Equals(frmPadreId)
                                             && x.ControlAtributoPadreId.HasValue
                                             && x.ControlAtributoPadreId.Equals(frmCtrlID)
                                             && x.AtributoPadreId.HasValue
                                             && x.AtributoPadreId.Equals(listAttrItemId)
                                             ).GroupBy(x => x.GroupId);


                //instancio la lista de servicio
                var newItemLista = listaInstace.GetType().GetProperty("items", bindFlags).PropertyType.GetConstructor(Type.EmptyTypes).Invoke(null);
                var listaItems = listaInstace.GetType().GetProperty("items", bindFlags);
                listaItems.SetValue(listaInstace, newItemLista);

                //creo los items
                itemsLista.ToList().ForEach(itms =>
                {
                    var servtype = typeof(ItemServicio<>).MakeGenericType(listaDataType);
                    var srvInstance = Activator.CreateInstance(servtype);

                    itms.ToList().ForEach(itm =>
                    {
                        var srvpropInfo = srvInstance.GetType().GetProperty(itm.AtributoDesc, bindFlags);
                        var srvpropValue = itm.AtributoValor.ParseGenericVal(srvpropInfo.PropertyType);

                        srvpropInfo.SetValue(srvInstance, srvpropValue);
                    });

                    //agrego un elemento a la lista.
                    var propItemsInstance = listaItems.GetValue(listaInstace, null);
                    listaItems.PropertyType.GetMethod("add", bindFlags).Invoke(propItemsInstance, new[] { srvInstance });
                });

                //setear lista de controles maps

                form.Items = new List<MapsControlBase>();
                form.Items.Add(listaInstace as MapsControlBase);


            }

            return form;
        }

        public static MapsControlBase ConstruirItems(List<ValorCtrlServicioDbResp> list)
        {
            MapsControlBase result = null;

            if (list != null && list.Count > 0)
            {
                var defLista = list.Where(x => x.ControlPadreId.HasValue && x.ControlAtributoPadreId.HasValue && !x.AtributoPadreId.HasValue);

                decimal listaPadreId = defLista.Select(x => x.ControlDefId).FirstOrDefault();
                decimal? listaPropItemID = defLista.Where(x => x.AtributoDesc.ToLower().Equals("items")).Select(x => x.ControlId).FirstOrDefault();

                //var ctrlLista = list.Where(x => x.ControlPadreId.HasValue
                //                             && x.ControlPadreId.Equals(listaPadreId)
                //                             && x.ControlAtributoPadreId.HasValue
                //                             && x.ControlAtributoPadreId.Equals(listaPropItemID)
                //                             && !x.AtributoPadreId.HasValue
                //                             );

                var listaDataType = defLista.Where(x => x.AtributoDesc.ToLower().Equals("tipo"))
                                        .Select(y => y.AtributoDataType)
                                        .FirstOrDefault()
                                        .ToType();
                var listatype = typeof(Lista<>).MakeGenericType(typeof(ItemServicio<>).MakeGenericType(listaDataType));
                var listaInstace = Activator.CreateInstance(listatype);

                //setear las lista.

                defLista.ToList().ForEach(atr =>
                {
                    var propInfo = listaInstace.GetType().GetProperty(atr.AtributoDesc, bindFlags);
                    propInfo.SetValue(listaInstace, atr.AtributoValor);

                });

                //obtener los items 
                var itemsLista = list.Where(x => x.AtributoPadreId.HasValue
                                             && x.AtributoPadreId.Equals(listaPropItemID)
                                             && x.ControlDefId.Equals(listaPadreId)
                                             && x.ControlPadreId.HasValue
                                             && x.ControlPadreId.Equals(defLista.FirstOrDefault().ControlPadreId)
                                             && x.ControlAtributoPadreId.HasValue
                                             && x.ControlAtributoPadreId.Equals(defLista.FirstOrDefault().ControlAtributoPadreId)
                                             ).GroupBy(x => x.GroupId);


                //instanacio la lista de servicio
                var newItemLista = listaInstace.GetType().GetProperty("items", bindFlags).PropertyType.GetConstructor(Type.EmptyTypes).Invoke(null);
                var listaItems = listaInstace.GetType().GetProperty("items", bindFlags);
                listaItems.SetValue(listaInstace, newItemLista);

                //creo los items
                itemsLista.ToList().ForEach(itms =>
                {
                    var servtype = typeof(ItemServicio<>).MakeGenericType(listaDataType);
                    var srvInstance = Activator.CreateInstance(servtype);

                    itms.ToList().ForEach(itm =>
                    {
                        var srvpropInfo = srvInstance.GetType().GetProperty(itm.AtributoDesc, bindFlags);
                        var srvpropValue = itm.AtributoValor.ParseGenericVal(srvpropInfo.PropertyType);

                        srvpropInfo.SetValue(srvInstance, srvpropValue);
                    });

                    //agrego un elemento a la lista.
                    var propItemsInstance = listaItems.GetValue(listaInstace, null);
                    listaItems.PropertyType.GetMethod("add", bindFlags).Invoke(propItemsInstance, new[] { srvInstance });
                });

                //convierto la instancia al tipo control maps.
                result = listaInstace as MapsControlBase;
            }

            return result;
        }

    }
}
