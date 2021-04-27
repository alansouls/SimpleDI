using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleDependencyInjection.Infrastructure
{
    public class Scope : IIncrementableScope
    {
        private bool disposed = false;

        public bool Disposed => disposed;

        private readonly InstanceContainer _instances;

        private readonly InstanceContainer _singletonInstances;

        internal Scope(Dictionary<Type, Func<object[], object>> initializers, InstanceContainer singletonInstances)
        {
            _instances = new InstanceContainer(initializers, singletonInstances.RegisteredTypes.Union(initializers.Select(s => s.Key).ToList()).ToList());
            _singletonInstances = singletonInstances;
        }

        public T GetScoped<T>() where T : class => GetScoped(typeof(T)) as T;

        public object GetScoped(Type type)
        {
            var dependencies = _instances.GetDependencies(type);
            foreach (var dependency in dependencies)
            {
                if (_instances.Instances.ContainsKey(dependency))
                    continue;
                if (_instances.InitializersTypes.Contains(dependency))
                {
                    GetScoped(dependency);
                    continue;
                }

                var dependencyInstance = _singletonInstances.ResolveType(dependency);
                _instances.AddType(dependency, dependencyInstance);
            }
            return _instances.ResolveType(type);
        }

        public void AddTypeInstance(Type type, object instance) => _instances.AddType(type, instance);

        public void Dispose()
        {
            disposed = true;
        }
    }
}
