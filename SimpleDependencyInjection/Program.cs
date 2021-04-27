using SimpleDependencyInjection.Infrastructure;
using SimpleDependencyInjection.Test;
using System;

namespace SimpleDependencyInjection
{
    class Program
    {
        static void Main(string[] args)
        {
            var injector = new Injector();
            using var myFactory = GetFactory(injector);
            var resolved = myFactory.GetSingleton<Service1>();
            resolved.ServiceInfo = "This is singleton";
            Console.WriteLine(resolved.ServiceInfo);
            resolved = myFactory.GetSingleton<Service1>();
            Console.WriteLine(resolved.ServiceInfo);
        }

        static IFactory GetFactory(IInjector injector)
        {
            injector.RegisterSingleton(() => new Service1
            {
                ServiceInfo = "This is service 1 at start"
            });
            injector.RegisterScoped(() => new Service2()
            {
                ServiceInfo = "This is service 2 at start"
            });
            injector.RegisterScoped(() => new Service3()
            {
                ServiceInfo = "This is service 3 at start"
            });
            injector.RegisterScoped<DIClass>();
            injector.RegisterScoped<DIClass2>();
            injector.RegisterScoped<DIClass3>();
            injector.RegisterScoped<DIClass4>();

            return injector.GetFactory();
        }
    }
}
