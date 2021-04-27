using FluentAssertions;
using SimpleDependencyInjection.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SimpleDITests.Units
{
    public class FactoryTests
    {
        private readonly Dictionary<Type, Func<object[], object>> scopeds = new Dictionary<Type, Func<object[], object>>
        {
            {typeof(Scoped1), null},
            {typeof(Scoped2), null},
        };
        private readonly Dictionary<Type, Func<object[], object>> singletons = new Dictionary<Type, Func<object[], object>>
        {
            {typeof(Singleton), null},
        };
        private readonly Factory factory;

        public FactoryTests()
        {
            factory = new Factory(scopeds, singletons);
        }

        [Fact]
        public void GetSingletonShouldReturnSameInstanceForAFactory()
        {
            var singleton1 = factory.GetSingleton<Singleton>();
            var singleton2 = factory.GetSingleton<Singleton>();

            (singleton1 == singleton2).Should().BeTrue();
        }

        [Fact]
        public void GetSingletonWithoutRegisteredTypeAsSingletonShouldThrowException()
        {
            Action action = () => factory.GetSingleton<NotRegistered>();

            action.Should().Throw<Exception>()
                .WithMessage($"Could not find an initializer or registered instance for type {nameof(NotRegistered)}");
        }

        [Fact]
        public void CreateScopeShouldAlwaysReturnDifferentInstances()
        {
            var scope1 = factory.CreateScope();
            var scope2 = factory.CreateScope();

            (scope1 == scope2).Should().BeFalse();
        }

        class Scoped1
        {
        }

        class Scoped2
        {
        }

        class Singleton
        {
        }

        class NotRegistered
        {
        }
    }
}
