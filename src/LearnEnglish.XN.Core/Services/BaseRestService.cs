using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LearnEnglish.XN.Core.Services
{
    public abstract class BaseRestService
    {
        private readonly HttpClient _client;

        protected BaseRestService(HttpClient client) => _client = client;

        protected async Task<TResult> SendAsync<TResult>(HttpRequestMessage message, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _client.SendAsync(message, cancellationToken);

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync());
        }
    }
}
