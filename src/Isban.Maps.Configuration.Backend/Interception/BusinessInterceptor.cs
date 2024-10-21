namespace Isban.MapsMB.Configuration.Backend.Interception
{
    using Common.Entity;
    using Mercados;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using System;
    using System.Linq;
    using System.Transactions;
    using Isban.Mercados.LogTrace;
    using Isban.Mercados.UnityInject;
    using Isban.MapsMB.Business.Attributes;

    public class BusinessInterceptor : InterceptorBase
    {
        public override IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            if (input.MethodBase.CustomAttributes.Any(x => x.AttributeType.Name == nameof(NonTransactionInterceptorAttribute)))
            {
                foreach (var item in input.Arguments)
                {
                    var validacion = item as EntityBase;
                    if (validacion != null && !validacion.Validar())
                    {
                        var resultRequest = new VirtualMethodReturn(input, new Exception("Error Controlado en Bussiness. Verificar InnerException.", item as Exception));

                        input.CreateMethodReturn(null);
                        return resultRequest;
                    }
                }

                IMethodReturn methodReturn = getNext()(input, getNext);
                InterceptExceptions(methodReturn, input);

                return methodReturn;
            }
            else
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    foreach (var item in input.Arguments)
                    {
                        var validacion = item as EntityBase;
                        if (validacion != null && !validacion.Validar())
                        {
                            var resultRequest = new VirtualMethodReturn(input, new Exception("Error Controlado en Bussiness. Verificar InnerException.", item as Exception));

                            input.CreateMethodReturn(null);
                            return resultRequest;
                        }
                    }

                    IMethodReturn methodReturn = getNext()(input, getNext);
                    if (methodReturn.Exception == null)
                        scope.Complete();
                    InterceptExceptions(methodReturn, input);

                    return methodReturn;
                }
            }
        }

        private void InterceptExceptions(IMethodReturn result, IMethodInvocation input)
        {
            if (result.Exception != null)
            {
                if ((result.Exception as IsbanException) == null)
                {
                    var message = string.Format("Error {0} | Día y Hora: {1}. | Exception: {2}", this.GetType().Name,
                        DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), result.Exception);
                    LoggingHelper.Instance.Error(result.Exception, message);
                    result.Exception = new BusinessException("Error no controlado. Verificar InnerException.", result.Exception);
                }
            }
        }
    }
}
