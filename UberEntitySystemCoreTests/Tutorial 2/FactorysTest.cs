using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberEntitySystemCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UberEntitySystemCoreTests.Tutorial_2
{

    [Factory(typeof(FactoryTestObj2Factory), typeof(FactoryTestObj2))]
    class FactoryTestObj2
    {
        public int currentID = 0;
        public string currentString = "";
    }
    class FactoryTestObjNoCleaner
    {
        public int currentID { get; set; }
        public string currentString { get; set; }
    }

    class FactoryTestObj2Factory : IObjectFactory<FactoryTestObj2>
    {
        public FactoryTestObj2 CleanForReuse(FactoryTestObj2 obj)
        {
            obj.currentID = 0;
            obj.currentString = "";
            return obj;
        }

        public FactoryTestObj2 CreateNew()
        {
            return new FactoryTestObj2();
        }
    }


    [TestClass()]
    public class FactoriesTests
    {
        [TestMethod()]
        public void InstantiationTest()
        {
            FactoryTestObj f1 = Factory.Get<FactoryTestObj>();
            FactoryTestObj f2 = Factory.Get<FactoryTestObj>();
            FactoryTestObj2 f3 = Factory.Get<FactoryTestObj2>();
            FactoryTestObj2 f4 = Factory.Get<FactoryTestObj2>();

            Assert.AreNotSame(f1, f2);
            Assert.IsNotNull(f1);
            Assert.IsNotNull(f2);

            Assert.AreEqual(0, f1.currentID);
            Assert.AreEqual(0, f2.currentID);
            Assert.AreEqual("", f1.currentString);
            Assert.AreEqual("", f2.currentString);
        }

        [TestMethod()]
        public void CleanupTest()
        {
            FactoryTestObj f1 = Factory.Get<FactoryTestObj>();
            FactoryTestObj f2 = Factory.Get<FactoryTestObj>();
            FactoryTestObj2 f3 = Factory.Get<FactoryTestObj2>();
            FactoryTestObj2 f4 = Factory.Get<FactoryTestObj2>();
            FactoryTestObjNoCleaner f5 = Factory.Get<FactoryTestObjNoCleaner>();
            FactoryTestObjNoCleaner f6 = Factory.Get<FactoryTestObjNoCleaner>();

            Assert.AreNotSame(f1, f2);
            Assert.AreNotSame(f3, f4);
            Assert.AreNotSame(f5, f6);
            Assert.IsNotNull(f1);
            Assert.IsNotNull(f2);
            Assert.IsNotNull(f3);
            Assert.IsNotNull(f4);
            Assert.IsNotNull(f5);
            Assert.IsNotNull(f6);

            f1.currentID = 78;
            f2.currentID = 732;
            f3.currentID = 267;
            f4.currentID = 13;
            f5.currentID = 42;
            f6.currentID = 165;

            f1.currentString = "test1";
            f2.currentString = "test2";
            f3.currentString = "test3";
            f4.currentString = "test4";
            f5.currentString = "test5";
            f6.currentString = "test6";

            Assert.AreEqual(78, f1.currentID);
            Assert.AreEqual(732, f2.currentID);
            Assert.AreEqual(267, f3.currentID);
            Assert.AreEqual(13, f4.currentID);
            Assert.AreEqual(42, f5.currentID);
            Assert.AreEqual(165, f6.currentID);

            Assert.AreEqual("test1", f1.currentString);
            Assert.AreEqual("test2", f2.currentString);
            Assert.AreEqual("test3", f3.currentString);
            Assert.AreEqual("test4", f4.currentString);
            Assert.AreEqual("test5", f5.currentString);
            Assert.AreEqual("test6", f6.currentString);

            FactoryTestObj f7 = Factory.Clean<FactoryTestObj>(f1);
            FactoryTestObj f8 = Factory.Clean<FactoryTestObj>(f2);
            FactoryTestObj2 f9 = Factory.Clean<FactoryTestObj2>(f3);
            FactoryTestObj2 f10 = Factory.Clean<FactoryTestObj2>(f4);
            FactoryTestObjNoCleaner f11 = Factory.Clean<FactoryTestObjNoCleaner>(f5);
            FactoryTestObjNoCleaner f12 = Factory.Clean<FactoryTestObjNoCleaner>(f6);

            Assert.AreSame(f1, f7);
            Assert.AreSame(f2, f8);
            Assert.AreSame(f3, f9);
            Assert.AreSame(f4, f10);
            Assert.AreSame(f5, f11);
            Assert.AreSame(f6, f12);

            Assert.AreEqual(0, f1.currentID);
            Assert.AreEqual(0, f2.currentID);
            Assert.AreEqual(0, f3.currentID);
            Assert.AreEqual(0, f4.currentID);
            Assert.AreEqual(42, f5.currentID);
            Assert.AreEqual(165, f6.currentID);
            Assert.AreEqual("", f1.currentString);
            Assert.AreEqual("", f2.currentString);
            Assert.AreEqual("", f3.currentString);
            Assert.AreEqual("", f4.currentString);
            Assert.AreEqual("test5", f5.currentString);
            Assert.AreEqual("test6", f6.currentString);
        }
    }
}
