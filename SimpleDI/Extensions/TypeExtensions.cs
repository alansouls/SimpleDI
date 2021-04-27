using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleDependencyInjection.Extensions
{
    public static class TypeExtensions
    {
        public static ConstructorInfo GetSuitableConstructor(this Type type, List<Type> avaibleTypes)
        {
            var constructors = type.GetConstructors().OrderByDescending(c => c.GetParameters().Length).ToList();

            var parameterConstructors = constructors.Where(c => c.GetParameters().Length > 0).ToList();

            foreach (var ctor in parameterConstructors)
            {
                var parameters = ctor.GetParameters();
                var valid = true;
                foreach (var param in parameters)
                {
                    if (!avaibleTypes.Contains(param.ParameterType))
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid == false)
                    continue;
                return ctor;
            }

            return constructors.FirstOrDefault(c => !c.GetParameters().Any());
        }
    }
}
