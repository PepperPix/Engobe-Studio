namespace Engobe.Studio.Services;

public interface INewPageDialog
{
    Task<NewPageRequest?> ShowAsync(IReadOnlyList<string> collectionNames);
}
