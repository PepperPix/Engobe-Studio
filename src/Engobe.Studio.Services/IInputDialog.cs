namespace Engobe.Studio.Services;

public interface IInputDialog
{
    Task<string?> PromptAsync(string title, string message);
}
