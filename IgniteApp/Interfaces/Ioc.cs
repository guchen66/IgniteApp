using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteApp.Interfaces
{
    public sealed partial class Ioc : StyletIoC.IContainer
    {
        /// <summary>
        /// Gets the default <see cref="Ioc"/> instance.
        /// </summary>
        public static Ioc Default { get; } = new Ioc();

        /// <summary>
        /// The <see cref="IServiceProvider"/> instance to use, if initialized.
        /// </summary>
        private volatile IContainer serviceProvider;

        /// <inheritdoc/>
        public object GetService(Type serviceType)
        {
            IContainer provider = this.serviceProvider;

            if (provider is null)
            {
                ThrowInvalidOperationExceptionForMissingInitialization();
            }

            return provider.Get(serviceType);
        }

        public T GetService<T>()
            where T : class
        {
            IContainer provider = this.serviceProvider;

            if (provider is null)
            {
                ThrowInvalidOperationExceptionForMissingInitialization();
            }

            return (T)provider.Get(typeof(T));
        }

        public T GetRequiredService<T>()
            where T : class
        {
            IContainer provider = this.serviceProvider;

            if (provider is null)
            {
                ThrowInvalidOperationExceptionForMissingInitialization();
            }

            T service = (T)provider.Get(typeof(T));

            if (service is null)
            {
                ThrowInvalidOperationExceptionForUnregisteredType();
            }

            return service;
        }

        public void ConfigureServices(IContainer serviceProvider)
        {
            //  ArgumentNullException.ThrowIfNull(serviceProvider);

            IContainer oldServices = Interlocked.CompareExchange(ref this.serviceProvider, serviceProvider, null);

            if (!(oldServices is null))
            {
                ThrowInvalidOperationExceptionForRepeatedConfiguration();
            }
        }

        private static void ThrowInvalidOperationExceptionForMissingInitialization()
        {
            throw new InvalidOperationException("The service provider has not been configured yet.");
        }

        private static void ThrowInvalidOperationExceptionForUnregisteredType()
        {
            throw new InvalidOperationException("The requested service type was not registered.");
        }

        private static void ThrowInvalidOperationExceptionForRepeatedConfiguration()
        {
            throw new InvalidOperationException("The default service provider has already been configured.");
        }

        public void Compile(bool throwOnError = true)
        {
            throw new NotImplementedException();
        }

        public object Get(Type type, string key = null)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetAll(Type type, string key = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll<T>(string key = null)
        {
            throw new NotImplementedException();
        }

        public object GetTypeOrAll(Type type, string key = null)
        {
            throw new NotImplementedException();
        }

        public T GetTypeOrAll<T>(string key = null)
        {
            throw new NotImplementedException();
        }

        public void BuildUp(object item)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}