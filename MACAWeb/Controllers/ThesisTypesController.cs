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
    public class ThesisTypesController : Controller
    {
        private ThesisTypeDbContext db = new ThesisTypeDbContext();

        // GET: ThesisTypes
        public ActionResult Index()
        {
            return View(db.ThesisTypes.ToList());
        }

        // GET: ThesisTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThesisType thesisType = db.ThesisTypes.Find(id);
            if (thesisType == null)
            {
                return HttpNotFound();
            }
            return View(thesisType);
        }

        // GET: ThesisTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ThesisTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description")] ThesisType thesisType)
        {
            if (ModelState.IsValid)
            {
                thesisType.ThesisTypeID = Guid.NewGuid();

                thesisType.DateCreated = DateTime.Now;
                thesisType.DateModified = thesisType.DateCreated;

                thesisType.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                thesisType.UserModifiedID = thesisType.UserCreatedID;

                db.ThesisTypes.Add(thesisType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(thesisType);
        }

        // GET: ThesisTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThesisType thesisType = db.ThesisTypes.Find(id);
            if (thesisType == null)
            {
                return HttpNotFound();
            }
            return View(thesisType);
        }

        // POST: ThesisTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ThesisTypeID,Name,Description")] ThesisTypeViewModel thesisTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                ThesisType model = db.ThesisTypes.Find(thesisTypeViewModel.ThesisTypeID);

                model.Name = thesisTypeViewModel.Name;
                model.Description = thesisTypeViewModel.Description;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(thesisTypeViewModel);
        }

        // GET: ThesisTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThesisType thesisType = db.ThesisTypes.Find(id);
            if (thesisType == null)
            {
                return HttpNotFound();
            }
            return View(thesisType);
        }

        // POST: ThesisTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ThesisType thesisType = db.ThesisTypes.Find(id);
            db.ThesisTypes.Remove(thesisType);
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
