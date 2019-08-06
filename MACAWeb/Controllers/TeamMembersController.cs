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
    public class TeamMembersController : Controller
    {
        private MACADbContext db = new MACADbContext();

        // GET: TeamMembers
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var teamMembers = db.TeamMembers.OrderBy(x => x.LastName);

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
                teamMembers = teamMembers.Where(m => m.LastName.Contains(searchString)
                                      || m.FirstName.Contains(searchString)
                                      || m.Description.Contains(searchString)
                                      || m.Affiliation.Contains(searchString))
                                      .OrderBy(x => x.LastName);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["generalItemsOnPage"]);
            int pageNumber = (page ?? 1);

            return View(teamMembers.ToPagedList(pageNumber, pageSize));
        }

        // GET: TeamMembers/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeamMember teamMember = db.TeamMembers.Find(id);
            if (teamMember == null)
            {
                return HttpNotFound();
            }
            return View(teamMember);
        }

        // GET: TeamMembers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TeamMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,Description,Affiliation,HomepageLink,Image")] TeamMemberCreateViewModel teamMemberViewModel)
        {
            if (ModelState.IsValid)
            {
                TeamMember teamMember = new TeamMember();
                teamMember.TeamMemberID = Guid.NewGuid();
                teamMember.FirstName = teamMemberViewModel.FirstName;
                teamMember.LastName = teamMemberViewModel.LastName;
                teamMember.Description = teamMemberViewModel.Description;
                teamMember.Affiliation = teamMemberViewModel.Affiliation;
                teamMember.HomepageLink = teamMemberViewModel.HomepageLink;

                teamMember.DateCreated = DateTime.Now;
                teamMember.DateModified = DateTime.Now;
                teamMember.UserCreatedID = User.Identity.GetUserId();
                teamMember.UserModifiedID = User.Identity.GetUserId();

                // Handle the image
                if (teamMemberViewModel.Image != null && teamMemberViewModel.Image.ContentLength > 0)
                {
                    if (!Auxiliaries.ValidImageTypes.Contains(teamMemberViewModel.Image.ContentType))
                    {
                        ModelState.AddModelError("Image", "Choose an image in one of the following formats: GIF, JPG, or PNG.");
                    }
                    else
                    {
                        using (var reader = new BinaryReader(teamMemberViewModel.Image.InputStream))
                        {
                            teamMember.Image = reader.ReadBytes(teamMemberViewModel.Image.ContentLength);
                        }
                    }
                }

                db.TeamMembers.Add(teamMember);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(teamMemberViewModel);
        }

        // GET: TeamMembers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeamMember teamMember = db.TeamMembers.Find(id);
            if (teamMember == null)
            {
                return HttpNotFound();
            }

            TeamMemberEditViewModel teamMemberView = new TeamMemberEditViewModel();
            teamMemberView.TeamMemberID = teamMember.TeamMemberID;
            teamMemberView.FirstName = teamMember.FirstName;
            teamMemberView.LastName = teamMember.LastName;
            teamMemberView.Description = teamMember.Description;
            teamMemberView.Affiliation = teamMember.Affiliation;
            teamMemberView.HomepageLink = teamMember.HomepageLink;
            if (teamMember.Image != null && teamMember.Image.Length > 0)
            {
                teamMemberView.Image = (HttpPostedFileBase)new MemoryPostedFile(teamMember.Image);

                var base64 = Convert.ToBase64String(teamMember.Image);
                var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                ViewBag.ImgSrc = imgSrc;
            }

            return View(teamMemberView);
        }

        // POST: TeamMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeamMemberID,FirstName,LastName,Description,Affiliation,HomepageLink,Image")] TeamMemberEditViewModel teamMemberView)
        {
            if (ModelState.IsValid)
            {
                TeamMember teamMember = db.TeamMembers.Find(teamMemberView.TeamMemberID); ;
                teamMember.FirstName = teamMemberView.FirstName;
                teamMember.LastName = teamMemberView.LastName;
                teamMember.Description = teamMemberView.Description;
                teamMember.Affiliation = teamMemberView.Affiliation;
                teamMember.HomepageLink = teamMemberView.HomepageLink;

                teamMember.DateModified = DateTime.Now;
                teamMember.UserModifiedID = User.Identity.GetUserId();

                // Handle the image
                if (teamMemberView.Image != null && teamMemberView.Image.ContentLength > 0)
                {
                    if (!Auxiliaries.ValidImageTypes.Contains(teamMemberView.Image.ContentType))
                    {
                        ModelState.AddModelError("Image", "Choose an image in one of the following formats: GIF, JPG, or PNG.");
                    }
                    else
                    {
                        using (var reader = new BinaryReader(teamMemberView.Image.InputStream))
                        {
                            teamMember.Image = reader.ReadBytes(teamMemberView.Image.ContentLength);
                        }
                    }
                }

                db.Entry(teamMember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teamMemberView);
        }

        // GET: TeamMembers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeamMember teamMember = db.TeamMembers.Find(id);
            if (teamMember == null)
            {
                return HttpNotFound();
            }
            return View(teamMember);
        }

        // POST: TeamMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TeamMember teamMember = db.TeamMembers.Find(id);
            db.TeamMembers.Remove(teamMember);
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
