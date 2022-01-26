using CLC3_Project.Frontend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CLC3_Project.Frontend.Controllers
{
    /// <summary>
    /// Manages all Request dealing with the book side of things.
    /// </summary>
    [Route("/[controller]")]
    public class BooksController : Controller
    {
        private readonly BooksService service;

        public BooksController(BooksService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Returns a view containing all books in a list
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> IndexAsync()
        {
            
            return View(await service.getAllBooksAsync());
        }
       
        /// <summary>
        /// Returns a view which contains the detailed information
        /// </summary>
        /// <param name="isbn">isbn of the selected book</param>
        [HttpGet]
        [Route("{isbn}")]
        public async Task<IActionResult> Details(string isbn)
        {
            return View(await service.getBookByIsbn(isbn));
        }


    }
}
