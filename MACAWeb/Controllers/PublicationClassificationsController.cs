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
    public class PublicationClassificationsController : Controller
    {
        private PublicationClassificationDbContext dbPubClass = new PublicationClassificationDbContext();
        private PublicationClassificationCoefficientDbContext dbCoeffs = new PublicationClassificationCoefficientDbContext();

        // GET: PublicationClassifications
        public ActionResult Index()
        {
            return View(dbPubClass.PublicationClassification.OrderBy(x => x.Name).ToList());
        }

        // GET: PublicationClassifications/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationClassification publicationClassification = dbPubClass.PublicationClassification.Find(id);
            if (publicationClassification == null)
            {
                return HttpNotFound();
            }
            return View(publicationClassification);
        }

        // GET: PublicationClassifications/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PublicationClassifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description")] PublicationClassification publicationClassification)
        {
            if (ModelState.IsValid)
            {
                publicationClassification.PublicationClassificationID = Guid.NewGuid();

                publicationClassification.DateCreated = DateTime.Now;
                publicationClassification.DateModified = publicationClassification.DateCreated;

                publicationClassification.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                publicationClassification.UserModifiedID = publicationClassification.UserCreatedID;

                dbPubClass.PublicationClassification.Add(publicationClassification);
                dbPubClass.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(publicationClassification);
        }

        // GET: PublicationClassifications/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationClassification publicationClassification = dbPubClass.PublicationClassification.Find(id);
            if (publicationClassification == null)
            {
                return HttpNotFound();
            }
            return View(publicationClassification);
        }

        // POST: PublicationClassifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PublicationClassificationID,Name,Description")] PublicationClassificationViewModel publicationClassificationViewModel)
        {
            if (ModelState.IsValid)
            {
                PublicationClassification model = dbPubClass.PublicationClassification.Find(publicationClassificationViewModel.PublicationClassificationID);

                model.Name = publicationClassificationViewModel.Name;
                model.Description = publicationClassificationViewModel.Description;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                dbPubClass.Entry(model).State = EntityState.Modified;
                dbPubClass.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(publicationClassificationViewModel);
        }

        // GET: PublicationClassifications/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationClassification publicationClassification = dbPubClass.PublicationClassification.Find(id);
            if (publicationClassification == null)
            {
                return HttpNotFound();
            }
            return View(publicationClassification);
        }

        // POST: PublicationClassifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PublicationClassification publicationClassification = dbPubClass.PublicationClassification.Find(id);
            dbPubClass.PublicationClassification.Remove(publicationClassification);
            dbPubClass.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbPubClass.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Coefficients

        public ActionResult CoefficientsIndex(Guid pubClassId)
        {
            var coeffs = dbCoeffs.PublicationClassificationCoefficients
                .Where(x => x.PublicationClassificationID == pubClassId)
                .OrderByDescending(x => x.Year).ThenBy(x => x.PublicationTypeGroup.Name);

            ViewBag.PublicationClassificationID = pubClassId;
            return View(coeffs);
        }

        public ActionResult CoefficientsCreate(Guid pubClassId)
        {
            PopulatePubTypeGroupsDropDownList();
            ViewBag.PublicationClassificationID = pubClassId;

            return View();
        }

        private void PopulatePubTypeGroupsDropDownList(object selectedPubTypeGroup = null)
        {
            var pubTypeGroupsQuery = from c in dbCoeffs.PublicationTypeGroups
                                        orderby c.Name
                                        select c;
            ViewBag.PublicationTypeGroupID = new SelectList(pubTypeGroupsQuery, "PublicationTypeGroupID", "Name", selectedPubTypeGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CoefficientsCreate([Bind(Include = "PublicationTypeGroupID,Coefficient,Year,Description,PublicationClassificationID")] PublicationClassificationCoefficient model, string pubClassId)
        {
            if (ModelState.IsValid)
            {
                model.PublicationClassificationCoefficientID = Guid.NewGuid();
                model.PublicationClassificationID = new Guid(pubClassId);

                model.DateCreated = DateTime.Now;
                model.DateModified = model.DateCreated;

                model.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                model.UserModifiedID = model.UserCreatedID;

                dbCoeffs.PublicationClassificationCoefficients.Add(model);
                dbCoeffs.SaveChanges();
                return RedirectToAction("CoefficientsIndex", new { pubClassId = pubClassId });
            }

            return View(model);
        }

        public ActionResult CoefficientsEdit(Guid? id, Guid pubClassId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationClassificationCoefficient model = dbCoeffs.PublicationClassificationCoefficients.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            PopulatePubTypeGroupsDropDownList(model.PublicationTypeGroupID);
            ViewBag.PublicationClassificationID = pubClassId;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CoefficientsEdit(
            [Bind(Include = "PublicationClassificationCoefficientID,PublicationTypeGroupID,Year,Coefficient,Description,PublicationClassificationID")] PublicationClassificationCoefficientViewModel viewModel, string pubClassId)
        {
            if (ModelState.IsValid)
            {
                PublicationClassificationCoefficient model = dbCoeffs.PublicationClassificationCoefficients.Find(viewModel.PublicationClassificationCoefficientID);

                model.PublicationTypeGroupID = viewModel.PublicationTypeGroupID;
                model.Year = viewModel.Year;
                model.Coefficient = viewModel.Coefficient;
                model.Description = viewModel.Description;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                dbCoeffs.Entry(model).State = EntityState.Modified;
                dbCoeffs.SaveChanges();
                return RedirectToAction("CoefficientsIndex", new { pubClassId = pubClassId });
            }
            return View(viewModel);
        }

        public ActionResult CoefficientsDelete(Guid? id, Guid pubClassId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationClassificationCoefficient model = dbCoeffs.PublicationClassificationCoefficients.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.PublicationClassificationID = pubClassId;

            return View(model);
        }

        [HttpPost, ActionName("CoefficientsDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult CoefficientsDeleteConfirmed(Guid id)
        {
            PublicationClassificationCoefficient model = dbCoeffs.PublicationClassificationCoefficients.Find(id);
            dbCoeffs.PublicationClassificationCoefficients.Remove(model);
            dbCoeffs.SaveChanges();
            return RedirectToAction("CoefficientsIndex", routeValues: new { pubClassId = model.PublicationClassificationID });
        }

        #endregion
    }
}
