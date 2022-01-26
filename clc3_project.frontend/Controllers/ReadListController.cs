using CLC3_Project.Frontend.Services;
using CLC3_Project.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CLC3_Project.Frontend.Controllers
{
    /// <summary>
    /// Manages all Request dealing with the readinglist side of things.
    /// </summary>
    [Route("/[controller]")]
    public class ReadListController : Controller
    {
        private readonly ReadListService readListService;
        private readonly BooksService bookService;
        private List<string> list;  //stores the users managed by this app -> needed because identity server did not function
        public ReadListController(ReadListService readListService, BooksService bookService)
        {
            this.readListService = readListService;
            this.bookService = bookService;
            list = new List<string> { "Julia", "Fabian", "Peter", "Hans", "user1" };
        }

        /// <summary>
        /// Returnes a View with every user managed by this app
        /// </summary>
        public ActionResult Index()
        {
            return View(list);
        }

        /// <summary>
        /// Returnes a view containing all reading lists of the selected user
        /// </summary>
        /// <param name="user">username</param>
        [Route("{user}")]
        public async Task<ActionResult> ListForUserAsync(string user)
        {
            return View((user, await readListService.GetListsForUser(user)));
        }

        /// <summary>
        /// Returns a view which can be used to create a new reading list for the user
        /// </summary>
        /// <param name="user">username</param>
        [Route("create")]
        public async Task<ActionResult> Create(string user)
        {
            return View("CreateUpdate", (user, new ReadingList(), await bookService.getAllBooksAsync()));
        }
        
        /// <summary>
        /// Returns a view to edit the selected reading lsit
        /// </summary>
        /// <param name="user">username</param>
        /// <param name="name">name of the reading list to modifiy</param>
        /// <returns></returns>
        [Route("edit")]
        public async Task<ActionResult> Edit(string user, string name)
        {
            return View("CreateUpdate",
                (user,
                await readListService.GetReadingList(user, name),
                await bookService.getAllBooksAsync())
               );
        }

        /// <summary>
        /// Removes the given reading list and retuns the index view
        /// </summary>
        /// <param name="user">username</param>
        /// <param name="name">name of the reading list to delete</param>
        /// <returns></returns>
        [Route("remove")]
        public async Task<ActionResult> Remove(string user, string name)
        {
            await readListService.RemoveReadingList(user,name);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Gets the information from the form and creates a new reading list for the user
        /// </summary>
        /// <param name="collection">data from the form</param>
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

        // <summary>
        /// Gets the information from the form and updates the reading list for the user
        /// </summary>
        /// <param name="collection">data from the form</param>
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
