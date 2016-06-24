using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspnet.Mvc.Extension
{
    public class ChinaIDCard
    {
        /// <summary>
        /// 验证位身份证
        /// </summary>
        /// <param name="Id">身份证号码</param>
        /// <returns>是否真实身份证</returns>
        public static bool Validate(string Id)
        {
            int intLen = Id.Length;
            long n = 0;

            if (intLen == 18)
            {
                if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
                {
                    return false;//数字验证
                }
                string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
                if (address.IndexOf(Id.Remove(2)) == -1)
                {
                    return false;//省份验证
                }
                string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
                DateTime time = new DateTime();
                if (DateTime.TryParse(birth, out time) == false)
                {
                    return false;//生日验证
                }
                string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
                string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
                char[] Ai = Id.Remove(17).ToCharArray();
                int sum = 0;
                for (int i = 0; i < 17; i++)
                {
                    sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
                }
                int y = -1;
                Math.DivRem(sum, 11, out y);
                if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
                {
                    return false;//校验码验证
                }
                return true;//符合GB11643-1999标准
            }
            else if (intLen == 15)
            {
                if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
                {
                    return false;//数字验证
                }
                string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
                if (address.IndexOf(Id.Remove(2)) == -1)
                {
                    return false;//省份验证
                }
                string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
                DateTime time = new DateTime();
                if (DateTime.TryParse(birth, out time) == false)
                {
                    return false;//生日验证
                }
                return true;//符合15位身份证标准
            }
            else
            {
                return false;//位数不对
            }
        }

        public static DateTime? GetBirthday(string Id)
        {
            if (!Validate(Id))
                return null;

            DateTime? birthday = null;

            int intLen = Id.Length;
            string bstr = "";
            if(intLen == 18)
            {
                bstr = Id.Substring(6, 4) + "-" + Id.Substring(10, 2) + "-" + Id.Substring(12, 2);
            }
            else if(intLen == 15)
            {
                bstr = "19" + Id.Substring(6, 2) + "-" + Id.Substring(8, 2) + "-" + Id.Substring(10, 2);
            }

            if(!string.IsNullOrEmpty(bstr))
            {
                birthday = Convert.ToDateTime(bstr);
            }

            return birthday;
        }
    }
}
