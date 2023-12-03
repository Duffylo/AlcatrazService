using AlcatrazService;
using AlcatrazService.Services;
using static AlcatrazService.Common.Constants;

internal class Program
{
    private static async Task Main(string[] args)
    {
        // Install
        if (await new Installer().InstallUninstallService(args))
        {
            return;
        }

        // Build and Run
        var builder = WebApplication.CreateBuilder(args);

        // Logs
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        // Add services to the container.
        builder.Services.AddGrpc();
        builder.Services.AddWindowsService(options =>
        {
            options.ServiceName = SERVICE_NAME;
        });

        // Database
        builder.Services.AddSingleton<IDB, DB>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<AlcatrazTimeService>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}