using SimpleDependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleDependencyInjection.Infrastructure
{
    public class Factory : IFactory
    {
        private bool disposed = false;
        private readonly Dictionary<Type, Func<object[], object>> _scopeds;
        private readonly Dictionary<Type, Func<object[], object>> _singletons;

        private readonly InstanceContainer _singletonContainer;

        private readonly List<IIncrementableScope> _scopes;

        public Factory(Dictionary<Type, Func<object[], object>> scopeds, Dictionary<Type, Func<object[], object>> singletons)
        {
            _scopeds = scopeds;
            _singletons = singletons;
            _singletonContainer = new InstanceContainer(_singletons, _singletons.Select(s => s.Key).ToList());
            _scopes = new List<IIncrementableScope>();
        }

        public IScope CreateScope()
        {
            var scope = new Scope(_scopeds, _singletonContainer);
            _scopes.Add(scope);

            return scope;
        }

        public T GetSingleton<T>() where T : class
        {
            var instance = _singletonContainer.ResolveType(typeof(T)) as T;
            _scopes.ForEach(s => s.AddTypeInstance(typeof(T), instance));
            return instance;
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
                    _scopes.ForEach(s => s.Dispose());

                    _singletonContainer.Dispose();
                }

                disposed = true;
            }
        }
    }
}
