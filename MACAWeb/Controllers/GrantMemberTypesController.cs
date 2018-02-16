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
    public class GrantMemberTypesController : Controller
    {
        private GrantMemberTypeDbContext db = new GrantMemberTypeDbContext();

        // GET: GrantMemberTypes
        public ActionResult Index()
        {
            return View(db.GrantMemberTypes.ToList());
        }

        // GET: GrantMemberTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrantMemberType grantMemberType = db.GrantMemberTypes.Find(id);
            if (grantMemberType == null)
            {
                return HttpNotFound();
            }
            return View(grantMemberType);
        }

        // GET: GrantMemberTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GrantMemberTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description,Coefficient")] GrantMemberType grantMemberType)
        {
            if (ModelState.IsValid)
            {
                grantMemberType.GrantMemberTypeID = Guid.NewGuid();

                grantMemberType.DateCreated = DateTime.Now;
                grantMemberType.DateModified = grantMemberType.DateCreated;

                grantMemberType.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                grantMemberType.UserModifiedID = grantMemberType.UserCreatedID;

                db.GrantMemberTypes.Add(grantMemberType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(grantMemberType);
        }

        // GET: GrantMemberTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrantMemberType grantMemberType = db.GrantMemberTypes.Find(id);
            if (grantMemberType == null)
            {
                return HttpNotFound();
            }
            return View(grantMemberType);
        }

        // POST: GrantMemberTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GrantMemberTypeID,Name,Description,Coefficient")] GrantMemberTypeViewModel grantMemberTypeView)
        {
            if (ModelState.IsValid)
            {
                GrantMemberType model = db.GrantMemberTypes.Find(grantMemberTypeView.GrantMemberTypeID);

                model.Name = grantMemberTypeView.Name;
                model.Description = grantMemberTypeView.Description;
                model.Coefficient = grantMemberTypeView.Coefficient;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grantMemberTypeView);
        }

        // GET: GrantMemberTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrantMemberType grantMemberType = db.GrantMemberTypes.Find(id);
            if (grantMemberType == null)
            {
                return HttpNotFound();
            }
            return View(grantMemberType);
        }

        // POST: GrantMemberTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            GrantMemberType grantMemberType = db.GrantMemberTypes.Find(id);
            db.GrantMemberTypes.Remove(grantMemberType);
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
