using FluentAssertions;
using SimpleDependencyInjection.Infrastructure;
using System;
using Xunit;

namespace SimpleDITests.Units
{
    public class InjectorTests
    {
        private readonly Injector injector = new Injector();

        [Fact]
        public void FactoryShouldReturnSameInstanceForAInjector()
        {
            var factory1 = injector.GetFactory();
            var factory2 = injector.GetFactory();

            (factory1 == factory2).Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnExceptionInjectingScopedTypeAlreadyInjectedAsSingleton()
        {
            injector.RegisterSingleton<SampleType>();

            Action action = () => injector.RegisterScoped<SampleType>();
            action.Should().Throw<Exception>().WithMessage("This type is already registered as a singleton.");
        }

        [Fact]
        public void ShouldReturnExceptionInjectingSingletonTypeAlreadyInjectedAsScoped()
        {
            injector.RegisterScoped<SampleType>();

            Action action = () => injector.RegisterSingleton<SampleType>();
            action.Should().Throw<Exception>().WithMessage("This type is already registered as scoped.");
        }

        class SampleType
        {
        }
    }
}
