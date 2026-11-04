namespace Engobe.Studio.Services;

public interface IRecentProjectsStore
{
    IReadOnlyList<RecentProject> GetAll();
    void Add(string path, string name);
}
