namespace Nahl.NetCore.HealthChecks.Options
{
    public class HealthCheckOptions
    {
        public int Port { get; set; }

        public HealthCheckOptions()
        {
            Port = 6000;
        }
    }
}
