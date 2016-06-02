using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MVCValidateDemo.MVCExtension
{
    public class DateAttribute : ValidationAttribute, IClientValidatable
    {
        private string otherProperty;
        private string otherPropertyDisplayName;
        private DataCompare _operator;

        public DateAttribute(DataCompare op, string otherProperty, string errorMessage)
            : base(errorMessage)
        {
            if(string.IsNullOrEmpty(otherProperty) || string.IsNullOrWhiteSpace(otherProperty))
            {
                throw new ArgumentException("otherProperty参数不能为空。", otherProperty);
            }
            this._operator = op;
            this.otherProperty = otherProperty;
            this.ErrorMessage = errorMessage;
        }

        public override string FormatErrorMessage(string name)
        {
            if (string.IsNullOrEmpty(this.otherPropertyDisplayName))
            {
                this.otherPropertyDisplayName = this.otherProperty;
            }
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, this.otherPropertyDisplayName);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            DateTime d1;
            if (value is DateTime)
            {
                d1 = (DateTime)value;
            }
            else
            {
                string str1 = value.ToString();
                if (string.IsNullOrEmpty(str1))
                {
                    return ValidationResult.Success;
                }
                if (!DateTime.TryParse(str1, out d1))
                {
                    return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
                }
            }

            PropertyInfo property = validationContext.ObjectType.GetProperty(this.otherProperty);
            if (property == null)
            {
                return new ValidationResult(string.Format("属性{0}不存在。", this.otherProperty));
            }
            //if(!property.PropertyType.Equals(typeof(DateTime)))
            //{
            //    throw new ArgumentException("otherProperty参数不是DateTime类型。", otherProperty);
            //}
            this.otherPropertyDisplayName = GetDisplayNameForProperty(validationContext.ObjectType, this.otherProperty);

            object value2 = property.GetValue(validationContext.ObjectInstance, null);
            if (value2 == null)
            {
                return ValidationResult.Success;
            }
            DateTime d2;
            if (value2 is DateTime)
            {
                d2 = (DateTime)value2;
            }
            else
            {
                string str2 = value2.ToString();
                if (string.IsNullOrEmpty(str2))
                {
                    return ValidationResult.Success;
                }
                if (!DateTime.TryParse(str2, out d2))
                {
                    return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
                }
            }

            switch (this._operator)
            {
                case DataCompare.EQ:
                    if (d1 == d2)
                    {
                        return ValidationResult.Success;
                    }
                    break;
                case DataCompare.NE:
                    if (d1 != d2)
                    {
                        return ValidationResult.Success;
                    }
                    break;
                case DataCompare.LT:
                    if (d1 < d2)
                    {
                        return ValidationResult.Success;
                    }
                    break;
                case DataCompare.LE:
                    if (d1 <= d2)
                    {
                        return ValidationResult.Success;
                    }
                    break;
                case DataCompare.GT:
                    if (d1 > d2)
                    {
                        return ValidationResult.Success;
                    }
                    break;
                case DataCompare.GE:
                    if (d1 >= d2)
                    {
                        return ValidationResult.Success;
                    }
                    break;
                default:
                    break;
            }

            return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            if (string.IsNullOrEmpty(this.otherPropertyDisplayName))
            {
                this.otherPropertyDisplayName = GetDisplayNameForProperty(metadata.ContainerType, this.otherProperty);
            }

            string vtype = "";
            switch (this._operator)
            {
                case DataCompare.EQ:
                    vtype = "dateeq";
                    break;
                case DataCompare.NE:
                    vtype = "datene";
                    break;
                case DataCompare.LT:
                    vtype = "datelt";
                    break;
                case DataCompare.LE:
                    vtype = "datele";
                    break;
                case DataCompare.GT:
                    vtype = "dategt";
                    break;
                case DataCompare.GE:
                    vtype = "datege";
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(vtype))
            {
                var rule = new ModelClientValidationRule
                {
                    ValidationType = vtype,
                    ErrorMessage = this.FormatErrorMessage(metadata.DisplayName)
                };
                rule.ValidationParameters.Add("toid", this.otherProperty);
                yield return rule;
            }
        }

        private static ICustomTypeDescriptor GetTypeDescriptor(Type type)
        {
            return new AssociatedMetadataTypeTypeDescriptionProvider(type).GetTypeDescriptor(type);
        }

        private static string GetDisplayNameForProperty(Type containerType, string propertyName)
        {
            ICustomTypeDescriptor typeDescriptor = GetTypeDescriptor(containerType);
            PropertyDescriptor propertyDescriptor = typeDescriptor.GetProperties().Find(propertyName, true);
            if (propertyDescriptor == null)
            {
                throw new ArgumentException(string.Format("属性{0}不存在。", propertyName));
            }
            IEnumerable<Attribute> source = propertyDescriptor.Attributes.Cast<Attribute>();
            DisplayAttribute displayAttribute = source.OfType<DisplayAttribute>().FirstOrDefault<DisplayAttribute>();
            if (displayAttribute != null)
            {
                return displayAttribute.GetName();
            }
            DisplayNameAttribute displayNameAttribute = source.OfType<DisplayNameAttribute>().FirstOrDefault<DisplayNameAttribute>();
            if (displayNameAttribute != null)
            {
                return displayNameAttribute.DisplayName;
            }
            return propertyName;
        }
    }
    public enum DataCompare
    {
        /// <summary>
        /// Equal(等于)
        /// </summary>
        EQ,
        /// <summary>
        /// NotEqual(不等于)
        /// </summary>
        NE,
        /// <summary>
        /// Less than(小于)
        /// </summary>
        LT,
        /// <summary>
        /// Less than or equal(小于等于)
        /// </summary>
        LE,
        /// <summary>
        /// Greater than(大于)
        /// </summary>
        GT,
        /// <summary>
        /// Greater than or equal(大于等于)
        /// </summary>
        GE
    }
}