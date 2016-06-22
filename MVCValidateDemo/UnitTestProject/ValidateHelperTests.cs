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
        #region Zipcode
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
        #endregion

        #region Mobile

        [TestMethod()]
        public void CheckMobileTest()
        {
            var r = ValidateHelper.CheckMobile("13745612358");
            Assert.IsTrue(r);
        }

        [TestMethod()]
        public void CheckMobileTest2()
        {
            var r = ValidateHelper.CheckMobile("8613745612358");
            Assert.IsTrue(r);
        }

        [TestMethod()]
        public void CheckMobileTest3()
        {
            var r = ValidateHelper.CheckMobile("+8613745612358");
            Assert.IsTrue(r);
        }

        [TestMethod()]
        public void CheckMobileTest4()
        {
            var r = ValidateHelper.CheckMobile("23745612358");
            Assert.IsFalse(r);
        }

        [TestMethod()]
        public void CheckMobileTest5()
        {
            var r = ValidateHelper.CheckMobile("137456123589");
            Assert.IsFalse(r);
        }

        [TestMethod()]
        public void CheckMobileTest6()
        {
            var r = ValidateHelper.CheckMobile("1374561235");
            Assert.IsFalse(r);
        }
        #endregion

        #region IDCard

        [TestMethod()]
        public void CheckIDCardTest()
        {
            var r = ValidateHelper.CheckIDCard("36042419781114235X");
            Assert.IsTrue(r);
        }

        [TestMethod()]
        public void CheckIDCardTest2()
        {
            var r = ValidateHelper.CheckIDCard("441723199102154913");
            Assert.IsTrue(r);
        }

        [TestMethod()]
        public void CheckIDCardTest3()
        {
            var r = ValidateHelper.CheckIDCard("36042419781114235X0");
            Assert.IsFalse(r);
        }

        [TestMethod()]
        public void CheckIDCardTest4()
        {
            var r = ValidateHelper.CheckIDCard("44172319910215491");
            Assert.IsFalse(r);
        }
        #endregion

        [TestMethod()]
        public void CheckNumericTest()
        {
            var r = ValidateHelper.CheckNumeric("123");
            Assert.IsTrue(r);
        }

        [TestMethod()]
        public void CheckNumericTest2()
        {
            var r = ValidateHelper.CheckNumeric("12.3");
            Assert.IsTrue(r);
        }

        [TestMethod()]
        public void CheckNumericTest3()
        {
            var r = ValidateHelper.CheckNumeric("0");
            Assert.IsTrue(r);
        }

        [TestMethod()]
        public void CheckNumericTest4()
        {
            var r = ValidateHelper.CheckNumeric("+123");
            Assert.IsTrue(r);
        }

        [TestMethod()]
        public void CheckNumericTest5()
        {
            var r = ValidateHelper.CheckNumeric("+12.3");
            Assert.IsTrue(r);
        }

        [TestMethod()]
        public void CheckNumericTest6()
        {
            var r = ValidateHelper.CheckNumeric("-123");
            Assert.IsTrue(r);
        }

        [TestMethod()]
        public void CheckNumericTest7()
        {
            var r = ValidateHelper.CheckNumeric("-12.3");
            Assert.IsTrue(r);
        }

        [TestMethod()]
        public void CheckNumericTest8()
        {
            var r = ValidateHelper.CheckNumeric("1.2.3");
            Assert.IsFalse(r);
        }

        [TestMethod()]
        public void CheckNumericTest9()
        {
            var r = ValidateHelper.CheckNumeric("001354566");
            Assert.IsFalse(r);
        }

        [TestMethod()]
        public void CheckNumericTest10()
        {
            var r = ValidateHelper.CheckNumeric("00");
            Assert.IsFalse(r);
        }

        [TestMethod()]
        public void CheckNumericTest11()
        {
            var r = ValidateHelper.CheckNumeric("00.00");
            Assert.IsFalse(r);
        }

        [TestMethod()]
        public void CheckNumericTest12()
        {
            var r = ValidateHelper.CheckNumeric("0.00100");
            Assert.IsTrue(r);
        }
    }
}