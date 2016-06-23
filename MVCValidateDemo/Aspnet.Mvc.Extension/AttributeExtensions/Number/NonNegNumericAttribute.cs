using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Aspnet.Mvc.Extension
{
    /// <summary>
    /// 非负数（正整数，正小数，零）
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NonNegNumericAttribute : ValidationAttribute, IClientValidatable
    {
        public NonNegNumericAttribute()
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

            return ValidateHelper.CheckNonNegNumeric(str);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule { ValidationType = "nonnegnumeric", ErrorMessage = this.FormatErrorMessage(metadata.DisplayName) };
        }
    }
}
