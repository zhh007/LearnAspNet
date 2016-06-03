using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCValidateDemo.MVCExtension
{
    public class ValidateResult
    {
        public string MemberName { get; set; }

        public string Message { get; set; }

        public ValidateResult(string memeberName, string message)
        {
            this.MemberName = memeberName;
            this.Message = message;
        }

        public ValidateResult(string message)
        {
            this.Message = message;
        }
    }

    public static class ControllerExtensions
    {
        public static void AddModelErrors(this Controller controller, IEnumerable<ValidateResult> validationResults, string defaultErrorKey = null)
        {
            if (validationResults != null)
            {
                foreach (ValidateResult validationResult in validationResults)
                {
                    if (!string.IsNullOrEmpty(validationResult.MemberName))
                    {
                        controller.ModelState.AddModelError(validationResult.MemberName, validationResult.Message);
                    }
                    else if (defaultErrorKey != null)
                    {
                        controller.ModelState.AddModelError(defaultErrorKey, validationResult.Message);
                    }
                    else
                    {
                        controller.ModelState.AddModelError(string.Empty, validationResult.Message);
                    }
                }
            }
        }

        public static void AddModelErrors(this ModelStateDictionary modelState, IEnumerable<ValidateResult> validationResults, string defaultErrorKey = null)
        {
            if (validationResults != null)
            {
                foreach (ValidateResult validationResult in validationResults)
                {
                    string key = validationResult.MemberName ?? (defaultErrorKey ?? string.Empty);
                    modelState.AddModelError(key, validationResult.Message);
                }
            }
        }
    }
}