using Aspnet.Mvc.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCValidateDemo.Models
{
    public class Student
    {
        public int ID { get; set; }
        [Required]
        [DisplayName("学生姓名")]
        public string Name { get; set; }

        [Required]
        [DisplayName("年龄")]
        public int Age { get; set; }

        [ZipCode(ErrorMessage = "请输入正确的{0}！")]
        [DisplayName("收件人邮编")]
        public string ZipCode { get; set; }

        [DisplayName("生日")]
        [Date(DataCompare.LT, "BaomingTime", "{0}必须小于{1}。")]
        public DateTime Birthday { get; set; }

        [DisplayName("报名时间")]
        public DateTime BaomingTime { get; set; }

        [DisplayName("个人简介")]
        public string Content { get; set; }
    }
}