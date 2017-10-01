using Microsoft.VisualStudio.TestTools.UnitTesting;
using UberEntityComponentSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberEntityComponentSystem.Tests
{
    using NUnit.Framework;
    [TestClass()]
    public class TagTests
    {
        #region Object Setup
        private class Tag1 : ITag { }
        private class Tag2 : ITag { }
        private class Tag3 : ITag { }
        private class Tag4 : ITag { }
        private class Tag5 : ITag { }
        private class Tag6 : ITag { }
        private class Tag7 : ITag { }
        private class Tag8 : ITag { }
        private class Tag9 : ITag { }

        private class NotTag1 { }
        private class NotTag2 { }

        #endregion //Object Setup

        #region Tests
        [Test]
        public void Generics()
        {
            Entity e1 = new Entity();
            Entity e2 = new Entity();
            
            Assert.AreEqual(false, e1.setTag<Tag1>());    //Should return false because nothing was set yet
            Assert.AreEqual(true, e1.hasTag<Tag1>());    //Should return true because it should be what ever is or is not set
            Assert.AreEqual(true, e1.unsetTag<Tag1>());  //Should return true because it should be what ever is or is not set
            Assert.AreEqual(false, e1.hasTag<Tag1>());    //Should return false because it should be unset
            Assert.AreEqual(false, e1.setTag<Tag1>());    //Should return false because it should be unset
            Assert.AreEqual(true, e1.setTag<Tag1>());    //Should return true because it was set already
            Assert.AreEqual(true, e1.hasTag<Tag1>());    //Should return true because it was set


            Assert.AreEqual(false, e1.setTag<Tag2>());    //Should return false because nothing was set yet
            Assert.AreEqual(true, e1.hasTag<Tag2>());    //Should return true because it should be what ever is or is not set
            Assert.AreEqual(true, e1.unsetTag<Tag2>());  //Should return true because it should be what ever is or is not set
            Assert.AreEqual(false, e1.hasTag<Tag2>());    //Should return false because it should be unset
            Assert.AreEqual(false, e1.setTag<Tag2>());    //Should return false because it should be unset
            Assert.AreEqual(true, e1.setTag<Tag2>());    //Should return true because it was set already
            Assert.AreEqual(true, e1.hasTag<Tag2>());    //Should return true because it was set


            Assert.AreEqual(true, e1.hasTag<Tag1>()); //should still be set
        }

        [Test]
        [Sequential]
        public void TypeParam([Values(typeof(Tag1), typeof(NotTag1), typeof(Tag2), typeof(NotTag2), typeof(Tag3), typeof(NotTag1), typeof(NotTag2))]Type type, [Values(true, false, true, false, true, false, false)]bool valid)
        {
            Entity e1 = new Entity();
            Entity e2 = new Entity();

            Assert.AreEqual(false, e1.setTag(type));    //Should return false because nothing was set yet
            Assert.AreEqual(valid, e1.hasTag(type));    //Should return valid because it should be what ever is or is not set
            Assert.AreEqual(valid, e1.unsetTag(type));  //Should return valid because it should be what ever is or is not set
            Assert.AreEqual(false, e1.hasTag(type));    //Should return false because it should be unset
            Assert.AreEqual(false, e1.setTag(type));    //Should return false because it should be unset
            Assert.AreEqual(valid, e1.setTag(type));    //Should return valid because it was set already
            Assert.AreEqual(valid, e1.hasTag(type));    //Should return valid because it was set
        }


        [Test]
        [Sequential]
        public void MultiTypeParam([Values(
            new Type[]{ typeof(Tag1), typeof(NotTag1), typeof(Tag2) },
            new Type[]{ typeof(NotTag2), typeof(Tag3), typeof(NotTag1), typeof(NotTag2) })]Type[] type, [Values(
            new bool[]{ true,         false,           true },
            new bool[]{ false,           true,         false,           false })]bool[] valid)
        {
            Entity e1 = new Entity();
            Entity e2 = new Entity();

            for (int i = 0; i < type.Length; i++)
            {
                Assert.AreEqual(false, e1.setTag(type[i]));
                Assert.AreEqual(valid[i], e1.hasTag(type[i]));
                Assert.AreEqual(valid[i], e1.unsetTag(type[i]));
                Assert.AreEqual(false, e1.hasTag(type[i]));
                Assert.AreEqual(false, e1.setTag(type[i]));
                Assert.AreEqual(valid[i], e1.setTag(type[i]));
                Assert.AreEqual(valid[i], e1.hasTag(type[i]));
                Assert.AreEqual(valid[i], e1.unsetTag(type[i]));
                Assert.AreEqual(false, e1.hasTag(type[i]));
            }

            for (int i = 0; i < type.Length; i++)
            {
                Assert.AreEqual(false, e1.setTag(type[i]));
            }

            for (int i = 0; i < type.Length; i++)
            {
                Assert.AreEqual(valid[i], e1.hasTag(type[i]));
            }
        }
        #endregion //Tests

    }
}