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
    public class MentorshipTypesController : Controller
    {
        private MACADbContext db = new MACADbContext();

        // GET: MentorshipTypes
        public ActionResult Index()
        {
            return View(db.MentorshipTypes.OrderBy(x => x.Name).ToList());
        }

        // GET: MentorshipTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MentorshipType mentorshipType = db.MentorshipTypes.Find(id);
            if (mentorshipType == null)
            {
                return HttpNotFound();
            }
            return View(mentorshipType);
        }

        // GET: MentorshipTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MentorshipTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description")] MentorshipType model)
        {
            if (ModelState.IsValid)
            {
                model.MentorshipTypeID = Guid.NewGuid();

                model.DateCreated = DateTime.Now;
                model.DateModified = model.DateCreated;

                model.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                model.UserModifiedID = model.UserCreatedID;

                db.MentorshipTypes.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: MentorshipTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MentorshipType mentorshipType = db.MentorshipTypes.Find(id);
            if (mentorshipType == null)
            {
                return HttpNotFound();
            }
            return View(mentorshipType);
        }

        // POST: MentorshipTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MentorshipTypeID,Name,Description")] MentorshipTypeViewModel mentorshipTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                MentorshipType model = db.MentorshipTypes.Find(mentorshipTypeViewModel.MentorshipTypeID);

                model.Name = mentorshipTypeViewModel.Name;
                model.Description = mentorshipTypeViewModel.Description;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mentorshipTypeViewModel);
        }

        // GET: MentorshipTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MentorshipType mentorshipType = db.MentorshipTypes.Find(id);
            if (mentorshipType == null)
            {
                return HttpNotFound();
            }
            return View(mentorshipType);
        }

        // POST: MentorshipTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            MentorshipType mentorshipType = db.MentorshipTypes.Find(id);
            db.MentorshipTypes.Remove(mentorshipType);
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
