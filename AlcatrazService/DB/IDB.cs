namespace AlcatrazService
{
    public interface IDB
    {
        Task LogCallToDatabase(string message);
        Task<string[]> GetTimeRetrievalCalls();
    }
}
