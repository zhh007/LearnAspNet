using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Aspnet.Mvc.Extension
{
    public class MobileAttribute : ValidationAttribute, IClientValidatable
    {
        public MobileAttribute()
        {
            ErrorMessage = "{0}输入错误，格式为+861xxxxxxxxxx，其中\"+86\"为可选，x表示数字！";
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            if (string.IsNullOrEmpty(value.ToString())) return true;

            string idCardReg = @"^((\+86)|(86))?(1)\d{10}$";
            if (!Regex.IsMatch(value.ToString(), idCardReg))
            {
                return false;
            }

            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule { ValidationType = "mobile", ErrorMessage = this.FormatErrorMessage(metadata.DisplayName) };
        }
    }
}
