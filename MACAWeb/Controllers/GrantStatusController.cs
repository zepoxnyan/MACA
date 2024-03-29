﻿using System;
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
    public class GrantStatusController : Controller
    {
        private GrantStatusDbContext db = new GrantStatusDbContext();

        // GET: GrantStatus
        public ActionResult Index()
        {
            return View(db.GrantStatus.OrderBy(x => x.Name).ToList());
        }

        // GET: GrantStatus/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrantStatus grantStatus = db.GrantStatus.Find(id);
            if (grantStatus == null)
            {
                return HttpNotFound();
            }
            return View(grantStatus);
        }

        // GET: GrantStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GrantStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description")] GrantStatus grantStatus)
        {
            if (ModelState.IsValid)
            {
                grantStatus.GrantStatusID = Guid.NewGuid();

                grantStatus.DateCreated = DateTime.Now;
                grantStatus.DateModified = grantStatus.DateCreated;

                grantStatus.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                grantStatus.UserModifiedID = grantStatus.UserCreatedID;

                db.GrantStatus.Add(grantStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(grantStatus);
        }

        // GET: GrantStatus/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrantStatus grantStatus = db.GrantStatus.Find(id);
            if (grantStatus == null)
            {
                return HttpNotFound();
            }
            return View(grantStatus);
        }

        // POST: GrantStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GrantStatusID,Name,Description")] GrantStatusViewModel grantStatusViewModel)
        {
            if (ModelState.IsValid)
            {
                GrantStatus model = db.GrantStatus.Find(grantStatusViewModel.GrantStatusID);

                model.Name = grantStatusViewModel.Name;
                model.Description = grantStatusViewModel.Description;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grantStatusViewModel);
        }

        // GET: GrantStatus/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrantStatus grantStatus = db.GrantStatus.Find(id);
            if (grantStatus == null)
            {
                return HttpNotFound();
            }
            return View(grantStatus);
        }

        // POST: GrantStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            GrantStatus grantStatus = db.GrantStatus.Find(id);
            db.GrantStatus.Remove(grantStatus);
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
