using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MACAWeb.Models;
using System.Configuration;
using PagedList;
using Microsoft.AspNet.Identity;

namespace MACAWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GrantsController : Controller
    {
        private MACADbContext db = new MACADbContext();
    
        // GET: Grants
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var grants = db.Grants
                .Include(x => x.GrantStatus)
                .OrderByDescending(x => x.Start).ThenBy(x => x.Name);
            
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
                grants = grants.Where(m => m.Name.Contains(searchString)
                                      || m.Description.Contains(searchString)
                                      || m.GrantStatus.Name.Contains(searchString)
                                      || m.Start.ToString().Contains(searchString)
                                      || m.End.ToString().Contains(searchString))
                                      .OrderBy(m => m.Name);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["generalItemsOnPage"]);
            int pageNumber = (page ?? 1);

            return View(grants.ToPagedList(pageNumber, pageSize));
        }

        // GET: Grants/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grant grant = db.Grants.Find(id);
            if (grant == null)
            {
                return HttpNotFound();
            }
            return View(grant);
        }

        // GET: Grants/Create
        public ActionResult Create()
        {
            PopulateGrantStatusDropDownList();
            return View();
        }

        private void PopulateGrantStatusDropDownList(object selectedGrantStatus = null)
        {
            var grantStatusQuery = from c in db.GrantStatuses
                                   orderby c.Name
                                   select c;
            ViewBag.GrantStatusID = new SelectList(grantStatusQuery, "GrantStatusID", "Name", selectedGrantStatus);
        }

        // POST: Grants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GrantStatusID,Name,Description,Start,End")] Grant grant)
        {
            if (ModelState.IsValid)
            {
                grant.GrantID = Guid.NewGuid();

                grant.DateCreated = DateTime.Now;
                grant.DateModified = grant.DateCreated;

                grant.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                grant.UserModifiedID = grant.UserCreatedID;

                db.Grants.Add(grant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateGrantStatusDropDownList();
            return View(grant);
        }

        // GET: Grants/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grant grant = db.Grants.Find(id);
            if (grant == null)
            {
                return HttpNotFound();
            }
            PopulateGrantStatusDropDownList(grant.GrantStatusID);
            return View(grant);
        }

        // POST: Grants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GrantID,GrantStatusID,Name,Description,Start,End")] GrantViewModel grantViewModel)
        {
            if (ModelState.IsValid)
            {
                Grant model = db.Grants.Find(grantViewModel.GrantID);

                model.Name = grantViewModel.Name;
                model.GrantStatusID = grantViewModel.GrantStatusID;
                model.Description = grantViewModel.Description;
                model.Start = grantViewModel.Start;
                model.End = grantViewModel.End;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateGrantStatusDropDownList(grantViewModel.GrantStatusID);
            return View(grantViewModel);
        }

        // GET: Grants/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grant grant = db.Grants.Find(id);
            if (grant == null)
            {
                return HttpNotFound();
            }
            return View(grant);
        }

        // POST: Grants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Grant grant = db.Grants.Find(id);
            db.Grants.Remove(grant);
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

        #region Grant Budgets

        public ActionResult GrantBudgetsIndex(Guid grantId)
        {
            var grantBudgets = db.GrantBudgets.Where(x => x.GrantID == grantId).OrderByDescending(x => x.Year).ThenBy(x => x.GrantBudgetsType.Name);

            ViewBag.GrantID = grantId;
            return View(grantBudgets);
        }

        public ActionResult GrantBudgetsCreate(Guid grantId)
        {
            PopulateGrantBudgetTypesDropDownList();
            ViewBag.GrantID = grantId;

            return View();
        }

        private void PopulateGrantBudgetTypesDropDownList(object selectedGrantBudgetType = null)
        {
            var grantBudgetTypesQuery = from c in db.GrantBudgetTypes
                                     orderby c.Name
                                     select c;
            ViewBag.GrantBudgetsTypeID = new SelectList(grantBudgetTypesQuery, "GrantBudgetsTypeID", "Name", selectedGrantBudgetType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GrantBudgetsCreate([Bind(Include = "GrantBudgetsTypeID,Amount,Year,Description,GrantID")] GrantBudget grantBudget, string grantId)
        {
            if (ModelState.IsValid)
            {
                grantBudget.GrantBudgetID = Guid.NewGuid();

                grantBudget.DateCreated = DateTime.Now;
                grantBudget.DateModified = grantBudget.DateCreated;

                grantBudget.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                grantBudget.UserModifiedID = grantBudget.UserCreatedID;

                db.GrantBudgets.Add(grantBudget);
                db.SaveChanges();
                return RedirectToAction("GrantBudgetsIndex", new { grantId = grantId });
            }
            PopulateGrantBudgetTypesDropDownList();
            return View(grantBudget);
        }

        public ActionResult GrantBudgetsEdit(Guid? id, Guid grantId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrantBudget grantBudget = db.GrantBudgets.Find(id);
            if (grantBudget == null)
            {
                return HttpNotFound();
            }
            PopulateGrantBudgetTypesDropDownList(grantBudget.GrantBudgetsTypeID);
            ViewBag.GrantID = grantId;
            return View(grantBudget);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GrantBudgetsEdit([Bind(Include = "GrantBudgetID,GrantBudgetsTypeID,Year,Amount,Description,GrantID")] GrantBudgetViewModel grantBudgetViewModel, string grantId)
        {
            if (ModelState.IsValid)
            {
                GrantBudget grantBudget = db.GrantBudgets.Find(grantBudgetViewModel.GrantBudgetID);

                grantBudget.GrantBudgetID = grantBudgetViewModel.GrantBudgetID;
                grantBudget.GrantBudgetsTypeID = grantBudgetViewModel.GrantBudgetsTypeID;                
                grantBudget.Year = grantBudgetViewModel.Year;
                grantBudget.Amount = grantBudgetViewModel.Amount;
                grantBudget.Description = grantBudgetViewModel.Description;

                grantBudget.DateModified = DateTime.Now;
                grantBudget.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                db.Entry(grantBudget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GrantBudgetsIndex", new { grantId = grantId });
            }
            return View(grantBudgetViewModel);
        }

        public ActionResult GrantBudgetsDelete(Guid? id, Guid grantId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrantBudget grantBudget = db.GrantBudgets.Find(id);
            if (grantBudget == null)
            {
                return HttpNotFound();
            }
            ViewBag.GrantID = grantId;

            return View(grantBudget);
        }

        [HttpPost, ActionName("GrantBudgetsDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult GrantBudgetsDeleteConfirmed(Guid id)
        {
            GrantBudget grantBudget = db.GrantBudgets.Find(id);
            db.GrantBudgets.Remove(grantBudget);
            db.SaveChanges();
            return RedirectToAction("GrantBudgetsIndex", routeValues: new { grantId = grantBudget.GrantID });
        }

        #endregion


        #region Members

        public ActionResult GrantMembersIndex(Guid grantId)
        {
            var grantMembers = db.GrantMembers.Where(x => x.GrantID == grantId).OrderByDescending(x => x.Year).ThenBy(x => x.Person.Surname).ThenBy(x => x.Person.Name);

            ViewBag.GrantID = grantId;
            return View(grantMembers);
        }

        public ActionResult GrantMembersCreate(Guid grantId)
        {
            PopulateGrantMemberTypesDropDownList();
            PopulatePersonsDropDownList();
            ViewBag.GrantID = grantId;

            return View();
        }

        private void PopulateGrantMemberTypesDropDownList(object selectedGrantMemberType = null)
        {
            var grantMemberTypesQuery = from c in db.GrantMemberTypes
                                        orderby c.Name
                                        select c;
            ViewBag.GrantMemberTypeID = new SelectList(grantMemberTypesQuery, "GrantMemberTypeID", "Name", selectedGrantMemberType);
        }

        private void PopulatePersonsDropDownList(object selectedPerson = null)
        {
            var personQuery = from c in db.Persons
                                        orderby c.Surname, c.Name
                                        select c;
            ViewBag.PersonID = new SelectList(personQuery, "PersonID", "Fullname", selectedPerson);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GrantMembersCreate([Bind(Include = "GrantMemberTypeID,PersonID,Hours,Year,Description,GrantID")] GrantMember grantMember, string grantId)
        {
            if (ModelState.IsValid)
            {
                grantMember.GrantMemberID = Guid.NewGuid();

                grantMember.DateCreated = DateTime.Now;
                grantMember.DateModified = grantMember.DateCreated;

                grantMember.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                grantMember.UserModifiedID = grantMember.UserCreatedID;

                db.GrantMembers.Add(grantMember);
                db.SaveChanges();
                return RedirectToAction("GrantMembersIndex", new { grantId = grantId });
            }

            return View(grantMember);
        }

        public ActionResult GrantMembersEdit(Guid? id, Guid grantId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrantMember grantMember = db.GrantMembers.Find(id);
            if (grantMember == null)
            {
                return HttpNotFound();
            }
            PopulateGrantMemberTypesDropDownList(grantMember.GrantMemberTypeID);
            PopulatePersonsDropDownList(grantMember.PersonID);
            ViewBag.GrantID = grantId;
            return View(grantMember);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GrantMembersEdit([Bind(Include = "GrantMemberID,GrantMemberTypeID,PersonID,Hours,Year,Description,GrantID")] GrantMemberViewModel grantMemberViewModel, string grantId)
        {
            if (ModelState.IsValid)
            {
                GrantMember grantMember = db.GrantMembers.Find(grantMemberViewModel.GrantMemberID);

                grantMember.GrantMemberTypeID = grantMemberViewModel.GrantMemberTypeID;
                grantMember.PersonID = grantMemberViewModel.PersonID;
                grantMember.Year = grantMemberViewModel.Year;
                grantMember.Hours = grantMemberViewModel.Hours;
                grantMember.Description = grantMemberViewModel.Description;

                grantMember.DateModified = DateTime.Now;
                grantMember.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                db.Entry(grantMember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GrantMembersIndex", new { grantId = grantId });
            }
            return View(grantMemberViewModel);
        }

        public ActionResult GrantMembersDelete(Guid? id, Guid grantId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrantMember grantMember = db.GrantMembers.Find(id);
            if (grantMember == null)
            {
                return HttpNotFound();
            }
            ViewBag.GrantID = grantId;

            return View(grantMember);
        }

        [HttpPost, ActionName("GrantMembersDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult GrantMembersDeleteConfirmed(Guid id)
        {
            GrantMember grantMember = db.GrantMembers.Find(id);
            db.GrantMembers.Remove(grantMember);
            db.SaveChanges();
            return RedirectToAction("GrantMembersIndex", routeValues: new { grantId = grantMember.GrantID });
        }

        #endregion
    }
}
