using AlcatrazService;
using AlcatrazService.Services;
using static AlcatrazService.Common.Constants;

public class Program
{
    private static async Task Main(string[] args)
    {
        // Install
        if (await new Installer().InstallUninstallService(args))
        {
            return;
        }

        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
}