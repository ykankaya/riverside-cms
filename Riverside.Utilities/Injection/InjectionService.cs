using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Riverside.Utilities.Injection
{
    public class InjectionService : IInjectionService
    {
        private IServiceProvider _serviceProvider;

        public InjectionService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T CreateType<T>(Type type)
        {
            return (T)_serviceProvider.GetService(type);
        }
    }
}
