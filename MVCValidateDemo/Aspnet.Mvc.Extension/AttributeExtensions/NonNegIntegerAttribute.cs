using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Aspnet.Mvc.Extension.AttributeExtensions
{
    /// <summary>
    /// 非负整数
    /// </summary>
    public class NonNegIntegerAttribute : ValidationAttribute, IClientValidatable
    {
        public NonNegIntegerAttribute()
        {
            ErrorMessage = "请输入正确的{0}！";
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            if (string.IsNullOrEmpty(value.ToString())) return true;

            string idCardReg = @"^\d+*$";
            if (!Regex.IsMatch(value.ToString(), idCardReg))
            {
                return false;
            }

            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule { ValidationType = "nonnegint", ErrorMessage = this.FormatErrorMessage(metadata.DisplayName) };
        }
    }
}

