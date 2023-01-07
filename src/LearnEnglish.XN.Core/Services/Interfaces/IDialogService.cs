using System.Threading.Tasks;

namespace LearnEnglish.XN.Core.Services.Interfaces;

public interface IDialogService
{
    Task<string> DisplayInputAsync(string title, string message, string previousText, string accept, string cancel);

    Task DisplaySuccessMessageAsync();

    void ShowToast(string text);
}
