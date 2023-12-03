using AlcatrazService.Services;
using AlcatrazService;
using AlcatrazService.Common;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGrpc(o => o.EnableDetailedErrors = true);
        services.AddWindowsService(options =>
        {
            options.ServiceName = Constants.SERVICE_NAME;
        });

        services.AddLogging(configure =>
        {
            configure.ClearProviders();
            configure.AddConsole();
        });


        // Database
        services.AddSingleton<IDB, DB>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            // Configure the HTTP request pipeline.
            endpoints.MapGrpcService<AlcatrazTimeService>();
            endpoints.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        });
    }
}