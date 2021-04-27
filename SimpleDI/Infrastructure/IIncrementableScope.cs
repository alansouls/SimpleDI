using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDependencyInjection.Infrastructure
{
    interface IIncrementableScope : IScope
    {
        void AddTypeInstance(Type type, object instance);
    }
}
