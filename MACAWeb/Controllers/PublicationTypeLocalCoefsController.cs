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
    [Authorize(Roles = "SuperAdmin")]
    public class PublicationTypeLocalCoefsController : Controller
    {
        private PublicationTypeLocalCoefDbContext db = new PublicationTypeLocalCoefDbContext();

        // GET: PublicationTypeLocalCoefs
        public ActionResult Index()
        {
            var publicationTypesLocalCoef = db.PublicationTypesLocalCoef.Include(p => p.PublicationTypeLocal).OrderBy(x=> x.PublicationTypeLocal.Name).ThenBy(x => x.Year);
            return View(publicationTypesLocalCoef.ToList());
        }

        // GET: PublicationTypeLocalCoefs/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationTypeLocalCoef publicationTypeLocalCoef = db.PublicationTypesLocalCoef.Find(id);
            if (publicationTypeLocalCoef == null)
            {
                return HttpNotFound();
            }
            return View(publicationTypeLocalCoef);
        }

        // GET: PublicationTypeLocalCoefs/Create
        public ActionResult Create()
        {
            ViewBag.PublicationTypeLocalID = new SelectList(db.PublicationTypesLocal, "PublicationTypeLocalID", "Name");
            return View();
        }

        // POST: PublicationTypeLocalCoefs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PublicationTypeLocalCoefID,PublicationTypeLocalID,Year,Coefficient,Description,DateCreated,DateModified,UserCreatedID,UserModifiedID")] PublicationTypeLocalCoef publicationTypeLocalCoef)
        {
            if (ModelState.IsValid)
            {
                publicationTypeLocalCoef.PublicationTypeLocalCoefID = Guid.NewGuid();
                db.PublicationTypesLocalCoef.Add(publicationTypeLocalCoef);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PublicationTypeLocalID = new SelectList(db.PublicationTypesLocal, "PublicationTypeLocalID", "Name", publicationTypeLocalCoef.PublicationTypeLocalID);
            return View(publicationTypeLocalCoef);
        }

        // GET: PublicationTypeLocalCoefs/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationTypeLocalCoef publicationTypeLocalCoef = db.PublicationTypesLocalCoef.Find(id);
            if (publicationTypeLocalCoef == null)
            {
                return HttpNotFound();
            }
            ViewBag.PublicationTypeLocalID = new SelectList(db.PublicationTypesLocal, "PublicationTypeLocalID", "Name", publicationTypeLocalCoef.PublicationTypeLocalID);
            return View(publicationTypeLocalCoef);
        }

        // POST: PublicationTypeLocalCoefs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PublicationTypeLocalCoefID,PublicationTypeLocalID,Year,Coefficient,Description,DateCreated,DateModified,UserCreatedID,UserModifiedID")] PublicationTypeLocalCoef publicationTypeLocalCoef)
        {
            if (ModelState.IsValid)
            {
                db.Entry(publicationTypeLocalCoef).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PublicationTypeLocalID = new SelectList(db.PublicationTypesLocal, "PublicationTypeLocalID", "Name", publicationTypeLocalCoef.PublicationTypeLocalID);
            return View(publicationTypeLocalCoef);
        }

        // GET: PublicationTypeLocalCoefs/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationTypeLocalCoef publicationTypeLocalCoef = db.PublicationTypesLocalCoef.Find(id);
            if (publicationTypeLocalCoef == null)
            {
                return HttpNotFound();
            }
            return View(publicationTypeLocalCoef);
        }

        // POST: PublicationTypeLocalCoefs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PublicationTypeLocalCoef publicationTypeLocalCoef = db.PublicationTypesLocalCoef.Find(id);
            db.PublicationTypesLocalCoef.Remove(publicationTypeLocalCoef);
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
