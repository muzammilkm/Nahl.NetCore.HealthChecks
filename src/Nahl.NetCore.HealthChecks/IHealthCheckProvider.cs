namespace Nahl.NetCore.HealthChecks
{
    public interface IHealthCheckProvider
    {
        bool IsHealthy { get; }

        void Healthy();


        void UnHealthy();
    }
}
