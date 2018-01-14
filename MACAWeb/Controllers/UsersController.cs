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
    [Authorize]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
       public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var users = db.Users.OrderBy(x => x.UserName);

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.Currentfilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.UserName.Contains(searchString)).OrderBy(u => u.UserName);
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["generalItemsOnPage"]);
            int pageNumber = (page ?? 1);

            return View(users.ToPagedList(pageNumber, pageSize));
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
