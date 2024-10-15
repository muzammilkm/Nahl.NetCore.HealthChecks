using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nahl.NetCore.HealthChecks.Options;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Nahl.NetCore.HealthChecks
{
    public class TcpHealthCheckService : IHostedService
    {
        private readonly ILogger<TcpHealthCheckService> _logger;
        private readonly HealthCheckOptions _healthCheckOptions;
        private readonly IHealthCheckProvider _healthCheckProvider;
        private readonly TcpListener _listener;

        public TcpHealthCheckService(ILogger<TcpHealthCheckService> logger, IOptions<HealthCheckOptions> healthCheckOptions, IHealthCheckProvider healthCheckProvider)
        {
            _logger = logger;
            _healthCheckOptions = healthCheckOptions.Value;
            _healthCheckProvider = healthCheckProvider;
            _listener = new TcpListener(IPAddress.Any, _healthCheckOptions.Port);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            _listener.Start();
            while (!cancellationToken.IsCancellationRequested)
            {
                var tcpClient = await _listener.AcceptTcpClientAsync();
                var clientWriter = new StreamWriter(tcpClient.GetStream());
                var message = _healthCheckProvider.IsHealthy ? "Healthy" : "Unhealthy";
                var logLevel = _healthCheckProvider.IsHealthy ? LogLevel.Information : LogLevel.Warning;

                _logger.Log(logLevel, message);

                await clientWriter.WriteLineAsync(message);
                await clientWriter.FlushAsync();

                tcpClient.Close();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (_listener != null)
            {
                _listener.Stop();
            }
            return Task.CompletedTask;
        }
    }
}
