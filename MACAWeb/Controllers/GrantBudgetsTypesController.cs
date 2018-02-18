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
    public class GrantBudgetsTypesController : Controller
    {
        private GrantBudgetsTypeDbContext db = new GrantBudgetsTypeDbContext();

        // GET: GrantBudgetsTypes
        public ActionResult Index()
        {
            return View(db.GrantBudgetsTypes.OrderBy(x => x.Name).ToList());
        }

        // GET: GrantBudgetsTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrantBudgetsType grantBudgetsType = db.GrantBudgetsTypes.Find(id);
            if (grantBudgetsType == null)
            {
                return HttpNotFound();
            }
            return View(grantBudgetsType);
        }

        // GET: GrantBudgetsTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GrantBudgetsTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description")] GrantBudgetsType grantBudgetsType)
        {
            if (ModelState.IsValid)
            {
                grantBudgetsType.GrantBudgetsTypeID = Guid.NewGuid();

                grantBudgetsType.DateCreated = DateTime.Now;
                grantBudgetsType.DateModified = grantBudgetsType.DateCreated;

                grantBudgetsType.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                grantBudgetsType.UserModifiedID = grantBudgetsType.UserCreatedID;

                db.GrantBudgetsTypes.Add(grantBudgetsType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(grantBudgetsType);
        }

        // GET: GrantBudgetsTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrantBudgetsType grantBudgetsType = db.GrantBudgetsTypes.Find(id);
            if (grantBudgetsType == null)
            {
                return HttpNotFound();
            }
            return View(grantBudgetsType);
        }

        // POST: GrantBudgetsTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GrantBudgetsTypeID,Name,Description")] GrantBudgetsTypeViewModel grantBudgetsTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                GrantBudgetsType model = db.GrantBudgetsTypes.Find(grantBudgetsTypeViewModel.GrantBudgetsTypeID);

                model.Name = grantBudgetsTypeViewModel.Name;
                model.Description = grantBudgetsTypeViewModel.Description;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grantBudgetsTypeViewModel);
        }

        // GET: GrantBudgetsTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrantBudgetsType grantBudgetsType = db.GrantBudgetsTypes.Find(id);
            if (grantBudgetsType == null)
            {
                return HttpNotFound();
            }
            return View(grantBudgetsType);
        }

        // POST: GrantBudgetsTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            GrantBudgetsType grantBudgetsType = db.GrantBudgetsTypes.Find(id);
            db.GrantBudgetsTypes.Remove(grantBudgetsType);
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
