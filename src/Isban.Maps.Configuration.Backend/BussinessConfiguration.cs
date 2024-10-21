
namespace Isban.MapsMB.Configuration.Backend
{
    using Business;
    using IBusiness;
    using Isban.MapsMB.Bussiness;
    using Isban.MapsMB.Configuration.Backend.Interception;
    using Isban.MapsMB.IBussiness;
    using Isban.Mercados.Configuration;
    using Isban.Mercados.UnityInject;
    using Isban.Mercados.WebApiClient;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;

    public class BussinessConfiguration : ISetUp
    {
        public void Init()
        {
            DependencyFactory.RegisterType<IChequeoBusiness, ChequeoBusiness>(
            new InjectionMember[]
            {
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior<BusinessInterceptor>()
            }
            );

            DependencyFactory.RegisterType<ICallWebApi, CallWebApi>(
               new InjectionMember[]
               {
                    new Interceptor<VirtualMethodInterceptor>()
                    //TODO: hacer un interceptor para las web api.
                    //new InterceptionBehavior<BusinessInterceptor>()
               }
               );

            DependencyFactory.RegisterType<IMotorBusiness, MotorBusiness>(
               new InjectionMember[]
               {
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior<BusinessInterceptor>()
               }
               );

            DependencyFactory.RegisterType<IExternalWebApiService, ExternalWebApiService>(
             new InjectionMember[]
             {
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior<BusinessInterceptor>()
             }
             );

            DependencyFactory.RegisterType<IMotorAgdSuscripcionBusiness, MotorAgdSuscripcionBusiness>(
             new InjectionMember[]
             {
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior<BusinessInterceptor>()
             }
             );
            DependencyFactory.RegisterType<ICuentasBusiness, CuentasBusiness>(
            new InjectionMember[]
            {
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior<BusinessInterceptor>()
            }
            );
        }

        public void Dispose()
        {
            DependencyFactory.ClearContainer();
        }
    }
}
