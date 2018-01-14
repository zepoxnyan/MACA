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
    public class EventTypesController : Controller
    {
        private EventTypeDbContext db = new EventTypeDbContext();

        // GET: EventTypes
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var eventTypes = db.EventTypes.OrderBy(x => x.Code);

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.Currentfilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                eventTypes = eventTypes.Where(m => m.Description.Contains(searchString) || m.Code.Contains(searchString))
                                      .OrderBy(m => m.Code);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["catItemsOnPage"]);
            int pageNumber = (page ?? 1);

            return View(eventTypes.ToPagedList(pageNumber, pageSize));
        }

        // GET: EventTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventType eventType = db.EventTypes.Find(id);
            if (eventType == null)
            {
                return HttpNotFound();
            }
            return View(eventType);
        }

        // GET: EventTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventTypeID,Code,Description")] EventType eventType)
        {
            if (ModelState.IsValid)
            {
                eventType.EventTypeID = Guid.NewGuid();
                eventType.DateCreated = DateTime.Now;
                eventType.DateModified = DateTime.Now;
                eventType.UserCreatedID = new Guid(User.Identity.GetUserId());
                eventType.UserModifiedID = new Guid(User.Identity.GetUserId());

                db.EventTypes.Add(eventType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eventType);
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
