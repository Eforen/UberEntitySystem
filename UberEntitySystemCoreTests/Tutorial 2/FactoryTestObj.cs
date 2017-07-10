using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberEntitySystemCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UberEntitySystemCoreTests.Tutorial_2
{
    [Factory(typeof(FactoryTestObjFactory), typeof(FactoryTestObj))]
    class FactoryTestObj
    {
        public int currentID = 0;
        public string currentString = "";
    }

    class FactoryTestObjFactory : ObjectFactory<FactoryTestObj>
    {
        public override FactoryTestObj CleanForReuse(FactoryTestObj obj)
        {
            obj.currentID = 0;
            obj.currentString = "";
            return obj;
        }

        public override FactoryTestObj CreateNew()
        {
            return new FactoryTestObj();
        }
    }


    [TestClass()]
    public class FactoryTests
    {
        [TestMethod()]
        public void InstantiationTest()
        {
            FactoryTestObjFactory fact = new FactoryTestObjFactory();
            FactoryTestObj f1 = fact.CreateNew();
            FactoryTestObj f2 = fact.CreateNew();

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
            FactoryTestObjFactory fact = new FactoryTestObjFactory();
            FactoryTestObj f1 = fact.CreateNew();
            FactoryTestObj f2 = fact.CreateNew();

            Assert.AreNotSame(f1, f2);
            Assert.IsNotNull(f1);
            Assert.IsNotNull(f2);

            f1.currentID = 78;
            f2.currentID = 732;

            f1.currentString = "test1";
            f2.currentString = "test2";

            Assert.AreEqual(78, f1.currentID);
            Assert.AreEqual(732, f2.currentID);

            Assert.AreEqual("test1", f1.currentString);
            Assert.AreEqual("test2", f2.currentString);

            FactoryTestObj f3 = fact.CleanForReuse(f1);
            FactoryTestObj f4 = fact.CleanForReuse(f2);

            Assert.AreSame(f1, f3);
            Assert.AreSame(f2, f4);

            Assert.AreEqual(0, f1.currentID);
            Assert.AreEqual(0, f2.currentID);
            Assert.AreEqual("", f1.currentString);
            Assert.AreEqual("", f2.currentString);
        }
    }
}
