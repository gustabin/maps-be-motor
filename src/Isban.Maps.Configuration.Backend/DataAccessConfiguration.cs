
namespace Isban.MapsMB.Configuration.Backend
{
    using DataAccess;
    using IDataAccess;
    using MapsMB.Configuration.Backend.Interception;
    using MapsMB.IDataAccesss;
    using Mercados.Configuration;
    using Mercados.UnityInject;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;

    public class DataAccessConfiguration : ISetUp
    {

        public void Init()
        {
            DependencyFactory.RegisterType<IMotorServicioDataAccess, MotorServicioDataAccess>(new PerThreadLifetimeManager(),
              new InjectionMember[]
              {
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior<DataAccessInterceptor>()
              }
              );

            DependencyFactory.RegisterType<ICanalesIatxDA, CanalesIatxDA>(new PerThreadLifetimeManager(),
              new InjectionMember[]
              {
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior<DataAccessIatxInterceptor>()
              }
              );

            DependencyFactory.RegisterType<ISmcDA, SmcDA>(new PerThreadLifetimeManager(),
           new InjectionMember[]
           {
                     new Interceptor<VirtualMethodInterceptor>(),
                     new InterceptionBehavior<DataAccessInterceptor>()
           }
           );

            DependencyFactory.RegisterType<IOpicsDA, OpicsDA>(new PerThreadLifetimeManager(),
         new InjectionMember[]
         {
                     new Interceptor<VirtualMethodInterceptor>(),
                     new InterceptionBehavior<DataAccessInterceptor>()
         }
         );
            DependencyFactory.RegisterType<IPLDA, PLDA>(new PerThreadLifetimeManager(),
         new InjectionMember[]
         {
                     new Interceptor<VirtualMethodInterceptor>(),
                     new InterceptionBehavior<DataAccessInterceptor>()
         }
         );

        }
        public void Dispose()
        {
            DependencyFactory.ClearContainer();
        }
    }
}
