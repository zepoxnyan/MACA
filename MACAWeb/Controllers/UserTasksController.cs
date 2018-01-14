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

namespace MACAWeb.Controllers
{
    [Authorize]
    public class UserTasksController : Controller
    {
        private UserTaskDbContext db = new UserTaskDbContext();
        
        /*
         * Session persists for 20 minutes only, can be changed in config 
         */
        /*private List<UserTaskViewModel> ViewModelList
        {
            get { return Session["viewModel"] as List<UserTaskViewModel>; }
            set { Session["viewModel"] = value; }
        }*/

        // GET: UserTasks
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            /*ViewModelList = new List<UserTaskViewModel>();
            db.Tasks.ToList().ForEach(
                t =>
                {
                    UserTaskViewModel viewModel = new UserTaskViewModel
                    {
                        TaskId = t.TaskId,
                        InstanceName = (from i in db.Instances
                                        where i.InstanceId == t.InstanceId
                                        select i.Name).First(),
                        FunctionName = (from f in db.Functions
                                        where f.FunctionId == t.FunctionId
                                        select f.Name).First(),
                        Status = (from s in db.Statuses
                                  where s.TaskStatusId == t.StatusId
                                  select s.Message).First()
                    };
                    viewModel.TimeStarted = t.TimeStarted == null ? "N/A" : t.TimeStarted.ToString();
                    viewModel.TimeFinished = t.TimeFinished == null ? "N/A" : t.TimeFinished.ToString();
                    ViewModelList.Add(viewModel);
                });
            return View(ViewModelList);*/

            var tasks = db.UserTasks.Include(x => x.Function).Include(x => x.TaskStatus).Include(x => x.Instance).OrderBy(x => x.DateCreated);

            if (!User.IsInRole("Administrator"))
            {
                Guid userId = Auxiliaries.GetUserId(User);
                tasks = db.UserTasks.Where(c => c.UserCreatedId == userId).Include(x => x.Function).Include(x => x.TaskStatus).Include(x => x.Instance).OrderBy(x => x.DateCreated);
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
                tasks = tasks.Where(m => m.Name.Contains(searchString)
                                      || m.Remark.Contains(searchString)
                                      || m.Instance.Name.Contains(searchString)
                                      || m.Function.Name.Contains(searchString)
                                      || m.TaskStatus.Name.Contains(searchString))
                                      .OrderBy(m => m.DateCreated);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["generalItemsOnPage"]);
            int pageNumber = (page ?? 1);

            return View(tasks.ToPagedList(pageNumber, pageSize));
        }

        // GET: UserTasks/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTask userTask = db.UserTasks.Find(id);
            if (userTask == null)
            {
                return HttpNotFound();
            }
            return View(userTask);
        }

        // GET: UserTasks/Create
        public ActionResult Create()
        {
            PopulateDropDownLists();
            return View();
        }

        private void PopulateDropDownLists(object selectedInstance = null, object selectedFunction = null)
        {
            Guid userId = Auxiliaries.GetUserId(User);

            var instancesQuery = db.Instances.Where(x => x.UserCreatedId == userId).OrderBy(c => c.Name);

            var functionsQuery = from c in db.Functions
                                 orderby c.Name
                                 select c;

            ViewBag.InstanceId = new SelectList(instancesQuery, "InstanceId", "Name", selectedInstance);
            ViewBag.FunctionId = new SelectList(functionsQuery, "FunctionId", "Name", selectedFunction);
        }

        // POST: UserTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Remark,InstanceId,FunctionId")] UserTask userTask)
        {
            if (ModelState.IsValid)
            {
                userTask.TaskId = Guid.NewGuid();
                userTask.TaskStatusId = (from s in db.TaskStatuses
                                     where s.Name == TaskStatusFixedList.Pending.ToString() select s.TaskStatusId).First();
                
                userTask.DateCreated = DateTime.Now;
                userTask.DateModified = userTask.DateCreated;

                userTask.UserCreatedId = Auxiliaries.GetUserId(User);
                userTask.UserModifiedId = Auxiliaries.GetUserId(User);

                db.UserTasks.Add(userTask);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateDropDownLists(userTask.InstanceId, userTask.FunctionId);
            return View(userTask);
        }
        
        // GET: UserTasks/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTask task = db.UserTasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            /*UserTaskEditViewModel viewModel = new UserTaskEditViewModel()
            {
                TaskId = task.TaskId,
                InstanceId = task.InstanceId,
                FunctionId = task.FunctionId
            };*/

            PopulateDropDownLists(task.InstanceId, task.FunctionId);
            return View(task);
        }

        // POST: UserTasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaskId,Name,Remark,InstanceId,FunctionId")] UserTaskViewModel taskViewModel)
        {
            if (ModelState.IsValid)
            {
                UserTask task = db.UserTasks.Find(taskViewModel.TaskId);

                task.Name = taskViewModel.Name;
                task.Remark = taskViewModel.Remark;
                task.InstanceId = taskViewModel.InstanceId;
                task.FunctionId = taskViewModel.FunctionId;

                task.DateModified = DateTime.Now;
                task.UserModifiedId = Auxiliaries.GetUserId(User);

                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateDropDownLists(taskViewModel.InstanceId, taskViewModel.FunctionId);
            return View(taskViewModel);
        }

        // GET: UserTasks/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTask userTask = db.UserTasks.Find(id);
            if (userTask == null)
            {
                return HttpNotFound();
            }
            return View(userTask);
        }

        // POST: UserTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UserTask userTask = db.UserTasks.Find(id);
            db.UserTasks.Remove(userTask);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: UserTasks/Start/5
        public ActionResult Start(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTask task = db.UserTasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }

            task.TaskStatusId = db.TaskStatuses.Where(x => x.Name == TaskStatusFixedList.Started.ToString()).First().TaskStatusId;
            task.TimeStarted = DateTime.Now;
            task.DateModified = DateTime.Now;

            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: UserTasks/Start/5
        public ActionResult Abort(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTask task = db.UserTasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }

            task.TaskStatusId = db.TaskStatuses.Where(x => x.Name == TaskStatusFixedList.Aborted.ToString()).First().TaskStatusId;
            task.TimeFinished = DateTime.Now;
            task.DateModified = DateTime.Now;

            db.Entry(task).State = EntityState.Modified;
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
