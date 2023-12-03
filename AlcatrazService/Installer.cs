using AlcatrazService.Exceptions;
using CliWrap;
using static AlcatrazService.Common.Constants;

namespace AlcatrazService
{
    public class Installer
    {
        private readonly ILogger<Installer> _logger = LoggerFactory.Create(builder =>
            builder.AddConsole()).CreateLogger<Installer>();

        /// <summary>
        /// Installs or Uninstalls the Service
        /// </summary>
        /// <param name="args">The command arguments</param>
        /// <returns>True if service command is recognized and executed, else False</returns>
        /// <exception cref="InstallException">When errors during install/uninstall</exception>
        internal async Task<bool> InstallUninstallService(string[] args)
        {
            if (args is { Length: 1 }
            && new[] { CMD_INSTALL, CMD_UNINSTALL }.Contains(args[0]))
            {
                try
                {
                    string executablePath = Path.Combine(AppContext.BaseDirectory, SERVICE_NAME + ".exe");

                    _logger.LogWarning(executablePath);

                    if (args[0] is CMD_INSTALL)
                    {
                        // Install service
                        await InstallService(executablePath);
                    }
                    else if (args[0] is CMD_UNINSTALL)
                    {
                        await UninstallService();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    throw new InstallException("Exception during Installation", ex);
                }

                return true;
            }
            return false;
        }

        #region Private methods
        private async Task UninstallService()
        {
            try
            {
                // Stop service
                await Cli.Wrap("sc")
                    .WithArguments(new[] { "stop", SERVICE_NAME })
                    .ExecuteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                // check if service was not running
                if (!ex.Message.Contains("1062"))
                {
                    throw new InstallException("Exception during CMD_UNINSTALL - stop", ex);
                }
            }

            // Delete service
            await Cli.Wrap("sc")
                .WithArguments(new[] { "delete", SERVICE_NAME })
                .ExecuteAsync();
        }

        private async Task InstallService(string executablePath)
        {
            await Cli.Wrap("sc")
                            .WithArguments(new[] { "create", SERVICE_NAME, $"binPath={executablePath}", "start=demand" })
                            .ExecuteAsync();
        }

        #endregion
    }
}
