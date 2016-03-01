using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using ListPlanner.Models;

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
    }
}
