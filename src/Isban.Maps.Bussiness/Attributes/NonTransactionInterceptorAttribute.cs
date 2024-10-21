using System;

namespace Isban.MapsMB.Business.Attributes
{
    /// <summary>
    /// class NonTransactionInterceptorAttribute: informa que este metodo maneja su propia transacción y que el interceptor no debe abrir la transacción
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class NonTransactionInterceptorAttribute : Attribute
    {
    }
}
