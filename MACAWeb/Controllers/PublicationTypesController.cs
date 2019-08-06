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
    public class PublicationTypesController : Controller
    {
        private MACADbContext db = new MACADbContext();

        // GET: PublicationType
        public ActionResult Index()
        {
            return View(db.PublicationTypes.OrderBy(x => x.Name).ToList());
        }

        // GET: PublicationType/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationType publicationType = db.PublicationTypes.Find(id);
            if (publicationType == null)
            {
                return HttpNotFound();
            }
            return View(publicationType);
        }

        // GET: PublicationType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PublicationType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description")] PublicationType publicationType)
        {
            if (ModelState.IsValid)
            {
                publicationType.PublicationTypeID = Guid.NewGuid();

                publicationType.DateCreated = DateTime.Now;
                publicationType.DateModified = publicationType.DateCreated;

                publicationType.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                publicationType.UserModifiedID = publicationType.UserCreatedID;
                db.PublicationTypes.Add(publicationType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(publicationType);
        }

        // GET: PublicationType/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationType publicationType = db.PublicationTypes.Find(id);
            if (publicationType == null)
            {
                return HttpNotFound();
            }
            return View(publicationType);
        }

        // POST: PublicationType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PublicationTypeID,Name,Description")] PublicationTypeViewModel publicationTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                PublicationType model = db.PublicationTypes.Find(publicationTypeViewModel.PublicationTypeID);

                model.Name = publicationTypeViewModel.Name;
                model.Description = publicationTypeViewModel.Description;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());
            
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(publicationTypeViewModel);
        }

        // GET: PublicationType/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationType publicationType = db.PublicationTypes.Find(id);
            if (publicationType == null)
            {
                return HttpNotFound();
            }
            return View(publicationType);
        }

        // POST: PublicationType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PublicationType publicationType = db.PublicationTypes.Find(id);
            db.PublicationTypes.Remove(publicationType);
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
