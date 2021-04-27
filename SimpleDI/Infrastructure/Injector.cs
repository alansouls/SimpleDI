using SimpleDependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleDependencyInjection.Infrastructure
{
    public class Injector : IInjector
    {
        private readonly Dictionary<Type, Func<object[], object>> _scopeds;
        private readonly Dictionary<Type, Func<object[], object>> _singletons;

        private IFactory _factory;

        public Injector()
        {
            _scopeds = new Dictionary<Type, Func<object[], object>>();
            _singletons = new Dictionary<Type, Func<object[], object>>();
            _factory = null;
        }

        public IFactory GetFactory()
        {
            if (_factory == null)
                _factory = new Factory(_scopeds, _singletons);

            return _factory;
        }

        public void RegisterScoped<T>(Func<T> initializer) where T : class => RegisterScopedConverted<T>(ConvertInitializer(initializer));

        private void RegisterScopedConverted<T>(Func<object[], object> initializer) where T : class
        {
            if (_factory != null)
                throw new Exception("Cant registered type after factory has been generated.");
            if (_singletons.Any(s => s.Key == typeof(T)))
                throw new Exception("This type is already registered as a singleton.");


            _scopeds[typeof(T)] = initializer;
        }

        private Func<object[], object> ConvertInitializer<T>(Func<T> initializer) where T : class
        {
            Func<object[], object> result = (_) => initializer();

            return result;
        }

        public void RegisterScoped<T>() where T : class => RegisterScopedConverted<T>(null);

        public void RegisterSingleton<T>(Func<T> initializer) where T : class => RegisterSingletonConverted<T>(ConvertInitializer(initializer));

        private void RegisterSingletonConverted<T>(Func<object[], object> initializer) where T : class
        {
            if (_factory != null)
                throw new Exception("Cant registered type after factory has been generated.");
            if (_scopeds.Any(s => s.Key == typeof(T)))
                throw new Exception("This type is already registered as scoped.");


            _singletons[typeof(T)] = initializer;
        }

        public void RegisterSingleton<T>() where T : class => RegisterSingletonConverted<T>(null);
    }
}
