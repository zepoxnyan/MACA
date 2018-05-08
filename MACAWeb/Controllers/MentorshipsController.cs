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
    [Authorize(Roles = "Admin")]
    public class MentorshipsController : Controller
    {
        private MentorshipsDbContext db = new MentorshipsDbContext();

        // GET: Mentorships
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var mentorships = db.Mentorships.Include(m => m.Person).Include(m => m.ThesisType)
                                .OrderBy(x => x.Year).ThenByDescending(x => x.Semester).ThenBy(x => x.Person.Surname);

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
                mentorships = mentorships.Where(m => m.ThesisType.Name.Contains(searchString)
                                      || m.Remarks.Contains(searchString)
                                      || m.Person.FullName.Contains(searchString)
                                      || m.Student.ToString().Contains(searchString)
                                      || m.ThesisTitle.ToString().Contains(searchString)
                                      || m.Year.ToString().Contains(searchString)
                                      || m.Semester.ToString().Contains(searchString))
                                      .OrderBy(x => x.Year).ThenByDescending(x => x.Semester).ThenBy(x => x.Person.Surname);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["generalItemsOnPage"]);
            int pageNumber = (page ?? 1);

            return View(mentorships.ToPagedList(pageNumber, pageSize));
        }

        // GET: Mentorships/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mentorship mentorship = db.Mentorships.Find(id);
            if (mentorship == null)
            {
                return HttpNotFound();
            }
            return View(mentorship);
        }

        // GET: Mentorships/Create
        public ActionResult Create()
        {
            PopulateThesisTypesDropDownList();
            PopulatePersonsDropDownList();
            return View();
        }

        private void PopulateThesisTypesDropDownList(object selectedThesisType = null, object selectedMentorshipType = null)
        {
            var thesisTypesQuery = from c in db.ThesisTypes
                                   orderby c.Name
                                   select c;
            ViewBag.ThesisTypeID = new SelectList(thesisTypesQuery, "ThesisTypeID", "Name", selectedThesisType);

            var mentorshipTypesQuery = from c in db.MentorshipTypes
                                   orderby c.Name
                                   select c;
            ViewBag.MentorshipTypeID = new SelectList(mentorshipTypesQuery, "MentorshipTypeID", "Name", selectedMentorshipType);
        }

        private void PopulatePersonsDropDownList(object selectedPerson = null)
        {
            var personQuery = from c in db.Persons
                              orderby c.Surname, c.Name
                              select new { PersonID = c.PersonID, Name = c.Surname + " " + c.Name };
            ViewBag.PersonID = new SelectList(personQuery, "PersonID", "Name", selectedPerson);
        }

        // POST: Mentorships/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ThesisTypeID,MentorshipTypeID,PersonID,Student,ThesisTitle,Remarks,Year,Semester")] Mentorship mentorship)
        {
            if (ModelState.IsValid)
            {
                mentorship.MentorshipID = Guid.NewGuid();

                mentorship.DateCreated = DateTime.Now;
                mentorship.DateModified = mentorship.DateCreated;

                mentorship.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                mentorship.UserModifiedID = mentorship.UserCreatedID;

                db.Mentorships.Add(mentorship);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateThesisTypesDropDownList();
            PopulatePersonsDropDownList();
            return View(mentorship);
        }

        // GET: Mentorships/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mentorship mentorship = db.Mentorships.Find(id);
            if (mentorship == null)
            {
                return HttpNotFound();
            }

            PopulateThesisTypesDropDownList(mentorship.ThesisTypeID, mentorship.MentorshipTypeID);
            PopulatePersonsDropDownList(mentorship.PersonID);
            return View(mentorship);
        }

        // POST: Mentorships/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MentorshipID,ThesisTypeID,MentorshipTypeID,PersonID,Student,ThesisTitle,Remarks,Year,Semester")] MentorshipViewModel mentorshipViewModel)
        {
            if (ModelState.IsValid)
            {
                Mentorship model = db.Mentorships.Find(mentorshipViewModel.MentorshipID);

                model.ThesisTypeID = mentorshipViewModel.ThesisTypeID;
                model.MentorshipTypeID = mentorshipViewModel.MentorshipTypeID;
                model.PersonID = mentorshipViewModel.PersonID;
                model.Student = mentorshipViewModel.Student;
                model.ThesisTitle = mentorshipViewModel.ThesisTitle;
                model.Remarks = mentorshipViewModel.Remarks;
                model.Year = mentorshipViewModel.Year;
                model.Semester = mentorshipViewModel.Semester;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateThesisTypesDropDownList(mentorshipViewModel.ThesisTypeID, mentorshipViewModel.MentorshipTypeID);
            PopulatePersonsDropDownList(mentorshipViewModel.PersonID);
            return View(mentorshipViewModel);
        }

        // GET: Mentorships/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mentorship mentorship = db.Mentorships.Find(id);
            if (mentorship == null)
            {
                return HttpNotFound();
            }            
            return View(mentorship);
        }

        // POST: Mentorships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Mentorship mentorship = db.Mentorships.Find(id);
            db.Mentorships.Remove(mentorship);
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
