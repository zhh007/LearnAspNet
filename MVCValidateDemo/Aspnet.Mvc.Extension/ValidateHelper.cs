using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aspnet.Mvc.Extension
{
    public class ValidateHelper
    {
        private readonly static Regex RegZipcode = new Regex(@"^[1-9][0-9]{5}$");
        private readonly static Regex RegMobile = new Regex(@"^((\+86)|(86))?(1)\d{10}$");
        private readonly static Regex RegIDCard = new Regex(@"^[1-9]([0-9]{16}|[0-9]{13})[xX0-9]$");
        private readonly static Regex RegNumeric = new Regex(@"^[+-]?(([1-9]\d*)|0)(\.\d*[1-9]\d*)?$");
        private readonly static Regex RegNegNumeric = new Regex(@"^(-[1-9]*(\.\d*[1-9]\d*)?)$|^(-0\.\d*[1-9]\d*)$");
        private readonly static Regex RegNonNegNumeric = new Regex(@"^[+]?(([1-9]\d*)|0)(\.\d*[1-9]\d*)?$");

        /// <summary>
        /// 邮政编码
        /// </summary>
        public static bool CheckZipcode(string str)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                return false;
            return RegZipcode.IsMatch(str);
        }

        /// <summary>
        /// 手机号码
        /// </summary>
        public static bool CheckMobile(string str)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                return false;
            return RegMobile.IsMatch(str);
        }

        /// <summary>
        /// 身份证
        /// </summary>
        public static bool CheckIDCard(string str)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                return false;
            return RegIDCard.IsMatch(str);
        }

        /// <summary>
        /// 有效的数字（整数，小数，零）
        /// </summary>
        public static bool CheckNumeric(string str)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                return false;
            return RegNumeric.IsMatch(str);
        }

        /// <summary>
        /// 负数（负整数，负小数）
        /// </summary>
        public static bool CheckNegNumeric(string str)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                return false;
            return RegNegNumeric.IsMatch(str);
        }

        /// <summary>
        /// 非负数（正整数，正小数，零）
        /// </summary>
        public static bool CheckNonNegNumeric(string str)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                return false;
            return RegNonNegNumeric.IsMatch(str);
        }
    }
}
