using BaseLibrary.Dtos;
using System.Threading.Tasks;
namespace ClientLibrary.Helpers
{
    public class GetHttpClient(IHttpClientFactory httpClientFactory,LocalStorageService localStorageService)
    {
        private const string HeaderKey = "Authorization";

        public async Task<HttpClient> GetPrivateHttpClient()
        {
            var client = httpClientFactory.CreateClient("System Api client");
            var stringToken = await localStorageService.GetToken();
            if(string.IsNullOrEmpty(stringToken))
            {
                return client;

            }
            var deserializeToken=Serialization.DeserializeJsonString<UserSession>(stringToken);
            if(deserializeToken == null)
            {
                return client;
            }
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",deserializeToken.Token);
            return client;
        }

        public HttpClient GetPublicHttpClient()
        {
            var client = httpClientFactory.CreateClient("System API Client");
            client.DefaultRequestHeaders.Remove(HeaderKey);
            return client;
        }
    }
}
