using MACAWeb.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MACAWeb.Controllers
{
    public class ResearchController : Controller
    {
        private MACADbContext db = new MACADbContext();
        public Guid getPersonID()
        {
            var loggedUser = User.Identity.GetUserId();
            var personID = Guid.Parse(db.PersonUsers.Where(x => x.UserID == loggedUser).FirstOrDefault()?.PersonID.ToString());
            return personID;
        }
        public ActionResult Index()
        {
            
            Guid loggedPerson = getPersonID();
            var iinterest = db.Interests.Where(i => i.PersonID == loggedPerson).OrderBy(x => x.Title);
            var cTalk = db.ConferenceTalks.Where(i => i.PersonID == loggedPerson).OrderBy(x => x.Title);
            var sTalk = db.SeminarTalks.Where(i => i.PersonID == loggedPerson).OrderBy(x => x.Title);
            Research research = new Research { };
            research.interest = iinterest;
            research.conferenceTalk = cTalk;
            research.seminarTalk = sTalk;


            return View(research);
        }
        #region Interests
        public ActionResult InterestsIndex(string currentFilter, string searchString, int? page)
        {
            Guid loggedPerson = getPersonID();
            var interests = db.Interests.Where(i => i.PersonID== loggedPerson).OrderBy(x => x.Title);

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
                interests = interests.Where(m => m.Title.Contains(searchString)
                                      || m.Description.Contains(searchString)
                                      || m.Person.FullName.Contains(searchString))
                                      .OrderBy(x => x.Title).ThenBy(x => x.Person.Surname);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["generalItemsOnPage"]);
            int pageNumber = (page ?? 1);
            ViewBag.PersonID = getPersonID();
            return View(interests.ToPagedList(pageNumber, pageSize));
        }
        // GET: InterestsDetails
        public ActionResult InterestsDetails(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interest interest = db.Interests.Find(id);
            if (interest == null)
            {
                return HttpNotFound();
            }
            return View(interest);
        }

        // GET: InterestsCreate
        public ActionResult InterestsCreate(Guid personID)
        {
            ViewBag.PersonID = personID;
            return View();
        }

        // POST: InterestsCreate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InterestsCreate([Bind(Include = "PersonID,Title,Description")] Interest interest, string personID)
        {
            if (ModelState.IsValid)
            {
                interest.InterestID = Guid.NewGuid();

                interest.DateCreated = DateTime.Now;
                interest.DateModified = interest.DateCreated;

                db.Interests.Add(interest);
                db.SaveChanges();
                return RedirectToAction("InterestsIndex");
            }

            return View(interest);
        }
        public ActionResult InterestsEdit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interest interest = db.Interests.Find(id);
            if (interest == null)
            {
                return HttpNotFound();
            }
            return View(interest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InterestsEdit([Bind(Include = "InterestID,PersonID,Title,Description")] InterestView interestView, string personID)
        {
            if (ModelState.IsValid)
            {
                Interest interest = db.Interests.Find(interestView.InterestID);

                interest.Title = interestView.Title;
                interest.Description = interestView.Description;

                db.Entry(interest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("InterestsIndex", new { personID = personID });
            }
            return View(interestView);
        }

        public ActionResult InterestsDelete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interest interest = db.Interests.Find(id);
            if (interest == null)
            {
                return HttpNotFound();
            }
            return View(interest);
        }

        [HttpPost, ActionName("InterestsDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult InterestsDeleteConfirmed(Guid id)
        {
            Interest interest = db.Interests.Find(id);
            db.Interests.Remove(interest);
            db.SaveChanges();
            return RedirectToAction("InterestsIndex");
        }
        #endregion

        #region ConferenceTalks
        public ActionResult ConferenceTalksIndex(string currentFilter, string searchString, int? page)
        {
            Guid loggedPerson = getPersonID();
            var cTalk = db.ConferenceTalks.Where(c => c.PersonID == loggedPerson).OrderBy(x => x.Title);

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
                cTalk = cTalk.Where(m => m.Title.Contains(searchString)
                                      || m.ConferenceName.Contains(searchString)
                                      || m.City.Contains(searchString)
                                      || m.Country.Contains(searchString)
                                      || m.Year.Contains(searchString)
                                      || m.Person.FullName.Contains(searchString))
                                      .OrderBy(x => x.Title).ThenBy(x => x.DateModified);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["generalItemsOnPage"]);
            int pageNumber = (page ?? 1);
            ViewBag.PersonID = getPersonID();
            return View(cTalk.ToPagedList(pageNumber, pageSize));
        }
        // GET: InterestsDetails
        public ActionResult ConferenceTalksDetails(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConferenceTalk cTalk = db.ConferenceTalks.Find(id);
            if (cTalk == null)
            {
                return HttpNotFound();
            }
            return View(cTalk);
        }

        // GET: InterestsCreate
        public ActionResult ConferenceTalksCreate(Guid personID)
        {
            ViewBag.PersonID = personID;
            return View();
        }

        // POST: InterestsCreate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConferenceTalksCreate([Bind(Include = "PersonID,Title,ConferenceName,City,Country,Year,InvitedTalk,PdfLink")] ConferenceTalk cTalk, string personID)
        {
            if (ModelState.IsValid)
            {
                cTalk.ConferenceTalkID = Guid.NewGuid();

                cTalk.DateCreated = DateTime.Now;
                cTalk.DateModified = cTalk.DateCreated;

                db.ConferenceTalks.Add(cTalk);
                db.SaveChanges();
                return RedirectToAction("ConferenceTalksIndex");
            }

            return View(cTalk);
        }
        public ActionResult ConferenceTalksEdit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConferenceTalk cTalk = db.ConferenceTalks.Find(id);
            if (cTalk == null)
            {
                return HttpNotFound();
            }
            return View(cTalk);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConferenceTalksEdit([Bind(Include = "ConferenceTalkID,PersonID,Title,ConferenceName,City,Country,Year,InvitedTalk,PdfLink")] ConferenceTalkView cTalkView, string personID)
        {
            if (ModelState.IsValid)
            {
                ConferenceTalk cTalk = db.ConferenceTalks.Find(cTalkView.ConferenceTalkID);

                cTalk.Title = cTalkView.Title;
                cTalk.ConferenceName = cTalkView.ConferenceName;
                cTalk.City = cTalkView.City;
                cTalk.Country = cTalkView.Country;
                cTalk.Year = cTalkView.Year;
                cTalk.InvitedTalk = cTalkView.InvitedTalk;
                cTalk.PdfLink = cTalkView.PdfLink;


                db.Entry(cTalk).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ConferenceTalksIndex");
            }
            return View(cTalkView);
        }

        public ActionResult ConferenceTalksDelete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConferenceTalk cTalk = db.ConferenceTalks.Find(id);
            if (cTalk == null)
            {
                return HttpNotFound();
            }
            return View(cTalk);
        }

        [HttpPost, ActionName("ConferenceTalksDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult ConferenceTalksDeleteConfirmed(Guid id)
        {
            ConferenceTalk cTalk = db.ConferenceTalks.Find(id);
            db.ConferenceTalks.Remove(cTalk);
            db.SaveChanges();
            return RedirectToAction("ConferenceTalksIndex");
        }

        #endregion
        #region SeminarTalks
        public ActionResult SeminarTalksIndex(string currentFilter, string searchString, int? page)
        {
            Guid loggedPerson = getPersonID();
            var sTalk = db.SeminarTalks.Where(c => c.PersonID == loggedPerson).OrderBy(x => x.Title);

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
                sTalk = sTalk.Where(m => m.Title.Contains(searchString)
                                      || m.SeminarName.Contains(searchString)
                                      || m.University.Contains(searchString)
                                      || m.City.Contains(searchString)
                                      || m.Country.Contains(searchString)
                                      || m.Year.Contains(searchString)
                                      || m.Person.FullName.Contains(searchString))
                                      .OrderBy(x => x.Title).ThenBy(x => x.DateModified);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["generalItemsOnPage"]);
            int pageNumber = (page ?? 1);
            ViewBag.PersonID = getPersonID();
            return View(sTalk.ToPagedList(pageNumber, pageSize));
        }
        // GET: Details
        public ActionResult SeminarTalksDetails(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeminarTalk sTalk = db.SeminarTalks.Find(id);
            if (sTalk == null)
            {
                return HttpNotFound();
            }
            return View(sTalk);
        }

        // GET: Create
        public ActionResult SeminarTalksCreate(Guid personID)
        {
            ViewBag.PersonID = personID;
            return View();
        }

        // POST: Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeminarTalksCreate([Bind(Include = "PersonID,Title,SeminarName,University,City,Country,Year,PdfLink")] SeminarTalk sTalk, string personID)
        {
            if (ModelState.IsValid)
            {
                sTalk.SeminarTalkID = Guid.NewGuid();

                sTalk.DateCreated = DateTime.Now;
                sTalk.DateModified = sTalk.DateCreated;

                db.SeminarTalks.Add(sTalk);
                db.SaveChanges();
                return RedirectToAction("SeminarTalksIndex");
            }

            return View(sTalk);
        }
        public ActionResult SeminarTalksEdit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeminarTalk sTalk = db.SeminarTalks.Find(id);
            if (sTalk == null)
            {
                return HttpNotFound();
            }
            return View(sTalk);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeminarTalksEdit([Bind(Include = "SeminarTalkID,PersonID,Title,SeminarName,University,City,Country,Year,PdfLink")] SeminarTalkView sTalkView, string personID)
        {
            if (ModelState.IsValid)
            {
                SeminarTalk sTalk = db.SeminarTalks.Find(sTalkView.SeminarTalkID);

                sTalk.Title = sTalkView.Title;
                sTalk.SeminarName = sTalkView.SeminarName;
                sTalk.University = sTalkView.University;
                sTalk.City = sTalkView.City;
                sTalk.Country = sTalkView.Country;
                sTalk.Year = sTalkView.Year;
                sTalk.PdfLink = sTalkView.PdfLink;


                db.Entry(sTalk).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SeminarTalksIndex");
            }
            return View(sTalkView);
        }

        public ActionResult SeminarTalksDelete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeminarTalk sTalk = db.SeminarTalks.Find(id);
            if (sTalk == null)
            {
                return HttpNotFound();
            }
            return View(sTalk);
        }

        [HttpPost, ActionName("SeminarTalksDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult SeminarTalksDeleteConfirmed(Guid id)
        {
            SeminarTalk sTalk = db.SeminarTalks.Find(id);
            db.SeminarTalks.Remove(sTalk);
            db.SaveChanges();
            return RedirectToAction("SeminarTalksIndex");
        }
        #endregion
    }
}