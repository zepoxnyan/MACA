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
    public class PublicationTypeLocalsController : Controller
    {
        private PublicationTypeLocalDbContext dbPubTypesLocal = new PublicationTypeLocalDbContext();

        // GET: PublicationTypeLocals
        public ActionResult Index()
        {
            var publicationTypesLocal = dbPubTypesLocal.PublicationTypesLocal.Include(p => p.PublicationTypeGroup).OrderBy(x => x.Name);
            return View(publicationTypesLocal.ToList());
        }

        // GET: PublicationTypeLocals/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationTypeLocal publicationTypeLocal = dbPubTypesLocal.PublicationTypesLocal.Find(id);
            if (publicationTypeLocal == null)
            {
                return HttpNotFound();
            }
            return View(publicationTypeLocal);
        }

        private void PopulatePublicationTypeGroupDropDownList(object selectedPublicationTypeGroup = null)
        {
            var publicationTypeGroupQuery = from c in dbPubTypesLocal.PublicationTypeGroups
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

                dbPubTypesLocal.PublicationTypesLocal.Add(publicationTypeLocal);
                dbPubTypesLocal.SaveChanges();
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
            PublicationTypeLocal publicationTypeLocal = dbPubTypesLocal.PublicationTypesLocal.Find(id);
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
                PublicationTypeLocal publicationTypeLocal = dbPubTypesLocal.PublicationTypesLocal.Find(publicationTypeLocalViewModel.PublicationTypeLocalID);

                publicationTypeLocal.Name = publicationTypeLocalViewModel.Name;
                publicationTypeLocal.PublicationTypeGroupID = publicationTypeLocalViewModel.PublicationTypeGroupID;
                publicationTypeLocal.Description = publicationTypeLocalViewModel.Description;

                publicationTypeLocal.DateModified = DateTime.Now;
                publicationTypeLocal.UserModifiedID = Guid.Parse(User.Identity.GetUserId());


                dbPubTypesLocal.Entry(publicationTypeLocal).State = EntityState.Modified;
                dbPubTypesLocal.SaveChanges();
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
            PublicationTypeLocal publicationTypeLocal = dbPubTypesLocal.PublicationTypesLocal.Find(id);
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
            PublicationTypeLocal publicationTypeLocal = dbPubTypesLocal.PublicationTypesLocal.Find(id);
            dbPubTypesLocal.PublicationTypesLocal.Remove(publicationTypeLocal);
            dbPubTypesLocal.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbPubTypesLocal.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
