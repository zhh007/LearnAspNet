using Aspnet.Mvc.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCValidateDemo.Controllers
{
    public class FileUploadDemoController : Controller
    {
        // GET: FileUploadDemo
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FileUploadTestViewModel2 model)
        {
            //UpdateModel(model);
            //model.FileUploadComponentModel
            //FileUploadController.Save();

            return View(model);
        }
    }

    public class FileUploadTestViewModel2
    {
        public string Name { get; set; }

        [Display(Name = "附件")]
        [Required]
        //[FileUploadValidate(MaxFilesCount=2)]
        [FileUploadValidate(ConfigName = "testconfig")]
        public FileUploadComponentModel Files { get; set; }

        public FileUploadTestViewModel2()
        {
            Files = new FileUploadComponentModel();
        }
    }
}