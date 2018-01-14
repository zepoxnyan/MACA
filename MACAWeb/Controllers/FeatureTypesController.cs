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
    public class FeatureTypesController : Controller
    {
        private FeatureTypeDbContext db = new FeatureTypeDbContext();

        // GET: FeatureTypes
        public ActionResult Index()
        {
            return View(db.FeatureTypes.ToList());
        }

        // GET: FeatureTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeatureType featureType = db.FeatureTypes.Find(id);
            if (featureType == null)
            {
                return HttpNotFound();
            }
            return View(featureType);
        }

        // GET: FeatureTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FeatureTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description")] FeatureType featureType)
        {
            if (ModelState.IsValid)
            {
                featureType.FeatureTypeId = Guid.NewGuid();

                featureType.DateCreated = DateTime.Now;
                featureType.DateModified = featureType.DateCreated;

                featureType.UserCreatedId = Guid.Parse(User.Identity.GetUserId());
                featureType.UserModifiedId = featureType.UserCreatedId;

                db.FeatureTypes.Add(featureType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(featureType);
        }

        // GET: FeatureTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeatureType featureType = db.FeatureTypes.Find(id);
            if (featureType == null)
            {
                return HttpNotFound();
            }
            return View(featureType);
        }

        // POST: FeatureTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FeatureTypeId,Name,Description")] FeatureTypeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                FeatureType model = db.FeatureTypes.Find(viewModel.FeatureTypeId);

                model.Name = viewModel.Name;
                model.Description = viewModel.Description;

                model.DateModified = DateTime.Now;
                model.UserModifiedId = Guid.Parse(User.Identity.GetUserId());

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: FeatureTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeatureType featureType = db.FeatureTypes.Find(id);
            if (featureType == null)
            {
                return HttpNotFound();
            }
            return View(featureType);
        }

        // POST: FeatureTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            FeatureType featureType = db.FeatureTypes.Find(id);
            db.FeatureTypes.Remove(featureType);
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
