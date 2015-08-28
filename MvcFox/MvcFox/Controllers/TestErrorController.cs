using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcFox.Controllers
{
    public class TestErrorController : Controller
    {
        // GET: TestError
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Error500()
        {
            return View();
        }
    }
}