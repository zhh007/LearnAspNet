﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MvcFox.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return ServerError();
        }

        public ActionResult NotFound()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View("NotFound");
        }

        public ActionResult ServerError()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View("ServerError");
        }
    }
}