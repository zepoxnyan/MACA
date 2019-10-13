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
    [Authorize(Roles = "Admin, Employee, SuperAdmin")]
    public class SubjectsController : Controller
    {
        private MACADbContext db = new MACADbContext();

        // GET: Subjects
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var subjects = db.Subjects.OrderBy(x => x.Name).ThenBy(x => x.Year).ThenByDescending(x => x.Semester);

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
                                      || m.Year.Contains(searchString)
                                      || m.ShortName.ToString().Contains(searchString)
                                      || m.Department.ToString().Contains(searchString)
                                      || m.Semester.ToString().Contains(searchString))
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
            //PopulateStudyLevelDropDownList();
            return View();
        }

        /*private void PopulateStudyLevelDropDownList(object selectedStudyLevel = null)
        {
            var studyLevelQuery = from c in dbSubjects.StudyLevels
                                   orderby c.Name
                                   select c;
            ViewBag.StudyLevelID = new SelectList(studyLevelQuery, "StudyLevelID", "Name", selectedStudyLevel);
        }*/

        // POST: Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description,Year,Semester,AISCode,ShortName,Department")] Subject subject)
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

            //PopulateStudyLevelDropDownList();
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
            //PopulateStudyLevelDropDownList(subject.StudyLevelID);
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubjectID,Name,Description,Year,Semester,AISCode,ShortName,Department")] SubjectVievModel subjectViewModel)
        {
            if (ModelState.IsValid)
            {
                Subject subject = db.Subjects.Find(subjectViewModel.SubjectID);

                subject.Name = subjectViewModel.Name;
                //subject.StudyLevelID = subjectViewModel.StudyLevelID;
                subject.Description = subjectViewModel.Description;
                subject.Year = subjectViewModel.Year;
                subject.Semester = subjectViewModel.Semester;
                subject.AISCode = subjectViewModel.AISCode;
                subject.ShortName = subjectViewModel.ShortName;
                subject.Department = subjectViewModel.Department;

                subject.DateModified = DateTime.Now;
                subject.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                db.Entry(subject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //PopulateStudyLevelDropDownList(subjectViewModel.StudyLevelID);
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

        #region Teachers

        public ActionResult TeachersIndex(Guid subjectId)
        {
            var teachers = db.Teachings.Where(x => x.SubjectID == subjectId).OrderBy(x => x.Person.Surname);

            ViewBag.SubjectID = subjectId;
            return View(teachers);
        }

        public ActionResult TeachersCreate(Guid subjectId)
        {
            PopulateTeachingTypesDropDownList();
            PopulatePersonsDropDownList();
            ViewBag.SubjectID = subjectId;

            return View();
        }

        private void PopulateTeachingTypesDropDownList(object selectedTeachingType = null)
        {
            var teachingTypesQuery = from c in db.TeachingTypes
                                        orderby c.Name
                                        select c;
            ViewBag.TeachingTypeID = new SelectList(teachingTypesQuery, "TeachingTypeID", "Name", selectedTeachingType);
        }

        private void PopulatePersonsDropDownList(object selectedPerson = null)
        {
            var personQuery = from c in db.Persons
                              orderby c.Surname, c.Name
                              select new { c.PersonID, NameCombo = c.Surname + " " + c.Name };
            ViewBag.PersonID = new SelectList(personQuery, "PersonID", "NameCombo", selectedPerson);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TeachersCreate([Bind(Include = "TeachingTypeID,PersonID,Hours,Weight,Remark,SubjectID")] Teaching teaching, string subjectId)
        {
            if (ModelState.IsValid)
            {
                teaching.TeachingID = Guid.NewGuid();

                teaching.DateCreated = DateTime.Now;
                teaching.DateModified = teaching.DateCreated;

                teaching.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                teaching.UserModifiedID = teaching.UserCreatedID;

                db.Teachings.Add(teaching);
                db.SaveChanges();
                return RedirectToAction("TeachersIndex", new { subjectId = subjectId });
            }

            return View(teaching);
        }

        public ActionResult TeachersEdit(Guid? id, Guid subjectId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teaching teaching = db.Teachings.Find(id);
            if (teaching == null)
            {
                return HttpNotFound();
            }
            PopulateTeachingTypesDropDownList(teaching.TeachingTypeID);
            PopulatePersonsDropDownList(teaching.PersonID);
            ViewBag.SubjectID = subjectId;
            return View(teaching);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TeachersEdit([Bind(Include = "TeachingID,TeachingTypeID,PersonID,Hours,Weight,Remark,SubjectID")] TeachingVievModel teachingViewModel, string subjectId)
        {
            if (ModelState.IsValid)
            {
                Teaching teaching = db.Teachings.Find(teachingViewModel.TeachingID);

                teaching.TeachingTypeID = teachingViewModel.TeachingTypeID;
                teaching.PersonID = teachingViewModel.PersonID;
                teaching.Weight = teachingViewModel.Weight;
                teaching.Hours = teachingViewModel.Hours;
                teaching.Remark = teachingViewModel.Remark;

                teaching.DateModified = DateTime.Now;
                teaching.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                db.Entry(teaching).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("TeachersIndex", routeValues: new { subjectId = teaching.SubjectID });
            }
            return View(teachingViewModel);
        }

        public ActionResult TeachersDelete(Guid? id, Guid subjectId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teaching teaching = db.Teachings.Find(id);
            if (teaching == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectID = subjectId;

            return View(teaching);
        }

        [HttpPost, ActionName("TeachersDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult TeachersDeleteConfirmed(Guid id)
        {
            Teaching teaching = db.Teachings.Find(id);
            db.Teachings.Remove(teaching);
            db.SaveChanges();
            return RedirectToAction("TeachersIndex", routeValues: new { subjectId = teaching.SubjectID });
        }

        #endregion
    }
}
