using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDependencyInjection.Infrastructure
{
    public interface IScope : IDisposable
    {
        bool Disposed { get; }
        T GetScoped<T>() where T : class;
    }
}
