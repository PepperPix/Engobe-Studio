namespace Engobe.Studio.Services;

public interface IFolderPicker
{
    Task<string?> PickFolderAsync(string title);
}
