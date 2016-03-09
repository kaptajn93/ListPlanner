using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using ListPlanner.Models;
using System.Collections.Generic;

namespace ListPlanner.Controllers
{




    public class ToDoListsController : Controller
    {
        private ApplicationDbContext _context;

        public ToDoListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ToDoLists
        public IActionResult Index()
        {
            var applicationDbContext = _context.ToDoList.Include(t => t.User);
            return View(applicationDbContext.ToList());
        }

        //JUBII
        public JsonResult toDoJson()
        {
            var todolist = _context.ToDoList.Include(x => x.Items);


          //  var json = Newtonsoft.Json.JsonConvert.SerializeObject(todolist, Newtonsoft.Json.Formatting.Indented);


            return Json (todolist);
        }

        public JsonResult toDoByUser(int userId)
        {
            var todolist = _context.ToDoList.Where(t => t.UserID == userId);
            return Json(todolist);
        }

        // GET: ToDoLists/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            ToDoList toDoList = _context.ToDoList.Single(m => m.ToDoListID == id);
            if (toDoList == null)
            {
                return HttpNotFound();
            }

            return View(toDoList);
        }

        // GET: ToDoLists/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "User");
            return View();
        }

        // POST: ToDoLists/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ToDoList toDoList)
        {
            if (ModelState.IsValid)
            {
                var user =_context.User.FirstOrDefault();
                toDoList.User = user;
                              

                _context.ToDoList.Add(toDoList);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "User", toDoList.UserID);
            return View(toDoList);
        }

        // GET: ToDoLists/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            ToDoList toDoList = _context.ToDoList.Single(m => m.ToDoListID == id);
            if (toDoList == null)
            {
                return HttpNotFound();
            }
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "User", toDoList.UserID);
            return View(toDoList);
        }

        // POST: ToDoLists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ToDoList toDoList)
        {
            if (ModelState.IsValid)
            {
                _context.Update(toDoList);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "User", toDoList.UserID);
            return View(toDoList);
        }

        // GET: ToDoLists/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            ToDoList toDoList = _context.ToDoList.Single(m => m.ToDoListID == id);
            if (toDoList == null)
            {
                return HttpNotFound();
            }

            return View(toDoList);
        }

        // POST: ToDoLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            ToDoList toDoList = _context.ToDoList.Single(m => m.ToDoListID == id);
            _context.ToDoList.Remove(toDoList);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        //public ActionResult getID (ToDoList toDoList)
        //{
        //   var ToDoListID = _context.ToDoList.OrderByDescending(ID => ID.ToDoListID).FirstOrDefault();

        //    return ToDoListID;

        //}

    }



    //her
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }




        //public IList<ToDoList> Index2()
        //    {
        //        var applicationDbContext = _context.ToDoList.Include(t => t.User);
        //        var lists = applicationDbContext.ToList();
        //        return lists;
    }

    //og her
    [Route("api/[controller]")]
    public class ToDoListController : Controller
    {

        #region Dummy Data
        IList<ToDoList> toDoLists = new List<ToDoList>
        {

            //new ToDoList
            //{
            //    Selected = false,
            //    Title = "Tomato Soup",
            //    Items = new List<ListItem>
            //    {
            //        new ListItem { ItemName = "Tomato", IsDone = false},
            //        new ListItem { ItemName = "Soup", IsDone = false}
            //    },
                
            //},
            // new ToDoList
            //{
            //    Selected = false,
            //    Title = "Banana split",
            //    Items = new List<ListItem>
            //    {
            //        new ListItem { ItemName = "Banana", IsDone = false},
            //        new ListItem { ItemName = "Ice cream", IsDone = false}
            //    },
               
            //}
        }; 
        #endregion

        // /api/products
        [HttpGet]
        public IEnumerable<ToDoList> GetAllToDoLists()
        {
            return toDoLists;
        }

        // /api/products/1
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
          

            var toDoList = toDoLists.FirstOrDefault((p) => p.ToDoListID == id);
            if (toDoList == null)
            {
                return HttpNotFound();
            }
            return new ObjectResult(toDoList);
        }

        //ListItem[] items = new ListItem[]
        //{
        //    new ListItem {Name = "hej", ToDoListID = 0, ListItemID= 0, Parent = 0, IsDone = false },
        //    new ListItem {Name = "hej1", ToDoListID = 0, ListItemID= 1, Parent = 0, IsDone = false },
        //};
    }
}

//vm.toDoLists([
//    new ToDoList(false, "Sleep Over", [
//        new Item("Fine Wine", false),
//        new Item("Sleeping bag", false)]),
//    new ToDoList(false, "Party at 5th", [
//        new Item("Fine Wine", false),
//        new Item("Jelly shots", false)]),
//]);
