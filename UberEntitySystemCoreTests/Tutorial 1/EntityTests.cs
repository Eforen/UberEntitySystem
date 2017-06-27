using Microsoft.VisualStudio.TestTools.UnitTesting;
using UberEntitySystemCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberEntitySystemCore.Tests
{
    [TestClass()]
    public class EntityTests
    {
        [TestMethod()]
        public void InstantiationTest()
        {
            new Entity();
        }

        [TestMethod()]
        public void PhaseTest()
        {
            //Setup 1
            //Init both entities
            Entity t1 = new Entity();
            Entity t2 = new Entity();

            //Test 1
            //Confirm the handles passed out do not match each other
            Assert.AreNotEqual(t1.handle, t2.handle);
            Assert.AreNotSame(t1.handle, t2.handle);


            //Setup 3
            //Grab handles
            Handle h1 = t1.handle;
            Handle h2 = t2.handle;

            //Test 3
            //confirm the same handle is passed out and it still matches
            Assert.AreEqual(h1, t1.handle);
            Assert.AreSame(h1, t1.handle);
            Assert.AreEqual(h2, t2.handle);
            Assert.AreSame(h2, t2.handle);



            //Setup 3
            //Force out of phase
            t1.phaseUp();
            t2.phaseUp();

            //Test 3
            //Confirm they don't match anymore
            Assert.AreNotEqual(h1, t1.handle);
            Assert.AreNotSame(h1, t1.handle);
            Assert.AreNotEqual(h2, t2.handle);
            Assert.AreNotSame(h2, t2.handle);
            //Confirm they have the right phase
            Assert.AreEqual(1, t1.phase);
            Assert.AreEqual(1, t2.phase);

            //Force out of phase
            t1.phaseUp();
            t1.phaseUp();
            t1.phaseUp();
            t1.phaseUp();
            t1.phaseUp();
            t1.phaseUp();
            t1.phaseUp();
            t2.phaseUp();
            t2.phaseUp();
            t2.phaseUp();
            t2.phaseUp();

            //Test 3
            //Confirm they have the right phase
            Assert.AreEqual(8, t1.phase);
            Assert.AreEqual(5, t2.phase);
        }
    }
}