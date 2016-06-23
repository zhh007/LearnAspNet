using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Aspnet.Mvc.Extension
{
    /// <summary>
    /// 非正数（负整数，负小数，零）
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NonPosNumericAttribute : ValidationAttribute, IClientValidatable
    {
        public NonPosNumericAttribute()
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

            return ValidateHelper.CheckNonPosNumeric(str);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule { ValidationType = "nonposnumeric", ErrorMessage = this.FormatErrorMessage(metadata.DisplayName) };
        }
    }
}
