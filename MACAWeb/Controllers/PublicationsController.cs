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
using System.IO;

namespace MACAWeb.Controllers
{
    [Authorize(Roles = "Admin, Employee, SuperAdmin")]
    public class PublicationsController : Controller
    {
        private MACADbContext db = new MACADbContext();

        // GET: Publications
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var publications = db.Publications
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

            if (User.IsInRole("Employee") && !(User.IsInRole("Admin") || User.IsInRole("SuperAdmin")))
            {
                Person person = db.Persons.Find(Auxiliaries.GetConnectedPerson(User.Identity.GetUserId()));
                var personID = Guid.Parse(User.Identity.GetUserId());
                publications = db.PublicationAuthors.Where(ttp => ttp.AuthorID == person.AuthorID).Select(tp => tp.Publication).OrderByDescending(p => p.Year).ThenBy(p => p.Title);
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
            Publication publication = db.Publications.Find(id);
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
                new SelectList(db.PublicationClassifications.OrderBy(x => x.Name), "PublicationClassificationID", "Name", selectedPublicationClassification);
            ViewBag.PublicationStatusID =
                new SelectList(db.PublicationStatus.OrderBy(x => x.Name), "PublicationStatusID", "Name", selectedPublicationStatus);
            ViewBag.PublicationTypeID =
                new SelectList(db.PublicationTypes.OrderBy(x => x.Name), "PublicationTypeID", "Name", selectedPublicationType);
            ViewBag.PublicationTypeLocalID =
                new SelectList(db.PublicationTypesLocal.OrderBy(x => x.Name), "PublicationTypeLocalID", "Name", selectedPublicationTypeLocal);
        }

        // POST: Publications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PublicationTypeID,PublicationTypeLocalID,PublicationClassificationID,PublicationStatusID,Title,TitleEN,Journal,Year,Volume,Issue,Pages,DOI,Link,Note,Editors,Publisher,Series,Address,Edition,BookTitle,Organization,Chapter,Keywords,KeywordsEN,Abstract,BibtexFile")] PublicationViewModel publicationView)
        {
            if (ModelState.IsValid)
            {
                Publication publication = new Publication();
                publication.PublicationID = Guid.NewGuid();

                publication.PublicationTypeID = publicationView.PublicationTypeID;
                publication.PublicationTypeLocalID = publicationView.PublicationTypeLocalID;
                publication.PublicationClassificationID = publicationView.PublicationClassificationID;
                publication.PublicationStatusID = publicationView.PublicationStatusID;
                publication.Title = publicationView.Title;
                publication.TitleEN = publicationView.TitleEN;
                publication.Journal = publicationView.Journal;
                publication.Year = publicationView.Year;
                publication.Volume = publicationView.Volume;
                publication.Issue = publicationView.Issue;
                publication.Pages = publicationView.Pages;
                publication.DOI = publicationView.DOI;
                publication.Link = publicationView.Link;
                publication.Note = publicationView.Note;
                publication.Editors = publicationView.Editors;
                publication.Publisher = publicationView.Publisher;
                publication.Series = publicationView.Series;
                publication.Address = publicationView.Address;
                publication.Edition = publicationView.Edition;
                publication.BookTitle = publicationView.BookTitle;
                publication.Organization = publicationView.Organization;
                publication.Chapter = publicationView.Chapter;
                publication.Keywords = publicationView.Keywords;
                publication.KeywordsEN = publicationView.KeywordsEN;
                publication.Abstract = publicationView.Abstract;



                publication.DateCreated = DateTime.Now;
                publication.DateModified = publication.DateCreated;

                publication.UserCreatedID = new Guid(User.Identity.GetUserId());
                publication.UserModifiedID = publication.UserCreatedID;

                var test = publicationView.BibtexFile;
                Console.WriteLine(test);

                if (publicationView.BibtexFile != null)
                {


                    using (var reader = new BinaryReader(publicationView.BibtexFile.InputStream))
                    {
                        string data = Convert.ToBase64String(reader.ReadBytes(publicationView.BibtexFile.ContentLength));
                        
                        publication.BibtexFile = data;
                        
                    }

                }

                db.Publications.Add(publication);
                db.SaveChanges();

                return RedirectToAction("Index");

            }

            PopulateDropDownLists();
            return View(publicationView);
        }

        // GET: Publications/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publication publication = db.Publications.Find(id);
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
        public ActionResult Edit([Bind(Include = "PublicationID,PublicationTypeID,PublicationTypeLocalID,PublicationClassificationID,PublicationStatusID,Title,TitleEN,Journal,Year,Volume,Issue,Pages,DOI,Link,Note,Editors,Publisher,Series,Address,Edition,BookTitle,Organization,Chapter,Keywords,KeywordsEN,Abstract,BibtexFile")] PublicationViewModel publicationViewModel)
        {
            if (ModelState.IsValid)
            {
                Publication publication = db.Publications.Find(publicationViewModel.PublicationID);

                publication.PublicationClassificationID = publicationViewModel.PublicationClassificationID;
                publication.PublicationStatusID = publicationViewModel.PublicationStatusID;
                publication.PublicationTypeID = publicationViewModel.PublicationTypeID;
                publication.PublicationTypeLocalID = publicationViewModel.PublicationTypeLocalID;

                publication.Title = publicationViewModel.Title;
                publication.TitleEN = publicationViewModel.TitleEN;
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
                publication.KeywordsEN = publicationViewModel.KeywordsEN;
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

                if (publicationViewModel.BibtexFile != null)
                {


                    using (var reader = new BinaryReader(publicationViewModel.BibtexFile.InputStream))
                    {
                        string data = Convert.ToBase64String(reader.ReadBytes(publicationViewModel.BibtexFile.ContentLength));

                        publication.BibtexFile = data;

                    }

                }

                db.Entry(publication).State = EntityState.Modified;
                db.SaveChanges();
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
            Publication publication = db.Publications.Find(id);
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
            Publication publication = db.Publications.Find(id);
            db.Publications.Remove(publication);
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


        #region Publication Authors

        public ActionResult PubAuthorsIndex(Guid publicationId)
        {
            var pubAuthors = db.PublicationAuthors.Where(x => x.PublicationID == publicationId).OrderBy(x => x.Author.Surname).ThenBy(x => x.Author.FirstName);

            ViewBag.PublicationID = publicationId;
            return View(pubAuthors);
        }

        // GET: Grants/Create
        public ActionResult PubAuthorsCreate(Guid publicationId)
        {
            PopulateAuthorsDropDownList();
            ViewBag.PublicationID = publicationId;

            return View();
        }

        private void PopulateAuthorsDropDownList(object selectedAuthor = null, object selectedAuthorType = null)
        {
            var authorsQuery = from c in db.Authors
                               orderby c.Surname, c.FirstName
                               select new { c.AuthorID, Name = c.Surname + " " + c.FirstName };
            ViewBag.AuthorID = new SelectList(authorsQuery, "AuthorID", "Name", selectedAuthor);

            var authorTypesQuery = from c in db.AuthorTypes
                                   orderby c.Name
                                   select c;
            ViewBag.AuthorTypeID = new SelectList(authorTypesQuery, "AuthorTypeID", "Name", selectedAuthorType);
        }

        // POST: Grants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PubAuthorsCreate([Bind(Include = "AuthorID,AuthorTypeID,Percent,PublicationID")] PublicationAuthor model, string publicationId)
        {
            if (ModelState.IsValid)
            {
                model.PublicationAuthorID = Guid.NewGuid();
                model.PublicationID = new Guid(publicationId);

                model.DateCreated = DateTime.Now;
                model.DateModified = model.DateCreated;

                model.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                model.UserModifiedID = model.UserCreatedID;

                db.PublicationAuthors.Add(model);
                db.SaveChanges();
                return RedirectToAction("PubAuthorsIndex", new { publicationId = publicationId });
            }

            PopulateAuthorsDropDownList();
            return View(model);
        }

        public ActionResult PubAuthorsEdit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationAuthor pubAuthor = db.PublicationAuthors.Find(id);
            if (pubAuthor == null)
            {
                return HttpNotFound();
            }
            PopulateAuthorsDropDownList(pubAuthor.AuthorID, pubAuthor.AuthorTypeID);
            return View(pubAuthor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PubAuthorsEdit(
            [Bind(Include = "PublicationAuthorID,AuthorID,AuthorTypeID,Percent,PublicationID")] PublicationAuthorVievModel pubAuthorViewModel, string publicationId)
        {
            if (ModelState.IsValid)
            {
                PublicationAuthor pubAuthor = db.PublicationAuthors.Find(pubAuthorViewModel.PublicationAuthorID);

                pubAuthor.AuthorID = pubAuthorViewModel.AuthorID;
                pubAuthor.AuthorTypeID = pubAuthorViewModel.AuthorTypeID;
                pubAuthor.Percent = pubAuthorViewModel.Percent;

                pubAuthor.DateModified = DateTime.Now;
                pubAuthor.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                db.Entry(pubAuthor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PubAuthorsIndex", new { publicationId = publicationId });
            }
            return View(pubAuthorViewModel);
        }

        public ActionResult PubAuthorsDelete(Guid? id, Guid publicationId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicationAuthor pubAuthor = db.PublicationAuthors.Find(id);
            if (pubAuthor == null)
            {
                return HttpNotFound();
            }
            ViewBag.PublicationID = publicationId;

            return View(pubAuthor);
        }

        [HttpPost, ActionName("PubAuthorsDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult PubAuthorsDeleteConfirmed(Guid id)
        {
            PublicationAuthor pubAuthor = db.PublicationAuthors.Find(id);
            db.PublicationAuthors.Remove(pubAuthor);
            db.SaveChanges();
            return RedirectToAction("PubAuthorsIndex", routeValues: new { publicationId = pubAuthor.PublicationID });
        }

        #endregion


        #region Authors

        public ActionResult AuthorsIndex(string currentFilter, string searchString, int? page)
        {
            var authors = db.Authors.OrderBy(p => p.Surname).ThenBy(x => x.FirstName);

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
                authors = authors.Where(m => m.Surname.Contains(searchString)
                                      || m.FirstName.Contains(searchString))
                                      .OrderBy(p => p.Surname).ThenBy(x => x.FirstName);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["generalItemsOnPage"]);
            int pageNumber = (page ?? 1);

            return View(authors.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AuthorsDetails(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // GET: Grants/Create
        public ActionResult AuthorsCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuthorsCreate([Bind(Include = "Surname,FirstName,CREPCCode,ORCID")] Author model)
        {
            if (ModelState.IsValid)
            {
                model.AuthorID = Guid.NewGuid();

                model.DateCreated = DateTime.Now;
                model.DateModified = model.DateCreated;

                model.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                model.UserModifiedID = model.UserCreatedID;

                db.Authors.Add(model);
                db.SaveChanges();
                return RedirectToAction("AuthorsIndex");
            }
            return View(model);
        }

        public ActionResult AuthorsEdit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuthorsEdit(
            [Bind(Include = "AuthorID,Surname,FirstName,CREPCCode,ORCID")] AuthorVievModel authorViewModel)
        {
            if (ModelState.IsValid)
            {
                Author author = db.Authors.Find(authorViewModel.AuthorID);

                author.Surname = authorViewModel.Surname;
                author.FirstName = authorViewModel.FirstName;
                author.CREPCCode = authorViewModel.CREPCCode;
                author.ORCID = authorViewModel.ORCID;

                author.DateModified = DateTime.Now;
                author.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AuthorsIndex");
            }
            return View(authorViewModel);
        }

        public ActionResult AuthorsDelete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }

            return View(author);
        }

        [HttpPost, ActionName("AuthorsDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult AuthorsDeleteConfirmed(Guid id)
        {
            Author author = db.Authors.Find(id);
            db.Authors.Remove(author);
            db.SaveChanges();
            return RedirectToAction("AuthorsIndex");
        }

        #endregion
    }
}
