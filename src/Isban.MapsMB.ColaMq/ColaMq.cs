
namespace Isban.MapsMB.ColaMQ
{
    using Common.Entity.Request;
    using Isban.Common.Connector.MQ.Data;
    using Isban.MapsMB.IDataAccess;
    using Isban.Mercados.UnityInject;

    public class ColaMQ
    {
        public void DejarNovedad(MensajeMyA request)
        {
            using (var proxy = GetProxy())
            {
                var options = new RequestMessageRequest();

                proxy.DejarNovedad(request, options);
            }
        }

        private MqConnectorProxy GetProxy()
        {
            var proxy = new MqConnectorProxy(GetMQSetting());

            return proxy;
        }

        private MqConnectionSetting GetMQSetting()
        {
            var setting = new MqConnectionSetting
            {
                Channel = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerParametro("MQ_CHANNEL"),
                Host = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerParametro("MQ_HOST"),
                CharSet = 537,
                Timeout = 5000,
                Port = int.Parse(DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerParametro("MQ_PORT")),
            };
            var configValue = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerParametro("MQ_TRANSPORT_MODE");
            string transportMode;

            if (!string.IsNullOrEmpty(configValue) && TransportEnum.Mode.TryGetValue(configValue, out transportMode))
                setting.TransportMode = transportMode;

            setting.GetQueue = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerParametro("MQ_OUTPUT");
            setting.PutQueue = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerParametro("MQ_INPUT"); ;
            setting.MqManager = DependencyFactory.Resolve<IMotorServicioDataAccess>().ObtenerParametro("MQ_MANAGER");

            return setting;
        }

    }
}
