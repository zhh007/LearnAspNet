using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcFox.Models;
using System.Web.Script.Serialization;
using System.Net;

public class JsonFormResult : JsonResult
{
    public JsonFormResult(ModelStateDictionary modelStates)
    {
        _modelStates = modelStates;
    }

    private ModelStateDictionary _modelStates = null;

    //private const string JSONREQUEST_GETNOTALLOWED = "This request has been blocked because sensitive information could be disclosed to third party web sites when this is used in a GET request.To allow GET requests, set JsonRequestBehavior to AllowGet.";

    public override void ExecuteResult(ControllerContext context)
    {
        //if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
        //    String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
        //{
        //    throw new InvalidOperationException(JSONREQUEST_GETNOTALLOWED);
        //}

        HttpResponseBase response = context.HttpContext.Response;

        if (!String.IsNullOrEmpty(ContentType))
        {
            response.ContentType = ContentType;
        }
        else
        {
            response.ContentType = "application/json";
        }

        if (ContentEncoding != null)
        {
            response.ContentEncoding = ContentEncoding;
        }

        if (_modelStates.IsValid == false)
        {
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            var errors = new Dictionary<string, IEnumerable<string>>();
            foreach (var keyValue in _modelStates)
            {
                if (keyValue.Value.Errors != null && keyValue.Value.Errors.Count > 0)
                {
                    errors[keyValue.Key] = keyValue.Value.Errors.Select(e => e.ErrorMessage);
                }
            }

            response.Write(new JavaScriptSerializer().Serialize(new { success = false, errors = errors }));
        }
        else
        {
            response.Write(new JavaScriptSerializer().Serialize(new { success = true }));
        }
    }
}

namespace MvcFox.Controllers
{
    public class TestFormController : Controller
    {
        // GET: TestForm
        public ActionResult Index()
        {
            TestFormEditViewModel model = new TestFormEditViewModel();
            return View();
        }

        [HttpPost]
        public ActionResult Index(TestFormEditViewModel model)
        {
            try
            {
                ModelState.AddModelError("Name", "该姓名无法注册。");
                throw new Exception("发生异常，程序开小差了。");
            }
            catch (Exception ex)
            {
                // unexpected exception thrown.
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return new JsonFormResult(ModelState);
        }

        [HttpPost]
        public ActionResult RemoteValidTest(string name)
        {
            string[] invalidName = new string[] { "test", "admin" };
            return Json(!invalidName.Contains(name));
        }

        public ActionResult AjaxReturn()
        {
            return View();
        }
    }
}

namespace MvcFox.Models
{
    public class TestFormEditViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "姓名")]
        public string Name { get; set; }

    }
}