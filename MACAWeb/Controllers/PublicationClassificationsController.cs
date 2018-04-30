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
    public class PublicationClassificationsController : Controller
    {
        private PublicationClassificationDbContext db = new PublicationClassificationDbContext();

        // GET: PublicationClassifications
        public ActionResult Index()
        {
            return View(db.PublicationClassification.OrderBy(x => x.Name).ToList());
        }

        // GET: PublicationClassifications/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationClassification publicationClassification = db.PublicationClassification.Find(id);
            if (publicationClassification == null)
            {
                return HttpNotFound();
            }
            return View(publicationClassification);
        }

        // GET: PublicationClassifications/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PublicationClassifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description")] PublicationClassification publicationClassification)
        {
            if (ModelState.IsValid)
            {
                publicationClassification.PublicationClassificationID = Guid.NewGuid();

                publicationClassification.DateCreated = DateTime.Now;
                publicationClassification.DateModified = publicationClassification.DateCreated;

                publicationClassification.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                publicationClassification.UserModifiedID = publicationClassification.UserCreatedID;

                db.PublicationClassification.Add(publicationClassification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(publicationClassification);
        }

        // GET: PublicationClassifications/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationClassification publicationClassification = db.PublicationClassification.Find(id);
            if (publicationClassification == null)
            {
                return HttpNotFound();
            }
            return View(publicationClassification);
        }

        // POST: PublicationClassifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PublicationClassificationID,Name,Description")] PublicationClassificationViewModel publicationClassificationViewModel)
        {
            if (ModelState.IsValid)
            {
                PublicationClassification model = db.PublicationClassification.Find(publicationClassificationViewModel.PublicationClassificationID);

                model.Name = publicationClassificationViewModel.Name;
                model.Description = publicationClassificationViewModel.Description;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(publicationClassificationViewModel);
        }

        // GET: PublicationClassifications/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationClassification publicationClassification = db.PublicationClassification.Find(id);
            if (publicationClassification == null)
            {
                return HttpNotFound();
            }
            return View(publicationClassification);
        }

        // POST: PublicationClassifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PublicationClassification publicationClassification = db.PublicationClassification.Find(id);
            db.PublicationClassification.Remove(publicationClassification);
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
