using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LearnEnglish.XN.Core.Definitions.Constants;
using LearnEnglish.XN.Core.Definitions.RestResponses;
using LearnEnglish.XN.Core.Services.Interfaces;

namespace LearnEnglish.XN.Core.Services
{
    public class TranslateService : BaseRestService, ITranslateService
    {
        public TranslateService(HttpClient client)
            : base(client)
        {
        }

        public Task<TranslateResponse> TranslateAsync(string word, CancellationToken cancellationToken = default) =>
            SendAsync<TranslateResponse>(
                new HttpRequestMessage(HttpMethod.Post, $@"https://translate.yandex.net/api/v1.5/tr.json/translate?key={RestConstants.API_KEY}&text={word}&lang={RestConstants.TRANSLATE_DIRECTION}"),
                cancellationToken);
    }
}
