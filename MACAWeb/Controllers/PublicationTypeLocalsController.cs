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
    public class PublicationTypeLocalsController : Controller
    {
        private MACADbContext db = new MACADbContext();

        // GET: PublicationTypeLocals
        public ActionResult Index()
        {
            var publicationTypesLocal = db.PublicationTypesLocal.Include(p => p.PublicationTypeGroup).OrderBy(x => x.Name);
            return View(publicationTypesLocal.ToList());
        }

        // GET: PublicationTypeLocals/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationTypeLocal publicationTypeLocal = db.PublicationTypesLocal.Find(id);
            if (publicationTypeLocal == null)
            {
                return HttpNotFound();
            }
            return View(publicationTypeLocal);
        }

        private void PopulatePublicationTypeGroupDropDownList(object selectedPublicationTypeGroup = null)
        {
            var publicationTypeGroupQuery = from c in db.PublicationTypeGroups
                                   orderby c.Name
                                   select c;
            ViewBag.PublicationTypeGroupID = new SelectList(publicationTypeGroupQuery, "PublicationTypeGroupID", "Name", selectedPublicationTypeGroup);
        }

        // GET: PublicationTypeLocals/Create
        public ActionResult Create()
        {
            PopulatePublicationTypeGroupDropDownList();
            return View();
        }

        // POST: PublicationTypeLocals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PublicationTypeGroupID,Name,Description")] PublicationTypeLocal publicationTypeLocal)
        {
            if (ModelState.IsValid)
            {
                publicationTypeLocal.PublicationTypeLocalID = Guid.NewGuid();

                publicationTypeLocal.DateCreated = DateTime.Now;
                publicationTypeLocal.DateModified = publicationTypeLocal.DateCreated;

                publicationTypeLocal.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                publicationTypeLocal.UserModifiedID = publicationTypeLocal.UserCreatedID;

                db.PublicationTypesLocal.Add(publicationTypeLocal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulatePublicationTypeGroupDropDownList();
            return View(publicationTypeLocal);
        }

        // GET: PublicationTypeLocals/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationTypeLocal publicationTypeLocal = db.PublicationTypesLocal.Find(id);
            if (publicationTypeLocal == null)
            {
                return HttpNotFound();
            }

            PopulatePublicationTypeGroupDropDownList(publicationTypeLocal.PublicationTypeGroupID);
            return View(publicationTypeLocal);
        }

        // POST: PublicationTypeLocals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PublicationTypeLocalID,PublicationTypeGroupID,Name,Description")] PublicationTypeLocalViewModel publicationTypeLocalViewModel)
        {
            if (ModelState.IsValid)
            {
                PublicationTypeLocal publicationTypeLocal = db.PublicationTypesLocal.Find(publicationTypeLocalViewModel.PublicationTypeLocalID);

                publicationTypeLocal.Name = publicationTypeLocalViewModel.Name;
                publicationTypeLocal.PublicationTypeGroupID = publicationTypeLocalViewModel.PublicationTypeGroupID;
                publicationTypeLocal.Description = publicationTypeLocalViewModel.Description;

                publicationTypeLocal.DateModified = DateTime.Now;
                publicationTypeLocal.UserModifiedID = Guid.Parse(User.Identity.GetUserId());


                db.Entry(publicationTypeLocal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulatePublicationTypeGroupDropDownList(publicationTypeLocalViewModel.PublicationTypeGroupID);
            return View(publicationTypeLocalViewModel);
        }

        // GET: PublicationTypeLocals/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationTypeLocal publicationTypeLocal = db.PublicationTypesLocal.Find(id);
            if (publicationTypeLocal == null)
            {
                return HttpNotFound();
            }
            return View(publicationTypeLocal);
        }

        // POST: PublicationTypeLocals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PublicationTypeLocal publicationTypeLocal = db.PublicationTypesLocal.Find(id);
            db.PublicationTypesLocal.Remove(publicationTypeLocal);
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

        #region Type Coefficients

        public ActionResult CoefficientsIndex(Guid pubTypeLocalId)
        {
            var coefficients = db.PublicationTypesLocalCoef.Where(x => x.PublicationTypeLocalID == pubTypeLocalId).OrderByDescending(x => x.Year).ThenBy(x => x.PublicationTypeLocal.Name);

            ViewBag.PublicationTypeLocalID = pubTypeLocalId;
            return View(coefficients);
        }

        public ActionResult CoefficientsCreate(Guid pubTypeLocalId)
        {
            ViewBag.PublicationTypeLocalID = pubTypeLocalId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CoefficientsCreate([Bind(Include = "Year,Coefficient,Description")] PublicationTypeLocalCoef model, string pubTypeLocalId)
        {
            if (ModelState.IsValid)
            {
                model.PublicationTypeLocalCoefID = Guid.NewGuid();
                model.PublicationTypeLocalID = new Guid(pubTypeLocalId);

                model.DateCreated = DateTime.Now;
                model.DateModified = model.DateCreated;

                model.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                model.UserModifiedID = model.UserCreatedID;

                db.PublicationTypesLocalCoef.Add(model);
                db.SaveChanges();
                return RedirectToAction("CoefficientsIndex", new { pubTypeLocalId = pubTypeLocalId });
            }

            return View(model);
        }

        public ActionResult CoefficientsEdit(Guid? id, Guid pubTypeLocalId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationTypeLocalCoef model = db.PublicationTypesLocalCoef.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.PublicationTypeLocalID = pubTypeLocalId;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CoefficientsEdit([Bind(Include = "PublicationTypeLocalCoefID,Year,Coefficient,Description")] PublicationTypeLocalCoefViewModel viewModel, string pubTypeLocalId)
        {
            if (ModelState.IsValid)
            {
                PublicationTypeLocalCoef model = db.PublicationTypesLocalCoef.Find(viewModel.PublicationTypeLocalCoefID);

                model.Year = viewModel.Year;
                model.Coefficient = viewModel.Coefficient;
                model.Description = viewModel.Description;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CoefficientsIndex", new { pubTypeLocalId = pubTypeLocalId });
            }
            return View(viewModel);
        }

        public ActionResult CoefficientsDelete(Guid? id, Guid pubTypeLocalId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationTypeLocalCoef model = db.PublicationTypesLocalCoef.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.PublicationTypeLocalID = pubTypeLocalId;

            return View(model);
        }

        [HttpPost, ActionName("CoefficientsDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult CoefficientsDeleteConfirmed(Guid id)
        {
            PublicationTypeLocalCoef model = db.PublicationTypesLocalCoef.Find(id);
            db.PublicationTypesLocalCoef.Remove(model);
            db.SaveChanges();
            return RedirectToAction("CoefficientsIndex", routeValues: new { pubTypeLocalId = model.PublicationTypeLocalID });
        }

        #endregion
    }
}
