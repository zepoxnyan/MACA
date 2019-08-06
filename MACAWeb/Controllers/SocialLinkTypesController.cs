using MACAWeb.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MACAWeb.Controllers
{
    public class SocialLinkTypesController : Controller
    {
        private MACADbContext dbSocialLink = new MACADbContext();

        // GET: SocialLinkType
        public ActionResult Index()
        {
            return View(dbSocialLink.SocialLinkTypes.OrderBy(x => x.Name).ToList());
        }

        // GET: SocialLinkType/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SocialLinkType socialLinkType = dbSocialLink.SocialLinkTypes.Find(id);
            if (socialLinkType == null)
            {
                return HttpNotFound();
            }
            return View(socialLinkType);
        }

        // GET: SocialLinkType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SocialLinkType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,UrlShortcut,Description")] SocialLinkType socialLinkType)
        {
            if (ModelState.IsValid)
            {
                socialLinkType.SocialLinkTypeID = Guid.NewGuid();

                socialLinkType.DateCreated = DateTime.Now;
                socialLinkType.DateModified = socialLinkType.DateCreated;

                socialLinkType.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                socialLinkType.UserModifiedID = socialLinkType.UserCreatedID;
                dbSocialLink.SocialLinkTypes.Add(socialLinkType);
                dbSocialLink.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(socialLinkType);
        }

        // GET: SocialLinkType/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SocialLinkType socialLinkType = dbSocialLink.SocialLinkTypes.Find(id);
            if (socialLinkType == null)
            {
                return HttpNotFound();
            }
            return View(socialLinkType);
        }

        // POST: SocialLinkType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SocialLinkTypeID,Name,UrlShortcut,Description")] SocialLinkTypeViewModel socialLinkTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                SocialLinkType model = dbSocialLink.SocialLinkTypes.Find(socialLinkTypeViewModel.SocialLinkTypeID);

                model.Name = socialLinkTypeViewModel.Name;
                model.UrlShortcut = socialLinkTypeViewModel.UrlShortcut;
                model.Description = socialLinkTypeViewModel.Description;

                model.DateModified = DateTime.Now;
                model.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                dbSocialLink.Entry(model).State = EntityState.Modified;
                dbSocialLink.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(socialLinkTypeViewModel);
        }

        // GET: SocialLinkType/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SocialLinkType socialLinkType = dbSocialLink.SocialLinkTypes.Find(id);
            if (socialLinkType == null)
            {
                return HttpNotFound();
            }
            return View(socialLinkType);
        }

        // POST: SocialLinkType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            SocialLinkType socialLinkType = dbSocialLink.SocialLinkTypes.Find(id);
            dbSocialLink.SocialLinkTypes.Remove(socialLinkType);
            dbSocialLink.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbSocialLink.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}