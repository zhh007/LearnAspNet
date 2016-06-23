using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Aspnet.Mvc.Extension
{
    /// <summary>
    /// 正小数
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PosDecimalAttribute : ValidationAttribute, IClientValidatable
    {
        public PosDecimalAttribute()
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

            return ValidateHelper.CheckPosDecimal(str);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule { ValidationType = "posdecimal", ErrorMessage = this.FormatErrorMessage(metadata.DisplayName) };
        }
    }
}
