using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Aspnet.Mvc.Extension
{
    /// <summary>
    /// 整数（正整数，负整数，零）
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IntegerAttribute : ValidationAttribute, IClientValidatable
    {
        public IntegerAttribute()
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

            return ValidateHelper.CheckInteger(str);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule { ValidationType = "integer", ErrorMessage = this.FormatErrorMessage(metadata.DisplayName) };
        }
    }
}
