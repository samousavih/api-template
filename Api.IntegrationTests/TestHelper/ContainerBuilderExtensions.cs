using Autofac;
using Moq;

namespace SampleOrg.Api.IntegrationTests.TestHelper;

public static class ContainerBuilderExtensions
{
    public static ContainerBuilder RegisterMock<T>(this ContainerBuilder builder, Mock<T> mock)
        where T : class
    {
        builder.Register(c => mock.Object).As<T>();
        return builder;
    }

    public static ContainerBuilder RegisterMock<T>(
        this ContainerBuilder builder,
        Func<IComponentContext, Mock<T>> @delegate
    )
        where T : class
    {
        builder.Register((c, p) => @delegate(c).Object);
        return builder;
    }
}