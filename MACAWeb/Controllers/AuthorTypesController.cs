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

namespace MACAWeb.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AuthorTypesController : Controller
    {
        private MACADbContext db = new MACADbContext();

        // GET: AuthorTypes
        public ActionResult Index()
        {
            return View(db.AuthorTypes.OrderBy(x => x.Name).ToList());
        }

        // GET: AuthorTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuthorType authorType = db.AuthorTypes.Find(id);
            if (authorType == null)
            {
                return HttpNotFound();
            }
            return View(authorType);
        }

        // GET: AuthorTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthorTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description")] AuthorType authorType)
        {
            if (ModelState.IsValid)
            {
                authorType.AuthorTypeID = Guid.NewGuid();

                authorType.DateCreated = DateTime.Now;
                authorType.DateModified = authorType.DateCreated;

                authorType.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                authorType.UserModifiedID = authorType.UserCreatedID;

                db.AuthorTypes.Add(authorType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(authorType);
        }

        // GET: AuthorTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuthorType authorType = db.AuthorTypes.Find(id);
            if (authorType == null)
            {
                return HttpNotFound();
            }
            return View(authorType);
        }

        // POST: AuthorTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AuthorTypeID,Name,Description")] AuthorTypeViewModel authorTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                AuthorType model = db.AuthorTypes.Find(authorTypeViewModel.AuthorTypeID);

                model.Name = authorTypeViewModel.Name;
                model.Description = authorTypeViewModel.Description;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(authorTypeViewModel);
        }

        // GET: AuthorTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuthorType authorType = db.AuthorTypes.Find(id);
            if (authorType == null)
            {
                return HttpNotFound();
            }
            return View(authorType);
        }

        // POST: AuthorTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AuthorType authorType = db.AuthorTypes.Find(id);
            db.AuthorTypes.Remove(authorType);
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
