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
    public class SignatureTests
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

        private class Component1 : Component { }
        private class Component2 : Component {
            public int yolo = 0;
        }

        #endregion //Object Setup

        #region Tests
        [Test]
        public void ConstructorsAndSelfEquality()
        {
            Signature sig1 = Signature.Get(typeof(Tag1));
            Signature sig2 = Signature.Get(typeof(Tag1), typeof(Tag2), typeof(Tag3));
            Signature sig3 = Signature.Get(typeof(Tag1), typeof(Tag2), typeof(Tag3));
            Signature sig4 = Signature.Get(typeof(Tag1), typeof(Tag4), typeof(Tag5));

            Assert.AreEqual(false, sig1 == sig2);
            Assert.AreEqual(true, sig2 == sig3);
            Assert.AreEqual(false, sig3 == sig4);

            Assert.AreNotEqual(sig1, sig2);
            Assert.AreEqual(sig2, sig3);
            Assert.AreNotEqual(sig3, sig4);
        }
        [Test]
        public void ConstructorsUseFactories()
        {
            Factory.Reset(); //Clear Cache to insure the state of the caches are known

            Signature sig1 = Signature.Get(typeof(Tag1));
            Signature sig2 = Signature.Get(typeof(Tag1));
            Signature sig3 = Signature.Get(typeof(Tag1), typeof(Tag2), typeof(Tag3));
            Signature sig4 = Signature.Get(typeof(Tag1), typeof(Tag4), typeof(Tag5));

            Assert.AreEqual(0, sig1.phase);
            Assert.AreEqual(sig1, sig2);

            Factory.Cache(sig1); //Cache object so next out is this one.

            Assert.AreEqual(1, sig1.phase);

            Signature sig5 = Signature.Get(typeof(Tag3));

            Assert.AreSame(sig1, sig5); // Assert that the object we had before is this one even though it now has tag3 instead of tag1
            Assert.AreNotEqual(sig2, sig1);
            Assert.AreNotEqual(sig2, sig5);


        }

        [Test]
        public void Includes()
        {
            Signature sig1 = Signature.Get(typeof(Tag1));
            Signature sig2 = Signature.Get(typeof(Tag1));
            Signature sig3 = Signature.Get(typeof(Tag1), typeof(Tag2), typeof(Tag3));
            Signature sig4 = Signature.Get(typeof(Tag1), typeof(Tag4), typeof(Tag5));

            Assert.AreEqual(true, sig1.includes(typeof(Tag1)));
            Assert.AreEqual(false, sig1.includes(typeof(Tag2)));
            Assert.AreEqual(false, sig1.includes(typeof(Tag3)));
            Assert.AreEqual(false, sig1.includes(typeof(Tag4)));
            Assert.AreEqual(false, sig1.includes(typeof(Tag5)));
            Assert.AreEqual(false, sig1.includes(typeof(Tag6)));
            Assert.AreEqual(false, sig1.includes(typeof(Tag7)));
            Assert.AreEqual(false, sig1.includes(typeof(Tag8)));
            Assert.AreEqual(false, sig1.includes(typeof(Tag9)));

            Assert.AreEqual(true, sig2.includes(typeof(Tag1)));
            Assert.AreEqual(false, sig2.includes(typeof(Tag2)));
            Assert.AreEqual(false, sig2.includes(typeof(Tag3)));
            Assert.AreEqual(false, sig2.includes(typeof(Tag4)));
            Assert.AreEqual(false, sig2.includes(typeof(Tag5)));
            Assert.AreEqual(false, sig2.includes(typeof(Tag6)));
            Assert.AreEqual(false, sig2.includes(typeof(Tag7)));
            Assert.AreEqual(false, sig2.includes(typeof(Tag8)));
            Assert.AreEqual(false, sig2.includes(typeof(Tag9)));

            Assert.AreEqual(true, sig3.includes(typeof(Tag1)));
            Assert.AreEqual(true, sig3.includes(typeof(Tag2)));
            Assert.AreEqual(true, sig3.includes(typeof(Tag3)));
            Assert.AreEqual(false, sig3.includes(typeof(Tag4)));
            Assert.AreEqual(false, sig3.includes(typeof(Tag5)));
            Assert.AreEqual(false, sig3.includes(typeof(Tag6)));
            Assert.AreEqual(false, sig3.includes(typeof(Tag7)));
            Assert.AreEqual(false, sig3.includes(typeof(Tag8)));
            Assert.AreEqual(false, sig3.includes(typeof(Tag9)));

            Assert.AreEqual(true, sig4.includes(typeof(Tag1)));
            Assert.AreEqual(false, sig4.includes(typeof(Tag2)));
            Assert.AreEqual(false, sig4.includes(typeof(Tag3)));
            Assert.AreEqual(true, sig4.includes(typeof(Tag4)));
            Assert.AreEqual(true, sig4.includes(typeof(Tag5)));
            Assert.AreEqual(false, sig4.includes(typeof(Tag6)));
            Assert.AreEqual(false, sig4.includes(typeof(Tag7)));
            Assert.AreEqual(false, sig4.includes(typeof(Tag8)));
            Assert.AreEqual(false, sig4.includes(typeof(Tag9)));
        }

        [Test]
        public void EntityMatching()
        {
            Factory.Reset(); //Clear Cache to insure the state of the caches are known

            Entity ent1 = Factory.Get<Entity>();
            ent1.setTag<Tag1>();

            Entity ent2 = Factory.Get<Entity>();
            ent2.setTag<Tag1>();
            ent2.addComponent<Component1>();

            Entity ent3 = Factory.Get<Entity>();
            ent3.setTag<Tag1>();
            ent3.addComponent<Component2>().yolo = 5;


            Entity ent4 = Factory.Get<Entity>();
            ent4.setTag<Tag1>();
            ent4.addComponent<Component2>().yolo = 7;


            Signature sig1 = Signature.Get(typeof(Tag1));
            Signature sig2 = Signature.Get(typeof(Tag1), typeof(Component1));
            Signature sig3 = Signature.Get((e) => e.getComponent<Component2>().yolo == 5, typeof(Tag1), typeof(Component2));
            Signature sig4 = Signature.Get((e) => e.getComponent<Component2>().yolo == 7, typeof(Tag1), typeof(Component2));

            // Run tests

            //Tests for sig1
            Assert.AreEqual(true, sig1 == ent1);
            Assert.AreEqual(true, sig1 == ent2);
            Assert.AreEqual(true, sig1 == ent3);
            Assert.AreEqual(true, sig1 == ent4);

            //Tests for sig2
            Assert.AreEqual(false, sig2 == ent1);
            Assert.AreEqual(true, sig2 == ent2);
            Assert.AreEqual(false, sig2 == ent3);
            Assert.AreEqual(false, sig2 == ent4);

            //Tests for sig3
            Assert.AreEqual(false, sig3 == ent1);
            Assert.AreEqual(false, sig3 == ent2);
            Assert.AreEqual(true, sig3 == ent3);
            Assert.AreEqual(false, sig3 == ent4);

            //Tests for sig4
            Assert.AreEqual(false, sig4 == ent1);
            Assert.AreEqual(false, sig4 == ent2);
            Assert.AreEqual(false, sig4 == ent3);
            Assert.AreEqual(true, sig4 == ent4);
        }
        #endregion //Tests

    }
}