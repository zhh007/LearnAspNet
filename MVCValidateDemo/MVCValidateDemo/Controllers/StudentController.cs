using MVCValidateDemo.Models;
using MVCValidateDemo.MVCExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCValidateDemo.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Create2()
        {
            return View();
        }

        public ActionResult Create3()
        {
            return View();
        }

        public ActionResult Create4()
        {
            return View();
        }

        public ActionResult Create5()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Student stu)
        {
            IEnumerable<ValidateResult> resultList = CanAdd(stu);
            ModelState.AddModelErrors(resultList);

            if(User.Identity.IsAuthenticated)
            {
                ModelState.Remove("Name");
            }

            if (ModelState.IsValid)
            {
                //save to db
            }
            return View();
        }

        public IEnumerable<ValidateResult> CanAdd(Student dto)
        {
            if (dto.Name == "NULL")
            {
                yield return new ValidateResult("Name", "姓名不能为NULL。");
            }
        }
    }
}