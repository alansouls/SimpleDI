using SimpleDependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleDependencyInjection.Infrastructure
{
    internal class InstanceContainer : IDisposable
    {
        private bool disposed = false;
        private readonly List<Type> _registeredTypes;
        public IReadOnlyList<Type> RegisteredTypes => _registeredTypes;
        private readonly Dictionary<Type, Func<object[], object>> _initializers;
        public IReadOnlyList<Type> InitializersTypes => _initializers.Select(s => s.Key).ToList();

        private readonly Dictionary<Type, object> _instances;
        public IReadOnlyDictionary<Type, object> Instances { get => _instances.ToDictionary(s => s.Key, s => s.Value); }

        public InstanceContainer(Dictionary<Type, Func<object[], object>> initializers, List<Type> registeredTypes)
        {
            _registeredTypes = registeredTypes;
            _initializers = initializers;
            _instances = new Dictionary<Type, object>();
        }

        public void AddType(Type type, object instance) => _instances.Add(type, instance);

        public bool HasType(Type type)
        {
            return _initializers.Any(s => s.Key == type) || _instances.Any(s => s.Key == type);
        }

        public object ResolveType(Type type)
        {
            object instance = _instances.GetValueOrDefault(type);
            if (instance != null)
                return instance;

            if (!_initializers.ContainsKey(type))
                throw new Exception($"Could not find an initializer or registered instance for type {type.Name}");

            var initializer = _initializers[type];
            if (initializer == null)
            {
                var constructor = FindConstructor(type);
                var parameters = GetDependencies(type).Select(s => ResolveType(s)).ToArray();
                instance = constructor.Invoke(parameters);
                _instances.Add(type, instance);
                return instance;
            }
            else
                instance = _initializers[type](null);

            _instances.Add(type, instance);
            return instance;
        }

        public List<Type> GetDependencies(Type type)
        {
            var constructor = FindConstructor(type);
            var parametersType = constructor.GetParameters().Select(s => s.ParameterType);
            return parametersType.ToList();
        }

        private ConstructorInfo FindConstructor(Type type)
        {
            var constructor = type.GetSuitableConstructor(_registeredTypes);
            if (constructor == null)
                throw new Exception("No suitable constructor for registered types.");

            return constructor;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    _instances.ToList().ForEach(a =>
                    {
                        if (a.Value is IDisposable disposable)
                            disposable.Dispose();
                    });
                }

                disposed = true;
            }
        }
    }
}
