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
    [Authorize(Roles = "SuperAdmin")]
    public class SubjectsController : Controller
    {
        private SubjectsDbContext db = new SubjectsDbContext();

        // GET: Subjects
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var subjects = db.Subjects.Include(s => s.StudyLevel)
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Year)
                .ThenByDescending(x => x.Semester);

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
                subjects = subjects.Where(m => m.Name.Contains(searchString)
                                      || m.Description.Contains(searchString)
                                      || m.Year.ToString().Contains(searchString)
                                      || m.Semester.ToString().Contains(searchString)
                                      || m.StudyLevel.Name.ToString().Contains(searchString))
                                      .OrderBy(x => x.Name)
                                      .ThenBy(x => x.Year)
                                      .ThenByDescending(x => x.Semester);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["generalItemsOnPage"]);
            int pageNumber = (page ?? 1);

            return View(subjects.ToPagedList(pageNumber, pageSize));
        }

        // GET: Subjects/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // GET: Subjects/Create
        public ActionResult Create()
        {
            PopulateStudyLevelDropDownList();
            return View();
        }

        private void PopulateStudyLevelDropDownList(object selectedStudyLevel = null)
        {
            var studyLevelQuery = from c in db.StudyLevels
                                   orderby c.Name
                                   select c;
            ViewBag.StudyLevelID = new SelectList(studyLevelQuery, "StudyLevelID", "Name", selectedStudyLevel);
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,StudyLevelID,Description,Year,Semester")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                subject.SubjectID = Guid.NewGuid();

                subject.DateCreated = DateTime.Now;
                subject.DateModified = subject.DateCreated;

                subject.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                subject.UserModifiedID = subject.UserCreatedID;

                db.Subjects.Add(subject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateStudyLevelDropDownList();
            return View(subject);
        }

        // GET: Subjects/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            PopulateStudyLevelDropDownList(subject.StudyLevelID);
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubjectID,Name,StudyLevelID,Description,Year,Semester")] SubjectVievModel subjectViewModel)
        {
            if (ModelState.IsValid)
            {
                Subject subject = db.Subjects.Find(subjectViewModel.SubjectID);

                subject.Name = subjectViewModel.Name;
                subject.StudyLevelID = subjectViewModel.StudyLevelID;
                subject.Description = subjectViewModel.Description;
                subject.Year = subjectViewModel.Year;
                subject.Semester = subjectViewModel.Semester;

                subject.DateModified = DateTime.Now;
                subject.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                db.Entry(subject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateStudyLevelDropDownList(subjectViewModel.StudyLevelID);
            return View(subjectViewModel);
        }

        // GET: Subjects/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Subject subject = db.Subjects.Find(id);
            db.Subjects.Remove(subject);
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
