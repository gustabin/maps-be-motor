﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Isban.MapsMB.Business.MESACReportesService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="BaseParameter", Namespace="http://schemas.datacontract.org/2004/07/ISBAN.MESAC.ServiceContracts.Parameters")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Isban.MapsMB.Business.MESACReportesService.EnviarMailsWinParameter))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Isban.MapsMB.Business.MESACReportesService.ObtenerConfiguracionParameter))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Isban.MapsMB.Business.MESACReportesService.ObtenerPropiedadParameter))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Isban.MapsMB.Business.MESACReportesService.EnviarMensajeParameter))]
    public partial class BaseParameter : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IpField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UsuarioField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Ip {
            get {
                return this.IpField;
            }
            set {
                if ((object.ReferenceEquals(this.IpField, value) != true)) {
                    this.IpField = value;
                    this.RaisePropertyChanged("Ip");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Usuario {
            get {
                return this.UsuarioField;
            }
            set {
                if ((object.ReferenceEquals(this.UsuarioField, value) != true)) {
                    this.UsuarioField = value;
                    this.RaisePropertyChanged("Usuario");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EnviarMailsWinParameter", Namespace="http://schemas.datacontract.org/2004/07/ISBAN.MESAC.ServiceContracts.Parameters")]
    [System.SerializableAttribute()]
    public partial class EnviarMailsWinParameter : Isban.MapsMB.Business.MESACReportesService.BaseParameter {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ObtenerConfiguracionParameter", Namespace="http://schemas.datacontract.org/2004/07/ISBAN.MESAC.ServiceContracts.Parameters")]
    [System.SerializableAttribute()]
    public partial class ObtenerConfiguracionParameter : Isban.MapsMB.Business.MESACReportesService.BaseParameter {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AplicativoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ServidorField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Aplicativo {
            get {
                return this.AplicativoField;
            }
            set {
                if ((object.ReferenceEquals(this.AplicativoField, value) != true)) {
                    this.AplicativoField = value;
                    this.RaisePropertyChanged("Aplicativo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Servidor {
            get {
                return this.ServidorField;
            }
            set {
                if ((object.ReferenceEquals(this.ServidorField, value) != true)) {
                    this.ServidorField = value;
                    this.RaisePropertyChanged("Servidor");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ObtenerPropiedadParameter", Namespace="http://schemas.datacontract.org/2004/07/ISBAN.MESAC.ServiceContracts.Parameters")]
    [System.SerializableAttribute()]
    public partial class ObtenerPropiedadParameter : Isban.MapsMB.Business.MESACReportesService.BaseParameter {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CodigoField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Codigo {
            get {
                return this.CodigoField;
            }
            set {
                if ((object.ReferenceEquals(this.CodigoField, value) != true)) {
                    this.CodigoField = value;
                    this.RaisePropertyChanged("Codigo");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EnviarMensajeParameter", Namespace="http://schemas.datacontract.org/2004/07/ISBAN.MESAC.ServiceContracts.Parameters")]
    [System.SerializableAttribute()]
    public partial class EnviarMensajeParameter : Isban.MapsMB.Business.MESACReportesService.BaseParameter {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool AcuseLecturaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool AcuseReciboField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] AdjuntoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] AdjuntosBase64StringField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AsuntoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] CopiaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] CopiaOcultaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmisorField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool EsHtmlField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MensajeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] NombreAdjuntoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PrioridadField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] ReceptoresField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ResponderAField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TipoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] TipoAdjuntoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UsuarioMailField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool AcuseLectura {
            get {
                return this.AcuseLecturaField;
            }
            set {
                if ((this.AcuseLecturaField.Equals(value) != true)) {
                    this.AcuseLecturaField = value;
                    this.RaisePropertyChanged("AcuseLectura");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool AcuseRecibo {
            get {
                return this.AcuseReciboField;
            }
            set {
                if ((this.AcuseReciboField.Equals(value) != true)) {
                    this.AcuseReciboField = value;
                    this.RaisePropertyChanged("AcuseRecibo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] Adjunto {
            get {
                return this.AdjuntoField;
            }
            set {
                if ((object.ReferenceEquals(this.AdjuntoField, value) != true)) {
                    this.AdjuntoField = value;
                    this.RaisePropertyChanged("Adjunto");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] AdjuntosBase64String {
            get {
                return this.AdjuntosBase64StringField;
            }
            set {
                if ((object.ReferenceEquals(this.AdjuntosBase64StringField, value) != true)) {
                    this.AdjuntosBase64StringField = value;
                    this.RaisePropertyChanged("AdjuntosBase64String");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Asunto {
            get {
                return this.AsuntoField;
            }
            set {
                if ((object.ReferenceEquals(this.AsuntoField, value) != true)) {
                    this.AsuntoField = value;
                    this.RaisePropertyChanged("Asunto");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] Copia {
            get {
                return this.CopiaField;
            }
            set {
                if ((object.ReferenceEquals(this.CopiaField, value) != true)) {
                    this.CopiaField = value;
                    this.RaisePropertyChanged("Copia");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] CopiaOculta {
            get {
                return this.CopiaOcultaField;
            }
            set {
                if ((object.ReferenceEquals(this.CopiaOcultaField, value) != true)) {
                    this.CopiaOcultaField = value;
                    this.RaisePropertyChanged("CopiaOculta");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Emisor {
            get {
                return this.EmisorField;
            }
            set {
                if ((object.ReferenceEquals(this.EmisorField, value) != true)) {
                    this.EmisorField = value;
                    this.RaisePropertyChanged("Emisor");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool EsHtml {
            get {
                return this.EsHtmlField;
            }
            set {
                if ((this.EsHtmlField.Equals(value) != true)) {
                    this.EsHtmlField = value;
                    this.RaisePropertyChanged("EsHtml");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Mensaje {
            get {
                return this.MensajeField;
            }
            set {
                if ((object.ReferenceEquals(this.MensajeField, value) != true)) {
                    this.MensajeField = value;
                    this.RaisePropertyChanged("Mensaje");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] NombreAdjunto {
            get {
                return this.NombreAdjuntoField;
            }
            set {
                if ((object.ReferenceEquals(this.NombreAdjuntoField, value) != true)) {
                    this.NombreAdjuntoField = value;
                    this.RaisePropertyChanged("NombreAdjunto");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Prioridad {
            get {
                return this.PrioridadField;
            }
            set {
                if ((object.ReferenceEquals(this.PrioridadField, value) != true)) {
                    this.PrioridadField = value;
                    this.RaisePropertyChanged("Prioridad");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] Receptores {
            get {
                return this.ReceptoresField;
            }
            set {
                if ((object.ReferenceEquals(this.ReceptoresField, value) != true)) {
                    this.ReceptoresField = value;
                    this.RaisePropertyChanged("Receptores");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ResponderA {
            get {
                return this.ResponderAField;
            }
            set {
                if ((object.ReferenceEquals(this.ResponderAField, value) != true)) {
                    this.ResponderAField = value;
                    this.RaisePropertyChanged("ResponderA");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Tipo {
            get {
                return this.TipoField;
            }
            set {
                if ((object.ReferenceEquals(this.TipoField, value) != true)) {
                    this.TipoField = value;
                    this.RaisePropertyChanged("Tipo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] TipoAdjunto {
            get {
                return this.TipoAdjuntoField;
            }
            set {
                if ((object.ReferenceEquals(this.TipoAdjuntoField, value) != true)) {
                    this.TipoAdjuntoField = value;
                    this.RaisePropertyChanged("TipoAdjunto");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UsuarioMail {
            get {
                return this.UsuarioMailField;
            }
            set {
                if ((object.ReferenceEquals(this.UsuarioMailField, value) != true)) {
                    this.UsuarioMailField = value;
                    this.RaisePropertyChanged("UsuarioMail");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MESACServiceFault", Namespace="http://schemas.datacontract.org/2004/07/ISBAN.MESAC.ServiceContracts")]
    [System.SerializableAttribute()]
    public partial class MESACServiceFault : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int CodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MessageField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Code {
            get {
                return this.CodeField;
            }
            set {
                if ((this.CodeField.Equals(value) != true)) {
                    this.CodeField = value;
                    this.RaisePropertyChanged("Code");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Message {
            get {
                return this.MessageField;
            }
            set {
                if ((object.ReferenceEquals(this.MessageField, value) != true)) {
                    this.MessageField = value;
                    this.RaisePropertyChanged("Message");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Configuracion", Namespace="http://schemas.datacontract.org/2004/07/ISBAN.MESAC.Entity.Entity.Reportes")]
    [System.SerializableAttribute()]
    public partial class Configuracion : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string Actualizark__BackingFieldField;
        
        private string Aplicativok__BackingFieldField;
        
        private string Estadok__BackingFieldField;
        
        private System.Nullable<System.DateTime> FechaActualk__BackingFieldField;
        
        private System.Nullable<System.DateTime> FechaAltak__BackingFieldField;
        
        private string Servidork__BackingFieldField;
        
        private string SrvActivok__BackingFieldField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Actualizar>k__BackingField", IsRequired=true)]
        public string Actualizark__BackingField {
            get {
                return this.Actualizark__BackingFieldField;
            }
            set {
                if ((object.ReferenceEquals(this.Actualizark__BackingFieldField, value) != true)) {
                    this.Actualizark__BackingFieldField = value;
                    this.RaisePropertyChanged("Actualizark__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Aplicativo>k__BackingField", IsRequired=true)]
        public string Aplicativok__BackingField {
            get {
                return this.Aplicativok__BackingFieldField;
            }
            set {
                if ((object.ReferenceEquals(this.Aplicativok__BackingFieldField, value) != true)) {
                    this.Aplicativok__BackingFieldField = value;
                    this.RaisePropertyChanged("Aplicativok__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Estado>k__BackingField", IsRequired=true)]
        public string Estadok__BackingField {
            get {
                return this.Estadok__BackingFieldField;
            }
            set {
                if ((object.ReferenceEquals(this.Estadok__BackingFieldField, value) != true)) {
                    this.Estadok__BackingFieldField = value;
                    this.RaisePropertyChanged("Estadok__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<FechaActual>k__BackingField", IsRequired=true)]
        public System.Nullable<System.DateTime> FechaActualk__BackingField {
            get {
                return this.FechaActualk__BackingFieldField;
            }
            set {
                if ((this.FechaActualk__BackingFieldField.Equals(value) != true)) {
                    this.FechaActualk__BackingFieldField = value;
                    this.RaisePropertyChanged("FechaActualk__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<FechaAlta>k__BackingField", IsRequired=true)]
        public System.Nullable<System.DateTime> FechaAltak__BackingField {
            get {
                return this.FechaAltak__BackingFieldField;
            }
            set {
                if ((this.FechaAltak__BackingFieldField.Equals(value) != true)) {
                    this.FechaAltak__BackingFieldField = value;
                    this.RaisePropertyChanged("FechaAltak__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<Servidor>k__BackingField", IsRequired=true)]
        public string Servidork__BackingField {
            get {
                return this.Servidork__BackingFieldField;
            }
            set {
                if ((object.ReferenceEquals(this.Servidork__BackingFieldField, value) != true)) {
                    this.Servidork__BackingFieldField = value;
                    this.RaisePropertyChanged("Servidork__BackingField");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Name="<SrvActivo>k__BackingField", IsRequired=true)]
        public string SrvActivok__BackingField {
            get {
                return this.SrvActivok__BackingFieldField;
            }
            set {
                if ((object.ReferenceEquals(this.SrvActivok__BackingFieldField, value) != true)) {
                    this.SrvActivok__BackingFieldField = value;
                    this.RaisePropertyChanged("SrvActivok__BackingField");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MESACReportesService.IMESACReportesService")]
    public interface IMESACReportesService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMESACReportesService/CrearMensaje", ReplyAction="http://tempuri.org/IMESACReportesService/CrearMensajeResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Isban.MapsMB.Business.MESACReportesService.MESACServiceFault), Action="http://tempuri.org/IMESACReportesService/CrearMensajeMESACServiceFaultFault", Name="MESACServiceFault", Namespace="http://schemas.datacontract.org/2004/07/ISBAN.MESAC.ServiceContracts")]
        string CrearMensaje(Isban.MapsMB.Business.MESACReportesService.EnviarMensajeParameter parameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMESACReportesService/CrearMensaje", ReplyAction="http://tempuri.org/IMESACReportesService/CrearMensajeResponse")]
        System.Threading.Tasks.Task<string> CrearMensajeAsync(Isban.MapsMB.Business.MESACReportesService.EnviarMensajeParameter parameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMESACReportesService/EnviarMailsWin", ReplyAction="http://tempuri.org/IMESACReportesService/EnviarMailsWinResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Isban.MapsMB.Business.MESACReportesService.MESACServiceFault), Action="http://tempuri.org/IMESACReportesService/EnviarMailsWinMESACServiceFaultFault", Name="MESACServiceFault", Namespace="http://schemas.datacontract.org/2004/07/ISBAN.MESAC.ServiceContracts")]
        string EnviarMailsWin(Isban.MapsMB.Business.MESACReportesService.EnviarMailsWinParameter parameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMESACReportesService/EnviarMailsWin", ReplyAction="http://tempuri.org/IMESACReportesService/EnviarMailsWinResponse")]
        System.Threading.Tasks.Task<string> EnviarMailsWinAsync(Isban.MapsMB.Business.MESACReportesService.EnviarMailsWinParameter parameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMESACReportesService/ObtenerConfiguracion", ReplyAction="http://tempuri.org/IMESACReportesService/ObtenerConfiguracionResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Isban.MapsMB.Business.MESACReportesService.MESACServiceFault), Action="http://tempuri.org/IMESACReportesService/ObtenerConfiguracionMESACServiceFaultFau" +
            "lt", Name="MESACServiceFault", Namespace="http://schemas.datacontract.org/2004/07/ISBAN.MESAC.ServiceContracts")]
        Isban.MapsMB.Business.MESACReportesService.Configuracion ObtenerConfiguracion(Isban.MapsMB.Business.MESACReportesService.ObtenerConfiguracionParameter parameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMESACReportesService/ObtenerConfiguracion", ReplyAction="http://tempuri.org/IMESACReportesService/ObtenerConfiguracionResponse")]
        System.Threading.Tasks.Task<Isban.MapsMB.Business.MESACReportesService.Configuracion> ObtenerConfiguracionAsync(Isban.MapsMB.Business.MESACReportesService.ObtenerConfiguracionParameter parameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMESACReportesService/ObtenerPropiedad", ReplyAction="http://tempuri.org/IMESACReportesService/ObtenerPropiedadResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Isban.MapsMB.Business.MESACReportesService.MESACServiceFault), Action="http://tempuri.org/IMESACReportesService/ObtenerPropiedadMESACServiceFaultFault", Name="MESACServiceFault", Namespace="http://schemas.datacontract.org/2004/07/ISBAN.MESAC.ServiceContracts")]
        string ObtenerPropiedad(Isban.MapsMB.Business.MESACReportesService.ObtenerPropiedadParameter parameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMESACReportesService/ObtenerPropiedad", ReplyAction="http://tempuri.org/IMESACReportesService/ObtenerPropiedadResponse")]
        System.Threading.Tasks.Task<string> ObtenerPropiedadAsync(Isban.MapsMB.Business.MESACReportesService.ObtenerPropiedadParameter parameter);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMESACReportesServiceChannel : Isban.MapsMB.Business.MESACReportesService.IMESACReportesService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MESACReportesServiceClient : System.ServiceModel.ClientBase<Isban.MapsMB.Business.MESACReportesService.IMESACReportesService>, Isban.MapsMB.Business.MESACReportesService.IMESACReportesService {
        
        public MESACReportesServiceClient() {
        }
        
        public MESACReportesServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MESACReportesServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MESACReportesServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MESACReportesServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string CrearMensaje(Isban.MapsMB.Business.MESACReportesService.EnviarMensajeParameter parameter) {
            return base.Channel.CrearMensaje(parameter);
        }
        
        public System.Threading.Tasks.Task<string> CrearMensajeAsync(Isban.MapsMB.Business.MESACReportesService.EnviarMensajeParameter parameter) {
            return base.Channel.CrearMensajeAsync(parameter);
        }
        
        public string EnviarMailsWin(Isban.MapsMB.Business.MESACReportesService.EnviarMailsWinParameter parameter) {
            return base.Channel.EnviarMailsWin(parameter);
        }
        
        public System.Threading.Tasks.Task<string> EnviarMailsWinAsync(Isban.MapsMB.Business.MESACReportesService.EnviarMailsWinParameter parameter) {
            return base.Channel.EnviarMailsWinAsync(parameter);
        }
        
        public Isban.MapsMB.Business.MESACReportesService.Configuracion ObtenerConfiguracion(Isban.MapsMB.Business.MESACReportesService.ObtenerConfiguracionParameter parameter) {
            return base.Channel.ObtenerConfiguracion(parameter);
        }
        
        public System.Threading.Tasks.Task<Isban.MapsMB.Business.MESACReportesService.Configuracion> ObtenerConfiguracionAsync(Isban.MapsMB.Business.MESACReportesService.ObtenerConfiguracionParameter parameter) {
            return base.Channel.ObtenerConfiguracionAsync(parameter);
        }
        
        public string ObtenerPropiedad(Isban.MapsMB.Business.MESACReportesService.ObtenerPropiedadParameter parameter) {
            return base.Channel.ObtenerPropiedad(parameter);
        }
        
        public System.Threading.Tasks.Task<string> ObtenerPropiedadAsync(Isban.MapsMB.Business.MESACReportesService.ObtenerPropiedadParameter parameter) {
            return base.Channel.ObtenerPropiedadAsync(parameter);
        }
    }
}
