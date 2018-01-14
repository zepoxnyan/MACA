using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MACAWeb.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SelectLanguage(String Lang)
        {
            if (!String.IsNullOrEmpty(Lang))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Lang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Lang);
            }

            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = Lang;
            Response.Cookies.Add(cookie);
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }
    }
}