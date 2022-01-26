using CLC3_Project.Frontend.Services;
using CLC3_Project.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CLC3_Project.Frontend.Controllers
{
    [Route("/[controller]")]
    public class ReadListController : Controller
    {
        private readonly ReadListService readListService;
        private readonly BooksService bookService;
        private List<string> list;
        public ReadListController(ReadListService readListService, BooksService bookService)
        {
            this.readListService = readListService;
            this.bookService = bookService;
            list = new List<string> { "Julia", "Fabian", "Peter", "Hans", "user1" };
        }

        public ActionResult Index()
        {
            return View(list);
        }

        [Route("{user}")]
        public async Task<ActionResult> ListForUserAsync(string user)
        {
            return View((user, await readListService.GetListsForUser(user)));
        }

        [Route("create")]
        public async Task<ActionResult> Create(string user)
        {
            return View("CreateUpdate", (user, new ReadingList(), await bookService.getAllBooksAsync()));
        }
        [Route("edit")]
        public async Task<ActionResult> Edit(string user, string name)
        {
            return View("CreateUpdate",
                (user,
                await readListService.GetReadingList(user, name),
                await bookService.getAllBooksAsync())
               );
        }
        [Route("remove")]
        public async Task<ActionResult> Remove(string user, string name)
        {
            await readListService.RemoveReadingList(user,name);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<ActionResult> CreateAsync(IFormCollection collection)
        {

            var b = await bookService.getAllBooksAsync();
            var selected = collection["books"].ToString().Split(",");
            var newList = new ReadingList()
            {
                Id = String.Empty,
                Name = collection["name"],
                Owner = collection["owner"],
                Books = b.Where(x => selected.Contains(x.ISBN)).ToList()
            };

            await readListService.CreateReadingList(newList.Owner, newList);
            return View("CreateUpdate", (newList.Owner, new ReadingList(), b));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("update")]
        public async Task<ActionResult> Update(IFormCollection collection)
        {
            var b = await bookService.getAllBooksAsync();
            var selected = collection["books"].ToString().Split(",");
            var updateList = new ReadingList()
            {
                Id = String.Empty,
                Name = collection["name"],
                Owner = collection["owner"],
                Books = b.Where(x => selected.Contains(x.ISBN)).ToList()
            };

            await readListService.UpdateReadingList(updateList.Owner, collection["oldName"], updateList);
            return View("CreateUpdate", (updateList.Owner, updateList, b));
        }
    }
}
