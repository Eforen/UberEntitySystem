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
    public class HandleTests
    {
        [Test]
        public void InstantiationTest()
        {
            new Handle(new Entity());
        }

        #region Entity Acquisition

        [Test]
        public void EntityGetTest()
        {
            //setup
            Entity e = new Entity();
            Handle h = new Handle(e);
            
            //test
            Assert.IsNotNull(h.entity);
        }

        [Test]
        public void OutOfPhaseNullTest()
        {
            //setup
            Entity e = new Entity();
            Handle h = new Handle(e);
            
            Assert.AreEqual(0, e.phase);
            Assert.AreEqual(0, h.phase);
            e.phaseUp();

            //test
            Assert.IsNull(h.entity);
        }

        [Test]
        public void InPhaseGetTest()
        {
            //setup
            Entity e = new Entity();

            Assert.AreEqual(0, e.phase);
            e.phaseUp();

            Handle h = new Handle(e);
            Assert.AreEqual(1, h.phase);

            //test
            Assert.AreEqual(1, e.phase);
            Assert.AreEqual(1, h.phase);
            Assert.IsNotNull(h.entity);
            Assert.AreSame(e, h.entity);
        }

        #endregion //Entity Acquisition

        #region EqualsMethod
        [Test]
        public void EqualsMethodNotOtherTest()
        {
            //setup
            Entity e = new Entity();
            Handle h = new Handle(e);

            //test
            Assert.IsFalse(h.Equals(e)); //TODO: Should this evaluate true? 
            Assert.IsFalse(h.Equals(new object()));
        }

        [Test]
        public void EqualsMethodNullTest()
        {
            //setup
            Entity e = new Entity();
            Handle h = new Handle(e);

            //test
            Assert.IsFalse(h.Equals(null));
        }

        public void EqualsMethodSameTest()
        {
            //setup
            Entity e = new Entity();
            e.phaseUp();

            Handle h1 = new Handle(e);
            Handle h2 = new Handle(e);
            Assert.AreNotSame(h1, h2);

            //test
            Assert.IsTrue(h1.Equals(h2));
        }

        [Test]
        public void EqualsMethodNotSameTest()
        {
            //setup
            Entity e1 = new Entity();
            Entity e2 = new Entity();
            Assert.AreNotSame(e1, e2);
            e1.phaseUp();
            e2.phaseUp();

            Handle h1 = new Handle(e1);
            Handle h2 = new Handle(e2);
            Assert.AreNotSame(h1, h2);

            //test
            Assert.IsFalse(h1.Equals(h2));
        }

        [Test]
        public void EqualsMethodOutOfPhaseTest()
        {
            //setup
            Entity e = new Entity();
            Handle h1 = new Handle(e);
            e.phaseUp();

            Handle h2 = new Handle(e);
            Assert.AreNotSame(h1, h2);

            //test
            Assert.IsFalse(h1.Equals(h2));
        }
        #endregion //EqualsMethod

        #region OpEquals
        [Test]
        public void OpEqualsOtherObjTest()
        {
            //setup
            Entity e = new Entity();
            Handle h = new Handle(e);

            //test
            Assert.IsFalse(h == new object());
        }

        [Test]
        public void OpEqualsNullTest()
        {
            //setup
            Entity e = new Entity();
            Handle h = null;

            //test
            Assert.IsTrue(h == null);
            Assert.IsTrue(null == h);

            //more setup
            h = new Handle(e);

            //more test
            Assert.IsFalse(h == null);
            Assert.IsFalse(null == h);
        }

        [Test]
        public void OpEqualsMethodSameTest()
        {
            //setup
            Entity e = new Entity();
            e.phaseUp();

            Handle h1 = new Handle(e);
            Handle h2 = new Handle(e);
            Assert.AreNotSame(h1, h2);

            //test
            Assert.IsTrue(h1 == h2);
        }

        [Test]
        public void OpEqualsMethodNotSameTest()
        {
            //setup
            Entity e1 = new Entity();
            Entity e2 = new Entity();
            Assert.AreNotSame(e1, e2);
            e1.phaseUp();
            e2.phaseUp();

            Handle h1 = new Handle(e1);
            Handle h2 = new Handle(e2);
            Assert.AreNotSame(h1, h2);

            //test
            Assert.IsFalse(h1 == h2);
        }

        [Test]
        public void OpEqualsMethodOutOfPhaseTest()
        {
            //setup
            Entity e = new Entity();
            Handle h1 = new Handle(e);
            e.phaseUp();

            Handle h2 = new Handle(e);
            Assert.AreNotSame(h1, h2);

            //test
            Assert.IsFalse(h1 == h2);
        }
        #endregion //OpEquals

        #region OpNotEquals
        [Test]
        public void OpNotEqualsOtherObjTest()
        {
            //setup
            Entity e = new Entity();
            Handle h = new Handle(e);

            //test
            Assert.IsTrue(h != new object());
        }

        [Test]
        public void OpNotEqualsNullTest()
        {
            //setup
            Entity e = new Entity();
            Handle h = null;

            //test
            Assert.IsFalse(h != null);
            Assert.IsFalse(null != h);

            //more setup
            h = new Handle(e);

            //more test
            Assert.IsTrue(h != null);
            Assert.IsTrue(null != h);
        }

        [Test]
        public void OpNotEqualsMethodSameTest()
        {
            //setup
            Entity e = new Entity();
            e.phaseUp();

            Handle h1 = new Handle(e);
            Handle h2 = new Handle(e);
            Assert.AreNotSame(h1, h2);

            //test
            Assert.IsFalse(h1 != h2);
        }

        [Test]
        public void OpNotEqualsMethodNotSameTest()
        {
            //setup
            Entity e1 = new Entity();
            Entity e2 = new Entity();
            Assert.AreNotSame(e1, e2);
            e1.phaseUp();
            e2.phaseUp();

            Handle h1 = new Handle(e1);
            Handle h2 = new Handle(e2);
            Assert.AreNotSame(h1, h2);

            //test
            Assert.IsTrue(h1 != h2);
        }

        [Test]
        public void OpNotEqualsMethodOutOfPhaseTest()
        {
            //setup
            Entity e = new Entity();
            Handle h1 = new Handle(e);
            e.phaseUp();

            Handle h2 = new Handle(e);
            Assert.AreNotSame(h1, h2);

            //test
            Assert.IsTrue(h1 != h2);
        }
        #endregion //OpNotEquals

    }
}