using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MACAWeb.Models;
using System.Configuration;
using PagedList;
using Microsoft.AspNet.Identity;

namespace MACAWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GrantsController : Controller
    {
        private GrantDbContext db = new GrantDbContext();

        // GET: Grants
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var grants = db.Grants.Include(x => x.GrantStatus).OrderByDescending(x => x.Start).ThenBy(x => x.Name);
            
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
                grants = grants.Where(m => m.Name.Contains(searchString)
                                      || m.Description.Contains(searchString)
                                      || m.GrantStatus.Name.Contains(searchString)
                                      || m.Start.ToString().Contains(searchString)
                                      || m.End.ToString().Contains(searchString))
                                      .OrderBy(m => m.Name);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["generalItemsOnPage"]);
            int pageNumber = (page ?? 1);

            return View(grants.ToPagedList(pageNumber, pageSize));
        }

        // GET: Grants/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grant grant = db.Grants.Find(id);
            if (grant == null)
            {
                return HttpNotFound();
            }
            return View(grant);
        }

        // GET: Grants/Create
        public ActionResult Create()
        {
            PopulateGrantStatusDropDownList();
            return View();
        }

        private void PopulateGrantStatusDropDownList(object selectedGrantStatus = null)
        {
            var grantStatusQuery = from c in db.GrantStatuses
                                   orderby c.Name
                                   select c;
            ViewBag.GrantStatusID = new SelectList(grantStatusQuery, "GrantStatusID", "Name", selectedGrantStatus);
        }

        // POST: Grants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GrantStatusID,Name,Description,Start,End")] Grant grant)
        {
            if (ModelState.IsValid)
            {
                grant.GrantID = Guid.NewGuid();

                grant.DateCreated = DateTime.Now;
                grant.DateModified = grant.DateCreated;

                grant.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                grant.UserModifiedID = grant.UserCreatedID;

                db.Grants.Add(grant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(grant);
        }

        // GET: Grants/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grant grant = db.Grants.Find(id);
            if (grant == null)
            {
                return HttpNotFound();
            }
            PopulateGrantStatusDropDownList(grant.GrantStatusID);
            return View(grant);
        }

        // POST: Grants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GrantID,GrantStatusID,Name,Description,Start,End")] GrantViewModel grantViewModel)
        {
            if (ModelState.IsValid)
            {
                Grant model = db.Grants.Find(grantViewModel.GrantID);

                model.Name = grantViewModel.Name;
                model.GrantStatusID = grantViewModel.GrantStatusID;
                model.Description = grantViewModel.Description;
                model.Start = grantViewModel.Start;
                model.End = grantViewModel.End;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grantViewModel);
        }

        // GET: Grants/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grant grant = db.Grants.Find(id);
            if (grant == null)
            {
                return HttpNotFound();
            }
            return View(grant);
        }

        // POST: Grants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Grant grant = db.Grants.Find(id);
            db.Grants.Remove(grant);
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
