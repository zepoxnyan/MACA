using MACAWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MACAWeb.Controllers
{
    public class HomeController : Controller
    {
        /*private NewsDbContext dbNews = new NewsDbContext();
        private TeamMemberDbContext dbTeamMembers = new TeamMemberDbContext();
        private FunctionDbContext dbFunctions = new FunctionDbContext();*/

        public ActionResult Index()
        {
            //News lastNews = dbNews.News.OrderByDescending(x => x.DatePublished).First();
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Press()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Papers()
        {
            return View();
        }

        public ActionResult Features()
        {
            //List<Function> functions = dbFunctions.Functions.OrderBy(x => x.Name).ToList();
            return View();
        }

        public ActionResult News()
        {
            //List<News> orderedNews = dbNews.News.OrderByDescending(x => x.DatePublished).ToList();
            return View();
        }

        public ActionResult Team()
        {
            //List<TeamMember> teamMembers = dbTeamMembers.TeamMembers.OrderByDescending(x => x.PagePosition).ThenBy(x => x.LastName).ToList();
            return View();
        }

        public ActionResult FAQs()
        {
            FAQDbContext dbFAQs = new FAQDbContext();
            List<FAQ> sortedFAQs = dbFAQs.FAQs.OrderBy(x => x.Title).ToList();
            return View(sortedFAQs);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Administration()
        {
            ViewBag.Message = "Administration";

            return View();
        }
    }
}