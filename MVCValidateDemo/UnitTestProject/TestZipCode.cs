using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCValidateDemo.MVCExtension;
using System.ComponentModel.DataAnnotations;

namespace UnitTestProject
{
    [TestClass]
    public class TestZipCode
    {
        [TestMethod]
        public void TestMethod1()
        {
            StudentForTest model = new StudentForTest();
            model.ZipCode = null;
            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestMethod2()
        {
            StudentForTest model = new StudentForTest();
            model.ZipCode = string.Empty;
            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestMethod3()
        {
            StudentForTest model = new StudentForTest();
            model.ZipCode = "310000";
            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestMethod4()
        {
            StudentForTest model = new StudentForTest();
            model.ZipCode = "010000";
            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestMethod5()
        {
            StudentForTest model = new StudentForTest();
            model.ZipCode = "31000";
            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestMethod6()
        {
            StudentForTest model = new StudentForTest();
            model.ZipCode = "3100011";
            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsFalse(result);
        }
    }

    public class StudentForTest
    {
        [ZipCode]
        public string ZipCode { get; set; }
    }
}
