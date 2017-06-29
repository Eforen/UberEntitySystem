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
    public class EntityTests
    {
        [Test]
        public void InstantiationTest()
        {
            new Entity();
        }

        #region Phase

        [Test]
        public void PhaseUpTest()
        {
            //Setup
            //Init both entities
            Entity e1 = new Entity();
            Entity e2 = new Entity();

            //Test
            //Check Phase
            Assert.IsNotNull(e1.phase);
            Assert.IsNotNull(e2.phase);

            Assert.AreEqual(0, e1.phase);
            Assert.AreEqual(0, e2.phase);

            //Setup
            //Force Out of Phase
            e1.phaseUp();
            e2.phaseUp();

            //Test
            //Confirm phase up
            Assert.AreEqual(1, e1.phase);
            Assert.AreEqual(1, e2.phase);
        }

        [Test]
        public void PhaseUpRandTest([Random(0, 200, 5)]int c)
        {
            //Setup
            //Init both entities
            Entity e1 = new Entity();
            Entity e2 = new Entity();

            //Validate Setup
            Assert.AreNotSame(e1, e2);

            //Run Random Set
            for (int i = 0; i < c; i++)
            {
                e1.phaseUp();
                e2.phaseUp();
            }

            Assert.AreEqual(c, e1.phase);
            Assert.AreEqual(c, e2.phase);
        }
        #endregion //Phase

        #region Handle
        [Test]
        public void ReturnsDiffHandles()
        {
            //Setup
            Entity e1 = new Entity();
            Entity e2 = new Entity();

            //test
            Assert.AreNotEqual(e1.handle, e2.handle);
            Assert.AreNotSame(e1.handle, e2.handle);
            Assert.IsNotNull(e1.handle);
            Assert.IsNotNull(e2.handle);
        }

        [Test]
        public void ReturnsHandle()
        {
            //Setup
            Entity e1 = new Entity();
            Entity e2 = new Entity();

            //test
            Assert.IsInstanceOf(typeof(Handle), e1.handle);
            Assert.IsInstanceOf(typeof(Handle), e2.handle);
        }

        [Test]
        public void ReturnsValidHandle()
        {
            //Setup
            Entity e1 = new Entity();
            Entity e2 = new Entity();
            Handle h1 = new Handle(e1);
            Handle h2 = new Handle(e2);

            //test
            Assert.AreEqual(h1, e1.handle);
            Assert.AreEqual(h2, e2.handle);
        }

        [Test]
        public void ReturnsInPhaseHandle()
        {
            //Setup
            Entity e1 = new Entity();
            Entity e2 = new Entity();
            Handle h1 = new Handle(e1);
            Handle h2 = new Handle(e2);

            //test
            //In phase
            Assert.AreEqual(h1, e1.handle);
            Assert.AreEqual(h2, e2.handle);

            //setup 2
            //go out of phase
            e1.phaseUp();
            e2.phaseUp();

            //test 2
            //are out of phase
            Assert.AreNotEqual(h1, e1.handle);
            Assert.AreNotEqual(h2, e2.handle);

            //setup 3
            //go in phase
            h1 = new Handle(e1);
            h2 = new Handle(e2);

            //test
            //In phase again
            Assert.AreEqual(h1, e1.handle);
            Assert.AreEqual(h2, e2.handle);
        }

        [Test]
        public void ReturnsSameInphaseHandle()
        {
            //Setup
            Entity e = new Entity();
            Handle h = e.handle;

            //test
            //is same when In phase
            Assert.AreSame(h, e.handle);

            //setup 2
            //go out of phase
            e.phaseUp();

            //test 2
            //not same when out of phase
            Assert.AreNotSame(h, e.handle);

            //setup 3
            //go in phase
            h = e.handle;

            //test
            //is same when in phase again
            Assert.AreEqual(h, e.handle);
        }
        #endregion
    }
}