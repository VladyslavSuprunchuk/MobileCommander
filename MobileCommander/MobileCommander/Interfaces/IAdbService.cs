namespace MobileCommander.Interfaces
{
    public interface IAdbService
    {
        Task KillAllBackgroundTasksAsync();

        Task<string> GetChromeSearchAsync(string searchQuery);
    }
}
