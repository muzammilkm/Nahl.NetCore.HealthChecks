# Nahl.NetCore.HealthChecks

Tcp Health checks for dotnet console application hosted on kubernetes.

## Usage

* Add Reference Nahl.NetCore.HealthChecks package

``` json
PM > Install-Package Nahl.NetCore.HealthChecks
```

* Configure Nahl.NetCore.HealthChecks service

``` c#
using Nahl.NetCore.HealthChecks.Extensions;

services
    .AddTcpHealthChecks();
```

* Set the application state as healthy or unhealthy by injecting IHealthCheckProvider

``` c#

using Nahl.NetCore.HealthChecks;


public Task DoWorkAsync()
{
    if(all service are up and running correctly)
        _healthCheckProvider.Healthy();
    else
        _healthCheckProvider.Unhealthy();
}
```
