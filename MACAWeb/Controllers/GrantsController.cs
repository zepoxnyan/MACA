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
        private GrantDbContext dbGrants = new GrantDbContext();
        private GrantBudgetDbContext dbGrantBudgets = new GrantBudgetDbContext();
        private GrantMemberDbContext dbGrantsMembers = new GrantMemberDbContext();

        // GET: Grants
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var grants = dbGrants.Grants.Include(x => x.GrantStatus).OrderByDescending(x => x.Start).ThenBy(x => x.Name);
            
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
            Grant grant = dbGrants.Grants.Find(id);
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
            var grantStatusQuery = from c in dbGrants.GrantStatuses
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

                dbGrants.Grants.Add(grant);
                dbGrants.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(grant);
        }

        // GET: Grants/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grant grant = dbGrants.Grants.Find(id);
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
                Grant model = dbGrants.Grants.Find(grantViewModel.GrantID);

                model.Name = grantViewModel.Name;
                model.GrantStatusID = grantViewModel.GrantStatusID;
                model.Description = grantViewModel.Description;
                model.Start = grantViewModel.Start;
                model.End = grantViewModel.End;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                dbGrants.Entry(model).State = EntityState.Modified;
                dbGrants.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grantViewModel);
        }

        // GET: Grants/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grant grant = dbGrants.Grants.Find(id);
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
            Grant grant = dbGrants.Grants.Find(id);
            dbGrants.Grants.Remove(grant);
            dbGrants.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbGrants.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Grant Budgets

        public ActionResult GrantBudgetsIndex(Guid grantId)
        {
            var grantBudgets = dbGrantBudgets.GrantBudgets.Where(x => x.GrantID == grantId).OrderByDescending(x => x.Year).ThenBy(x => x.GrantBudgetsType.Name);

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
            var grantBudgetTypesQuery = from c in dbGrantBudgets.GrantBudgetTypes
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

                dbGrantBudgets.GrantBudgets.Add(grantBudget);
                dbGrantBudgets.SaveChanges();
                return RedirectToAction("GrantBudgetsIndex", new { grantId = grantId });
            }

            return View(grantBudget);
        }

        public ActionResult GrantBudgetsEdit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrantBudget grantBudget = dbGrantBudgets.GrantBudgets.Find(id);
            if (grantBudget == null)
            {
                return HttpNotFound();
            }
            PopulateGrantBudgetTypesDropDownList(grantBudget.GrantBudgetsTypeID);
            return View(grantBudget);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GrantBudgetsEdit([Bind(Include = "GrantBudgetID,GrantBudgetsTypeID,Year,Amount,Description,GrantID")] GrantBudgetViewModel grantBudgetViewModel, string grantId)
        {
            if (ModelState.IsValid)
            {
                GrantBudget grantBudget = dbGrantBudgets.GrantBudgets.Find(grantBudgetViewModel.GrantBudgetID);

                grantBudget.GrantBudgetID = grantBudgetViewModel.GrantBudgetID;
                grantBudget.GrantBudgetsTypeID = grantBudgetViewModel.GrantBudgetsTypeID;                
                grantBudget.Year = grantBudgetViewModel.Year;
                grantBudget.Amount = grantBudgetViewModel.Amount;
                grantBudget.Description = grantBudgetViewModel.Description;

                grantBudget.DateModified = DateTime.Now;
                grantBudget.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                dbGrantBudgets.Entry(grantBudget).State = EntityState.Modified;
                dbGrantBudgets.SaveChanges();
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
            GrantBudget grantBudget = dbGrantBudgets.GrantBudgets.Find(id);
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
            GrantBudget grantBudget = dbGrantBudgets.GrantBudgets.Find(id);
            dbGrantBudgets.GrantBudgets.Remove(grantBudget);
            dbGrantBudgets.SaveChanges();
            return RedirectToAction("GrantBudgetsIndex", routeValues: new { grantId = grantBudget.GrantID });
        }

        #endregion


        #region Members

        public ActionResult GrantMembersIndex(Guid grantId)
        {
            var grantMembers = dbGrantsMembers.GrantMembers.Where(x => x.GrantID == grantId).OrderByDescending(x => x.Year).ThenBy(x => x.Person.Surname).ThenBy(x => x.Person.Name);

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
            var grantMemberTypesQuery = from c in dbGrantsMembers.GrantMemberTypes
                                        orderby c.Name
                                        select c;
            ViewBag.GrantMemberTypeID = new SelectList(grantMemberTypesQuery, "GrantMemberTypeID", "Name", selectedGrantMemberType);
        }

        private void PopulatePersonsDropDownList(object selectedPerson = null)
        {
            var personQuery = from c in dbGrantsMembers.Persons
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

                dbGrantsMembers.GrantMembers.Add(grantMember);
                dbGrantsMembers.SaveChanges();
                return RedirectToAction("GrantMembersIndex", new { grantId = grantId });
            }

            return View(grantMember);
        }

        public ActionResult GrantMembersEdit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrantMember grantMember = dbGrantsMembers.GrantMembers.Find(id);
            if (grantMember == null)
            {
                return HttpNotFound();
            }
            PopulateGrantMemberTypesDropDownList(grantMember.GrantMemberTypeID);
            PopulatePersonsDropDownList(grantMember.PersonID);
            return View(grantMember);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GrantMembersEdit([Bind(Include = "GrantMemberID,GrantMemberTypeID,PersonID,Hours,Year,Description,GrantID")] GrantMemberViewModel grantMemberViewModel, string grantId)
        {
            if (ModelState.IsValid)
            {
                GrantMember grantMember = dbGrantsMembers.GrantMembers.Find(grantMemberViewModel.GrantMemberID);

                grantMember.GrantMemberID = grantMemberViewModel.GrantMemberID;
                grantMember.GrantMemberTypeID = grantMemberViewModel.GrantMemberTypeID;
                grantMember.PersonID = grantMemberViewModel.PersonID;
                grantMember.Year = grantMemberViewModel.Year;
                grantMember.Hours = grantMemberViewModel.Hours;
                grantMember.Description = grantMemberViewModel.Description;

                grantMember.DateModified = DateTime.Now;
                grantMember.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                dbGrantsMembers.Entry(grantMember).State = EntityState.Modified;
                dbGrantsMembers.SaveChanges();
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
            GrantMember grantMember = dbGrantsMembers.GrantMembers.Find(id);
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
            GrantMember grantMember = dbGrantsMembers.GrantMembers.Find(id);
            dbGrantsMembers.GrantMembers.Remove(grantMember);
            dbGrantsMembers.SaveChanges();
            return RedirectToAction("GrantMembersIndex", routeValues: new { grantId = grantMember.GrantID });
        }

        #endregion
    }
}
