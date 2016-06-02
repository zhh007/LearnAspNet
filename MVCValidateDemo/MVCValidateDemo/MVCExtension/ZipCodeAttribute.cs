using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MVCValidateDemo.MVCExtension
{
    public class ZipCodeAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly static Regex ZipcodeReg = new Regex("^[1-9][0-9]{5}$");

        //public ZipCodeAttribute()
        //{
        //    base.ErrorMessage = "请输入正确的{0}！";
        //}

        //public override string FormatErrorMessage(string name)
        //{
        //    return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
        //}

        public override bool IsValid(object value)
        {
            bool result = true;
            if (value == null)
            {
                result = true;
            }
            else if (string.IsNullOrEmpty(value.ToString()))
            {
                result = true;
            }
            else
            {
                result = ZipcodeReg.IsMatch(value.ToString());
            }
            return result;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ValidationType = "zipcode",
                ErrorMessage = this.FormatErrorMessage(metadata.DisplayName)
            };
        }
    }
}