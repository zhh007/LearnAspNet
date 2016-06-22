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
        private readonly static Regex RegNumeric = new Regex(@"^[+-]?(([1-9]\d*)|0)(.[0-9]+)?$");

        public static bool CheckZipcode(string str)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                return false;
            return RegZipcode.IsMatch(str);
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


    }
}
