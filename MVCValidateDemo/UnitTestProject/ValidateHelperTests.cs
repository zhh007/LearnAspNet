using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aspnet.Mvc.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspnet.Mvc.Extension.Tests
{
    [TestClass()]
    public class ValidateHelperTests
    {
        public void Run(Func<string, bool> func, List<string> successList, List<string> failList)
        {
            var fails = new List<string>();
            foreach (var item in successList)
            {
                if (func(item) == false)
                {
                    fails.Add(item);
                }
            }

            foreach (var item in failList)
            {
                if (func(item) == true)
                {
                    fails.Add(item);
                }
            }

            if (fails.Count > 0)
            {
                Assert.Fail("测试失败：" + string.Join(", ", fails.ToArray()));
            }
        }

        [TestMethod()]
        public void CheckZipcodeTest()
        {
            Run(p => ValidateHelper.CheckZipcode(p),
                new List<string> {
                    "310000"
                },
                new List<string> {
                    "12345", "1234567"
                });
        }

        [TestMethod()]
        public void CheckMobileTest()
        {
            Run(p => ValidateHelper.CheckMobile(p),
                new List<string> {
                "13745612358","8613745612358","+8613745612358"
                },
                new List<string> {
                "23745612358", "137456123589", "1374561235"
                });
        }

        [TestMethod()]
        public void CheckIDCardTest()
        {
            Run(p => ValidateHelper.CheckIDCard(p),
                new List<string> {
                "36042419781114235X","441723199102154913"
                },
                new List<string> {
                "36042419781114235X0", "44172319910215491"
                });
        }

        [TestMethod()]
        public void CheckNumericTest()
        {
            Run(p => ValidateHelper.CheckNumeric(p),
                new List<string> {
                    "123", "12.3", "0", "+123", "+12.3", "-123", "-12.3", "0.01", "0.00100", "1.00", "+1.00", "-1.00"
                },
                new List<string> {
                    "1.2.3", "001354566", "00", "00.00"
                });
        }

        [TestMethod()]
        public void CheckNegNumericTest()
        {
            Run(p => ValidateHelper.CheckNegNumeric(p),
                new List<string> {
                    "-123", "-12.3", "-0.01", "-0.00100", "-1.00"
                },
                new List<string> {
                    "-1.2.3", "-001354566", "-00", "0", "-00.00", "-0.00", "123", "1.23", "+123", "+1.23"
                });
        }

        [TestMethod()]
        public void CheckNonNegNumericTest()
        {
            Run(p => ValidateHelper.CheckNonNegNumeric(p),
                new List<string> {
                    "123", "12.3", "0", "+123", "+12.3", "0.01", "0.00100", "1.00", "+1.00"
                },
                new List<string> {
                    "-1.2.3", "-001354566", "-00", "-00.00", "-123", "-1.23", "-1.00"
                });
        }

        [TestMethod()]
        public void CheckPosNumericTest()
        {
            Run(p => ValidateHelper.CheckPosNumeric(p),
                new List<string> {
                    "123000", "12.3", "0.01", "0.00100", "1.00", "+123000", "+12.3", "+0.01", "+0.00100", "+1.00"
                },
                new List<string> {
                    "1.2.3", "001354566", "00", "00.00", "0", "0.00000", "-123000", "-12.3", "-0.01", "-0.00100", "-1.00"
                });
        }

        [TestMethod()]
        public void CheckNonPosNumericTest()
        {
            Run(p => ValidateHelper.CheckNonPosNumeric(p),
                new List<string> {
                    "-123", "-12.3", "0", "-0.01", "-0.00100", "-1.00"
                },
                new List<string> {
                    "-1.2.3", "-001354566", "-00", "-00.00", "123", "1.23", "1.00", "+1.00"
                });
        }

        [TestMethod()]
        public void CheckIntegerTest()
        {
            Run(p => ValidateHelper.CheckInteger(p),
                new List<string> {
                    "12300", "0", "+12300", "-12300"
                },
                new List<string> {
                    "1.2.3", "001", "00", "-1.0", "+1.0", "+0", "-0"
                });
        }

        [TestMethod()]
        public void CheckPosIntegerTest()
        {
            Run(p => ValidateHelper.CheckPosInteger(p),
                new List<string> {
                    "12300", "+12300"
                },
                new List<string> {
                    "1.2.3", "001", "00", "-1.0", "+1.0", "+0", "-0", "0", "-12300"
                });
        }

        [TestMethod()]
        public void CheckNegIntegerTest()
        {
            Run(p => ValidateHelper.CheckNegInteger(p),
                new List<string> {
                    "-12300"
                },
                new List<string> {
                    "1.2.3", "001", "00", "-1.0", "+1.0", "+0", "-0", "12300", "0", "+12300"
                });
        }

        [TestMethod()]
        public void CheckNonPosIntegerTest()
        {
            Run(p => ValidateHelper.CheckNonPosInteger(p),
                new List<string> {
                    "0", "-12300"
                },
                new List<string> {
                    "1.2.3", "001", "00", "-1.0", "+1.0", "+0", "-0", "12300", "+12300"
                });
        }

        [TestMethod()]
        public void CheckNonNegIntegerTest()
        {
            Run(p => ValidateHelper.CheckNonNegInteger(p),
                new List<string> {
                    "12300", "0", "+12300"
                },
                new List<string> {
                    "1.2.3", "001", "00", "-1.0", "+1.0", "+0", "-0", "-12300"
                });
        }

        [TestMethod()]
        public void CheckDecimalTest()
        {
            Run(p => ValidateHelper.CheckDecimal(p),
                new List<string> {
                    "1.00", "0.00100", "+1.00", "+0.00100", "-1.00", "-0.00100"
                },
                new List<string> {
                    "1.2.3", "001", "00", "12", "0", "+0", "-0", "-12300"
                });
        }

        [TestMethod()]
        public void CheckPosDecimalTest()
        {
            Run(p => ValidateHelper.CheckPosDecimal(p),
                new List<string> {
                    "1.00", "0.00100", "+1.00", "+0.00100"
                },
                new List<string> {
                    "1.2.3", "001", "00", "12", "0", "+0", "-0", "-12300", "-1.00", "-0.00100"
                });
        }

        [TestMethod()]
        public void CheckNegDecimalTest()
        {
            Run(p => ValidateHelper.CheckNegDecimal(p),
                new List<string> {
                    "-1.00", "-0.00100"
                },
                new List<string> {
                    "1.2.3", "001", "00", "12", "0", "+0", "-0", "-12300", "1.00", "0.00100", "+1.00", "+0.00100"
                });
        }
    }
}