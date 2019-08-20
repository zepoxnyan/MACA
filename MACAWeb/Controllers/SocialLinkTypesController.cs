using MACAWeb.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MACAWeb.Controllers
{
    public class SocialLinkTypesController : Controller
    {
        private MACADbContext db = new MACADbContext();

        // GET: SocialLinkType
        public ActionResult Index()
        {
            return View(db.SocialLinkTypes.OrderBy(x => x.Name).ToList());
        }

        // GET: SocialLinkType/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SocialLinkType socialLinkType = db.SocialLinkTypes.Find(id);
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
        public ActionResult Create([Bind(Include = "Name,UrlShortcut,Description,Logo")] SocialLinkTypeViewModel sltView)
        {
            if (ModelState.IsValid)
            {
                SocialLinkType slt = new SocialLinkType();
                slt.SocialLinkTypeID = Guid.NewGuid();
                slt.Name = sltView.Name;
                slt.UrlShortcut = sltView.UrlShortcut;
                slt.Description = sltView.Description;


                slt.DateCreated = DateTime.Now;
                slt.DateModified = slt.DateCreated;

                slt.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                slt.UserModifiedID = slt.UserCreatedID;

                // Handle the image
                if (sltView.Logo != null && sltView.Logo.ContentLength > 0)
                {
                    if (!Auxiliaries.ValidImageTypes.Contains(sltView.Logo.ContentType))
                    {
                        ModelState.AddModelError("Image", "Choose an image in one of the following formats: GIF, JPG, or PNG.");
                    }
                    else
                    {
                        using (var reader = new BinaryReader(sltView.Logo.InputStream))
                        {
                            var img = reader.ReadBytes(sltView.Logo.ContentLength);
                            slt.Logo = Auxiliaries.CreateThumbnail(img, 80);
                        }
                    }
                }
                
                db.SocialLinkTypes.Add(slt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sltView);
        }

        // GET: SocialLinkType/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SocialLinkType slt = db.SocialLinkTypes.Find(id);
            if (slt == null)
            {
                return HttpNotFound();
            }
            SocialLinkTypeViewModel sltView = new SocialLinkTypeViewModel();
            sltView.SocialLinkTypeID = slt.SocialLinkTypeID;
            sltView.Name = slt.Name;
            sltView.UrlShortcut = slt.UrlShortcut;
            sltView.Description = slt.Description;
            

            if (slt.Logo != null && slt.Logo.Length > 0)
            {
                sltView.Logo = (HttpPostedFileBase)new MemoryPostedFile(slt.Logo);

                var base64 = Convert.ToBase64String(slt.Logo);
                var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                ViewBag.ImgSrc = imgSrc;
            }
            return View(sltView);
        }

        // POST: SocialLinkType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SocialLinkTypeID,Name,UrlShortcut,Description,Logo")] SocialLinkTypeViewModel sltView)
        {
            if (ModelState.IsValid)
            {
                SocialLinkType slt = db.SocialLinkTypes.Find(sltView.SocialLinkTypeID);

                slt.Name = sltView.Name;
                slt.UrlShortcut = sltView.UrlShortcut;
                slt.Description = sltView.Description;

                slt.DateModified = DateTime.Now;
                slt.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                // Handle the image
                if (sltView.Logo != null && sltView.Logo.ContentLength > 0)
                {
                    if (!Auxiliaries.ValidImageTypes.Contains(sltView.Logo.ContentType))
                    {
                        ModelState.AddModelError("Image", "Choose an image in one of the following formats: GIF, JPG, or PNG.");
                    }
                    else
                    {
                        using (var reader = new BinaryReader(sltView.Logo.InputStream))
                        {
                            slt.Logo = Auxiliaries.CreateThumbnail(reader.ReadBytes(sltView.Logo.ContentLength), 80);
                        }
                    }
                }
                db.Entry(slt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sltView);
        }

        // GET: SocialLinkType/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SocialLinkType socialLinkType = db.SocialLinkTypes.Find(id);
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
            SocialLinkType socialLinkType = db.SocialLinkTypes.Find(id);
            db.SocialLinkTypes.Remove(socialLinkType);
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