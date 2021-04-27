using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDependencyInjection.Infrastructure
{
    public interface IFactory : IDisposable
    {
        T GetSingleton<T>() where T : class;

        IScope CreateScope();
    }
}
