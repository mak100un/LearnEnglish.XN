using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LearnEnglish.XN.Core.Definitions.Constants;
using LearnEnglish.XN.Core.Services.Interfaces;

namespace LearnEnglish.XN.Core.Services;

public class RandomizerService : BaseRestService, IRandomizerService
{
    public RandomizerService(HttpClient client)
        : base(client)
    {
    }

    public Task<IEnumerable<string>> GetRandomWordsAsync(CancellationToken cancellationToken = default) =>
        SendAsync<IEnumerable<string>>(
            new HttpRequestMessage(HttpMethod.Get,$"https://random-word-api.herokuapp.com/word?lang={RestConstants.LANG}&number={RestConstants.WORDS_COUNT}"),
            cancellationToken);
}
