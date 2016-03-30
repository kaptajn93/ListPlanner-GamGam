 using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace ListPlanner_GamGam.Controllers
{
    
    public class UsersController : ApiController
    {
        private HsmDbContext _context;

        public UsersController()
        {
            _context = new HsmDbContext();
        }

        public ICollection<User> GetList()
        {
            var users = _context.User.ToList();
            return users;
        }

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public IHttpActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                _context.User.Add(user);
                _context.SaveChanges();
                return Ok("User successfully created");
            }
            return NotFound();
        }

        [System.Web.Mvc.ActionName("Delete")]
        public IHttpActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            User user = _context.User.Single(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IHttpActionResult DeleteConfirmed(string id)
        {
            User user = _context.User.Single(m => m.Id == id);
            _context.User.Remove(user);
            _context.SaveChanges();
            return Ok("Successfully deleted user!");
        }
    }



}
