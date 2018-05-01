using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MACAWeb.Models;

namespace MACAWeb.Controllers
{
    public class PublicationClassificationCoefficientsController : Controller
    {
        private PublicationClassificationCoefficientDbContext db = new PublicationClassificationCoefficientDbContext();

        // GET: PublicationClassificationCoefficients
        public ActionResult Index()
        {
            var publicationClassificationCoefficients = db.PublicationClassificationCoefficients.Include(p => p.PublicationClassification).Include(p => p.PublicationTypeGroup);
            return View(publicationClassificationCoefficients.ToList());
        }

        // GET: PublicationClassificationCoefficients/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationClassificationCoefficient publicationClassificationCoefficient = db.PublicationClassificationCoefficients.Find(id);
            if (publicationClassificationCoefficient == null)
            {
                return HttpNotFound();
            }
            return View(publicationClassificationCoefficient);
        }

        // GET: PublicationClassificationCoefficients/Create
        public ActionResult Create()
        {
            ViewBag.PublicationClassificationID = new SelectList(db.PublicationClassifications, "PublicationClassificationID", "Name");
            ViewBag.PublicationTypeGroupID = new SelectList(db.PublicationTypeGroups, "PublicationTypeGroupID", "Name");
            return View();
        }

        // POST: PublicationClassificationCoefficients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PublicationClassificationCoefficientID,PublicationClassificationID,PublicationTypeGroupID,Coefficient,Year,Description,DateCreated,DateModified,UserCreatedID,UserModifiedID")] PublicationClassificationCoefficient publicationClassificationCoefficient)
        {
            if (ModelState.IsValid)
            {
                publicationClassificationCoefficient.PublicationClassificationCoefficientID = Guid.NewGuid();
                db.PublicationClassificationCoefficients.Add(publicationClassificationCoefficient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PublicationClassificationID = new SelectList(db.PublicationClassifications, "PublicationClassificationID", "Name", publicationClassificationCoefficient.PublicationClassificationID);
            ViewBag.PublicationTypeGroupID = new SelectList(db.PublicationTypeGroups, "PublicationTypeGroupID", "Name", publicationClassificationCoefficient.PublicationTypeGroupID);
            return View(publicationClassificationCoefficient);
        }

        // GET: PublicationClassificationCoefficients/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationClassificationCoefficient publicationClassificationCoefficient = db.PublicationClassificationCoefficients.Find(id);
            if (publicationClassificationCoefficient == null)
            {
                return HttpNotFound();
            }
            ViewBag.PublicationClassificationID = new SelectList(db.PublicationClassifications, "PublicationClassificationID", "Name", publicationClassificationCoefficient.PublicationClassificationID);
            ViewBag.PublicationTypeGroupID = new SelectList(db.PublicationTypeGroups, "PublicationTypeGroupID", "Name", publicationClassificationCoefficient.PublicationTypeGroupID);
            return View(publicationClassificationCoefficient);
        }

        // POST: PublicationClassificationCoefficients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PublicationClassificationCoefficientID,PublicationClassificationID,PublicationTypeGroupID,Coefficient,Year,Description,DateCreated,DateModified,UserCreatedID,UserModifiedID")] PublicationClassificationCoefficient publicationClassificationCoefficient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(publicationClassificationCoefficient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PublicationClassificationID = new SelectList(db.PublicationClassifications, "PublicationClassificationID", "Name", publicationClassificationCoefficient.PublicationClassificationID);
            ViewBag.PublicationTypeGroupID = new SelectList(db.PublicationTypeGroups, "PublicationTypeGroupID", "Name", publicationClassificationCoefficient.PublicationTypeGroupID);
            return View(publicationClassificationCoefficient);
        }

        // GET: PublicationClassificationCoefficients/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationClassificationCoefficient publicationClassificationCoefficient = db.PublicationClassificationCoefficients.Find(id);
            if (publicationClassificationCoefficient == null)
            {
                return HttpNotFound();
            }
            return View(publicationClassificationCoefficient);
        }

        // POST: PublicationClassificationCoefficients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PublicationClassificationCoefficient publicationClassificationCoefficient = db.PublicationClassificationCoefficients.Find(id);
            db.PublicationClassificationCoefficients.Remove(publicationClassificationCoefficient);
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
