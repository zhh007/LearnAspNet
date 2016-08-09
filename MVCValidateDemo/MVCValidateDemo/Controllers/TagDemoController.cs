using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCValidateDemo.Controllers
{
    public class TagDemoController : Controller
    {
        // GET: TagDemo
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(TagDemoViewModel model)
        {
            return View();
        }

        public ActionResult GetRecommendTags()
        {
            List<string> tags = new List<string>() { "标签1", "标签2", "标签3", "标签4", "标签5", "标签6", "标签7", "标签8", "标签9", "标签10" };

            return Json(new { success = true, tags = tags }, "text/html");
        }

        public ActionResult GetTags(string term)
        {
            List<string> tags = new List<string>() { "标签1", "标签2", "标签3", "标签4", "标签5", "标签6", "标签7", "标签8", "标签9", "标签10" };

            var list = (from p in tags
                        select new { id = p, label = p, value = p });

            if (list != null)
            {
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            return Json(new string[] { }, JsonRequestBehavior.AllowGet);
        }
    }

    public class TagDemoViewModel
    {
        public List<string> Tags { get; set; }

        public TagDemoViewModel()
        {
            //不用初始化也行
            //this.Tags = new List<string>();
        }
    }
}