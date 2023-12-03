using AlcatrazService;
using AlcatrazService.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Alcatraz.UnitTests
{
    [TestClass]
    public class DBUnitTests
    {
        IDB _db;

        [TestInitialize]
        public void Setup()
        {
            // Build and Run
            var builder = WebApplication.CreateBuilder([]);

            // Logs
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            // Database
            builder.Services.AddSingleton<IDB, DB>();

            var serviceProvider = builder.Services.BuildServiceProvider();

            _db = serviceProvider.GetService<IDB>();
        }

        [TestMethod]
        public async Task TestLogCallToDatabase()
        {
            // Arrange
            var now = DateTime.UtcNow.ToString();
            var path = Path.Combine(AppContext.BaseDirectory, Constants.DB_FN);

            // Act
            await _db.LogCallToDatabase(now);
            var result = await File.ReadAllLinesAsync(path);

            // Assert
            Assert.AreEqual(true, result.Contains(now));
        }
    }
}