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
    public class SystemTests
    {
        #region Execute System Tests
        private class ExecuteSystem1 : ExecuteSystem
        {
            public override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void ExecuteSystems()
        {
            //Kinda redundant if it compiles but enough to insure Execute System Extends EntitySystem
            PoolSystem sys = new ExecuteSystem1();
        }

        #endregion //Execute System

        public class SetupSys : PoolSystem
        {
            public override void Setup()
            {
                throw new Exception("Worked");
            }

            public override void Execute()
            {
                throw new NotImplementedException();
            }
        }
        [Test]
        public void SystemSetupCalled()
        {
            try
            {
                Pool pool = new Pool();
                UberEntityComponentSystem.Systems sys = new UberEntityComponentSystem.Systems(pool).add(new SetupSys());
            }
            catch (Exception e)
            {
                if (e.Message == "Worked") return;
                Assert.Fail("Setup Not Called: Other Exception");
            }

            Assert.Fail("Setup Not Called: No Exception");
        }

        #region AutoSort Tests
        #region AutoSortClasses

        private class Sorted1Sorted : Exception { }
        private class Sorted1 : PoolSystem
        {
            public Group group;
            public override void Setup()
            {
                group = pool.getGroup(Signature.Get(typeof(Tracker)));
            }
            public override void Execute()
            {
                group[0].entity.getComponent<Tracker>().history += "1";
            }
        }

        private class Sorted1AltSorted : Exception { }
        private class Sorted1Alt : PoolSystem
        {
            public Group group;
            public override void Setup()
            {
                group = pool.getGroup(Signature.Get(typeof(Tracker)));
            }
            public override void Execute()
            {
                group[0].entity.getComponent<Tracker>().history += "a";
            }
        }

        private class Sorted2Sorted : Exception { }
        private class Sorted2 : PoolSystem
        {
            public Group group;
            public override void Setup()
            {
                group = pool.getGroup(Signature.Get(typeof(Tracker)));
            }
            public override Type[] dependencies
            {
                get
                {
                    return new Type[] { typeof(Sorted1) };
                }
            }

            public override void Execute()
            {
                group[0].entity.getComponent<Tracker>().history += "2";
            }
        }

        private class Sorted3Sorted : Exception { }
        private class Sorted3 : PoolSystem
        {
            public Group group;
            public override void Setup()
            {
                group = pool.getGroup(Signature.Get(typeof(Tracker)));
            }
            public override Type[] dependencies
            {
                get
                {
                    return new Type[] { typeof(Sorted2) };
                }
            }

            public override void Execute()
            {
                group[0].entity.getComponent<Tracker>().history += "3";
            }
        }

        private class Sorted4Sorted : Exception { }
        private class Sorted4 : PoolSystem
        {
            public Group group;
            public override void Setup()
            {
                group = pool.getGroup(Signature.Get(typeof(Tracker)));
            }
            public override Type[] dependencies
            {
                get
                {
                    return new Type[] { typeof(Sorted3) };
                }
            }

            public override void Execute()
            {
                group[0].entity.getComponent<Tracker>().history += "4";
            }
        }

        private class Tracker : Component
        {
            public string history = "";
        }

        #endregion //AutoSortClasses
        [Test]
        public void SystemAutosort()
        {
            Pool pool = new Pool();
            Entity entity = pool.newEntity;
            Tracker tracker = entity.addComponent<Tracker>();
            UberEntityComponentSystem.Systems sys = new UberEntityComponentSystem.Systems(pool)
                .add(new Sorted4())
                .add(new Sorted2())
                .add(new Sorted1Alt())
                .add(new Sorted3())
                .add(new Sorted1());

            sys.Execute();

            Assert.AreEqual("a1234", tracker.history);
            
        }
        #endregion //System Tests

        #region Feature Tests
        [Test]
        [Ignore("Don't know how I want to handle this.")]
        public void Features()
        {
        }
        #endregion //Feature Tests

    }
}