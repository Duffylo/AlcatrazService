using AlcatrazGrpcService;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IntegrationTests;

[TestClass]
public class AlcatrazIntegrationTests
{
    private IHost? _host;
    private Action<IWebHostBuilder>? _configureWebHost;
    private TestServer? _server;
    private HttpMessageHandler? _handler;

    public GrpcChannel _channel { get; }

    public AlcatrazIntegrationTests()
    {
        _channel = GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
        {
            HttpHandler = Handler
        });
    }

    #region snippet_GetTimeTest
    [TestMethod]
    public async Task GetTimeTest()
    {
        // Arrange
        var client = new TimeService.TimeServiceClient(_channel);

        // Act
        var response = await client.GetTimeAsync(new Empty());

        // Assert
        Assert.IsNotNull(response.Message);
    }
    #endregion

    private HttpMessageHandler Handler
    {
        get
        {
            EnsureServer();
            return _handler!;
        }
    }

    private void EnsureServer()
    {
        if (_host == null)
        {
            var builder = new HostBuilder()
                .ConfigureServices(services =>
                {

                })
                .ConfigureLogging(logging =>
                {
                    // Logs
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webHost =>
                {
                    webHost
                        .UseTestServer()
                        .UseStartup<Startup>();

                    _configureWebHost?.Invoke(webHost);
                });
            _host = builder.Start();
            _server = _host.GetTestServer();
            _handler = _server.CreateHandler();
        }
    }
}