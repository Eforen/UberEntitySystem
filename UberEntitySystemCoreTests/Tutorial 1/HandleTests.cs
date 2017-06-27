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
    public class HandleTests
    {
        [TestMethod()]
        public void initTest()
        {
            //setup
            Entity e = new Entity();
            Handle h = new Handle(e);

            //test
            Assert.IsNotNull(h.entity);
        }

        [TestMethod()]
        public void equelButNotSameTest()
        {
            //Setup
            Entity e = new Entity();
            Handle h1 = new Handle(e);
            Handle h2 = new Handle(e);

            //Test
            Assert.IsNotNull(h1.entity);
            Assert.IsNotNull(h2.entity);
            Assert.AreEqual(h1.entity, h2.entity);
            Assert.AreEqual(h1.phase, h2.phase);
            Assert.AreEqual(h1, h2);
            Assert.AreNotSame(h1, h2);
        }

        [TestMethod()]
        public void nullOutOfPhaseTest()
        {
            //Setup
            Entity e = new Entity();
            Handle h1 = new Handle(e);
            e.phaseUp();
            Handle h2 = new Handle(e);

            //Test
            Assert.IsNull(h1.entity);
            Assert.IsNotNull(h2.entity);
        }

        [TestMethod()]
        public void entityInPhaseTest()
        {
            //setup 1
            Entity e = new Entity();
            Handle h = new Handle(e);

            //test 1
            Assert.AreSame(e, h.entity);


            //setup 2
            e.phaseUp();
            e.phaseUp();
            e.phaseUp();
            e.phaseUp();
            e.phaseUp();
            h = new Handle(e);

            //test 2
            Assert.AreSame(e, h.entity);
        }
    }
}