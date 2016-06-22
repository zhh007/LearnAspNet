using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Aspnet.Mvc.Extension
{
    /// <summary>
    /// 身份证验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IDCardAttribute : ValidationAttribute, IClientValidatable
    {
        public IDCardAttribute()
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

            return ValidateHelper.CheckIDCard(str);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule { ValidationType = "idcard", ErrorMessage = this.FormatErrorMessage(metadata.DisplayName) };
        }
    }
}
