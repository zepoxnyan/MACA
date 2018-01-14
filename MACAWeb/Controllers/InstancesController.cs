using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MACAWeb.Models;
using System.IO;
using Microsoft.AspNet.Identity;
using System.Configuration;
using PagedList;
using System.Web.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Mime;

namespace MACAWeb.Controllers
{
    [Authorize]
    public class InstancesController : Controller
    {
        private InstanceDbContext db = new InstanceDbContext();
       
        // GET: Instances
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var instances = db.Instances.OrderBy(x => x.Name);
            if (!User.IsInRole("SuperAdmin"))
            {
                Guid userId = Auxiliaries.GetUserId(User);
                instances = db.Instances.Where(c => c.UserCreatedId == userId).OrderBy(x => x.Name);
            }

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
                instances = instances.Where(m => m.Name.Contains(searchString)
                                      || m.FileName.Contains(searchString)
                                      || m.Description.Contains(searchString))
                                      .OrderBy(m => m.Name);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["generalItemsOnPage"]);
            int pageNumber = (page ?? 1);

            return View(instances.ToPagedList(pageNumber, pageSize));
        }

        // GET: Instances/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instance instance = db.Instances.Find(id);
            if (instance == null)
            {
                return HttpNotFound();
            }
            return View(instance);
        }

        // GET: Instances/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Instances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description,UploadedFile")] Instance instance)
        {
            if (ModelState.IsValid)
            {
                //CONDITION CHECK
                //Check if correct format or show error
                string dirString = String.Format("~/InstanceFiles/{0}", Auxiliaries.GetUserId(User).ToString());
                string dirPath = Server.MapPath(dirString);

                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                string fileName = Path.GetFileName(instance.UploadedFile.FileName);               
                string filePath = Path.Combine(dirPath, fileName);
                
                instance.UploadedFile.SaveAs(filePath);
                instance.FileName = fileName;

                instance.InstanceId = Guid.NewGuid();
                instance.UserId = Auxiliaries.GetUserId(User);

                instance.DateCreated = DateTime.Now;
                instance.DateModified = DateTime.Now;

                instance.UserCreatedId = Auxiliaries.GetUserId(User);
                instance.UserModifiedId = Auxiliaries.GetUserId(User);

                db.Instances.Add(instance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(instance);
        }

        // GET: Instances/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instance instance = db.Instances.Find(id);
            if (instance == null)
            {
                return HttpNotFound();
            }

            InstanceViewModel instanceViewModel = new InstanceViewModel
            {
                InstanceId = instance.InstanceId,
                UserId = instance.UserId,
                Name = instance.Name,
                Description = instance.Description,
                FileName = instance.FileName
            };

            return View(instanceViewModel);
        }

        // POST: Instances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InstanceId,UserId,Name,Description,FileName,UploadedFile")] InstanceViewModel instanceViewModel)
        {
            if (ModelState.IsValid)
            {
                DeleteFile(instanceViewModel.FileName, instanceViewModel.UserId.ToString());
                              
                Instance instance = db.Instances.Find(instanceViewModel.InstanceId);

                instance.Name = instanceViewModel.Name;
                instance.Description = instanceViewModel.Description;
                //CONDITION CHECK
                //Check if correct format
                if (instanceViewModel.UploadedFile != null)
                {
                    string fileName = Path.GetFileName(instanceViewModel.UploadedFile.FileName);
                    string dirString = String.Format("~/InstanceFiles/{0}", instanceViewModel.UserId.ToString());
                    string dirPath = Server.MapPath(dirString);
                    string filePath = Path.Combine(dirPath, fileName);
                    instanceViewModel.UploadedFile.SaveAs(filePath);
                    instance.FileName = fileName;
                }               

                instance.DateModified = DateTime.Now;
                instance.UserModifiedId = Auxiliaries.GetUserId(User);
               
                db.Entry(instance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(instanceViewModel);
        }

        // GET: Instances/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instance instance = db.Instances.Find(id);
            if (instance == null)
            {
                return HttpNotFound();
            }
            return View(instance);
        }

        // POST: Instances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Instance instance = db.Instances.Find(id);
            DeleteFile(instance.FileName, instance.UserId.ToString());

            db.Instances.Remove(instance);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public FilePathResult Download(string fileName, string userId)
        {
            string filePath = String.Format("~/InstanceFiles/{0}/{1}", userId, fileName);
            return File(filePath, MediaTypeNames.Text.Plain, fileName);
        }
        
        private void DeleteFile(string fileName, string userId)
        {
            string dirString = String.Format("~/InstanceFiles/{0}", userId);
            string dirPath = Server.MapPath(dirString);
            string filePath = Path.Combine(dirPath, fileName);
            if(System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
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
