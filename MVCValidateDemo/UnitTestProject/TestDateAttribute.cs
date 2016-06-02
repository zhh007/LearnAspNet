using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCValidateDemo.MVCExtension;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace UnitTestProject
{
    [TestClass]
    public class TestDateAttribute
    {
        [TestMethod]
        public void TestEQ1()
        {
            TestDateEQModel model = new TestDateEQModel();
            model.prop1 = DateTime.Now;
            model.prop2 = model.prop1;

            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestEQ2()
        {
            TestDateEQModel model = new TestDateEQModel();
            model.prop1 = DateTime.Now;
            model.prop2 = DateTime.Now.AddDays(+1);

            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestEQ3()
        {
            TestDateEQModel model = new TestDateEQModel();
            model.prop2 = model.prop1;

            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestEQ4()
        {
            TestDateEQModel model = new TestDateEQModel();
            model.prop1 = DateTime.Now;

            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestNE1()
        {
            TestDateNEModel model = new TestDateNEModel();
            model.prop1 = DateTime.Now;
            model.prop2 = model.prop1;

            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestNE2()
        {
            TestDateNEModel model = new TestDateNEModel();
            model.prop1 = DateTime.Now;
            model.prop2 = DateTime.Now.AddDays(+1);

            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestLT1()
        {
            TestDateLTModel model = new TestDateLTModel();
            model.prop1 = DateTime.Now;
            model.prop2 = model.prop1;

            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestLT2()
        {
            TestDateLTModel model = new TestDateLTModel();
            model.prop1 = DateTime.Now;
            model.prop2 = DateTime.Now.AddDays(+1);

            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestLT3()
        {
            TestDateLTModel model = new TestDateLTModel();
            model.prop1 = DateTime.Now.AddDays(+1);
            model.prop2 = DateTime.Now;

            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestLE1()
        {
            TestDateLEModel model = new TestDateLEModel();
            model.prop1 = DateTime.Now;
            model.prop2 = model.prop1;

            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestLE2()
        {
            TestDateLEModel model = new TestDateLEModel();
            model.prop1 = DateTime.Now;
            model.prop2 = DateTime.Now.AddDays(+1);

            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestLE3()
        {
            TestDateLEModel model = new TestDateLEModel();
            model.prop1 = DateTime.Now.AddDays(+1);
            model.prop2 = DateTime.Now;

            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestGT1()
        {
            TestDateGTModel model = new TestDateGTModel();
            model.prop1 = DateTime.Now;
            model.prop2 = model.prop1;

            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestGT2()
        {
            TestDateGTModel model = new TestDateGTModel();
            model.prop1 = DateTime.Now;
            model.prop2 = DateTime.Now.AddDays(+1);

            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestGT3()
        {
            TestDateGTModel model = new TestDateGTModel();
            model.prop1 = DateTime.Now.AddDays(+1);
            model.prop2 = DateTime.Now;

            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestGE1()
        {
            TestDateGEModel model = new TestDateGEModel();
            model.prop1 = DateTime.Now;
            model.prop2 = model.prop1;

            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestGE2()
        {
            TestDateGEModel model = new TestDateGEModel();
            model.prop1 = DateTime.Now;
            model.prop2 = DateTime.Now.AddDays(+1);

            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestGE3()
        {
            TestDateGEModel model = new TestDateGEModel();
            model.prop1 = DateTime.Now.AddDays(+1);
            model.prop2 = DateTime.Now;

            var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), null, true);
            Assert.IsTrue(result);
        }
    }

    public class TestDateEQModel
    {
        [DisplayName("日期1")]
        [Date(DataCompare.EQ, "prop2", "{0}必须等于{1}。")]
        public DateTime? prop1 { get; set; }

        [DisplayName("日期2")]
        public DateTime? prop2 { get; set; }
    }

    public class TestDateNEModel
    {
        [DisplayName("日期1")]
        [Date(DataCompare.NE, "prop2", "{0}不能等于{1}。")]
        public DateTime? prop1 { get; set; }

        [DisplayName("日期2")]
        public DateTime? prop2 { get; set; }
    }

    public class TestDateLTModel
    {
        [DisplayName("日期1")]
        [Date(DataCompare.LT, "prop2", "{0}必须小于{1}。")]
        public DateTime? prop1 { get; set; }

        [DisplayName("日期2")]
        public DateTime? prop2 { get; set; }
    }

    public class TestDateLEModel
    {
        [DisplayName("日期1")]
        [Date(DataCompare.LE, "prop2", "{0}必须小于等于{1}。")]
        public DateTime? prop1 { get; set; }

        [DisplayName("日期2")]
        public DateTime? prop2 { get; set; }
    }

    public class TestDateGTModel
    {
        [DisplayName("日期1")]
        [Date(DataCompare.GT, "prop2", "{0}必须大于{1}。")]
        public DateTime? prop1 { get; set; }

        [DisplayName("日期2")]
        public DateTime? prop2 { get; set; }
    }

    public class TestDateGEModel
    {
        [DisplayName("日期1")]
        [Date(DataCompare.GE, "prop2", "{0}必须大于等于{1}。")]
        public DateTime? prop1 { get; set; }

        [DisplayName("日期2")]
        public DateTime? prop2 { get; set; }
    }
}
