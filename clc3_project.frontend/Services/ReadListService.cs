
using CLC3_Project.Model;

namespace CLC3_Project.Frontend.Services
{
    /// <summary>
    /// Service Class to get reading list for user from the api.
    /// </summary>
    public class ReadListService
    {
        private readonly HttpClient _client;
        private readonly string HEADER = "owner";

        /// <summary>
        /// Sets the required Header for the API
        /// </summary>
        /// <param name="username"></param>
        private void RefreshHeader(string username)
        {
            _client.DefaultRequestHeaders.Remove(HEADER);
            _client.DefaultRequestHeaders.Add(HEADER, username);
        }

        public ReadListService(IHttpClientFactory clientFactory, IConfiguration config)
        {
            _client = clientFactory.CreateClient();
            _client.BaseAddress = new Uri(config["API_URL"]);
        }

        /// <summary>
        /// Get all Reading list for the given user
        /// </summary>
        /// <param name="username">name of user</param>
        /// <returns>All reading list owned by the user</returns>
        public async Task<List<ReadingList>> GetListsForUser(string username)
        {
            RefreshHeader(username);
            return await _client.GetFromJsonAsync<List<ReadingList>>("api/readlist");
        }

        /// <summary>
        /// Get a specific reading list for a user
        /// </summary>
        /// <param name="username">name of user</param>
        /// <param name="name">name of the reading list belonging to the user</param>
        /// <returns></returns>
        public async Task<ReadingList> GetReadingList(string username, string name)
        {
            RefreshHeader(username);
            return await _client.GetFromJsonAsync<ReadingList>($"api/readlist/{name}");
        }

        /// <summary>
        /// Updates a reading list for a given user
        /// </summary>
        /// <param name="username">name of the user</param>
        /// <param name="oldname">previous name of hte reading list</param>
        /// <param name="newList">the updated list</param>
        /// <returns>true if created, else false</returns>
        public async Task<bool> UpdateReadingList(string username, string oldname, ReadingList newList)
        {
            RefreshHeader(username);
            var resp = await _client.PutAsync($"api/readlist/{oldname}", JsonContent.Create(newList));
            return resp.IsSuccessStatusCode;
        }

        /// <summary>
        /// Creates a new reading list for the given user
        /// </summary>
        /// <param name="username">name of the user</param>
        /// <param name="newList">the new reading list</param>
        /// <returns>true if created, false else</returns>
        public async Task<bool> CreateReadingList(string username, ReadingList newList)
        {
            RefreshHeader(username);
            var resp = await _client.PostAsJsonAsync<ReadingList>("api/readlist", newList);
            return resp.IsSuccessStatusCode;
        }

        /// <summary>
        /// Removes a reading list for the given user.
        /// </summary>
        /// <param name="username">name of the user</param>
        /// <param name="name">name of the reading list</param>
        /// <returns>true if deleted</returns>
        public async Task<bool> RemoveReadingList(string username, string name)
        {
            RefreshHeader(username);
            var resp = await _client.DeleteAsync($"api/readlist/{name}");
            return resp.IsSuccessStatusCode;
        }

    }
}
