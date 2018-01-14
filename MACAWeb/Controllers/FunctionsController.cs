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
    public class FunctionsController : Controller
    {
        private FunctionDbContext db = new FunctionDbContext();
        /*private List<FunctionIDDModel> IDDModelList
        {
            get { return Session["index"] as List<FunctionIDDModel>; }
            set { Session["index"] = value; }
        }*/

        // GET: Functions
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            /*IDDModelList = new List<FunctionIDDModel>();
            db.Functions.ToList().ForEach(
                f =>
                {
                    FunctionIDDModel indexModel = new FunctionIDDModel()
                    {
                        FunctionId = f.FunctionId,
                        Name = f.Name,
                        InputDescription = f.InputDescription,
                        OutputDescription = f.OutputDescription,
                        Description = f.Description,
                        FeatureType = (from t in db.Types
                                       where t.FeatureTypeId == f.FeatureTypeId
                                       select t.Name).First()
                    };
                    IDDModelList.Add(indexModel);
                });

            return View(IDDModelList);*/

            var functions = db.Functions.Include(x => x.FeatureType).OrderBy(x => x.Name);

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
                functions = functions.Where(m => m.Name.Contains(searchString)
                                      || m.InputDescription.Contains(searchString)
                                      || m.OutputDescription.Contains(searchString)
                                      || m.Description.Contains(searchString))
                                      .OrderBy(m => m.Name);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["generalItemsOnPage"]);
            int pageNumber = (page ?? 1);

            return View(functions.ToPagedList(pageNumber, pageSize));
        }

        // GET: Functions/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Function function = db.Functions.Find(id);
            if (function == null)
            {
                return HttpNotFound();
            }
            return View(function /*IDDModelList.Where(f => f.FunctionId == function.FunctionId).First()*/);
        }

        // GET: Functions/Create
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

        // POST: Functions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FeatureTypeId,Name,InputDescription,OutputDescription,Description")] Function function)
        {
            if (ModelState.IsValid)
            {
                function.FunctionId = Guid.NewGuid();

                function.DateCreated = DateTime.Now;
                function.DateModified = function.DateCreated;

                function.UserCreatedId = Guid.Parse(User.Identity.GetUserId());
                function.UserModifiedId = function.UserCreatedId;

                db.Functions.Add(function);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateFeaturesDropDownList(function.FeatureTypeId);
            return View(function);
        }

        // GET: Functions/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Function function = db.Functions.Find(id);
            if (function == null)
            {
                return HttpNotFound();
            }

            /*FunctionViewModel viewModel = new FunctionViewModel()
            {
                FunctionId = model.FunctionId,
                FeatureTypeId = model.FeatureTypeId,
                Name = model.Name,
                InputDescription = model.InputDescription,
                OutputDescription = model.OutputDescription,
                Description = model.Description
            };*/

            PopulateFeaturesDropDownList(function.FeatureTypeId);
            return View(function);
        }

        // POST: Functions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FeatureTypeId,FunctionId,Name,InputDescription,OutputDescription,Description")] FunctionViewModel functionViewModel)
        {
            if (ModelState.IsValid)
            {
                Function function = db.Functions.Find(functionViewModel.FunctionId);

                function.FeatureTypeId = functionViewModel.FeatureTypeId;
                function.Name = functionViewModel.Name;
                function.InputDescription = functionViewModel.InputDescription;
                function.OutputDescription = functionViewModel.OutputDescription;
                function.Description = functionViewModel.Description;

                function.DateModified = DateTime.Now;
                function.UserModifiedId = Guid.Parse(User.Identity.GetUserId());

                db.Entry(function).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateFeaturesDropDownList(functionViewModel.FeatureTypeId);
            return View(functionViewModel);
        }

        // GET: Functions/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Function function = db.Functions.Find(id);
            if (function == null)
            {
                return HttpNotFound();
            }
            return View(function /*IDDModelList.Where(f => f.FunctionId == function.FunctionId).First()*/);
        }

        // POST: Functions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Function function = db.Functions.Find(id);
            db.Functions.Remove(function);
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