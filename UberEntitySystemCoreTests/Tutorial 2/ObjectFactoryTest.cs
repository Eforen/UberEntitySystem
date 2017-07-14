using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberEntitySystemCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UberEntitySystemCoreTests.Tutorial_2
{
    using NUnit.Framework;
    class FactoryTestObjTesterCleanForReuseCalled : Exception { }
    class FactoryTestObjTesterCreateNewCalled : Exception { }

    [Factory(typeof(FactoryTestObjTesterFactory), typeof(FactoryTestObjTester))]
    class FactoryTestObjTester
    {
    }

    class FactoryTestObjTesterFactory : ObjectFactory<FactoryTestObjTester>
    {
        public override FactoryTestObjTester CleanForReuse(FactoryTestObjTester obj)
        {
            throw new FactoryTestObjTesterCleanForReuseCalled(); //Prove the method was called
        }

        public override FactoryTestObjTester CreateNew()
        {
            throw new FactoryTestObjTesterCreateNewCalled(); //Prove the method was called
        }
    }


    [TestClass()]
    public class ObjectFactoryTests
    {
        [Test]
        public void InstantiationTest()
        {
            FactoryTestObjTesterFactory fact = new FactoryTestObjTesterFactory();
            Assert.IsInstanceOf<ObjectFactory<FactoryTestObjTester>>(fact, "Factory Test not of type ObjectFactory");
            Assert.IsInstanceOf<ObjectFactoryBase>(fact, "Factory Test not of type ObjectFactoryBase");
        }


        //ObjectFactoryTest::CleanForReuse();
        [Test]
        public void CleanForReuseTest()
        {
            //Setup
            FactoryTestObjTesterFactory fact = new FactoryTestObjTesterFactory();

            //Test
            Assert.Throws<FactoryTestObjTesterCleanForReuseCalled>(() => fact.CleanForReuse(new FactoryTestObjTester()), "Factory New didn't work.");
        }

        //ObjectFactoryTest::ObjCleanForReuse();
        [Test]
        public void CleanForReuseBaseTest()
        {
            //Setup
            FactoryTestObjTesterFactory fact = new FactoryTestObjTesterFactory();

            //Test
            Assert.Throws<FactoryTestObjTesterCleanForReuseCalled>(() => fact.ObjCleanForReuse(new FactoryTestObjTester()), "Factory Base New didn't work.");
        }

        //ObjectFactoryTest::CreateNew();
        [Test]
        public void CreateTest()
        {
            //Setup
            FactoryTestObjTesterFactory fact = new FactoryTestObjTesterFactory();

            //Test
            Assert.Throws<FactoryTestObjTesterCreateNewCalled>(() => fact.CreateNew(), "Factory New didn't work.");
        }

        //ObjectFactoryTest::ObjCreateNew();
        [Test]
        public void CreateObjectTest()
        {
            //Setup
            FactoryTestObjTesterFactory fact = new FactoryTestObjTesterFactory();

            //Test
            Assert.Throws<FactoryTestObjTesterCreateNewCalled>(() => fact.ObjCreateNew(), "Factory Base New didn't work.");
        }
    }
}
