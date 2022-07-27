using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Api;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
	.ConfigureServices(s =>
	{
        s.AddSingleton<IProductData, ProductData>();
	})
    .Build();

host.Run();
