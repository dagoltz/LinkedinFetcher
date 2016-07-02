using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinkedinFetcher.MVC.Controllers
{
    /// <summary>
    /// Is incahrge of home pages
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Returns home page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Help");
        }
    }
}
