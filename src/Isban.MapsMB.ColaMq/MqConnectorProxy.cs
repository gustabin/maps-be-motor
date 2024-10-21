using Isban.Common.Connector;
using Isban.Common.Connector.MQ.Data;
using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace Isban.MapsMB.ColaMQ
{
    //Clase proxy demo
    public class MqConnectorProxy : ConnectorProxy, IDisposable
    {
        private bool disposed;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public MqConnectorProxy(MqConnectionSetting setting)
            : base(new XmlMessageParser(), new MQProcessor(setting))
        {

        }

        public PutResult DejarNovedad(object request, RequestMessageRequest options)
        {
            var parsedObject = Parser.Serialize(request);

            return (PutResult)Processor.Send(parsedObject, options);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }
    }
}
