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
    public class PublicationTypeGroupsController : Controller
    {
        private MACADbContext db = new MACADbContext();

        // GET: PublicationTypeGroup
        public ActionResult Index()
        {
            return View(db.PublicationTypeGroups.OrderBy(x => x.Name).ToList());
        }

        // GET: PublicationTypeGroup/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationTypeGroup publicationTypeGroup = db.PublicationTypeGroups.Find(id);
            if (publicationTypeGroup == null)
            {
                return HttpNotFound();
            }
            return View(publicationTypeGroup);
        }

        // GET: PublicationTypeGroup/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST:PublicationTypeGroup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description")] PublicationTypeGroup publicationTypeGroup)
        {
            if (ModelState.IsValid)
            {
                publicationTypeGroup.PublicationTypeGroupID = Guid.NewGuid();

                publicationTypeGroup.DateCreated = DateTime.Now;
                publicationTypeGroup.DateModified = publicationTypeGroup.DateCreated;

                publicationTypeGroup.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                publicationTypeGroup.UserModifiedID = publicationTypeGroup.UserCreatedID;
                db.PublicationTypeGroups.Add(publicationTypeGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(publicationTypeGroup);
        }

        // GET: publicationTypeGroup/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationTypeGroup publicationTypeGroup = db.PublicationTypeGroups.Find(id);
            if (publicationTypeGroup == null)
            {
                return HttpNotFound();
            }
            return View(publicationTypeGroup);
        }

        // POST: PublicationTypeGroup/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PublicationTypeGroupID,Name,Description")] PublicationTypeGroupViewModel publicationTypeGroupViewModel)
        {
            if (ModelState.IsValid)
            {
                PublicationTypeGroup model = db.PublicationTypeGroups.Find(publicationTypeGroupViewModel.PublicationTypeGroupID);

                model.Name = publicationTypeGroupViewModel.Name;
                model.Description = publicationTypeGroupViewModel.Description;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(publicationTypeGroupViewModel);
        }

        // GET: publicationTypeGroup/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationTypeGroup publicationTypeGroup = db.PublicationTypeGroups.Find(id);
            if (publicationTypeGroup == null)
            {
                return HttpNotFound();
            }
            return View(publicationTypeGroup);
        }

        // POST: PublicationTypeGroup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PublicationTypeGroup publicationTypeGroup = db.PublicationTypeGroups.Find(id);
            db.PublicationTypeGroups.Remove(publicationTypeGroup);
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
