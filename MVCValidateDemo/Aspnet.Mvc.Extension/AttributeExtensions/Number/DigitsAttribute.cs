using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Aspnet.Mvc.Extension
{
    /// <summary>
    /// 数字
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DigitsAttribute : ValidationAttribute, IClientValidatable
    {
        public DigitsAttribute()
        {
            ErrorMessage = "请输入正确的{0}！";
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            string str = value.ToString();
            if (string.IsNullOrEmpty(str))
                return true;

            return ValidateHelper.CheckDigits(str);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            //jquery.validate.js 自带验证
            yield return new ModelClientValidationRule { ValidationType = "digits", ErrorMessage = this.FormatErrorMessage(metadata.DisplayName) };
        }
    }
}
