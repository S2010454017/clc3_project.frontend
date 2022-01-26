using CLC3_Project.Frontend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CLC3_Project.Frontend.Controllers
{
    [Route("/[controller]")]
    public class BooksController : Controller
    {
        private readonly BooksService service;

        public BooksController(BooksService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult> IndexAsync()
        {
            
            return View(await service.getAllBooksAsync());
        }
       
        [HttpGet]
        [Route("{isbn}")]
        public async Task<IActionResult> Details(string isbn)
        {
            return View(await service.getBookByIsbn(isbn));
        }


    }
}
