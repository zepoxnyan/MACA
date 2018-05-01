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
    public class PublicationsController : Controller
    {
        private PublicationsDbContext dbPublications = new PublicationsDbContext();
        private PublicationTypeDbContext dbPublicationTypes = new PublicationTypeDbContext();
        private PublicationTypeLocalDbContext dbPublicationTypesLocal = new PublicationTypeLocalDbContext();
        private PublicationClassificationDbContext dbPublicationClassifications = new PublicationClassificationDbContext();
        private PublicationStatusDbContext dbPublicationStatus = new PublicationStatusDbContext();

        // GET: Publications
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var publications = dbPublications.Publications
                .Include(p => p.PublicationClassification)
                .Include(p => p.PublicationStatus)
                .Include(p => p.PublicationType)
                .Include(p => p.PublicationTypeLocal)
                .OrderByDescending(p => p.Year)
                .ThenBy(p => p.Title);

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
                publications = publications.Where(m => m.Year.ToString().Contains(searchString)
                                      || m.Title.Contains(searchString)
                                      || m.PublicationStatus.Name.Contains(searchString))
                                      .OrderByDescending(p => p.Year)
                                      .ThenBy(p => p.Title);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["generalItemsOnPage"]);
            int pageNumber = (page ?? 1);

            return View(publications.ToPagedList(pageNumber, pageSize));
        }

        // GET: Publications/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publication publication = dbPublications.Publications.Find(id);
            if (publication == null)
            {
                return HttpNotFound();
            }
            return View(publication);
        }

        // GET: Publications/Create
        public ActionResult Create()
        {
            PopulateDropDownLists();
            return View();
        }

        private void PopulateDropDownLists(
            object selectedPublicationClassification = null,
            object selectedPublicationStatus = null,
            object selectedPublicationType = null,
            object selectedPublicationTypeLocal = null)
        {
            ViewBag.PublicationClassificationID = 
                new SelectList(dbPublications.PublicationClassifications.OrderBy(x => x.Name), "PublicationClassificationID", "Name", selectedPublicationClassification);
            ViewBag.PublicationStatusID = 
                new SelectList(dbPublications.PublicationStatus.OrderBy(x => x.Name), "PublicationStatusID", "Name", selectedPublicationStatus);
            ViewBag.PublicationTypeID = 
                new SelectList(dbPublications.PublicationTypes.OrderBy(x => x.Name), "PublicationTypeID", "Name", selectedPublicationType);
            ViewBag.PublicationTypeLocalID = 
                new SelectList(dbPublications.PublicationTypesLocal.OrderBy(x => x.Name), "PublicationTypeLocalID", "Name", selectedPublicationTypeLocal);
        }

        // POST: Publications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PublicationTypeID,PublicationTypeLocalID,PublicationClassificationID,PublicationStatusID,Title,Journal,Year,Volume,Issue,Pages,DOI,Link,Note,Editors,Publisher,Series,Address,Edition,BookTitle,Organization,Chapter,Keywords,Abstract")] Publication publication)
        {
            if (ModelState.IsValid)
            {
                publication.PublicationID = Guid.NewGuid();

                publication.DateCreated = DateTime.Now;
                publication.DateModified = publication.DateCreated;

                publication.UserCreatedID = new Guid(User.Identity.GetUserId());
                publication.UserModifiedID = publication.UserCreatedID;

                dbPublications.Publications.Add(publication);
                dbPublications.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateDropDownLists();
            return View(publication);
        }

        // GET: Publications/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publication publication = dbPublications.Publications.Find(id);
            if (publication == null)
            {
                return HttpNotFound();
            }
            PopulateDropDownLists(publication.PublicationClassificationID, publication.PublicationStatusID,
                publication.PublicationTypeID, publication.PublicationTypeLocalID);
            return View(publication);
        }

        // POST: Publications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PublicationID,PublicationTypeID,PublicationTypeLocalID,PublicationClassificationID,PublicationStatusID,Title,Journal,Year,Volume,Issue,Pages,DOI,Link,Note,Editors,Publisher,Series,Address,Edition,BookTitle,Organization,Chapter,Keywords,Abstract")] PublicationViewModel publicationViewModel)
        {
            if (ModelState.IsValid)
            {
                Publication publication = dbPublications.Publications.Find(publicationViewModel.PublicationID);

                publication.PublicationClassificationID = publicationViewModel.PublicationClassificationID;
                publication.PublicationStatusID = publicationViewModel.PublicationStatusID;
                publication.PublicationTypeID = publicationViewModel.PublicationTypeID;
                publication.PublicationTypeLocalID = publicationViewModel.PublicationTypeLocalID;

                publication.Title = publicationViewModel.Title;
                publication.Abstract = publicationViewModel.Abstract;
                publication.Address = publicationViewModel.Address;
                publication.BookTitle = publicationViewModel.BookTitle;
                publication.Chapter = publicationViewModel.Chapter;
                publication.DOI = publicationViewModel.DOI;
                publication.Edition = publicationViewModel.Edition;
                publication.Editors = publicationViewModel.Editors;
                publication.Issue = publicationViewModel.Issue;
                publication.Journal = publicationViewModel.Journal;
                publication.Keywords = publicationViewModel.Keywords;
                publication.Link = publicationViewModel.Link;
                publication.Note = publicationViewModel.Note;
                publication.Organization = publicationViewModel.Organization;
                publication.Pages = publicationViewModel.Pages;
                publication.PreprintLink = publicationViewModel.PreprintLink;
                publication.Publisher = publicationViewModel.Publisher;
                publication.Series = publicationViewModel.Series;
                publication.Volume = publicationViewModel.Volume;
                publication.Year = publicationViewModel.Year;

                publication.DateModified = DateTime.Now;
                publication.UserModifiedID = new Guid(User.Identity.GetUserId());

                dbPublications.Entry(publication).State = EntityState.Modified;
                dbPublications.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateDropDownLists(publicationViewModel.PublicationClassificationID, publicationViewModel.PublicationStatusID,
                publicationViewModel.PublicationTypeID, publicationViewModel.PublicationTypeLocalID);
            return View(publicationViewModel);
        }

        // GET: Publications/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publication publication = dbPublications.Publications.Find(id);
            if (publication == null)
            {
                return HttpNotFound();
            }
            return View(publication);
        }

        // POST: Publications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Publication publication = dbPublications.Publications.Find(id);
            dbPublications.Publications.Remove(publication);
            dbPublications.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbPublications.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
