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
using System.Configuration;
using PagedList;

namespace MACAWeb.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DataFormatsController : Controller
    {
        private DataFormatDbContext db = new DataFormatDbContext();

        // GET: DataFormats
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var dataFormats = db.DataFormats.Include(x => x.FeatureType).OrderBy(x => x.Name);

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
                dataFormats = dataFormats.Where(m => m.Name.Contains(searchString)
                                      || m.Description.Contains(searchString))
                                      .OrderBy(m => m.Name);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["generalItemsOnPage"]);
            int pageNumber = (page ?? 1);

            return View(dataFormats.ToPagedList(pageNumber, pageSize));
        }

        // GET: DataFormats/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataFormat dataFormat = db.DataFormats.Find(id);
            if (dataFormat == null)
            {
                return HttpNotFound();
            }
            return View(dataFormat);
        }

        // GET: DataFormats/Create
        public ActionResult Create()
        {
            PopulateFeaturesDropDownList();
            return View();
        }

        private void PopulateFeaturesDropDownList(object selectedFeatureType = null)
        {
            var featureTypeQuery = from c in db.FeatureTypes
                                  orderby c.Name
                                  select c;
            ViewBag.FeatureTypeID = new SelectList(featureTypeQuery, "FeatureTypeID", "Name", selectedFeatureType);
        }


        // POST: DataFormats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description,FeatureTypeId")] DataFormat dataFormat)
        {
            if (ModelState.IsValid)
            {
                dataFormat.DataFormatId = Guid.NewGuid();

                dataFormat.DateCreated = DateTime.Now;
                dataFormat.DateModified = dataFormat.DateCreated;
                dataFormat.UserCreatedId = Auxiliaries.GetUserId(User);
                dataFormat.UserModifiedId = dataFormat.UserCreatedId;

                db.DataFormats.Add(dataFormat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateFeaturesDropDownList(dataFormat.FeatureTypeId);
            return View(dataFormat);
        }

        // GET: DataFormats/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataFormat dataFormat = db.DataFormats.Find(id);
            if (dataFormat == null)
            {
                return HttpNotFound();
            }

            PopulateFeaturesDropDownList(dataFormat.FeatureTypeId);
            return View(dataFormat);
        }

        // POST: DataFormats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DataFormatId,Name,Description,FeatureTypeId")] DataFormatViewModel dataFormatViewModel)
        {
            if (ModelState.IsValid)
            {
                DataFormat dataFormat = db.DataFormats.Find(dataFormatViewModel.DataFormatId);

                dataFormat.Name = dataFormatViewModel.Name;
                dataFormat.Description = dataFormatViewModel.Description;
                dataFormat.FeatureTypeId = dataFormatViewModel.FeatureTypeId;

                dataFormat.DateModified = DateTime.Now;
                dataFormat.UserModifiedId = Auxiliaries.GetUserId(User);

                db.Entry(dataFormat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateFeaturesDropDownList(dataFormatViewModel.FeatureTypeId);
            return View(dataFormatViewModel);
        }

        // GET: DataFormats/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataFormat dataFormat = db.DataFormats.Find(id);
            if (dataFormat == null)
            {
                return HttpNotFound();
            }
            return View(dataFormat);
        }

        // POST: DataFormats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            DataFormat dataFormat = db.DataFormats.Find(id);
            db.DataFormats.Remove(dataFormat);
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
