using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Aspnet.Mvc.Extension
{
    /// <summary>
    /// 手机格式验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MobileAttribute : ValidationAttribute, IClientValidatable
    {
        public MobileAttribute()
        {
            ErrorMessage = "{0}输入错误，格式为+861xxxxxxxxxx，其中\"+86\"为可选，x表示数字！";
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            string str = value.ToString();
            if (string.IsNullOrEmpty(str))
                return true;

            return ValidateHelper.CheckMobile(str);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule { ValidationType = "mobile", ErrorMessage = this.FormatErrorMessage(metadata.DisplayName) };
        }
    }
}
