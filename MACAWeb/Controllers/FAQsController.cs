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
using System.Configuration;
using PagedList;

namespace MACAWeb.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class FAQsController : Controller
    {
        private MACADbContext db = new MACADbContext();

        // GET: FAQs
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var faqs = db.FAQs.OrderBy(x => x.Title);

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
                faqs = faqs.Where(m => m.Title.Contains(searchString)
                                      || m.Answer.Contains(searchString)
                                      || m.Question.Contains(searchString))
                                      .OrderBy(m => m.Title);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["generalItemsOnPage"]);
            int pageNumber = (page ?? 1);

            return View(faqs.ToPagedList(pageNumber, pageSize));
        }

        // GET: FAQs/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FAQ fAQ = db.FAQs.Find(id);
            if (fAQ == null)
            {
                return HttpNotFound();
            }
            return View(fAQ);
        }

        // GET: FAQs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FAQs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FaqID,Title,Question,Answer,Author")] FAQ faq)
        {
            if (ModelState.IsValid)
            {
                faq.FaqID = Guid.NewGuid();

                faq.DateCreated = DateTime.Now;
                faq.DateModified = DateTime.Now;
                faq.UserCreatedID = Auxiliaries.GetUserId(User);
                faq.UserModifiedID = Auxiliaries.GetUserId(User);

                db.FAQs.Add(faq);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(faq);
        }

        // GET: FAQs/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FAQ fAQ = db.FAQs.Find(id);
            if (fAQ == null)
            {
                return HttpNotFound();
            }
            return View(fAQ);
        }

        // POST: FAQs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FaqID,Title,Question,Answer,Author")] FAQViewModel faqView)
        {
            if (ModelState.IsValid)
            {
                FAQ faq = db.FAQs.Find(faqView.FaqID);
                faq.Title = faqView.Title;
                faq.Question = faqView.Question;
                faq.Answer = faqView.Answer;
                faq.Author = faqView.Author;

                faq.DateModified = DateTime.Now;
                faq.UserModifiedID = Auxiliaries.GetUserId(User);

                db.Entry(faq).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(faqView);
        }

        // GET: FAQs/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FAQ fAQ = db.FAQs.Find(id);
            if (fAQ == null)
            {
                return HttpNotFound();
            }
            return View(fAQ);
        }

        // POST: FAQs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            FAQ fAQ = db.FAQs.Find(id);
            db.FAQs.Remove(fAQ);
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
