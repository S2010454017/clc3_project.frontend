using CLC3_Project.Model;

namespace CLC3_Project.Frontend.Services
{
    public class BooksService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;

        public BooksService(IHttpClientFactory clientFactory, IConfiguration config)
        {
            _client = clientFactory.CreateClient();
            _client.BaseAddress = new Uri(config["API_URL"]);
            _config = config;
        }


        public async Task<List<Book>> getAllBooksAsync()
        {
           return await _client.GetFromJsonAsync<List<Book>>("/api/books");
        }

        internal async Task<Book> getBookByIsbn(string isbn)
        {
            return await _client.GetFromJsonAsync<Book>($"/api/books/{isbn}");
        }
    }
}
