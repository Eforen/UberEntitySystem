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

    [Factory(typeof(FactoryTestObjFactory), typeof(FactoryTestObj))]
    class FactoryTestObj
    {
        public int currentID = 0;
        public string currentString = "";
    }
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
    class FactoryTestObj2Factory : ObjectFactory<FactoryTestObj2>
    {
        public override FactoryTestObj2 CleanForReuse(FactoryTestObj2 obj)
        {
            obj.currentID = 0;
            obj.currentString = "";
            return obj;
        }

        public override FactoryTestObj2 CreateNew()
        {
            return new FactoryTestObj2();
        }
    }

    [TestClass()]
    public class FactoryTests
    {
        [SetUp]
        public void ResetEverything()
        {
            Factory.Reset();
            GC.Collect(); //Force garbage collection right now!
        }

        #region Generic Methods


        [Test]
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

        [Test]
        public void ResetCache()
        {
            //setup
            Factory.Cache<FactoryTestObj>(new FactoryTestObj());
            Factory.Cache<FactoryTestObj>(new FactoryTestObj());
            Factory.Cache<FactoryTestObj>(new FactoryTestObj());
            Factory.Cache<FactoryTestObj>(new FactoryTestObj());
            Factory.Cache<FactoryTestObj>(new FactoryTestObj());
            Factory.Cache<FactoryTestObj2>(new FactoryTestObj2());
            Factory.Cache<FactoryTestObj2>(new FactoryTestObj2());
            Factory.Cache<FactoryTestObj2>(new FactoryTestObj2());
            Assert.AreEqual(5, Factory.Count<FactoryTestObj>(), "Setup Failed");
            Assert.AreEqual(3, Factory.Count<FactoryTestObj2>(), "Setup Failed");

            //test
            Factory.Reset();
            Assert.AreEqual(0, Factory.Count<FactoryTestObj>(), "FactoryTestObj Not Cleared");
            Assert.AreEqual(0, Factory.Count<FactoryTestObj2>(), "FactoryTestObj2 Not Cleared");
        }

        [Test]
        public void ResetTargetedCache()
        {
            //setup
            Factory.Cache<FactoryTestObj>(new FactoryTestObj());
            Factory.Cache<FactoryTestObj>(new FactoryTestObj());
            Factory.Cache<FactoryTestObj>(new FactoryTestObj());
            Factory.Cache<FactoryTestObj>(new FactoryTestObj());
            Factory.Cache<FactoryTestObj>(new FactoryTestObj());
            Factory.Cache<FactoryTestObj2>(new FactoryTestObj2());
            Factory.Cache<FactoryTestObj2>(new FactoryTestObj2());
            Factory.Cache<FactoryTestObj2>(new FactoryTestObj2());
            Assert.AreEqual(5, Factory.Count<FactoryTestObj>(), "Setup Failed");
            Assert.AreEqual(3, Factory.Count<FactoryTestObj2>(), "Setup Failed");

            //test
            Factory.Reset<FactoryTestObj>();
            Assert.AreEqual(0, Factory.Count<FactoryTestObj>(), "FactoryTestObj Not Cleared");
            Assert.AreEqual(3, Factory.Count<FactoryTestObj2>(), "FactoryTestObj2 Not Cleared");

            Factory.Reset<FactoryTestObj>();
            Assert.AreEqual(0, Factory.Count<FactoryTestObj>(), "FactoryTestObj Not Cleared");
            Assert.AreEqual(3, Factory.Count<FactoryTestObj2>(), "FactoryTestObj2 Not Cleared");

            Factory.Reset<FactoryTestObj2>();
            Assert.AreEqual(0, Factory.Count<FactoryTestObj2>(), "FactoryTestObj2 Not Cleared");
        }

        [Test]
        [Ignore("Not sure I want this to be part of the Spec")]
        public void DoNotDoubleCache()
        {
            //TODO: This stuff will likely need to be optimized later
            FactoryTestObj f1 = new FactoryTestObj();

            //Actually Cache Obj
            Factory.Cache<FactoryTestObj>(f1);
            Assert.AreEqual(1, Factory.Count<FactoryTestObj>());

            //Don't Cache again Obj
            Factory.Cache<FactoryTestObj>(f1);
            Assert.AreEqual(1, Factory.Count<FactoryTestObj>());

            //Object was not double cached without incrementing the the count
            Assert.AreNotSame(Factory.Get<FactoryTestObj>(), Factory.Get<FactoryTestObj>());
        }

        [Test]
        public void CacheAndGet()
        {
            //setup
            FactoryTestObj f1 = Factory.Get<FactoryTestObj>();
            FactoryTestObj f2 = Factory.Get<FactoryTestObj>();
            FactoryTestObj2 f3 = Factory.Get<FactoryTestObj2>();
            FactoryTestObj2 f4 = Factory.Get<FactoryTestObj2>();

            //Test new() Getters
            Assert.AreNotSame(f1, f2);
            Assert.AreNotSame(f3, f4);
            Assert.IsNotNull(f1);
            Assert.IsNotNull(f2);
            Assert.IsNotNull(f3);
            Assert.IsNotNull(f4);

            Assert.IsInstanceOf<FactoryTestObj>(f1);
            Assert.IsInstanceOf<FactoryTestObj>(f2);
            Assert.IsInstanceOf<FactoryTestObj2>(f3);
            Assert.IsInstanceOf<FactoryTestObj2>(f4);

            //Cache all
            Factory.Cache<FactoryTestObj>(f1);
            Factory.Cache<FactoryTestObj>(f2);
            Factory.Cache<FactoryTestObj2>(f3, f4);

            Assert.AreSame(f1, Factory.Get<FactoryTestObj>());
            Assert.AreSame(f2, Factory.Get<FactoryTestObj>());
            Assert.AreSame(f3, Factory.Get<FactoryTestObj2>());
            Assert.AreSame(f4, Factory.Get<FactoryTestObj2>());
        }

        [Test]
        [Repeat(5000)]
        public void CacheCount()
        {
            //Setup and test 1
            Factory.Cache<FactoryTestObj>(new FactoryTestObj());
            Assert.AreEqual(1, Factory.Count<FactoryTestObj>());
            Assert.AreEqual(0, Factory.Count<FactoryTestObj2>());

            //Setup and test 2
            Factory.Cache<FactoryTestObj>(new FactoryTestObj());
            Factory.Cache<FactoryTestObj2>(new FactoryTestObj2());
            Assert.AreEqual(2, Factory.Count<FactoryTestObj>());
            Assert.AreEqual(1, Factory.Count<FactoryTestObj2>());

            //Setup and test 3
            Factory.Cache<FactoryTestObj>(new FactoryTestObj());
            Factory.Cache<FactoryTestObj>(new FactoryTestObj());
            Assert.AreEqual(4, Factory.Count<FactoryTestObj>());
            Assert.AreEqual(1, Factory.Count<FactoryTestObj2>());

            //Setup and test 4
            Factory.Cache<FactoryTestObj2>(new FactoryTestObj2());
            Factory.Cache<FactoryTestObj2>(new FactoryTestObj2());
            Factory.Cache<FactoryTestObj2>(new FactoryTestObj2());
            Assert.AreEqual(4, Factory.Count<FactoryTestObj>());
            Assert.AreEqual(4, Factory.Count<FactoryTestObj2>());
        }

        [Test]
        public void SimpleFactoryCleanupTest()
        {
            //Setup
            FactoryTestObj f1 = Factory.Get<FactoryTestObj>();
            FactoryTestObj f2 = Factory.Get<FactoryTestObj>();

            //Validate Setup
            Assert.AreNotSame(f1, f2);
            Assert.IsNotNull(f1);
            Assert.IsNotNull(f2);

            //More Setup
            f1.currentID = 78;
            f2.currentID = 732;

            f1.currentString = "test1";
            f2.currentString = "test2";

            //Validate more setup
            Assert.AreEqual(78, f1.currentID);
            Assert.AreEqual(732, f2.currentID);

            Assert.AreEqual("test1", f1.currentString);
            Assert.AreEqual("test2", f2.currentString);

            //Test
            FactoryTestObj f7 = Factory.Clean<FactoryTestObj>(f1);
            FactoryTestObj f8 = Factory.Clean<FactoryTestObj>(f2);

            //Validate Test
            Assert.AreSame(f1, f7);
            Assert.AreSame(f2, f8);

            Assert.AreEqual(0, f1.currentID);
            Assert.AreEqual(0, f2.currentID);
            Assert.AreEqual("", f1.currentString);
            Assert.AreEqual("", f2.currentString);
        }

        [Test]
        public void SimpleNoFactoryCleanupTest()
        {
            //Setup
            FactoryTestObjNoCleaner f1 = Factory.Get<FactoryTestObjNoCleaner>();
            FactoryTestObjNoCleaner f2 = Factory.Get<FactoryTestObjNoCleaner>();

            //Validate Setup
            Assert.AreNotSame(f1, f2);
            Assert.IsNotNull(f1);
            Assert.IsNotNull(f2);

            //More Setup
            f1.currentID = 42;
            f2.currentID = 165;

            f1.currentString = "test5";
            f2.currentString = "test6";

            //Validate more setup
            Assert.AreEqual(42, f1.currentID);
            Assert.AreEqual(165, f2.currentID);

            Assert.AreEqual("test5", f1.currentString);
            Assert.AreEqual("test6", f2.currentString);

            //Test
            FactoryTestObjNoCleaner f11 = Factory.Clean<FactoryTestObjNoCleaner>(f1);
            FactoryTestObjNoCleaner f12 = Factory.Clean<FactoryTestObjNoCleaner>(f2);

            //Validate Test
            Assert.AreSame(f1, f11);
            Assert.AreSame(f2, f12);

            Assert.AreEqual(42, f1.currentID);
            Assert.AreEqual(165, f2.currentID);
            Assert.AreEqual("test5", f1.currentString);
            Assert.AreEqual("test6", f2.currentString);
        }

        [Test]
        public void DeepCleanupTest()
        {
            //Setup
            FactoryTestObj f1 = Factory.Get<FactoryTestObj>();
            FactoryTestObj f2 = Factory.Get<FactoryTestObj>();
            FactoryTestObj2 f3 = Factory.Get<FactoryTestObj2>();
            FactoryTestObj2 f4 = Factory.Get<FactoryTestObj2>();
            FactoryTestObjNoCleaner f5 = Factory.Get<FactoryTestObjNoCleaner>();
            FactoryTestObjNoCleaner f6 = Factory.Get<FactoryTestObjNoCleaner>();

            //Validate Setup
            Assert.AreNotSame(f1, f2);
            Assert.AreNotSame(f3, f4);
            Assert.AreNotSame(f5, f6);
            Assert.IsNotNull(f1);
            Assert.IsNotNull(f2);
            Assert.IsNotNull(f3);
            Assert.IsNotNull(f4);
            Assert.IsNotNull(f5);
            Assert.IsNotNull(f6);

            //More Setup
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

            //Validate more setup
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

            //Test
            FactoryTestObj f7 = Factory.Clean<FactoryTestObj>(f1);
            FactoryTestObj f8 = Factory.Clean<FactoryTestObj>(f2);
            FactoryTestObj2 f9 = Factory.Clean<FactoryTestObj2>(f3);
            FactoryTestObj2 f10 = Factory.Clean<FactoryTestObj2>(f4);
            FactoryTestObjNoCleaner f11 = Factory.Clean<FactoryTestObjNoCleaner>(f5);
            FactoryTestObjNoCleaner f12 = Factory.Clean<FactoryTestObjNoCleaner>(f6);

            //Validate Test
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
        #endregion

        #region Anon Method Tests
        [Test]
        public void MethodInstantiationTest()
        {
            FactoryTestObj f1 = Factory.Get(typeof(FactoryTestObj)) as FactoryTestObj;
            FactoryTestObj f2 = Factory.Get(typeof(FactoryTestObj)) as FactoryTestObj;
            FactoryTestObj2 f3 = Factory.Get(typeof(FactoryTestObj2)) as FactoryTestObj2;
            FactoryTestObj2 f4 = Factory.Get(typeof(FactoryTestObj2)) as FactoryTestObj2;

            Assert.AreNotSame(f1, f2);
            Assert.IsNotNull(f1);
            Assert.IsNotNull(f2);

            Assert.AreEqual(0, f1.currentID);
            Assert.AreEqual(0, f2.currentID);
            Assert.AreEqual("", f1.currentString);
            Assert.AreEqual("", f2.currentString);
        }

        [Test]
        public void MethodResetCache()
        {
            //setup
            Factory.Cache(new FactoryTestObj());
            Factory.Cache(new FactoryTestObj());
            Factory.Cache(new FactoryTestObj());
            Factory.Cache(new FactoryTestObj());
            Factory.Cache(new FactoryTestObj());
            Factory.Cache(new FactoryTestObj2());
            Factory.Cache(new FactoryTestObj2());
            Factory.Cache(new FactoryTestObj2());
            Assert.AreEqual(5, Factory.Count(typeof(FactoryTestObj)), "Setup Failed");
            Assert.AreEqual(3, Factory.Count(typeof(FactoryTestObj2)), "Setup Failed");

            //test
            Factory.Reset();
            Assert.AreEqual(0, Factory.Count(typeof(FactoryTestObj)), "FactoryTestObj Not Cleared");
            Assert.AreEqual(0, Factory.Count(typeof(FactoryTestObj2)), "FactoryTestObj2 Not Cleared");
        }

        [Test]
        public void MethodResetTargetedCache()
        {
            //setup
            Factory.Cache(new FactoryTestObj());
            Factory.Cache(new FactoryTestObj());
            Factory.Cache(new FactoryTestObj());
            Factory.Cache(new FactoryTestObj());
            Factory.Cache(new FactoryTestObj());
            Factory.Cache(new FactoryTestObj2());
            Factory.Cache(new FactoryTestObj2());
            Factory.Cache(new FactoryTestObj2());
            Assert.AreEqual(5, Factory.Count(typeof(FactoryTestObj)), "Setup Failed");
            Assert.AreEqual(3, Factory.Count(typeof(FactoryTestObj2)), "Setup Failed");

            //test
            Factory.Reset(typeof(FactoryTestObj));
            Assert.AreEqual(0, Factory.Count(typeof(FactoryTestObj)), "FactoryTestObj Not Cleared");
            Assert.AreEqual(3, Factory.Count(typeof(FactoryTestObj2)), "FactoryTestObj2 Not Cleared");

            Factory.Reset(typeof(FactoryTestObj));
            Assert.AreEqual(0, Factory.Count(typeof(FactoryTestObj)), "FactoryTestObj Not Cleared");
            Assert.AreEqual(3, Factory.Count(typeof(FactoryTestObj2)), "FactoryTestObj2 Not Cleared");

            Factory.Reset(typeof(FactoryTestObj2));
            Assert.AreEqual(0, Factory.Count(typeof(FactoryTestObj2)), "FactoryTestObj2 Not Cleared");
        }

        [Test]
        public void MethodCacheAndGet()
        {
            //setup
            FactoryTestObj f1 = Factory.Get(typeof(FactoryTestObj)) as FactoryTestObj;
            FactoryTestObj f2 = Factory.Get(typeof(FactoryTestObj)) as FactoryTestObj;
            FactoryTestObj2 f3 = Factory.Get(typeof(FactoryTestObj2)) as FactoryTestObj2;
            FactoryTestObj2 f4 = Factory.Get(typeof(FactoryTestObj2)) as FactoryTestObj2;

            //Test new() Getters
            Assert.AreNotSame(f1, f2);
            Assert.AreNotSame(f3, f4);
            Assert.IsNotNull(f1);
            Assert.IsNotNull(f2);
            Assert.IsNotNull(f3);
            Assert.IsNotNull(f4);

            Assert.IsInstanceOf<FactoryTestObj>(f1);
            Assert.IsInstanceOf<FactoryTestObj>(f2);
            Assert.IsInstanceOf<FactoryTestObj2>(f3);
            Assert.IsInstanceOf<FactoryTestObj2>(f4);

            //Cache all
            Factory.Cache(f1);
            Factory.Cache(f2);
            Factory.Cache(f3, f4);

            Assert.AreSame(f1, Factory.Get(typeof(FactoryTestObj)));
            Assert.AreSame(f2, Factory.Get(typeof(FactoryTestObj)));
            Assert.AreSame(f3, Factory.Get(typeof(FactoryTestObj2)));
            Assert.AreSame(f4, Factory.Get(typeof(FactoryTestObj2)));
        }

        [Test]
        [Ignore("Not sure I want this to be part of the Spec")]
        public void MethodDoNotDoubleCache()
        {
            FactoryTestObj f1 = new FactoryTestObj();

            //Actually Cache Obj
            Factory.Cache(f1);
            Assert.AreEqual(1, Factory.Count(typeof(FactoryTestObj)));

            //Don't Cache again Obj
            Factory.Cache(f1);
            Assert.AreEqual(1, Factory.Count(typeof(FactoryTestObj)));

            //Object was not double cached without incrementing the the count
            Assert.AreNotSame(Factory.Get(typeof(FactoryTestObj)), Factory.Get(typeof(FactoryTestObj)));
        }

        [Test]
        public void MethodCacheCount()
        {
            //Setup and test 1
            Factory.Cache(new FactoryTestObj());
            Assert.AreEqual(1, Factory.Count(typeof(FactoryTestObj)));
            Assert.AreEqual(0, Factory.Count(typeof(FactoryTestObj2)));
            Assert.AreEqual(1, Factory.Count(typeof(FactoryTestObj), typeof(FactoryTestObj2)));

            //Setup and test 2
            Factory.Cache(new FactoryTestObj());
            Factory.Cache(new FactoryTestObj2());
            Assert.AreEqual(2, Factory.Count(typeof(FactoryTestObj)));
            Assert.AreEqual(1, Factory.Count(typeof(FactoryTestObj2)));
            Assert.AreEqual(3, Factory.Count(typeof(FactoryTestObj), typeof(FactoryTestObj2)));

            //Setup and test 3
            Factory.Cache(new FactoryTestObj());
            Factory.Cache(new FactoryTestObj());
            Assert.AreEqual(4, Factory.Count(typeof(FactoryTestObj)));
            Assert.AreEqual(1, Factory.Count(typeof(FactoryTestObj2)));
            Assert.AreEqual(5, Factory.Count(typeof(FactoryTestObj), typeof(FactoryTestObj2)));

            //Setup and test 4
            Factory.Cache(new FactoryTestObj2());
            Factory.Cache(new FactoryTestObj2());
            Factory.Cache(new FactoryTestObj2());
            Assert.AreEqual(4, Factory.Count(typeof(FactoryTestObj)));
            Assert.AreEqual(4, Factory.Count(typeof(FactoryTestObj2)));
            Assert.AreEqual(8, Factory.Count(typeof(FactoryTestObj), typeof(FactoryTestObj2)));
            Assert.AreEqual(8, Factory.Count(new Type[] { typeof(FactoryTestObj), typeof(FactoryTestObj2) }));
        }

        [Test]
        public void SimpleFactoryMethodCleanupTest()
        {
            //Setup
            FactoryTestObj f1 = Factory.Get(typeof(FactoryTestObj)) as FactoryTestObj;
            FactoryTestObj f2 = Factory.Get(typeof(FactoryTestObj)) as FactoryTestObj;

            //Validate Setup
            Assert.AreNotSame(f1, f2);
            Assert.IsNotNull(f1);
            Assert.IsNotNull(f2);

            //More Setup
            f1.currentID = 78;
            f2.currentID = 732;

            f1.currentString = "test1";
            f2.currentString = "test2";

            //Validate more setup
            Assert.AreEqual(78, f1.currentID);
            Assert.AreEqual(732, f2.currentID);

            Assert.AreEqual("test1", f1.currentString);
            Assert.AreEqual("test2", f2.currentString);

            //Test
            FactoryTestObj f7 = Factory.Clean(f1);
            FactoryTestObj f8 = Factory.Clean(f2);

            //Validate Test
            Assert.AreSame(f1, f7);
            Assert.AreSame(f2, f8);

            Assert.AreEqual(0, f1.currentID);
            Assert.AreEqual(0, f2.currentID);
            Assert.AreEqual("", f1.currentString);
            Assert.AreEqual("", f2.currentString);
        }

        [Test]
        public void SimpleNoFactoryMethodCleanupTest()
        {
            //Setup
            FactoryTestObjNoCleaner f1 = Factory.Get(typeof(FactoryTestObjNoCleaner)) as FactoryTestObjNoCleaner;
            FactoryTestObjNoCleaner f2 = Factory.Get(typeof(FactoryTestObjNoCleaner)) as FactoryTestObjNoCleaner;

            //Validate Setup
            Assert.AreNotSame(f1, f2);
            Assert.IsNotNull(f1);
            Assert.IsNotNull(f2);

            //More Setup
            f1.currentID = 42;
            f2.currentID = 165;

            f1.currentString = "test5";
            f2.currentString = "test6";

            //Validate more setup
            Assert.AreEqual(42, f1.currentID);
            Assert.AreEqual(165, f2.currentID);

            Assert.AreEqual("test5", f1.currentString);
            Assert.AreEqual("test6", f2.currentString);

            //Test
            FactoryTestObjNoCleaner f11 = Factory.Clean(f1);
            FactoryTestObjNoCleaner f12 = Factory.Clean(f2);

            //Validate Test
            Assert.AreSame(f1, f11);
            Assert.AreSame(f2, f12);

            Assert.AreEqual(42, f1.currentID);
            Assert.AreEqual(165, f2.currentID);
            Assert.AreEqual("test5", f1.currentString);
            Assert.AreEqual("test6", f2.currentString);
        }

        [Test]
        public void MethodCleanupTest()
        {
            //Setup
            FactoryTestObj f1 = Factory.Get(typeof(FactoryTestObj)) as FactoryTestObj;
            FactoryTestObj f2 = Factory.Get(typeof(FactoryTestObj)) as FactoryTestObj;
            FactoryTestObj2 f3 = Factory.Get(typeof(FactoryTestObj2)) as FactoryTestObj2;
            FactoryTestObj2 f4 = Factory.Get(typeof(FactoryTestObj2)) as FactoryTestObj2;
            FactoryTestObjNoCleaner f5 = Factory.Get(typeof(FactoryTestObjNoCleaner)) as FactoryTestObjNoCleaner;
            FactoryTestObjNoCleaner f6 = Factory.Get(typeof(FactoryTestObjNoCleaner)) as FactoryTestObjNoCleaner;

            //Validate Setup
            Assert.AreNotSame(f1, f2);
            Assert.AreNotSame(f3, f4);
            Assert.AreNotSame(f5, f6);
            Assert.IsNotNull(f1);
            Assert.IsNotNull(f2);
            Assert.IsNotNull(f3);
            Assert.IsNotNull(f4);
            Assert.IsNotNull(f5);
            Assert.IsNotNull(f6);

            //More Setup
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

            //Validate more setup
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

            //Test
            FactoryTestObj f7 = Factory.Clean(f1);
            FactoryTestObj f8 = Factory.Clean(f2);
            FactoryTestObj2 f9 = Factory.Clean(f3);
            FactoryTestObj2 f10 = Factory.Clean(f4);
            FactoryTestObjNoCleaner f11 = Factory.Clean(f5);
            FactoryTestObjNoCleaner f12 = Factory.Clean(f6);

            //Validate Test
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
        #endregion
    }
}
