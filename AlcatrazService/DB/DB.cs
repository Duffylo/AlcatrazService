using static AlcatrazService.Common.Constants;

namespace AlcatrazService
{
    public class DB(ILogger<DB> logger) : IDB
    {
        static readonly string _path = Path.Combine(AppContext.BaseDirectory, DB_FN);
        private readonly ILogger<DB> logger = logger;

        public async Task LogCallToDatabase(string message)
        {
            try
            {
                await File.AppendAllLinesAsync(_path, new[] { message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Exception during {nameof(LogCallToDatabase)}");
            }
        }

        public async Task<string[]> GetTimeRetrievalCalls()
        {
            return await File.ReadAllLinesAsync(_path);
        }
    }
}
