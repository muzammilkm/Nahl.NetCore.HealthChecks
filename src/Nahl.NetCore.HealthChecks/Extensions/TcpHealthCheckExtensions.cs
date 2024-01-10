using Microsoft.Extensions.DependencyInjection;
using Nahl.NetCore.HealthChecks.Options;
using System;

namespace Nahl.NetCore.HealthChecks.Extensions
{
    public static class TcpHealthCheckExtensions
    {
        public static IServiceCollection AddTcpHealthChecks(this IServiceCollection services, Action<HealthCheckOptions> setupAction = null)
        {
            services
                .AddOptions()
                .Configure<HealthCheckOptions>(options =>
                {
                    setupAction?.Invoke(options);
                })
                .AddScoped<IHealthCheckProvider, HealthCheckProvider>()
                .AddHostedService<TcpHealthCheckService>();

            return services;
        }
    }
}
