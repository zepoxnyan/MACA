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
using System.IO;
using System.Configuration;
using PagedList;

namespace MACAWeb.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class NewsController : Controller
    {
        private NewsDbContext db = new NewsDbContext();

        // GET: News
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var news = db.News.OrderByDescending(x => x.DatePublished);

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
                news = news.Where(m => m.Title.Contains(searchString)
                                      || m.Text.Contains(searchString)
                                      || m.Author.Contains(searchString)
                                      || m.Abstract.Contains(searchString))
                                      .OrderByDescending(x => x.DatePublished);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["generalItemsOnPage"]);
            int pageNumber = (page ?? 1);

            return View(news.ToPagedList(pageNumber, pageSize));
        }

        // GET: News/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }

            return View(news);
        }

        // GET: News/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Abstract,Text,Author,Source,SourceLink,DatePublished,Image,ImageAuthor,ImageDescription")] NewsViewModel newsView)
        {
            if (ModelState.IsValid)
            {
                News news = new News();
                news.NewsID = Guid.NewGuid();
                news.Title = newsView.Title;
                news.Abstract = newsView.Abstract;
                news.Text = newsView.Text;
                news.Author = newsView.Author;
                news.Source = newsView.Source;
                news.SourceLink = newsView.SourceLink;
                news.DatePublished = newsView.DatePublished;
                news.ImageAuthor = newsView.ImageAuthor;
                news.ImageDescription = newsView.ImageDescription;

                news.DateCreated = DateTime.Now;
                news.DateModified = DateTime.Now;
                news.UserCreatedID = User.Identity.GetUserId();
                news.UserModifiedID = User.Identity.GetUserId();

                // Handle the image
                if (newsView.Image != null && newsView.Image.ContentLength > 0)
                {
                    if (!Auxiliaries.ValidImageTypes.Contains(newsView.Image.ContentType))
                    {
                        ModelState.AddModelError("Image", "Choose an image in one of the following formats: GIF, JPG, or PNG.");
                    }
                    else
                    {
                        using (var reader = new BinaryReader(newsView.Image.InputStream))
                        {
                            news.Image = reader.ReadBytes(newsView.Image.ContentLength);
                        }
                    }
                }

                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newsView);
        }

        // GET: News/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }

            NewsViewModel newsView = new NewsViewModel();
            newsView.NewsID = news.NewsID;
            newsView.Title = news.Title;
            newsView.Abstract = news.Abstract;
            newsView.Text = news.Text;
            newsView.Author = news.Author;
            newsView.Source = news.Source;
            newsView.SourceLink = news.SourceLink;
            newsView.DatePublished = news.DatePublished;
            newsView.ImageAuthor = news.ImageAuthor;
            newsView.ImageDescription = news.ImageDescription;
            if (news.Image != null && news.Image.Length > 0)
            {
                newsView.Image = (HttpPostedFileBase)new MemoryPostedFile(news.Image);

                var base64 = Convert.ToBase64String(news.Image);
                var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                ViewBag.ImgSrc = imgSrc;
            }

            return View(newsView);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NewsID,Title,Abstract,Text,Author,Source,SourceLink,DatePublished,Image,ImageAuthor,ImageDescription")] NewsViewModel newsView)
        {
            if (ModelState.IsValid)
            {
                News news = db.News.Find(newsView.NewsID);
                news.Title = newsView.Title;
                news.Abstract = newsView.Abstract;
                news.Text = newsView.Text;
                news.Author = newsView.Author;
                news.Source = newsView.Source;
                news.SourceLink = newsView.SourceLink;
                news.DatePublished = newsView.DatePublished;
                news.ImageAuthor = newsView.ImageAuthor;
                news.ImageDescription = newsView.ImageDescription;

                news.DateModified = DateTime.Now;
                news.UserModifiedID = User.Identity.GetUserId();

                // Handle the image
                if (newsView.Image != null && newsView.Image.ContentLength > 0)
                {
                    if (!Auxiliaries.ValidImageTypes.Contains(newsView.Image.ContentType))
                    {
                        ModelState.AddModelError("Image", "Choose an image in one of the following formats: GIF, JPG, or PNG.");
                    }
                    else
                    {
                        using (var reader = new BinaryReader(newsView.Image.InputStream))
                        {
                            news.Image = reader.ReadBytes(newsView.Image.ContentLength);
                        }
                    }
                }

                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newsView);
        }

        // GET: News/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
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
