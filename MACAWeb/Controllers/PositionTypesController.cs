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
    public class PositionTypesController : Controller
    {
        private MACADbContext db = new MACADbContext();

        // GET: PositionTypes
        public ActionResult Index()
        {
            return View(db.PositionTypes.OrderBy(x => x.Name).ToList());
        }

        // GET: PositionTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PositionType positionType = db.PositionTypes.Find(id);
            if (positionType == null)
            {
                return HttpNotFound();
            }
            return View(positionType);
        }

        // GET: PositionTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PositionTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description")] PositionType positionType)
        {
            if (ModelState.IsValid)
            {
                positionType.PositionTypeID = Guid.NewGuid();

                positionType.DateCreated = DateTime.Now;
                positionType.DateModified = positionType.DateCreated;

                positionType.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                positionType.UserModifiedID = positionType.UserCreatedID;

                db.PositionTypes.Add(positionType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(positionType);
        }

        // GET: PositionTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PositionType positionType = db.PositionTypes.Find(id);
            if (positionType == null)
            {
                return HttpNotFound();
            }
            return View(positionType);
        }

        // POST: PositionTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PositionTypeID,Name,Description")] PositionTypeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                PositionType model = db.PositionTypes.Find(viewModel.PositionTypeID);

                model.Name = viewModel.Name;
                model.Description = viewModel.Description;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: PositionTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PositionType positionType = db.PositionTypes.Find(id);
            if (positionType == null)
            {
                return HttpNotFound();
            }
            return View(positionType);
        }

        // POST: PositionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PositionType positionType = db.PositionTypes.Find(id);
            db.PositionTypes.Remove(positionType);
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
