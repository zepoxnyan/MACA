using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MACAWeb.Models;
using Microsoft.AspNet.Identity;
using System.Configuration;
using PagedList;

namespace MACAWeb.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TaskStatusController : Controller
    {
        private TaskStatusDbContext db = new TaskStatusDbContext();

        // GET: TaskStatus
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var statuses = db.Statuses.OrderBy(x => x.Name);

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                statuses = statuses.Where(m => m.Name.Contains(searchString)
                                      || m.Description.Contains(searchString))
                                      .OrderBy(m => m.Name);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["generalItemsOnPage"]);
            int pageNumber = (page ?? 1);

            return View(statuses.ToPagedList(pageNumber, pageSize));
        }

        // GET: TaskStatus/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskStatus taskStatus = db.Statuses.Find(id);
            if (taskStatus == null)
            {
                return HttpNotFound();
            }
            return View(taskStatus);
        }

        // GET: TaskStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TaskStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description,Message")] TaskStatus status)
        {
            if (ModelState.IsValid)
            {
                status.TaskStatusId = Guid.NewGuid();

                status.DateCreated = DateTime.Now;
                status.DateModified = status.DateCreated;
                status.UserCreatedId = Auxiliaries.GetUserId(User);
                status.UserModifiedId = status.UserCreatedId;

                db.Statuses.Add(status);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(status);
        }

        // GET: TaskStatus/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskStatus taskStatus = db.Statuses.Find(id);
            if (taskStatus == null)
            {
                return HttpNotFound();
            }
            return View(taskStatus);
        }

        // POST: TaskStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaskStatusId,Name,Description,Message")] TaskStatusViewModel taskStatusViewModel)
        {
            if (ModelState.IsValid)
            {
                TaskStatus status = db.Statuses.Find(taskStatusViewModel.TaskStatusId);

                status.Name = taskStatusViewModel.Name;
                status.Description = taskStatusViewModel.Description;
                status.Message = taskStatusViewModel.Message;

                status.DateModified = DateTime.Now;
                status.UserModifiedId = Auxiliaries.GetUserId(User);              
            
                db.Entry(status).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(taskStatusViewModel);
        }

        // GET: TaskStatus/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskStatus taskStatus = db.Statuses.Find(id);
            if (taskStatus == null)
            {
                return HttpNotFound();
            }
            return View(taskStatus);
        }

        // POST: TaskStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TaskStatus taskStatus = db.Statuses.Find(id);
            db.Statuses.Remove(taskStatus);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}