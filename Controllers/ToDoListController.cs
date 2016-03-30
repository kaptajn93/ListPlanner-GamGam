using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using NLog;

namespace ListPlanner_GamGam.Controllers
{
    //[RoutePrefix("api/todolist")]
    //[RoutePrefix("api/todolist")]
    public class ToDoListController : ApiController
    {
        readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly HsmDbContext _context;

        const string ToDoListCacheKeyPrefix = "ToDoListCache_";
        const string ToDoByUserCacheKeyPrefix = "ToDoByUserCache_";

        public ToDoListController()
        {
            _context = new HsmDbContext();
        }

        //[Route("")]
        //[Route("")]
        public ICollection<ToDoList> Get()
        {
            var todolist = _context.ToDoList.Include(x => x.ListItem).ToList();
            return todolist;
        }

        [Route("ToDoByUser/{userid}")]
        public IHttpActionResult GetToDoByUser(string userId)
        {
            IList<ToDoList> todolist;

            //var cacheKey = GetCacheKey(ToDoByUserCacheKeyPrefix, userId);
            //cache.TryGetValue(cacheKey, out todolist);

            //if (todolist == null)
            //{
            todolist = _context.ToDoList.Where(t => t.UserID == userId).ToList();

            //    cache.Set(cacheKey, todolist, new MemoryCacheEntryOptions
            //    {
            //        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5)
            //    });
            //}

            return Json(todolist);
        }

        //[Route("Create")]
        public IHttpActionResult PostCreate([FromBody]ToDoList toDoList)
        {
            if (ModelState.IsValid)
            {
                _context.ToDoList.Add(toDoList);
                _context.SaveChanges();
                _logger.Info("Created list with id: " + toDoList.ToDoListID + " name: " + toDoList.Title);

                return Json(new ListItemsController.AjaxResponse { IsSuccess = true });
            }

            var errorList = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var error = new
            {
                Errors = errorList,
                Message = "Ret fejlen"
            };

            return Json(new ListItemsController.AjaxResponse
            {
                IsSuccess = false,
                Errors = errorList.Select(x => string.Join(",", x.Value)).ToList(),
                Message = "An error occured!"
            });

        }

        // [Route("Delete/{id:int}")]
        //[HttpPost]
        public IHttpActionResult Delete(int id)
        {
            ToDoList toDoList = _context.ToDoList.SingleOrDefault(m => m.ToDoListID == id);

            if (toDoList == null)
            {
                return NotFound();
            }
            _context.ToDoList.Remove(toDoList);
            _context.SaveChanges();
            _logger.Info("Deleted list: " + id + " name: " + toDoList.Title);
            return Json("OK");
        }

        //[Route("DeleteItems/{id:int}")]
        //public IHttpActionResult DeleteItems(int id)
        //{
        //    ListItem listItem = _context.ListItem.SingleOrDefault(m => m.ListItemID == id);
        //    _context.ListItem.Remove(listItem);
        //    _context.SaveChanges();
        //    _logger.Info("Deleted item id: " + id + " name: " + listItem.ItemName);

        //    return Json("OK");
        //}

    }
}