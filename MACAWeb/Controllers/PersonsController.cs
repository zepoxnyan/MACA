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
    [Authorize(Roles = "Employee, Admin, SuperAdmin")]
    public class PersonsController : Controller
    {
        private MACADbContext db = new MACADbContext ();
        private ApplicationDbContext dbApplication = new ApplicationDbContext();




        // GET: Persons
        [Authorize(Roles = "Employee")]
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var persons = db.Persons.OrderBy(x => x.Surname).ThenBy(x => x.Name);
            
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
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.Person = "";
            //string selectedUser = dbPersonUsers.PersonUsers.Where(x => x.PersonID == person.PersonID).FirstOrDefault()?.UserID.ToString();
            string selectedUser = CheckUserLink(person.PersonID);

            if (selectedUser != null)
            {
                ViewBag.Person = selectedUser;
            }
            return View(person);
        }

        // GET: Persons/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Persons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Surname,Name,FullName,Description,Email,Image")] PersonViewModel personView)
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
                person.Email = personView.Email;
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
                            person.ImageThumb = Auxiliaries.CreateThumbnail(person.Image);
                        }
                    }
                }

                db.Persons.Add(person);
                db.SaveChanges();

                // Automatically add a person to authors
                Author author = new Author();
                author.AuthorID = authorGuid;
                author.Surname = person.Surname;
                author.FirstName = person.Name;

                author.DateCreated = DateTime.Now;
                author.DateModified = DateTime.Now;
                author.UserCreatedID = new Guid(User.Identity.GetUserId());
                author.UserModifiedID = author.UserCreatedID;

                db.Authors.Add(author);
                db.SaveChanges();

                person = db.Persons.Find(person.PersonID);
                person.AuthorID = authorGuid;
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(personView);
        }

        // GET: Persons/Edit/5
        //Test urejanja za employee
        [Authorize(Roles = "Employee")]
        //------------------------------
        public ActionResult Edit(Guid? id)
        {
            if (User.IsInRole("Employee") && id==null)
            {
                string loggedUser = User.Identity.GetUserId();
                var selectedUser = db.PersonUsers.Where(x => x.UserID == loggedUser).FirstOrDefault()?.PersonID.ToString();
                if (selectedUser != null) { id = Guid.Parse(selectedUser); }
               
            }
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
            personView.Email = person.Email;

            if (person.Image != null && person.Image.Length > 0)
            {
                personView.Image = (HttpPostedFileBase)new MemoryPostedFile(person.Image);

                var base64 = Convert.ToBase64String(person.Image);
                var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                ViewBag.ImgSrc = imgSrc;
            }
            ViewBag.LoggedUser = CheckUserLink(person.PersonID);
            var userlist = dbApplication.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.Id, Text = uu.UserName }).ToList();
            ViewBag.Users = userlist;
            return View(personView);
        }

        // POST: Persons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Test urejanja za employee
        [Authorize(Roles = "Employee")]
        //------------------------------
        public ActionResult Edit([Bind(Include = "PersonID,Surname,Name,FullName,Description,Email,Image,ImageThumb")] PersonViewModel personView)
        {
            if (ModelState.IsValid)
            {
                Person person = db.Persons.Find(personView.PersonID);
                person.Surname = personView.Surname;
                person.Name = personView.Name;
                person.FullName = personView.FullName;
                person.Description = personView.Description;
                person.Email = personView.Email;
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
                            person.ImageThumb = Auxiliaries.CreateThumbnail(person.Image);
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
            PersonUser PSlink = db.PersonUsers.Where(ps => ps.PersonID == person.PersonID)?.FirstOrDefault();
            if(PSlink != null)
            {
                db.PersonUsers.Remove(PSlink);
                db.SaveChanges();
            }
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


        #region Positions

        private bool IsUserAuthorized(String id)
        {
            return User.IsInRole("Admin"); //|| User.Identity.GetUserId().Contains(id);
        }

        public ActionResult PositionsIndex(Guid personId)
        {
            var positions = db.Positions.Where(x => x.PersonID == personId).OrderByDescending(x => x.Year).ThenBy(x => x.Semester);

            Person person = db.Persons.Where(p => p.PersonID == personId).Single();
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
            var positionTypesQuery = from c in db.PositionTypes
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

                db.Positions.Add(position);
                db.SaveChanges();
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
            Position position = db.Positions.Find(id);
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
                Position position = db.Positions.Find(positionViewModel.PositionID);

                position.PositionID = positionViewModel.PositionID;
                position.PositionTypeID = positionViewModel.PositionTypeID;
                position.Description = positionViewModel.Description;
                position.Year = positionViewModel.Year;
                position.Semester = positionViewModel.Semester;

                position.DateModified = DateTime.Now;
                position.UserModifiedID = Guid.Parse(User.Identity.GetUserId());

                db.Entry(position).State = EntityState.Modified;
                db.SaveChanges();
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
            Position position = db.Positions.Find(id);
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
            Position position = db.Positions.Find(id);
            db.Positions.Remove(position);
            db.SaveChanges();
            return RedirectToAction("PositionsIndex", routeValues: new { personId = position.PersonID });
        }

        #endregion
        #region PSLink
        private string CheckUserLink(Guid selectedID)
        {
            string selectedUser = db.PersonUsers.Where(x => x.PersonID == selectedID).FirstOrDefault()?.UserID.ToString();
            if (selectedUser != null)
            {
                ApplicationUser user = dbApplication.Users.Where(u => u.Id.Equals(selectedUser)).FirstOrDefault();
                return user.UserName;
            }
            else
            {
                return null;
            }
        }

        public ActionResult PSLinkIndex(Guid personId)
        {
            var pslinks = db.PersonUsers.Where(x => x.PersonID == personId);
            Person person = db.Persons.Where(p => p.PersonID == personId).Single();
            
            ViewBag.PersonID = personId;
            return View(pslinks);
        }

        public ActionResult PSLinkCreate(Guid personId)
        {
            ViewBag.PersonID = personId;
            var userlist = dbApplication.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.Id.ToString(), Text = uu.UserName }).ToList();
            ViewBag.Users = userlist;
            return View();
        }

        

        // POST: Grants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PSLinkCreate(string selectedUser, PersonUser pslink, Guid personId)
        {
            if (ModelState.IsValid)
            {
                pslink.PersonUserID = Guid.NewGuid();

                pslink.PersonID = personId;
                pslink.UserID = selectedUser;


                db.PersonUsers.Add(pslink);
                db.SaveChanges();
                return RedirectToAction("PSLinkIndex", new { personId = personId });
            }

            return View();
        }

        public ActionResult PSLinkDelete(Guid selectedLink, Guid personId)
        {
            var thisLink = db.PersonUsers.Where(x => x.PersonUserID == selectedLink).FirstOrDefault();
            db.PersonUsers.Remove(thisLink);
            db.SaveChanges();
            return RedirectToAction("PSLinkIndex", new { personId = personId });
        }


        #endregion

        #region SocialLinks

        public ActionResult SocialLinkIndex(Guid personId)
        {
            var sociallinks = db.SocialLinks.Where(x => x.PersonID == personId);

            Person person = db.Persons.Where(p => p.PersonID == personId).Single();
            ViewBag.PersonID = personId;
            return View(sociallinks);
        }

        // GET: Grants/Create
        public ActionResult SocialLinkCreate(Guid personId)
        {
            PopulateSocialLinkTypesDropDownList();
            ViewBag.PersonID = personId;

            return View();
        }

        private void PopulateSocialLinkTypesDropDownList(object selectedSocialLinkType = null)
        {
            var TypesQuery = from c in db.SocialLinkTypes
                                     orderby c.Name
                                     select c;
            ViewBag.SocialLinkTypeID = new SelectList(TypesQuery, "SocialLinkTypeID", "Name", selectedSocialLinkType);
        }

        // POST: Grants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SocialLinkCreate([Bind(Include = "SocialLinkTypeID,ProfileUrl,PersonID")] SocialLink sl, string personId)
        {
            if (ModelState.IsValid)
            {
                sl.SocialLinkID = Guid.NewGuid(); 
                db.SocialLinks.Add(sl);
                db.SaveChanges();
                return RedirectToAction("SocialLinkIndex", new { personId = personId });
            }

            return View(sl);
        }

        public ActionResult SocialLinkEdit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SocialLink sl = db.SocialLinks.Find(id);
            if (sl == null)
            {
                return HttpNotFound();
            }
            PopulateSocialLinkTypesDropDownList(sl.SocialLinkTypeID);
            return View(sl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SocialLinkEdit([Bind(Include = "SocialLinkID,SocialLinkTypeID,ProfileUrl,PersonID")] SocialLinkViewModel slViewModel, string personId)
        {
            if (ModelState.IsValid)
            {
                SocialLink sl = db.SocialLinks.Find(slViewModel.SocialLinkID);

                sl.SocialLinkID = slViewModel.SocialLinkID;
                sl.SocialLinkTypeID = slViewModel.SocialLinkTypeID;
                sl.ProfileUrl = slViewModel.ProfileUrl;
                
                db.Entry(sl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SocialLinkIndex", new { personId = personId });
            }
            return View(slViewModel);
        }

        public ActionResult SocialLinkDelete(Guid? id, Guid personId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SocialLink sl = db.SocialLinks.Find(id);
            if (sl == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonID = personId;

            return View(sl);
        }

        [HttpPost, ActionName("SocialLinkDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult SocialLinkDeleteConfirmed(Guid id)
        {
            SocialLink sl = db.SocialLinks.Find(id);
            db.SocialLinks.Remove(sl);
            db.SaveChanges();
            return RedirectToAction("SocialLinkIndex", routeValues: new { personId = sl.PersonID });
        }

        #endregion

    }
}
