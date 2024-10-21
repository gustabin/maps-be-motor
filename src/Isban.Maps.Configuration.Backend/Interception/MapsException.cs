using Isban.MapsMB.Common.Entity;
using Isban.Mercados;
using System;

namespace Isban.MapsMB.Configuration.Backend.Interception
{
    public class MapsException : IsbanException
    {
        private MotorEntityBase obj;
        private string v;

        public MapsException(string v, MotorEntityBase obj)
            : base(v)
        {
            this.v = v;
            this.obj = obj;
        }

        public MotorEntityBase ParametroRequest
        {
            get { return obj; }
        }
        public override TipoExcepcion TipoExcepcion
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}