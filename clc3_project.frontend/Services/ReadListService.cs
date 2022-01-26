
using CLC3_Project.Model;

namespace CLC3_Project.Frontend.Services
{
    public class ReadListService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        private readonly string HEADER = "owner";
        private bool alreadyDone;

        private void RefreshHeader(string username)
        {
            _client.DefaultRequestHeaders.Remove(HEADER);
            _client.DefaultRequestHeaders.Add(HEADER, username);
        }

        public ReadListService(IHttpClientFactory clientFactory, IConfiguration config)
        {
            _client = clientFactory.CreateClient();
            _client.BaseAddress = new Uri(config["API_URL"]);
            _config = config;
        }

        public async Task<List<ReadingList>> GetListsForUser(string username)
        {
            RefreshHeader(username);
            return await _client.GetFromJsonAsync<List<ReadingList>>("api/readlist");
        }

        public async Task<ReadingList> GetReadingList(string username, string name)
        {
            RefreshHeader(username);
            return await _client.GetFromJsonAsync<ReadingList>($"api/readlist/{name}");
        }

        public async Task<bool> UpdateReadingList(string username, string oldname, ReadingList newList)
        {
            RefreshHeader(username);
            var resp = await _client.PutAsync($"api/readlist/{oldname}", JsonContent.Create(newList));
            if (!resp.IsSuccessStatusCode)
            {
                throw new Exception(resp.ReasonPhrase);
            }
            return resp.IsSuccessStatusCode;
        }
        public async Task<bool> CreateReadingList(string username, ReadingList newList)
        {
            RefreshHeader(username);
            var resp = await _client.PostAsJsonAsync<ReadingList>("api/readlist", newList);
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveReadingList(string username, string name)
        {
            RefreshHeader(username);
            var resp = await _client.DeleteAsync($"api/readlist/{name}");
            return resp.IsSuccessStatusCode;
        }

    }
}
