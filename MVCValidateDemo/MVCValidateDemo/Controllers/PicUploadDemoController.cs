﻿using Aspnet.Mvc.Extension;
using MVCValidateDemo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCValidateDemo.Controllers
{
    public class PicUploadDemoController : Controller
    {
        // GET: PicUploadDemo
        public ActionResult Index()
        {
            PicUploadDemoModel model = new PicUploadDemoModel();
            model.PhotoFolderId = Guid.NewGuid();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(PicUploadDemoModel model)
        {
            var photos = PicHelper.Build(Request, model.PhotoFolderId, "Gain");
            model.Photos = photos;
            foreach (var item in model.Photos)
            {
                item.ThumbPath = PicHelper.GetUrl(item.ThumbPath);
            }
            return View(model);
        }

        public ActionResult Index2()
        {
            PicUploadDemoViewModel model = new PicUploadDemoViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index2(PicUploadDemoViewModel model)
        {
            return View(model);
        }
    }

    public class PicUploadDemoViewModel
    {
        public string Name { get; set; }

        [Display(Name = "图片")]
        [Required]
        //[FileUploadValidate(MaxFilesCount=2)]
        //[FileUploadValidate(ConfigName = "testconfig")]
        public PicUploadModel Files { get; set; }

        public PicUploadDemoViewModel()
        {
            Files = new PicUploadModel();
        }
    }
}