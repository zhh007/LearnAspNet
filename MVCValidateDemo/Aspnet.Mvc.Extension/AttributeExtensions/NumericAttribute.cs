using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Aspnet.Mvc.Extension.AttributeExtensions
{
    /// <summary>
    /// 数字验证
    /// </summary>
    public class NumericAttribute : ValidationAttribute, IClientValidatable
    {
        public NumericAttribute()
        {
            ErrorMessage = "请输入正确的{0}！";
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            if (string.IsNullOrEmpty(value.ToString())) return true;

            string idCardReg = @"^[+-]?(([1-9]\d*)|0)(.[0-9]+)?$";
            if (!Regex.IsMatch(value.ToString(), idCardReg))
            {
                return false;
            }

            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule { ValidationType = "numeric", ErrorMessage = this.FormatErrorMessage(metadata.DisplayName) };
        }
    }
}
