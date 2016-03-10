using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using ListPlanner.Models;


namespace ListPlanner.Controllers
{
    public class ListItemsController : Controller
    {
        private ApplicationDbContext _context;

        public ListItemsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: ListItems


        public IActionResult Index()
        {
            // var applicationDbContext = _context.ListItem.Include(l => l.ToDoList);
            //return View(applicationDbContext.ToList());
            return View(_context.ListItem.ToList());
        }

        // GET: ListItems/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            ListItem listItem = _context.ListItem.Single(m => m.ListItemID == id);
            if (listItem == null)
            {
                return HttpNotFound();
            }

            return View(listItem);
        }

        // GET: ListItems/Create
        public IActionResult Create()
        {
            ViewData["ToDoListID"] = new SelectList(_context.Set<ToDoList>(), "ToDoListID", "ToDoList");
            return View();
        }
        //// POST: ListItems/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(ListItem listItem)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.ListItem.Add(listItem);
        //        _context.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewData["ToDoListID"] = new SelectList(_context.Set<ToDoList>(), "ToDoListID", "ToDoList", listItem.ToDoListID);
        //    return View(listItem);
        //}

        // POST: ListItems/Create
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult Create([FromBody]ListItem listItem)
        {
            if (ModelState.IsValid)
            {
              
                _context.ListItem.Add(listItem);
                _context.SaveChanges();
                return Json(new AjaxResponse { IsSuccess = true });
            }

         

            var errorList = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );

            var error = new
            {
                ErrorCount = ModelState.ErrorCount,
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


        // GET: ListItems/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            ListItem listItem = _context.ListItem.Single(m => m.ListItemID == id);
            if (listItem == null)
            {
                return HttpNotFound();
            }
            ViewData["ToDoListID"] = new SelectList(_context.Set<ToDoList>(), "ToDoListID", "ToDoList", listItem.ToDoListID);
            return View(listItem);
        }

        // POST: ListItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ListItem listItem)
        {
            if (ModelState.IsValid)
            {
                _context.Update(listItem);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["ToDoListID"] = new SelectList(_context.Set<ToDoList>(), "ToDoListID", "ToDoList", listItem.ToDoListID);
            return View(listItem);
        }

        // GET: ListItems/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            ListItem listItem = _context.ListItem.Single(m => m.ListItemID == id);
            if (listItem == null)
            {
                return HttpNotFound();
            }

            return View(listItem);
        }

        // POST: ListItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            ListItem listItem = _context.ListItem.Single(m => m.ListItemID == id);
            _context.ListItem.Remove(listItem);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
