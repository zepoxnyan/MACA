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
    public class StudyLevelsController : Controller
    {
        private MACADbContext db = new MACADbContext();

        // GET: StudyLevels
        public ActionResult Index()
        {
            return View(db.StudyLevels.OrderBy(x => x.Name).ToList());
        }

        // GET: StudyLevels/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudyLevel studyLevel = db.StudyLevels.Find(id);
            if (studyLevel == null)
            {
                return HttpNotFound();
            }
            return View(studyLevel);
        }

        // GET: StudyLevels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudyLevels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description")] StudyLevel studyLevel)
        {
            if (ModelState.IsValid)
            {
                studyLevel.StudyLevelID = Guid.NewGuid();

                studyLevel.DateCreated = DateTime.Now;
                studyLevel.DateModified = studyLevel.DateCreated;

                studyLevel.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                studyLevel.UserModifiedID = studyLevel.UserCreatedID;

                db.StudyLevels.Add(studyLevel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(studyLevel);
        }

        // GET: StudyLevels/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudyLevel studyLevel = db.StudyLevels.Find(id);
            if (studyLevel == null)
            {
                return HttpNotFound();
            }
            return View(studyLevel);
        }

        // POST: StudyLevels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudyLevelID,Name,Description")] StudyLevelViewModel studyLevelViewModel)
        {
            if (ModelState.IsValid)
            {
                StudyLevel model = db.StudyLevels.Find(studyLevelViewModel.StudyLevelID);

                model.Name = studyLevelViewModel.Name;
                model.Description = studyLevelViewModel.Description;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(studyLevelViewModel);
        }

        // GET: StudyLevels/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudyLevel studyLevel = db.StudyLevels.Find(id);
            if (studyLevel == null)
            {
                return HttpNotFound();
            }
            return View(studyLevel);
        }

        // POST: StudyLevels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            StudyLevel studyLevel = db.StudyLevels.Find(id);
            db.StudyLevels.Remove(studyLevel);
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
