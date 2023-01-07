using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LearnEnglish.XN.Core.Services.Interfaces;

public interface IRandomizerService
{
    Task<IEnumerable<string>> GetRandomWordsAsync(CancellationToken token = default);
}
