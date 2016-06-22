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
        [TestMethod()]
        public void CheckZipcodeTest()
        {
            var r = ValidateHelper.CheckZipcode("310000");
            Assert.IsTrue(r);
        }

        [TestMethod()]
        public void CheckZipcodeTest2()
        {
            var r = ValidateHelper.CheckZipcode("12345");
            Assert.IsFalse(r);
        }

        [TestMethod()]
        public void CheckZipcodeTest3()
        {
            var r = ValidateHelper.CheckZipcode("1234567");
            Assert.IsFalse(r);
        }
    }
}