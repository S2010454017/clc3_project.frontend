using CLC3_Project.Model;

namespace CLC3_Project.Frontend.Services
{
    /// <summary>
    /// Service to get Books from API
    /// </summary>
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


        /// <summary>
        /// Get all Books
        /// </summary>
        /// <returns>A list containing all books</returns>
        public async Task<List<Book>> getAllBooksAsync()
        {
           return await _client.GetFromJsonAsync<List<Book>>("/api/books");
        }

        /// <summary>
        /// Get a book with the given isbn
        /// </summary>
        /// <param name="isbn">isbn of the book</param>
        /// <returns>the book</returns>
        public async Task<Book> getBookByIsbn(string isbn)
        {
            return await _client.GetFromJsonAsync<Book>($"/api/books/{isbn}");
        }
    }
}
