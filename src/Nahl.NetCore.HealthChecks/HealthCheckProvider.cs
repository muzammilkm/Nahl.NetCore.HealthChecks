namespace Nahl.NetCore.HealthChecks
{
    public class HealthCheckProvider : IHealthCheckProvider
    {
        public bool IsHealthy { get; private set; }

        public void Healthy()
        {
            IsHealthy = true;
        }

        public void UnHealthy()
        {
            IsHealthy = false;
        }
    }
}
