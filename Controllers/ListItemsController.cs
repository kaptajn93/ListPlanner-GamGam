using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Http;
using NLog;

namespace ListPlanner_GamGam.Controllers
{
    
    public class ListItemsController : ApiController
    {
        readonly Logger logger = LogManager.GetCurrentClassLogger();
        private  readonly HsmDbContext _context;

        public ListItemsController()
        {
            _context = new HsmDbContext();

        }

        //[HttpPost]
        //   [Route("Create")]
        public IHttpActionResult PostCreate([FromBody] ListItem listItem)
        {
            if (ModelState.IsValid)
            {
                _context.ListItem.Add(listItem);
                _context.SaveChanges();
                logger.Info("Created item: " + listItem.ListItemID + " name: " + listItem.ItemName);

                return Json(new AjaxResponse { IsSuccess = true });
            }
            var errorList = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            var error = new
            {
                //ErrorCount = ModelState.ErrorCount,
                Errors = errorList,
                Message = "Ret fejlen"
            };

            return Json(new AjaxResponse
            {
                IsSuccess = false,
                Errors = errorList.Select(x => string.Join(",", x.Value)).ToList(),
                Message = "An error occured!"
            });

        }

        //[HttpPut]
        //  [Route("Update")]

        public IHttpActionResult PutUpdate([FromBody] ListItem listItem)
        {
            if (ModelState.IsValid)
            {

                _context.ListItem.AddOrUpdate(listItem);
                _context.SaveChanges();
                logger.Info("Updated item: " + listItem.ListItemID + " name: " + listItem.ItemName);

                return Json(new AjaxResponse { IsSuccess = true });
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

            return Json(new AjaxResponse
            {
                IsSuccess = false,
                Errors = errorList.Select(x => string.Join(",", x.Value)).ToList(),
                Message = "An error occured!"
            });

        }


        public class AjaxResponse
        {
            public bool IsSuccess { get; set; }
            public List<string> Errors { get; set; }
            public string Message { get; set; }
        }
        [HttpDelete]
        public IHttpActionResult DeleteItems(int id)
        {
            ListItem listItem = _context.ListItem.SingleOrDefault(m => m.ListItemID == id);
            _context.ListItem.Remove(listItem);
            _context.SaveChanges();
            logger.Info("Deleted item id: " + id + " name: " + listItem.ItemName);

            return Json("OK");
        }
    }
}
