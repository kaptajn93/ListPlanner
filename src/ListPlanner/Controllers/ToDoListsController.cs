using System;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using ListPlanner.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading;
using ListPlanner.Filters;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace ListPlanner.Controllers
{
    public class AjaxResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; }
        public AjaxResponse()
        {
            Errors = new List<string>();
        }
    }

    public class ToDoListsController : Controller
    {
        private ApplicationDbContext _context;

        const string ToDoListCacheKeyPrefix = "ToDoListCache_";
        const string ToDoByUserCacheKeyPrefix = "ToDoByUserCache_";

        private IMemoryCache cache;
        public ToDoListsController(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            this.cache = cache;
        }

        // GET: ToDoLists
        public IActionResult Index()
        {
            
            var applicationDbContext = _context.ToDoList.Include(t => t.User);
            return View(applicationDbContext.ToList());
        }



        //JUBII
        [LogActionFilter]
        [HsmCache(CacheKey = ToDoListCacheKeyPrefix, Duration = 60)]
        public JsonResult toDoJson()
        {
            IList<ToDoList> todolist = _context.ToDoList.Include(x => x.Items).ToList();
            return Json(todolist);
        }


        //[HsmCache(CacheKey = ToDoByUserCacheKeyPrefix, Duration = 60)]
        public JsonResult toDoByUser(int userId)
        {
            IList<ToDoList> todolist;

            var cacheKey = GetCacheKey(ToDoByUserCacheKeyPrefix, userId);
            cache.TryGetValue(cacheKey, out todolist);

            if (todolist == null)
            {
                todolist = _context.ToDoList.Where(t => t.UserID == userId).ToList();

                cache.Set(cacheKey, todolist, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5)
                });
            }

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
        // [ValidateAntiForgeryToken]
        public JsonResult Create([FromBody]ToDoList toDoList)
        {
            if (ModelState.IsValid)
            {
                _context.ToDoList.Add(toDoList);
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
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            ToDoList toDoList = _context.ToDoList.Single(m => m.ToDoListID == id);
            _context.ToDoList.Remove(toDoList);
            _context.SaveChanges();
            return Json("OK");
        }
        //_____________________________________________Items

        // GET: /todolists/DeleteItems/5
        [ActionName("DeleteItems")]
        public IActionResult DeleteItems(int? id)
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
        [HttpPost, ActionName("DeleteItems")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteItemsConfirmed(int id)
        {
            ListItem listItem = _context.ListItem.Single(m => m.ListItemID == id);
            _context.ListItem.Remove(listItem);
            _context.SaveChanges();
            return Json("OK");
        }

        //______________________________________________



        private string GetCacheKey(string prefix, object key)
        {
            return prefix + key;
        }
    }

    


    //her

    //og her
    [Route("api/[controller]")]
    public class ToDoListController : Controller
    {

        #region Dummy Data
        IList<ToDoList> toDoLists = new List<ToDoList>
        {
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
    }





}

