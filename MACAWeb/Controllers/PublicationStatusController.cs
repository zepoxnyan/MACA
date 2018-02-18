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
    public class PublicationStatusController : Controller
    {
        private PublicationStatusDbContext db = new PublicationStatusDbContext();

        // GET: PublicationStatus
        public ActionResult Index()
        {
            return View(db.PublicationStatus.OrderBy(x => x.Name).ToList());
        }

        // GET: PublicationStatus/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationStatus publicationStatus = db.PublicationStatus.Find(id);
            if (publicationStatus == null)
            {
                return HttpNotFound();
            }
            return View(publicationStatus);
        }

        // GET: PublicationStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PublicationStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description")] PublicationStatus publicationStatus)
        {
            if (ModelState.IsValid)
            {
                publicationStatus.PublicationStatusID = Guid.NewGuid();

                publicationStatus.DateCreated = DateTime.Now;
                publicationStatus.DateModified = publicationStatus.DateCreated;

                publicationStatus.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                publicationStatus.UserModifiedID = publicationStatus.UserCreatedID;
                db.PublicationStatus.Add(publicationStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(publicationStatus);
        }

        // GET: PublicationStatus/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationStatus publicationStatus = db.PublicationStatus.Find(id);
            if (publicationStatus == null)
            {
                return HttpNotFound();
            }
            return View(publicationStatus);
        }

        // POST: PublicationStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PublicationStatusID,Name,Description")] PublicationStatusViewModel publicationStatusViewModel)
        {
            if (ModelState.IsValid)
            {
                PublicationStatus model = db.PublicationStatus.Find(publicationStatusViewModel.PublicationStatusID);

                model.Name = publicationStatusViewModel.Name;
                model.Description = publicationStatusViewModel.Description;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());
            
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(publicationStatusViewModel);
        }

        // GET: PublicationStatus/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationStatus publicationStatus = db.PublicationStatus.Find(id);
            if (publicationStatus == null)
            {
                return HttpNotFound();
            }
            return View(publicationStatus);
        }

        // POST: PublicationStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PublicationStatus publicationStatus = db.PublicationStatus.Find(id);
            db.PublicationStatus.Remove(publicationStatus);
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
