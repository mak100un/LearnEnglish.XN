using System.Threading;
using System.Threading.Tasks;
using LearnEnglish.XN.Core.Definitions.RestResponses;

namespace LearnEnglish.XN.Core.Services.Interfaces;

public interface ITranslateService
{
    Task<TranslateResponse> TranslateAsync(string word, CancellationToken cancellationToken = default);
}
