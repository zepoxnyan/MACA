using MACAWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MACAWeb.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class RolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private void UpdateDropdown()
        {
            var rolelist = db.Roles.OrderBy(r => r.Id).ToList().Select(rr => new SelectListItem { Value = rr.Id.ToString(), Text = rr.Name }).ToList();
            var userlist = db.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            if (!User.IsInRole("SuperAdmin")) ViewBag.Roles = rolelist.Where(x => x.Text != "SuperAdmin");
            else ViewBag.Roles = rolelist;

            ViewBag.Users = userlist;
        }

        public ActionResult Index()
        {
            UpdateDropdown();            
            if (TempData["Type"] != null)
            {
                ViewBag.Type = TempData["Type"];
                ViewBag.Message = TempData["Message"];

            }
            else
            {
                ViewBag.Message = "";
            }
            return View();
        }


        // GET: /Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Roles/Create
        [HttpPost]
        public ActionResult Create(string roleName)
        {
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                try
                {
                    db.Roles.Add(new IdentityRole(roleName));
                    db.SaveChanges();
                    TempData["Type"] = "alert-success";
                    TempData["Message"] = roleName + " was created!";
                    return RedirectToAction("Index");                    
                }
                catch
                {
                    TempData["Type"] = "alert-warning";
                    TempData["Message"] = "There was an error trying to create "+roleName+", maybe it already exists.";
                    return RedirectToAction("Index");
                }
            }
            else
                return RedirectToAction("Index");
        }


        //DELETE
        public ActionResult Delete(string roleID)
        {
            if (!User.IsInRole("SuperAdmin") && db.Roles.Where(r => r.Id == roleID).First().Name == "SuperAdmin")
            {
                TempData["Type"] = "alert-warning";
                TempData["Message"]= "You don't have permission to delete this role.";
                return RedirectToAction("Index");

            }
            var thisRole = db.Roles.Where(r => r.Id.Equals(roleID, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            db.Roles.Remove(thisRole);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: /Roles/Edit/5
        public ActionResult Edit(string roleID)
        {
            if (!User.IsInRole("SuperAdmin") && db.Roles.Where(r => r.Id == roleID).First().Name == "SuperAdmin")
            {
                TempData["Type"] = "alert-warning";
                TempData["Message"] = "You don't have permission to modify this role.";
                return RedirectToAction("Index");

            }
            var thisRole = db.Roles.Where(r => r.Id.Equals(roleID, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            return View(thisRole);
        }

        
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IdentityRole role)
        {   
            try
            {
                db.Entry(role).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        //  ASINGING USERS TO ROLES
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRoles(string UserName, string roleID)
        {
            if (db != null)
            {

                ApplicationUser user = db.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var roleName = db.Roles.Where(r => r.Id == roleID).First().Name;

                try
                {
                    userManager.AddToRole(user.Id, roleName);
                    ViewBag.Type = "alert-success";
                    ViewBag.Message = "User was added to the " + roleName + " role!";
                }
                catch
                {
                    ViewBag.Type = "alert-danger";
                    ViewBag.Message = "There was an error trying to add the user to " + roleName + " role!";
                }
                
                //Dropdown Lists
                UpdateDropdown();
                return View("Index");
            }
            else
            {
                throw new ArgumentNullException("db", "There was an error parsing the database.");
            }
            
        }


        //GET USER ROLES
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = db.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new UserManager<ApplicationUser>(userStore);
                ViewBag.RolesForThisUser = userManager.GetRoles(user.Id);

                UpdateDropdown();         
                ViewBag.Type = "alert-info";
                ViewBag.Message = "Roles retrieved successfully!";
            }

            return View("Index");
        }

        //REMOVE USER ROLE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoles(string UserName, string roleID)
        {
            var account = new AccountController();
            ApplicationUser user = db.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var roleName = db.Roles.Where(r => r.Id == roleID).First().Name;
            if (!User.IsInRole("SuperAdmin") && roleName == "SuperAdmin")
            {
                ViewBag.Type = "alert-warning";
                ViewBag.Message = "You can't remove this role.";
            }
            else
            {
                if (userManager.IsInRole(user.Id, roleName))
                {
                    userManager.RemoveFromRole(user.Id, roleName);
                    ViewBag.Type = "alert-success";
                    ViewBag.Message = "Role removed from this user successfully!";
                }
                else
                {
                    ViewBag.Type = "alert-danger";
                    ViewBag.Message = "This user doesn't belong to the selected role.";
                }
                
            }

            //Dropdown Lists
            UpdateDropdown();
            return View("Index");
        }

        

    }
}