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
        private PersonsDbContext db = new PersonsDbContext();

        // GET: Persons
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var persons = db.Persons.OrderByDescending(x => x.Surname).ThenBy(x => x.Name);

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
                                      .OrderByDescending(x => x.Surname).ThenBy(x => x.Name);
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
            Person person = db.Persons.Find(id);
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

                db.Persons.Add(person);
                db.SaveChanges();
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
            Person person = db.Persons.Find(id);
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
                Person person = db.Persons.Find(personView.PersonID) ;
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

                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
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
            Person person = db.Persons.Find(id);
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
            Person person = db.Persons.Find(id);
            db.Persons.Remove(person);
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
