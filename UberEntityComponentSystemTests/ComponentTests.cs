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
    public class ComponentTests
    {
        [TearDown]
        public void Cleanup()
        {
            Factory.Reset();
        }

        public class DataComponent1 : Component
        {
            public float x, y, z;
        }

        public class DataComponent2 : Component
        {
            public string value;
        }

        #region Generics

        #region Component Generics
        [Test]
        public void OwnerSet()
        {
            DataComponent1 d1 = new DataComponent1();
            DataComponent1 d2 = new DataComponent1();

            Assert.IsNull(d1.owner);
            Assert.IsNull(d2.owner);

            Entity e1 = new Entity();
            Entity e2 = new Entity();

            d1.owner = e1;

            Assert.AreSame(e1, d1.owner);
            Assert.IsNull(d2.owner);

            e2.addComponent(d2);
            Assert.AreSame(e1, d1.owner);
            Assert.AreSame(e2, d2.owner);
        }

        private class RemoveCompCalled : Exception
        {

        }
        private class EntityProxy : Entity
        {
            public override T removeComponent<T>(T component = null, bool recycleComponent = true)
            {
                throw new RemoveCompCalled();
            }
        }

        [Test]
        public void OwnerChange()
        {
            DataComponent1 d1 = new DataComponent1();
            DataComponent1 d2 = new DataComponent1();

            Assert.IsNull(d1.owner);
            Assert.IsNull(d2.owner);

            Entity e1 = new Entity();
            Entity e2 = new Entity();

            d1.owner = e1;

            Assert.AreSame(e1, d1.owner);
            Assert.IsNull(d2.owner);

            e2.addComponent(d2);
            Assert.AreSame(e1, d1.owner);
            Assert.AreSame(e2, d2.owner);

            e2.removeComponent<DataComponent1>();
            Assert.AreSame(e1, d1.owner);
            Assert.IsNull(d2.owner);

            EntityProxy ep = new EntityProxy();
            ep.addComponent(d2);

            try
            {
                d2.owner = null;
                Assert.Fail("Remove Component not called");
            }
            catch (RemoveCompCalled)
            {
            }
        }
        #endregion //Component Generics

        #region Entity Generics

        [Test]
        public void AutoCreateComponentsFromFactories()
        {

            //TODO: Add Factory Check
            //Setup
            // Prime Factories
            DataComponent1 d1 = new DataComponent1();
            DataComponent2 d2 = new DataComponent2();

            Factory.Cache(d1);
            Factory.Cache(d2);

            //Init both entities
            Entity e1 = new Entity();
            DataComponent1 d12 = e1.addComponent<DataComponent1>();

            Entity e2 = new Entity();
            DataComponent2 d22 = e2.addComponent<DataComponent2>();


            //Test
            // Check not same
            Assert.AreSame(d1, d12);
            Assert.AreSame(d1, e1.getComponent<DataComponent1>());

            Assert.AreSame(d2, d22);
            Assert.AreSame(d2, e2.getComponent<DataComponent2>());
        }

        [Test]
        public void RecycleComponents()
        {
            Entity e1 = new Entity();
            Entity e2 = new Entity();

            DataComponent1 d1 = e1.addComponent<DataComponent1>();
            DataComponent2 d2 = e2.addComponent<DataComponent2>();

            Assert.IsTrue(e1.hasComponent<DataComponent1>());
            Assert.IsTrue(e2.hasComponent<DataComponent2>());

            e1.removeComponent<DataComponent1>();
            e2.removeComponent<DataComponent2>();

            Assert.AreSame(d1, Factory.Get<DataComponent1>());
            Assert.AreSame(d2, Factory.Get<DataComponent2>());
        }

        [Test]
        public void DontRecycleComponents()
        {
            Entity e1 = new Entity();
            Entity e2 = new Entity();

            DataComponent1 d1 = e1.addComponent<DataComponent1>();
            DataComponent2 d2 = e2.addComponent<DataComponent2>();

            Assert.IsTrue(e1.hasComponent<DataComponent1>());
            Assert.IsTrue(e2.hasComponent<DataComponent2>());

            e1.removeComponent<DataComponent1>(null, false);
            e2.removeComponent<DataComponent2>(null, false);

            Assert.AreNotSame(d1, Factory.Get<DataComponent1>());
            Assert.AreNotSame(d2, Factory.Get<DataComponent2>());
        }

        [Test]
        public void AutoCreateDiffComponents()
        {
            //Setup
            //Init both entities
            Entity e1 = new Entity();
            e1.addComponent<DataComponent1>();
            Entity e2 = new Entity();
            e2.addComponent<DataComponent1>();

            Entity e3 = new Entity();
            e3.addComponent<DataComponent2>();
            Entity e4 = new Entity();
            e4.addComponent<DataComponent2>();

            //Test
            Assert.AreNotSame(e1.getComponent<DataComponent1>(), e2.getComponent<DataComponent1>());
        }

        [Test]
        public void AddComponents()
        {
            //Setup
            //Init both entities
            Entity e1 = new Entity();
            DataComponent1 d1 = e1.addComponent<DataComponent1>();

            Entity e2 = new Entity();
            DataComponent2 d2 = e2.addComponent<DataComponent2>();

            //Test
            // Check not same
            Assert.AreSame(d1, e1.getComponent<DataComponent1>());

            Assert.AreSame(d2, e2.getComponent<DataComponent2>());
        }

        [Test]
        public void ReplaceComponents()
        {
            //Setup
            //Init both entities
            Entity e1 = new Entity();
            DataComponent1 d1 = e1.addComponent<DataComponent1>();
            DataComponent1 d12 = e1.replaceComponent<DataComponent1>();

            Entity e2 = new Entity();
            DataComponent2 d2 = e2.addComponent<DataComponent2>();
            DataComponent2 d22 = e2.replaceComponent<DataComponent2>();

            //Test
            // Check not same
            Assert.AreSame(d12, e1.getComponent<DataComponent1>());

            Assert.AreSame(d22, e2.getComponent<DataComponent2>());

            Assert.IsTrue(e1.replaced.Contains(typeof(DataComponent1)));
            Assert.IsTrue(e2.replaced.Contains(typeof(DataComponent2)));
        }

        [Test]
        public void HasComponents()
        {
            //Setup
            //Init both entities
            Entity e1 = new Entity();
            DataComponent1 d1 = new DataComponent1();
            e1.addComponent(d1);

            Entity e2 = new Entity();
            DataComponent2 d2 = new DataComponent2();
            e2.addComponent(d2);

            //Test
            // Check not same
            Assert.IsTrue(e1.hasComponent<DataComponent1>());
            Assert.IsFalse(e1.hasComponent<DataComponent2>());

            Assert.IsFalse(e2.hasComponent<DataComponent1>());
            Assert.IsTrue(e2.hasComponent<DataComponent2>());

            Assert.AreNotSame(e1, e2);
        }

        #endregion //Entity Generics

        #endregion //Generics

        #region TypeParam

        #region Component TypeParam

        [Test]
        public void OwnerChangeTypeParam()
        {
            DataComponent1 d1 = new DataComponent1();
            DataComponent1 d2 = new DataComponent1();

            Assert.IsNull(d1.owner);
            Assert.IsNull(d2.owner);

            Entity e1 = new Entity();
            Entity e2 = new Entity();

            d1.owner = e1;

            Assert.AreSame(e1, d1.owner);
            Assert.IsNull(d2.owner);

            e2.addComponent(d2);
            Assert.AreSame(e1, d1.owner);
            Assert.AreSame(e2, d2.owner);

            e2.removeComponent(typeof(DataComponent1));
            Assert.AreSame(e1, d1.owner);
            Assert.IsNull(d2.owner);

            EntityProxy ep = new EntityProxy();
            ep.addComponent(d2);

            try
            {
                d2.owner = null;
                Assert.Fail("Remove Component not called");
            }
            catch (RemoveCompCalled)
            {
            }
        }
        #endregion //Component TypeParam

        #region Entity TypeParam

        [Test]
        public void AutoCreateComponentsFromFactoriesTypeParam()
        {

            //TODO: Add Factory Check
            //Setup
            // Prime Factories
            DataComponent1 d1 = new DataComponent1();
            DataComponent2 d2 = new DataComponent2();

            Factory.Cache(d1);
            Factory.Cache(d2);

            //Init both entities
            Entity e1 = new Entity();
            DataComponent1 d12 = e1.addComponent(typeof(DataComponent1)) as DataComponent1;

            Entity e2 = new Entity();
            DataComponent2 d22 = e2.addComponent(typeof(DataComponent2)) as DataComponent2;


            //Test
            // Check not same
            Assert.AreSame(d1, d12);
            Assert.AreSame(d1, e1.getComponent(typeof(DataComponent1)));

            Assert.AreSame(d2, d22);
            Assert.AreSame(d2, e2.getComponent(typeof(DataComponent2)));
        }

        [Test]
        public void RecycleComponentsTypeParam()
        {
            Entity e1 = new Entity();
            Entity e2 = new Entity();

            DataComponent1 d1 = e1.addComponent(typeof(DataComponent1)) as DataComponent1;
            DataComponent2 d2 = e2.addComponent(typeof(DataComponent2)) as DataComponent2;

            Assert.IsTrue(e1.hasComponent(typeof(DataComponent1)));
            Assert.IsTrue(e2.hasComponent(typeof(DataComponent2)));

            e1.removeComponent(typeof(DataComponent1));
            e2.removeComponent(typeof(DataComponent2));

            Assert.AreSame(d1, Factory.Get(typeof(DataComponent1)));
            Assert.AreSame(d2, Factory.Get(typeof(DataComponent2)));
        }

        [Test]
        public void DontRecycleComponentsTypeParam()
        {
            Entity e1 = new Entity();
            Entity e2 = new Entity();

            DataComponent1 d1 = e1.addComponent(typeof(DataComponent1)) as DataComponent1;
            DataComponent2 d2 = e2.addComponent(typeof(DataComponent2)) as DataComponent2;

            Assert.IsTrue(e1.hasComponent(typeof(DataComponent1)));
            Assert.IsTrue(e2.hasComponent(typeof(DataComponent2)));

            e1.removeComponent(typeof(DataComponent1), false);
            e2.removeComponent(typeof(DataComponent2), false);

            Assert.AreNotSame(d1, Factory.Get(typeof(DataComponent1)));
            Assert.AreNotSame(d2, Factory.Get(typeof(DataComponent2)));
        }

        [Test]
        public void AutoCreateDiffComponentsTypeParam()
        {
            //Setup
            //Init both entities
            Entity e1 = new Entity();
            e1.addComponent(typeof(DataComponent1));
            Entity e2 = new Entity();
            e2.addComponent(typeof(DataComponent1));

            Entity e3 = new Entity();
            e3.addComponent(typeof(DataComponent2));
            Entity e4 = new Entity();
            e4.addComponent(typeof(DataComponent2));

            //Test
            Assert.AreNotSame(e1.getComponent(typeof(DataComponent1)), e2.getComponent(typeof(DataComponent1)));
        }

        [Test]
        public void AddComponentsTypeParam()
        {
            //Setup
            //Init both entities
            Entity e1 = new Entity();
            DataComponent1 d1 = new DataComponent1();
            e1.addComponent(d1);

            Entity e2 = new Entity();
            DataComponent2 d2 = new DataComponent2();
            e2.addComponent(d2);

            //Test
            // Check not same
            Assert.AreSame(d1, e1.getComponent(typeof(DataComponent1)));

            Assert.AreSame(d2, e2.getComponent(typeof(DataComponent2)));
        }

        [Test]
        public void ReplaceComponentsTypeParam()
        {
            //Setup
            //Init both entities
            Entity e1 = new Entity();
            DataComponent1 d1 = new DataComponent1();
            DataComponent1 d12 = new DataComponent1();
            e1.addComponent(d1);
            e1.replaceComponent(d12);

            Entity e2 = new Entity();
            DataComponent2 d2 = new DataComponent2();
            DataComponent2 d22 = new DataComponent2();
            e2.addComponent(d2);
            e2.replaceComponent(d22);

            //Test
            // Check not same
            Assert.AreSame(d12, e1.getComponent(typeof(DataComponent1)));

            Assert.AreSame(d22, e2.getComponent(typeof(DataComponent2)));
        }

        [Test]
        public void HasComponentsTypeParam()
        {
            //Setup
            //Init both entities
            Entity e1 = new Entity();
            DataComponent1 d1 = new DataComponent1();
            e1.addComponent(d1);

            Entity e2 = new Entity();
            DataComponent2 d2 = new DataComponent2();
            e2.addComponent(d2);

            //Test
            // Check not same
            Assert.IsTrue(e1.hasComponent(typeof(DataComponent1)));
            Assert.IsFalse(e1.hasComponent(typeof(DataComponent2)));

            Assert.IsFalse(e2.hasComponent(typeof(DataComponent1)));
            Assert.IsTrue(e2.hasComponent(typeof(DataComponent2)));

            Assert.AreNotSame(e1, e2);
        }

        #endregion //Entity TypeParam

        #endregion //TypeParam

    }
}