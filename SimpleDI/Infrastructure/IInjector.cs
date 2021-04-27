using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDependencyInjection.Infrastructure
{
    public interface IInjector
    {
        void RegisterScoped<T>(Func<T> initializer) where T : class;
        void RegisterScoped<T>() where T : class;
        void RegisterSingleton<T>(Func<T> initializer) where T : class;
        void RegisterSingleton<T>() where T : class;

        IFactory GetFactory();
    }
}
