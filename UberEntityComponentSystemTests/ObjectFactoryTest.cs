using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberEntityComponentSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UberEntityComponentSystemTests
{
    using NUnit.Framework;
    class FactoryTestObjTesterCleanForReuseCalled : Exception { }
    class FactoryTestObjTesterCreateNewCalled : Exception { }

    class FactoryTestObjTester { }

    /// <summary>
    /// DO NOT CHANGE UNLESS YOU KNOW WHAT YOUR DOING!
    /// </summary>
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

    class FactoryTestObjTesterCleanForReuseCalled2 : Exception { }
    class FactoryTestObjTesterCreateNewCalled2 : Exception { }

    class FactoryTestObjTester2 { }

    /// <summary>
    /// DO NOT CHANGE UNLESS YOU KNOW WHAT YOUR DOING!
    /// </summary>
    class FactoryTestObjTesterFactory2 : ObjectFactory<FactoryTestObjTester2>
    {
        public override FactoryTestObjTester2 CleanForReuse(FactoryTestObjTester2 obj)
        {
            throw new FactoryTestObjTesterCleanForReuseCalled2(); //Prove the method was called
        }

        public override FactoryTestObjTester2 CreateNew()
        {
            throw new FactoryTestObjTesterCreateNewCalled2(); //Prove the method was called
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
            FactoryTestObjTesterFactory2 fact2 = new FactoryTestObjTesterFactory2();

            //Test
            Assert.Throws<FactoryTestObjTesterCleanForReuseCalled>(() => fact.CleanForReuse(new FactoryTestObjTester()), "Factory New didn't work.");
            Assert.Throws<FactoryTestObjTesterCleanForReuseCalled2>(() => fact2.CleanForReuse(new FactoryTestObjTester2()), "Factory New didn't work.");
        }

        //ObjectFactoryTest::ObjCleanForReuse();
        [Test]
        public void CleanForReuseBaseTest()
        {
            //Setup
            FactoryTestObjTesterFactory fact = new FactoryTestObjTesterFactory();
            FactoryTestObjTesterFactory2 fact2 = new FactoryTestObjTesterFactory2();

            //Test
            Assert.Throws<FactoryTestObjTesterCleanForReuseCalled>(() => fact.ObjCleanForReuse(new FactoryTestObjTester()), "Factory Base New didn't work.");
            Assert.Throws<FactoryTestObjTesterCleanForReuseCalled2>(() => fact2.ObjCleanForReuse(new FactoryTestObjTester2()), "Factory Base New didn't work.");
        }

        //ObjectFactoryTest::ObjCleanForReuse();
        [Test]
        public void CleanForReuseBaseExplodeTest()
        {
            //Setup
            FactoryTestObjTesterFactory fact = new FactoryTestObjTesterFactory();
            FactoryTestObjTesterFactory2 fact2 = new FactoryTestObjTesterFactory2();

            //Test
            Assert.Throws<InvalidCastException>(() => fact.ObjCleanForReuse(new FactoryTestObjTester2()), "Factory Base New didn't work.");
            Assert.Throws<InvalidCastException>(() => fact2.ObjCleanForReuse(new FactoryTestObjTester()), "Factory Base New didn't work.");
        }

        //ObjectFactoryTest::CreateNew();
        [Test]
        public void CreateTest()
        {
            //Setup
            FactoryTestObjTesterFactory fact = new FactoryTestObjTesterFactory();
            FactoryTestObjTesterFactory2 fact2 = new FactoryTestObjTesterFactory2();

            //Test
            Assert.Throws<FactoryTestObjTesterCreateNewCalled>(() => fact.CreateNew(), "Factory New didn't work.");
            Assert.Throws<FactoryTestObjTesterCreateNewCalled2>(() => fact2.CreateNew(), "Factory New didn't work.");
        }

        //ObjectFactoryTest::ObjCreateNew();
        [Test]
        public void CreateObjectTest()
        {
            //Setup
            FactoryTestObjTesterFactory fact = new FactoryTestObjTesterFactory();
            FactoryTestObjTesterFactory2 fact2 = new FactoryTestObjTesterFactory2();

            //Test
            Assert.Throws<FactoryTestObjTesterCreateNewCalled>(() => fact.ObjCreateNew(), "Factory Base New didn't work.");
            Assert.Throws<FactoryTestObjTesterCreateNewCalled2>(() => fact2.ObjCreateNew(), "Factory Base New didn't work.");
        }
    }
}
