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
    public class TcpHealthCheckService : BackgroundService
    {
        private readonly ILogger<TcpHealthCheckService> _logger;
        private readonly IHealthCheckProvider _healthCheckProvider;
        private readonly TcpListener _listener;

        public TcpHealthCheckService(ILogger<TcpHealthCheckService> logger, IOptions<HealthCheckOptions> healthCheckOptions, IHealthCheckProvider healthCheckProvider)
        {
            _logger = logger;
            _healthCheckProvider = healthCheckProvider;
            _listener = new TcpListener(IPAddress.Any, healthCheckOptions.Value.Port);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _listener?.Stop();
            return base.StopAsync(cancellationToken);
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _listener.Start();
                while (!stoppingToken.IsCancellationRequested)
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
            catch (Exception)
            {
                _logger.LogWarning("Health check has stopped.");
            }
        }
    }
}
