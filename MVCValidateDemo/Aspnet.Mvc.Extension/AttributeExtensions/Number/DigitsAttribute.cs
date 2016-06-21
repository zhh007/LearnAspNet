using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
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
            if (value == null) return true;
            if (string.IsNullOrEmpty(value.ToString())) return true;

            string idCardReg = @"^\d+$";
            if (!Regex.IsMatch(value.ToString(), idCardReg))
            {
                return false;
            }

            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            //jquery.validate.js 自带验证
            yield return new ModelClientValidationRule { ValidationType = "digits", ErrorMessage = this.FormatErrorMessage(metadata.DisplayName) };
        }
    }
}
