using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SampleOrg.Api.IntegrationTests.TestHelper;

public class IntegrationApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        // workaround for Microsoft's bug as described here: https://github.com/dotnet/aspnetcore/issues/14907 
        builder.UseServiceProviderFactory(new CustomServiceProviderFactory());
        return base.CreateHost(builder);
    }
}

#region Microsoft Bug Workaround

/// <summary>
///     Based upon https://github.com/dotnet/aspnetcore/issues/14907#issuecomment-620750841 - only necessary because of an
///     issue in ASP.NET Core
/// </summary>
public class CustomServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
{
    private readonly AutofacServiceProviderFactory _wrapped;
    private IServiceCollection _services;

    public CustomServiceProviderFactory()
    {
        _wrapped = new AutofacServiceProviderFactory();
    }

    public ContainerBuilder CreateBuilder(IServiceCollection services)
    {
        // Store the services for later.
        _services = services;

        return _wrapped.CreateBuilder(services);
    }

    public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
    {
        var sp = _services.BuildServiceProvider();
#pragma warning disable CS0612 // Type or member is obsolete
        var filters = sp.GetRequiredService<IEnumerable<IStartupConfigureContainerFilter<ContainerBuilder>>>();
#pragma warning restore CS0612 // Type or member is obsolete

        foreach (var filter in filters) filter.ConfigureContainer(b => { })(containerBuilder);

        return _wrapped.CreateServiceProvider(containerBuilder);
    }
}

#endregion