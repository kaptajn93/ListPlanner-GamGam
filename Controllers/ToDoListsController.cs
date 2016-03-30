using System.Web.Mvc;
using ListPlanner_GamGam.Models;

namespace ListPlanner_GamGam.Controllers
{
    public class ToDoListsController : Controller
    {
        private HsmDbContext _context;

        const string ToDoListCacheKeyPrefix = "ToDoListCache_";
        const string ToDoByUserCacheKeyPrefix = "ToDoByUserCache_";

        //private IMemoryCache cache;
        //private readonly ILogger<ToDoListsController> _logger;

        public ToDoListsController()
        {
            _context = new HsmDbContext();
            //this.cache = cache;
            //_logger = logger;
            //_logger.LogDebug("Hejsa");
            //_logger.LogInformation("Processing GET request for values.");
        }

        // GET: ToDoLists
        public ActionResult Index()
        {
            ViewBag.Title = "ToDoLists";
            return View();
        }

    }
}

