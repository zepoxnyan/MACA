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
    public class TeachingTypesController : Controller
    {
        private TeachingTypeDbContext db = new TeachingTypeDbContext();

        // GET: TeachingTypes
        public ActionResult Index()
        {
            return View(db.TeachingTypes.ToList());
        }

        // GET: TeachingTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeachingType teachingType = db.TeachingTypes.Find(id);
            if (teachingType == null)
            {
                return HttpNotFound();
            }
            return View(teachingType);
        }

        // GET: TeachingTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TeachingTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description")] TeachingType teachingType)
        {
            if (ModelState.IsValid)
            {
                teachingType.TeachingTypeID = Guid.NewGuid();

                teachingType.DateCreated = DateTime.Now;
                teachingType.DateModified = teachingType.DateCreated;

                teachingType.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                teachingType.UserModifiedID = teachingType.UserCreatedID;

                db.TeachingTypes.Add(teachingType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(teachingType);
        }

        // GET: TeachingTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeachingType teachingType = db.TeachingTypes.Find(id);
            if (teachingType == null)
            {
                return HttpNotFound();
            }
            return View(teachingType);
        }

        // POST: TeachingTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeachingTypeID,Name,Description")] TeachingTypeViewModel teachingTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                TeachingType model = db.TeachingTypes.Find(teachingTypeViewModel.TeachingTypeID);

                model.Name = teachingTypeViewModel.Name;
                model.Description = teachingTypeViewModel.Description;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

            
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teachingTypeViewModel);
        }

        // GET: TeachingTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeachingType teachingType = db.TeachingTypes.Find(id);
            if (teachingType == null)
            {
                return HttpNotFound();
            }
            return View(teachingType);
        }

        // POST: TeachingTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TeachingType teachingType = db.TeachingTypes.Find(id);
            db.TeachingTypes.Remove(teachingType);
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
