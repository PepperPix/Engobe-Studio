namespace Engobe.Studio.Services;

public interface IFilePicker
{
    Task<string?> PickFileAsync(string title);
}
