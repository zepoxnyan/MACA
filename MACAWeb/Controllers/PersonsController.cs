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
    [Authorize(Roles = "Admin")]
    public class PersonsController : Controller
    {
        private PersonsDbContext dbPersons = new PersonsDbContext();
        private PositionDbContext dbPositions = new PositionDbContext();
        private AuthorsDbContext dbAuthors = new AuthorsDbContext();

        // GET: Persons
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var persons = dbPersons.Persons.OrderBy(x => x.Surname).ThenBy(x => x.Name);
            
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
                persons = persons.Where(m => m.Surname.Contains(searchString)
                                      || m.Name.Contains(searchString)
                                      || m.FullName.Contains(searchString)
                                      || m.Description.Contains(searchString))
                                      .OrderBy(x => x.Surname).ThenBy(x => x.Name);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["generalItemsOnPage"]);
            int pageNumber = (page ?? 1);

            return View(persons.ToPagedList(pageNumber, pageSize));
        }

        // GET: Persons/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = dbPersons.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: Persons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Persons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Surname,Name,FullName,Description,Image")] PersonViewModel personView)
        {
            if (ModelState.IsValid)
            {
                Guid authorGuid = Guid.NewGuid();

                Person person = new Person();
                person.PersonID = Guid.NewGuid();
                person.Surname = personView.Surname;
                person.Name = personView.Name;
                person.FullName = personView.FullName;
                person.Description = personView.Description;

                person.DateCreated = DateTime.Now;
                person.DateModified = DateTime.Now;
                person.UserCreatedID = User.Identity.GetUserId();
                person.UserModifiedID = User.Identity.GetUserId();

                // Handle the image
                if (personView.Image != null && personView.Image.ContentLength > 0)
                {
                    if (!Auxiliaries.ValidImageTypes.Contains(personView.Image.ContentType))
                    {
                        ModelState.AddModelError("Image", "Choose an image in one of the following formats: GIF, JPG, or PNG.");
                    }
                    else
                    {
                        using (var reader = new BinaryReader(personView.Image.InputStream))
                        {
                            person.Image = reader.ReadBytes(personView.Image.ContentLength);
                        }
                    }
                }

                dbPersons.Persons.Add(person);
                dbPersons.SaveChanges();

                // Automatically add a person to authors
                Author author = new Author();
                author.AuthorID = authorGuid;
                author.Surname = person.Surname;
                author.FirstName = person.Name;

                author.DateCreated = DateTime.Now;
                author.DateModified = DateTime.Now;
                author.UserCreatedID = new Guid(User.Identity.GetUserId());
                author.UserModifiedID = author.UserCreatedID;

                dbAuthors.Authors.Add(author);
                dbAuthors.SaveChanges();

                person = dbPersons.Persons.Find(person.PersonID);
                person.AuthorID = authorGuid;
                dbPersons.Entry(person).State = EntityState.Modified;
                dbPersons.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(personView);
        }

        // GET: Persons/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = dbPersons.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }

            PersonViewModel personView = new PersonViewModel();
            personView.PersonID = person.PersonID;
            personView.Surname = person.Surname;
            personView.Name = person.Name;
            personView.FullName = person.FullName;
            personView.Description = person.Description;

            if (person.Image != null && person.Image.Length > 0)
            {
                personView.Image = (HttpPostedFileBase)new MemoryPostedFile(person.Image);

                var base64 = Convert.ToBase64String(person.Image);
                var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                ViewBag.ImgSrc = imgSrc;
            }

            return View(personView);
        }

        // POST: Persons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonID,Surname,Name,FullName,Description,Image")] PersonViewModel personView)
        {
            if (ModelState.IsValid)
            {
                Person person = dbPersons.Persons.Find(personView.PersonID);
                person.Surname = personView.Surname;
                person.Name = personView.Name;
                person.FullName = personView.FullName;
                person.Description = personView.Description;

                person.DateModified = DateTime.Now;
                person.UserModifiedID = User.Identity.GetUserId();

                // Handle the image
                if (personView.Image != null && personView.Image.ContentLength > 0)
                {
                    if (!Auxiliaries.ValidImageTypes.Contains(personView.Image.ContentType))
                    {
                        ModelState.AddModelError("Image", "Choose an image in one of the following formats: GIF, JPG, or PNG.");
                    }
                    else
                    {
                        using (var reader = new BinaryReader(personView.Image.InputStream))
                        {
                            person.Image = reader.ReadBytes(personView.Image.ContentLength);
                        }
                    }
                }

                dbPersons.Entry(person).State = EntityState.Modified;
                dbPersons.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(personView);
        }

        // GET: Persons/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = dbPersons.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: Persons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Person person = dbPersons.Persons.Find(id);
            dbPersons.Persons.Remove(person);
            dbPersons.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbPersons.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Positions

        private bool IsUserAuthorized(String id)
        {
            return User.IsInRole("Admin"); //|| User.Identity.GetUserId().Contains(id);
        }

        public ActionResult PositionsIndex(Guid personId)
        {
            var positions = dbPositions.Positions.Where(x => x.PersonID == personId).OrderByDescending(x => x.Year).ThenBy(x => x.Semester);

            Person person = dbPersons.Persons.Where(p => p.PersonID == personId).Single();
            /*if (!IsUserAuthorized(company.UserCreatedID))
            {
                ViewBag.errorMessage = "Nimate dovoljenja za ogled podrobnosti ponudb tega podjetja.";
                return View("Error");
            }*/

            ViewBag.PersonID = personId;
            return View(positions);
        }

        // GET: Grants/Create
        public ActionResult PositionsCreate(Guid personId)
        {
            PopulatePositionTypesDropDownList();
            ViewBag.PersonID = personId;

            return View();
        }

        private void PopulatePositionTypesDropDownList(object selectedPositionType = null)
        {
            var positionTypesQuery = from c in dbPositions.PositionTypes
                                   orderby c.Name
                                   select c;
            ViewBag.PositionTypeID = new SelectList(positionTypesQuery, "PositionTypeID", "Name", selectedPositionType);
        }

        // POST: Grants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PositionsCreate([Bind(Include = "PositionTypeID,Description,Year,Semester,PersonID")] Position position, string personId)
        {
            if (ModelState.IsValid)
            {
                position.PositionID = Guid.NewGuid();

                position.DateCreated = DateTime.Now;
                position.DateModified = position.DateCreated;

                position.UserCreatedID = Guid.Parse(User.Identity.GetUserId());
                position.UserModifiedID = position.UserCreatedID;

                dbPositions.Positions.Add(position);
                dbPositions.SaveChanges();
                return RedirectToAction("PositionsIndex", new { personId = personId});
            }

            return View(position);
        }

        public ActionResult PositionsEdit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = dbPositions.Positions.Find(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            PopulatePositionTypesDropDownList(position.PositionTypeID);
            return View(position);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PositionsEdit([Bind(Include = "PositionID,PositionTypeID,Year,Semester,Description,PersonID")] PositionViewModel positionViewModel, string personId)
        {
            if (ModelState.IsValid)
            {
                Position position = dbPositions.Positions.Find(positionViewModel.PositionID);

                position.PositionID = positionViewModel.PositionID;
                position.PositionTypeID = positionViewModel.PositionTypeID;
                position.Description = positionViewModel.Description;
                position.Year = positionViewModel.Year;
                position.Semester = positionViewModel.Semester;

                position.DateModified = DateTime.Now;
                position.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                dbPositions.Entry(position).State = EntityState.Modified;
                dbPositions.SaveChanges();
                return RedirectToAction("PositionsIndex", new { personId = personId });
            }
            return View(positionViewModel);
        }

        public ActionResult PositionsDelete(Guid? id, Guid personId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = dbPositions.Positions.Find(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonID = personId;

            return View(position);
        }

        [HttpPost, ActionName("PositionsDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult PositionsDeleteConfirmed(Guid id)
        {
            Position position = dbPositions.Positions.Find(id);
            dbPositions.Positions.Remove(position);
            dbPositions.SaveChanges();
            return RedirectToAction("PositionsIndex", routeValues: new { personId = position.PersonID });
        }

        #endregion
    }
}
