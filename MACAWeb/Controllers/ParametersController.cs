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
    [Authorize(Roles = "Administrator")]
    public class ParametersController : Controller
    {
        private ParameterDbContext db = new ParameterDbContext();

        // GET: Parameters
        public ActionResult Index()
        {
            return View(db.Parameters.ToList());
        }

        // GET: Parameters/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parameter parameter = db.Parameters.Find(id);
            if (parameter == null)
            {
                return HttpNotFound();
            }
            return View(parameter);
        }

        // GET: Parameters/Create
        public ActionResult Create()
        {
            PopulateDropDownList();
            return View();
        }

        // POST: Parameters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FunctionId,Name,Description")] Parameter model)
        {
            if (ModelState.IsValid)
            {
                model.ParameterId = Guid.NewGuid();

                model.DateCreated = DateTime.Now;
                model.DateModified = model.DateCreated;

                model.UserCreatedId = Guid.Parse(User.Identity.GetUserId());
                model.UserModifiedId = model.UserCreatedId;

                db.Parameters.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateDropDownList(model.FunctionId);
            return View(model);
        }

        // GET: Parameters/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parameter parameter = db.Parameters.Find(id);
            if (parameter == null)
            {
                return HttpNotFound();
            }
            PopulateDropDownList(parameter.FunctionId);
            return View(parameter);
        }

        // POST: Parameters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ParameterId,FunctionId,Name,Description")] ParameterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Parameter model = db.Parameters.Find(viewModel.ParameterId);

                model.FunctionId = viewModel.FunctionId;
                model.Name = viewModel.Name;
                model.Description = viewModel.Description;

                model.DateModified = DateTime.Now;
                model.UserModifiedId = Guid.Parse(User.Identity.GetUserId());

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateDropDownList(viewModel.FunctionId);
            return View(viewModel);
        }
            
        private void PopulateDropDownList(object selectedFunction = null)
        {
            var functionsQuery = from c in db.Functions
                                 orderby c.Name
                                 select c;

            ViewBag.FunctionId = new SelectList(functionsQuery, "FunctionId", "Name", selectedFunction);
        }

        // GET: Parameters/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parameter parameter = db.Parameters.Find(id);
            if (parameter == null)
            {
                return HttpNotFound();
            }
            return View(parameter);
        }

        // POST: Parameters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Parameter parameter = db.Parameters.Find(id);
            db.Parameters.Remove(parameter);
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
