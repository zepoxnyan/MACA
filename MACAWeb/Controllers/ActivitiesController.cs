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
    public class ActivitiesController : Controller
    {
        private ActivitiesDbContext dbActivities = new ActivitiesDbContext();

        // GET: Activities
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var activities = dbActivities.Activities.Include(x => x.ActivityType).OrderByDescending(x => x.ActivityType.Year).ThenBy(x => x.ActivityType.Name).ThenBy(x => x.Person.FullName);
            
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
                activities = activities.Where(m => m.ActivityType.Name.Contains(searchString)
                                      || m.ActivityType.Year.ToString().Contains(searchString)
                                      || m.Person.FullName.Contains(searchString))
                                      .OrderByDescending(x => x.ActivityType.Year).ThenBy(x => x.ActivityType.Name).ThenBy(x => x.Person.FullName);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["generalItemsOnPage"]);
            int pageNumber = (page ?? 1);

            return View(activities.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = dbActivities.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // GET: Grants/Create
        public ActionResult Create()
        {
            PopulateActivityTypesDropDownList();
            PopulatePersonsDropDownList();
            return View();
        }

        private void PopulateActivityTypesDropDownList(object selectedActivityTypes = null)
        {
            /*var activityTypesQuery = from c in dbActivities.ActivityTypes
                                   orderby c.Name
                                   select c;*/

            var activityTypesQuery = dbActivities.ActivityTypes.Select(x => new
                    {
                        ActivityTypeID = x.ActivityTypeID,
                        Name = x.Name + " (" + x.Year + ")"
                    }).OrderBy(x => x.Name);

            ViewBag.ActivityTypeID = new SelectList(activityTypesQuery, "ActivityTypeID", "Name", selectedActivityTypes);
        }

        private void PopulatePersonsDropDownList(object selectedPerson = null)
        {
            var personQuery = from c in dbActivities.Persons
                              orderby c.Surname, c.Name
                              select c;
            ViewBag.PersonID = new SelectList(personQuery, "PersonID", "Fullname", selectedPerson);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ActivityTypeID,PersonID,Weight,Remark")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                activity.ActivityID = Guid.NewGuid();

                activity.DateCreated = DateTime.Now;
                activity.DateModified = activity.DateCreated;

                activity.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                activity.UserModifiedID = activity.UserCreatedID;

                dbActivities.Activities.Add(activity);
                dbActivities.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(activity);
        }

        // GET: Grants/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = dbActivities.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            PopulateActivityTypesDropDownList(activity.ActivityTypeID);
            PopulatePersonsDropDownList(activity.PersonID);
            return View(activity);
        }

        // POST: Grants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActivityID,ActivityTypeID,PersonID,Weight,Remark")] ActivityViewModel activityViewModel)
        {
            if (ModelState.IsValid)
            {
                Activity model = dbActivities.Activities.Find(activityViewModel.ActivityID);

                model.PersonID = activityViewModel.PersonID;
                model.ActivityTypeID = activityViewModel.ActivityTypeID;
                model.Weight = activityViewModel.Weight;
                model.Remark = activityViewModel.Remark;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                dbActivities.Entry(model).State = EntityState.Modified;
                dbActivities.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(activityViewModel);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = dbActivities.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Activity activity = dbActivities.Activities.Find(id);
            dbActivities.Activities.Remove(activity);
            dbActivities.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbActivities.Dispose();
            }
            base.Dispose(disposing);
        }                
    }
}
