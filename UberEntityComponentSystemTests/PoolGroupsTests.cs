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
    public class PoolGroupsTests
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
        private class Component2 : Component
        {
            public int yolo = 0;
        }

        #endregion //Object Setup

        [Test]
        public void InstantiationTest()
        {
            new Pool();
        }

        [Test]
        public void ArrayAccessOverride()
        {
            Pool pool = new Pool();
            Entity[] entities = { pool.newEntity, pool.newEntity, pool.newEntity, pool.newEntity, pool.newEntity, pool.newEntity, pool.newEntity, pool.newEntity, pool.newEntity, pool.newEntity, pool.newEntity, pool.newEntity, pool.newEntity, pool.newEntity };

            Group preGroup = pool.getGroup(Signature.Get(typeof(Tag1)));

            foreach (Entity e in entities)
            {
                e.setTag<Tag1>();
            }

            Group postGroup = pool.getGroup(Signature.Get(typeof(Tag1)));

            for (int i = 0; i < entities.Length; i++)
            {
                Assert.AreSame(preGroup[i], entities[i].handle);
                Assert.AreSame(postGroup[i], entities[i].handle);
            }
        }

        #region Entity

        [Test]
        public void GetGroup()
        {
            //Setup
            //Init both entities
            Pool pool = new Pool();
            Entity e1 = pool.newEntity;
            Entity e2 = pool.newEntity;

            Group g1 = pool.getGroup(Signature.Get(typeof(Tag1)));
            Group g2 = pool.getGroup(Signature.Get(typeof(Tag1)));
            Group g3 = pool.getGroup(Signature.Get(typeof(Tag2)));
            Group g4 = pool.getGroup(Signature.Get(typeof(Tag2)));
            Group g5 = pool.getGroup(Signature.Get(typeof(Tag1), typeof(Tag2)));
            Group g6 = pool.getGroup(Signature.Get(typeof(Tag1), typeof(Tag2)));


            //Test
            Assert.NotNull(g1);
            Assert.NotNull(g2);
            Assert.NotNull(g3);
            Assert.NotNull(g4);
            Assert.NotNull(g5);
            Assert.NotNull(g6);

            Assert.AreSame(g1, g2);
            Assert.AreNotSame(g1, g3);
            Assert.AreNotSame(g1, g5);

            Assert.AreSame(g3, g4);
            Assert.AreNotSame(g3, g5);

            Assert.AreSame(g5, g6);
        }

        [Test]
        public void FitEntityToGroup()
        {
            //Setup
            //Init both entities
            Pool pool = new Pool();
            Group g1 = pool.getGroup(Signature.Get(typeof(Tag1)));

            Assert.AreEqual(0, g1.count);

            Entity e1 = pool.newEntity;
            e1.setTag<Tag1>();
            e1.addComponent<Component1>();

            Assert.AreEqual(1, g1.count);

            Entity e2 = pool.newEntity;
            e2.setTag<Tag2>();
            e2.addComponent<Component1>();

            Assert.AreEqual(1, g1.count);

            Entity e3 = pool.newEntity;
            e3.setTag<Tag1>();
            e3.addComponent<Component1>();

            Assert.AreEqual(2, g1.count);

            //Test
            //TODO: This is wrong
            Group g2 = pool.getGroup(Signature.Get(typeof(Tag1)));
            Assert.AreEqual(2, g2.count);
            g2.fit(e1.handle);
            Assert.AreEqual(2, g2.count);
            g2.fit(e2.handle);
            Assert.AreEqual(2, g2.count);
            g2.fit(e3.handle);
            Assert.AreEqual(2, g2.count);
        }

        [Test]
        public void AutoFitEntityToGroup()
        {
            //Setup
            //Init both entities
            Pool pool = new Pool();


            Group g1 = pool.getGroup(Signature.Get(typeof(Tag1)));
            Group g2 = pool.getGroup(Signature.Get(typeof(Tag2)));
            Group g3 = pool.getGroup(Signature.Get(typeof(Component1)));
            Group g4 = pool.getGroup(Signature.Get(typeof(Component2)));
            Group g5 = pool.getGroup(Signature.Get(typeof(Tag2), typeof(Component1)));
            Group g6 = pool.getGroup(Signature.Get(typeof(Tag1), typeof(Component2)));

            Entity e1 = pool.newEntity;
            Assert.AreEqual(0, g1.count); // Tag1
            Assert.AreEqual(0, g2.count); // Tag2
            Assert.AreEqual(0, g3.count); // Component1
            Assert.AreEqual(0, g4.count); // Component2
            Assert.AreEqual(0, g5.count); // Tag2, Component1
            Assert.AreEqual(0, g6.count); // Tag1, Component2
            e1.addComponent<Component1>();
            Assert.AreEqual(0, g1.count); // Tag1
            Assert.AreEqual(0, g2.count); // Tag2
            Assert.AreEqual(1, g3.count); // Component1
            Assert.AreEqual(0, g4.count); // Component2
            Assert.AreEqual(0, g5.count); // Tag2, Component1
            Assert.AreEqual(0, g6.count); // Tag1, Component2
            e1.setTag<Tag1>();
            Assert.AreEqual(1, g1.count); // Tag1
            Assert.AreEqual(0, g2.count); // Tag2
            Assert.AreEqual(1, g3.count); // Component1
            Assert.AreEqual(0, g4.count); // Component2
            Assert.AreEqual(0, g5.count); // Tag2, Component1
            Assert.AreEqual(0, g6.count); // Tag1, Component2

            Entity e2 = pool.newEntity;
            Assert.AreEqual(1, g1.count); // Tag1
            Assert.AreEqual(0, g2.count); // Tag2
            Assert.AreEqual(1, g3.count); // Component1
            Assert.AreEqual(0, g4.count); // Component2
            Assert.AreEqual(0, g5.count); // Tag2, Component1
            Assert.AreEqual(0, g6.count); // Tag1, Component2
            e2.setTag<Tag2>();
            Assert.AreEqual(1, g1.count); // Tag1
            Assert.AreEqual(1, g2.count); // Tag2
            Assert.AreEqual(1, g3.count); // Component1
            Assert.AreEqual(0, g4.count); // Component2
            Assert.AreEqual(0, g5.count); // Tag2, Component1
            Assert.AreEqual(0, g6.count); // Tag1, Component2
            e2.addComponent<Component1>();
            Assert.AreEqual(1, g1.count); // Tag1
            Assert.AreEqual(1, g2.count); // Tag2
            Assert.AreEqual(2, g3.count); // Component1
            Assert.AreEqual(0, g4.count); // Component2
            Assert.AreEqual(1, g5.count); // Tag2, Component1
            Assert.AreEqual(0, g6.count); // Tag1, Component2

            Entity e3 = pool.newEntity;
            Assert.AreEqual(1, g1.count); // Tag1
            Assert.AreEqual(1, g2.count); // Tag2
            Assert.AreEqual(2, g3.count); // Component1
            Assert.AreEqual(0, g4.count); // Component2
            Assert.AreEqual(1, g5.count); // Tag2, Component1
            Assert.AreEqual(0, g6.count); // Tag1, Component2
            e3.setTag<Tag1>();
            Assert.AreEqual(2, g1.count); // Tag1
            Assert.AreEqual(1, g2.count); // Tag2
            Assert.AreEqual(2, g3.count); // Component1
            Assert.AreEqual(0, g4.count); // Component2
            Assert.AreEqual(1, g5.count); // Tag2, Component1
            Assert.AreEqual(0, g6.count); // Tag1, Component2
            e3.addComponent<Component2>();
            Assert.AreEqual(2, g1.count); // Tag1
            Assert.AreEqual(1, g2.count); // Tag2
            Assert.AreEqual(2, g3.count); // Component1
            Assert.AreEqual(1, g4.count); // Component2
            Assert.AreEqual(1, g5.count); // Tag2, Component1
            Assert.AreEqual(1, g6.count); // Tag1, Component2
        }

        #endregion //Entity

        #region Add Entity

        [Test]
        public void WhenAddedEntityIsInAddedList()
        {
            //Setup
            //Init both entities
            Pool pool = new Pool();
            
            Group g1 = pool.getGroup(Signature.Get(typeof(Tag1)));
            Group g2 = pool.getGroup(Signature.Get(typeof(Tag2)));
            Group g3 = pool.getGroup(Signature.Get(typeof(Component1)));
            Group g4 = pool.getGroup(Signature.Get(typeof(Component2)));
            Group g5 = pool.getGroup(Signature.Get(typeof(Tag1), typeof(Component1)));
            Group g6 = pool.getGroup(Signature.Get(typeof(Tag2), typeof(Component2)));
            Group g7 = pool.getGroup(Signature.Get(typeof(Tag2), typeof(Component1)));
            Group g8 = pool.getGroup(Signature.Get(typeof(Tag1), typeof(Component2)));

            Entity e1 = pool.newEntity;
            Entity e2 = pool.newEntity;
            Entity e3 = pool.newEntity;

            e1.addComponent<Component1>();
            e1.setTag<Tag1>();

            Assert.AreEqual(true, g1.added.Contains(e1.handle));
            Assert.AreEqual(false, g2.added.Contains(e1.handle));
            Assert.AreEqual(true, g3.added.Contains(e1.handle));
            Assert.AreEqual(false, g4.added.Contains(e1.handle));
            Assert.AreEqual(true, g5.added.Contains(e1.handle));
            Assert.AreEqual(false, g6.added.Contains(e1.handle));
            Assert.AreEqual(false, g7.added.Contains(e1.handle));
            Assert.AreEqual(false, g8.added.Contains(e1.handle));

            Assert.AreEqual(false, g1.added.Contains(e2.handle));
            Assert.AreEqual(false, g2.added.Contains(e2.handle));
            Assert.AreEqual(false, g3.added.Contains(e2.handle));
            Assert.AreEqual(false, g4.added.Contains(e2.handle));
            Assert.AreEqual(false, g5.added.Contains(e2.handle));
            Assert.AreEqual(false, g6.added.Contains(e2.handle));
            Assert.AreEqual(false, g7.added.Contains(e2.handle));
            Assert.AreEqual(false, g8.added.Contains(e2.handle));

            Assert.AreEqual(false, g1.added.Contains(e3.handle));
            Assert.AreEqual(false, g2.added.Contains(e3.handle));
            Assert.AreEqual(false, g3.added.Contains(e3.handle));
            Assert.AreEqual(false, g4.added.Contains(e3.handle));
            Assert.AreEqual(false, g5.added.Contains(e3.handle));
            Assert.AreEqual(false, g6.added.Contains(e3.handle));
            Assert.AreEqual(false, g7.added.Contains(e3.handle));
            Assert.AreEqual(false, g8.added.Contains(e3.handle));


            e2.setTag<Tag2>();
            e2.addComponent<Component1>();

            Assert.AreEqual(true, g1.added.Contains(e1.handle));
            Assert.AreEqual(false, g2.added.Contains(e1.handle));
            Assert.AreEqual(true, g3.added.Contains(e1.handle));
            Assert.AreEqual(false, g4.added.Contains(e1.handle));
            Assert.AreEqual(true, g5.added.Contains(e1.handle));
            Assert.AreEqual(false, g6.added.Contains(e1.handle));
            Assert.AreEqual(false, g7.added.Contains(e1.handle));
            Assert.AreEqual(false, g8.added.Contains(e1.handle));

            Assert.AreEqual(false, g1.added.Contains(e2.handle));
            Assert.AreEqual(true, g2.added.Contains(e2.handle));
            Assert.AreEqual(true, g3.added.Contains(e2.handle));
            Assert.AreEqual(false, g4.added.Contains(e2.handle));
            Assert.AreEqual(false, g5.added.Contains(e2.handle));
            Assert.AreEqual(false, g6.added.Contains(e2.handle));
            Assert.AreEqual(true, g7.added.Contains(e2.handle));
            Assert.AreEqual(false, g8.added.Contains(e2.handle));

            Assert.AreEqual(false, g1.added.Contains(e3.handle));
            Assert.AreEqual(false, g2.added.Contains(e3.handle));
            Assert.AreEqual(false, g3.added.Contains(e3.handle));
            Assert.AreEqual(false, g4.added.Contains(e3.handle));
            Assert.AreEqual(false, g5.added.Contains(e3.handle));
            Assert.AreEqual(false, g6.added.Contains(e3.handle));
            Assert.AreEqual(false, g7.added.Contains(e3.handle));
            Assert.AreEqual(false, g8.added.Contains(e3.handle));


            e3.setTag<Tag1>();
            e3.addComponent<Component2>();

            Assert.AreEqual(true, g1.added.Contains(e1.handle));
            Assert.AreEqual(false, g2.added.Contains(e1.handle));
            Assert.AreEqual(true, g3.added.Contains(e1.handle));
            Assert.AreEqual(false, g4.added.Contains(e1.handle));
            Assert.AreEqual(true, g5.added.Contains(e1.handle));
            Assert.AreEqual(false, g6.added.Contains(e1.handle));
            Assert.AreEqual(false, g7.added.Contains(e1.handle));
            Assert.AreEqual(false, g8.added.Contains(e1.handle));

            Assert.AreEqual(false, g1.added.Contains(e2.handle));
            Assert.AreEqual(true, g2.added.Contains(e2.handle));
            Assert.AreEqual(true, g3.added.Contains(e2.handle));
            Assert.AreEqual(false, g4.added.Contains(e2.handle));
            Assert.AreEqual(false, g5.added.Contains(e2.handle));
            Assert.AreEqual(false, g6.added.Contains(e2.handle));
            Assert.AreEqual(true, g7.added.Contains(e2.handle));
            Assert.AreEqual(false, g8.added.Contains(e2.handle));

            Assert.AreEqual(true, g1.added.Contains(e3.handle));
            Assert.AreEqual(false, g2.added.Contains(e3.handle));
            Assert.AreEqual(false, g3.added.Contains(e3.handle));
            Assert.AreEqual(true, g4.added.Contains(e3.handle));
            Assert.AreEqual(false, g5.added.Contains(e3.handle));
            Assert.AreEqual(false, g6.added.Contains(e3.handle));
            Assert.AreEqual(false, g7.added.Contains(e3.handle));
            Assert.AreEqual(true, g8.added.Contains(e3.handle));
        }
        [Test]
        public void WhenGroupIsSteppedAddedListIsCleared()
        {
            //Setup
            //Init both entities
            Pool pool = new Pool();

            Group g1 = pool.getGroup(Signature.Get(typeof(Tag1)));
            Group g2 = pool.getGroup(Signature.Get(typeof(Tag2)));
            Group g3 = pool.getGroup(Signature.Get(typeof(Component1)));
            Group g4 = pool.getGroup(Signature.Get(typeof(Component2)));
            Group g5 = pool.getGroup(Signature.Get(typeof(Tag1), typeof(Component1)));
            Group g6 = pool.getGroup(Signature.Get(typeof(Tag2), typeof(Component2)));
            Group g7 = pool.getGroup(Signature.Get(typeof(Tag2), typeof(Component1)));
            Group g8 = pool.getGroup(Signature.Get(typeof(Tag1), typeof(Component2)));

            Entity e1 = pool.newEntity;
            Entity e2 = pool.newEntity;
            Entity e3 = pool.newEntity;

            e1.addComponent<Component1>();
            e1.setTag<Tag1>();

            Assert.AreEqual(true, g1.added.Contains(e1.handle));
            Assert.AreEqual(false, g2.added.Contains(e1.handle));
            Assert.AreEqual(true, g3.added.Contains(e1.handle));
            Assert.AreEqual(false, g4.added.Contains(e1.handle));
            Assert.AreEqual(true, g5.added.Contains(e1.handle));
            Assert.AreEqual(false, g6.added.Contains(e1.handle));
            Assert.AreEqual(false, g7.added.Contains(e1.handle));
            Assert.AreEqual(false, g8.added.Contains(e1.handle));

            Assert.AreEqual(false, g1.added.Contains(e2.handle));
            Assert.AreEqual(false, g2.added.Contains(e2.handle));
            Assert.AreEqual(false, g3.added.Contains(e2.handle));
            Assert.AreEqual(false, g4.added.Contains(e2.handle));
            Assert.AreEqual(false, g5.added.Contains(e2.handle));
            Assert.AreEqual(false, g6.added.Contains(e2.handle));
            Assert.AreEqual(false, g7.added.Contains(e2.handle));
            Assert.AreEqual(false, g8.added.Contains(e2.handle));

            Assert.AreEqual(false, g1.added.Contains(e3.handle));
            Assert.AreEqual(false, g2.added.Contains(e3.handle));
            Assert.AreEqual(false, g3.added.Contains(e3.handle));
            Assert.AreEqual(false, g4.added.Contains(e3.handle));
            Assert.AreEqual(false, g5.added.Contains(e3.handle));
            Assert.AreEqual(false, g6.added.Contains(e3.handle));
            Assert.AreEqual(false, g7.added.Contains(e3.handle));
            Assert.AreEqual(false, g8.added.Contains(e3.handle));

            pool.incrementGroups();

            Assert.AreEqual(false, g1.added.Contains(e1.handle));
            Assert.AreEqual(false, g2.added.Contains(e1.handle));
            Assert.AreEqual(false, g3.added.Contains(e1.handle));
            Assert.AreEqual(false, g4.added.Contains(e1.handle));
            Assert.AreEqual(false, g5.added.Contains(e1.handle));
            Assert.AreEqual(false, g6.added.Contains(e1.handle));
            Assert.AreEqual(false, g7.added.Contains(e1.handle));
            Assert.AreEqual(false, g8.added.Contains(e1.handle));

            Assert.AreEqual(false, g1.added.Contains(e2.handle));
            Assert.AreEqual(false, g2.added.Contains(e2.handle));
            Assert.AreEqual(false, g3.added.Contains(e2.handle));
            Assert.AreEqual(false, g4.added.Contains(e2.handle));
            Assert.AreEqual(false, g5.added.Contains(e2.handle));
            Assert.AreEqual(false, g6.added.Contains(e2.handle));
            Assert.AreEqual(false, g7.added.Contains(e2.handle));
            Assert.AreEqual(false, g8.added.Contains(e2.handle));

            Assert.AreEqual(false, g1.added.Contains(e3.handle));
            Assert.AreEqual(false, g2.added.Contains(e3.handle));
            Assert.AreEqual(false, g3.added.Contains(e3.handle));
            Assert.AreEqual(false, g4.added.Contains(e3.handle));
            Assert.AreEqual(false, g5.added.Contains(e3.handle));
            Assert.AreEqual(false, g6.added.Contains(e3.handle));
            Assert.AreEqual(false, g7.added.Contains(e3.handle));
            Assert.AreEqual(false, g8.added.Contains(e3.handle));

            e2.setTag<Tag2>();
            e2.addComponent<Component1>();

            Assert.AreEqual(false, g1.added.Contains(e1.handle));
            Assert.AreEqual(false, g2.added.Contains(e1.handle));
            Assert.AreEqual(false, g3.added.Contains(e1.handle));
            Assert.AreEqual(false, g4.added.Contains(e1.handle));
            Assert.AreEqual(false, g5.added.Contains(e1.handle));
            Assert.AreEqual(false, g6.added.Contains(e1.handle));
            Assert.AreEqual(false, g7.added.Contains(e1.handle));
            Assert.AreEqual(false, g8.added.Contains(e1.handle));

            Assert.AreEqual(false, g1.added.Contains(e2.handle));
            Assert.AreEqual(true, g2.added.Contains(e2.handle));
            Assert.AreEqual(true, g3.added.Contains(e2.handle));
            Assert.AreEqual(false, g4.added.Contains(e2.handle));
            Assert.AreEqual(false, g5.added.Contains(e2.handle));
            Assert.AreEqual(false, g6.added.Contains(e2.handle));
            Assert.AreEqual(true, g7.added.Contains(e2.handle));
            Assert.AreEqual(false, g8.added.Contains(e2.handle));

            Assert.AreEqual(false, g1.added.Contains(e3.handle));
            Assert.AreEqual(false, g2.added.Contains(e3.handle));
            Assert.AreEqual(false, g3.added.Contains(e3.handle));
            Assert.AreEqual(false, g4.added.Contains(e3.handle));
            Assert.AreEqual(false, g5.added.Contains(e3.handle));
            Assert.AreEqual(false, g6.added.Contains(e3.handle));
            Assert.AreEqual(false, g7.added.Contains(e3.handle));
            Assert.AreEqual(false, g8.added.Contains(e3.handle));

            pool.incrementGroups();

            Assert.AreEqual(false, g1.added.Contains(e1.handle));
            Assert.AreEqual(false, g2.added.Contains(e1.handle));
            Assert.AreEqual(false, g3.added.Contains(e1.handle));
            Assert.AreEqual(false, g4.added.Contains(e1.handle));
            Assert.AreEqual(false, g5.added.Contains(e1.handle));
            Assert.AreEqual(false, g6.added.Contains(e1.handle));
            Assert.AreEqual(false, g7.added.Contains(e1.handle));
            Assert.AreEqual(false, g8.added.Contains(e1.handle));

            Assert.AreEqual(false, g1.added.Contains(e2.handle));
            Assert.AreEqual(false, g2.added.Contains(e2.handle));
            Assert.AreEqual(false, g3.added.Contains(e2.handle));
            Assert.AreEqual(false, g4.added.Contains(e2.handle));
            Assert.AreEqual(false, g5.added.Contains(e2.handle));
            Assert.AreEqual(false, g6.added.Contains(e2.handle));
            Assert.AreEqual(false, g7.added.Contains(e2.handle));
            Assert.AreEqual(false, g8.added.Contains(e2.handle));

            Assert.AreEqual(false, g1.added.Contains(e3.handle));
            Assert.AreEqual(false, g2.added.Contains(e3.handle));
            Assert.AreEqual(false, g3.added.Contains(e3.handle));
            Assert.AreEqual(false, g4.added.Contains(e3.handle));
            Assert.AreEqual(false, g5.added.Contains(e3.handle));
            Assert.AreEqual(false, g6.added.Contains(e3.handle));
            Assert.AreEqual(false, g7.added.Contains(e3.handle));
            Assert.AreEqual(false, g8.added.Contains(e3.handle));

            e3.setTag<Tag1>();
            e3.addComponent<Component2>();

            Assert.AreEqual(false, g1.added.Contains(e1.handle));
            Assert.AreEqual(false, g2.added.Contains(e1.handle));
            Assert.AreEqual(false, g3.added.Contains(e1.handle));
            Assert.AreEqual(false, g4.added.Contains(e1.handle));
            Assert.AreEqual(false, g5.added.Contains(e1.handle));
            Assert.AreEqual(false, g6.added.Contains(e1.handle));
            Assert.AreEqual(false, g7.added.Contains(e1.handle));
            Assert.AreEqual(false, g8.added.Contains(e1.handle));

            Assert.AreEqual(false, g1.added.Contains(e2.handle));
            Assert.AreEqual(false, g2.added.Contains(e2.handle));
            Assert.AreEqual(false, g3.added.Contains(e2.handle));
            Assert.AreEqual(false, g4.added.Contains(e2.handle));
            Assert.AreEqual(false, g5.added.Contains(e2.handle));
            Assert.AreEqual(false, g6.added.Contains(e2.handle));
            Assert.AreEqual(false, g7.added.Contains(e2.handle));
            Assert.AreEqual(false, g8.added.Contains(e2.handle));

            Assert.AreEqual(true, g1.added.Contains(e3.handle));
            Assert.AreEqual(false, g2.added.Contains(e3.handle));
            Assert.AreEqual(false, g3.added.Contains(e3.handle));
            Assert.AreEqual(true, g4.added.Contains(e3.handle));
            Assert.AreEqual(false, g5.added.Contains(e3.handle));
            Assert.AreEqual(false, g6.added.Contains(e3.handle));
            Assert.AreEqual(false, g7.added.Contains(e3.handle));
            Assert.AreEqual(true, g8.added.Contains(e3.handle));

            pool.incrementGroups();

            Assert.AreEqual(false, g1.added.Contains(e1.handle));
            Assert.AreEqual(false, g2.added.Contains(e1.handle));
            Assert.AreEqual(false, g3.added.Contains(e1.handle));
            Assert.AreEqual(false, g4.added.Contains(e1.handle));
            Assert.AreEqual(false, g5.added.Contains(e1.handle));
            Assert.AreEqual(false, g6.added.Contains(e1.handle));
            Assert.AreEqual(false, g7.added.Contains(e1.handle));
            Assert.AreEqual(false, g8.added.Contains(e1.handle));

            Assert.AreEqual(false, g1.added.Contains(e2.handle));
            Assert.AreEqual(false, g2.added.Contains(e2.handle));
            Assert.AreEqual(false, g3.added.Contains(e2.handle));
            Assert.AreEqual(false, g4.added.Contains(e2.handle));
            Assert.AreEqual(false, g5.added.Contains(e2.handle));
            Assert.AreEqual(false, g6.added.Contains(e2.handle));
            Assert.AreEqual(false, g7.added.Contains(e2.handle));
            Assert.AreEqual(false, g8.added.Contains(e2.handle));

            Assert.AreEqual(false, g1.added.Contains(e3.handle));
            Assert.AreEqual(false, g2.added.Contains(e3.handle));
            Assert.AreEqual(false, g3.added.Contains(e3.handle));
            Assert.AreEqual(false, g4.added.Contains(e3.handle));
            Assert.AreEqual(false, g5.added.Contains(e3.handle));
            Assert.AreEqual(false, g6.added.Contains(e3.handle));
            Assert.AreEqual(false, g7.added.Contains(e3.handle));
            Assert.AreEqual(false, g8.added.Contains(e3.handle));
        }
        #endregion //Add Entity


        #region Remove Entity
        [Test]
        public void WhenRemovedEntityIsInRemovedList()
        {
            //Setup
            //Init both entities
            Pool pool = new Pool();

            Group g1 = pool.getGroup(Signature.Get(typeof(Tag1)));
            Group g2 = pool.getGroup(Signature.Get(typeof(Tag2)));
            Group g3 = pool.getGroup(Signature.Get(typeof(Component1)));
            Group g4 = pool.getGroup(Signature.Get(typeof(Component2)));
            Group g5 = pool.getGroup(Signature.Get(typeof(Tag1), typeof(Component1)));
            Group g6 = pool.getGroup(Signature.Get(typeof(Tag2), typeof(Component2)));
            Group g7 = pool.getGroup(Signature.Get(typeof(Tag2), typeof(Component1)));
            Group g8 = pool.getGroup(Signature.Get(typeof(Tag1), typeof(Component2)));

            Entity e1 = pool.newEntity;
            Entity e2 = pool.newEntity;
            Entity e3 = pool.newEntity;

            e1.addComponent<Component1>();
            e1.setTag<Tag1>();
            e2.setTag<Tag2>();
            e2.addComponent<Component1>();
            e3.setTag<Tag1>();
            e3.addComponent<Component2>();

            pool.incrementGroups();

            e1.removeComponent<Component1>();
            e1.unsetTag<Tag1>();

            Assert.AreEqual(true, g1.removed.Contains(e1.handle));
            Assert.AreEqual(false, g2.removed.Contains(e1.handle));
            Assert.AreEqual(true, g3.removed.Contains(e1.handle));
            Assert.AreEqual(false, g4.removed.Contains(e1.handle));
            Assert.AreEqual(true, g5.removed.Contains(e1.handle));
            Assert.AreEqual(false, g6.removed.Contains(e1.handle));
            Assert.AreEqual(false, g7.removed.Contains(e1.handle));
            Assert.AreEqual(false, g8.removed.Contains(e1.handle));

            Assert.AreEqual(false, g1.removed.Contains(e2.handle));
            Assert.AreEqual(false, g2.removed.Contains(e2.handle));
            Assert.AreEqual(false, g3.removed.Contains(e2.handle));
            Assert.AreEqual(false, g4.removed.Contains(e2.handle));
            Assert.AreEqual(false, g5.removed.Contains(e2.handle));
            Assert.AreEqual(false, g6.removed.Contains(e2.handle));
            Assert.AreEqual(false, g7.removed.Contains(e2.handle));
            Assert.AreEqual(false, g8.removed.Contains(e2.handle));

            Assert.AreEqual(false, g1.removed.Contains(e3.handle));
            Assert.AreEqual(false, g2.removed.Contains(e3.handle));
            Assert.AreEqual(false, g3.removed.Contains(e3.handle));
            Assert.AreEqual(false, g4.removed.Contains(e3.handle));
            Assert.AreEqual(false, g5.removed.Contains(e3.handle));
            Assert.AreEqual(false, g6.removed.Contains(e3.handle));
            Assert.AreEqual(false, g7.removed.Contains(e3.handle));
            Assert.AreEqual(false, g8.removed.Contains(e3.handle));


            e2.unsetTag<Tag2>();
            e2.removeComponent<Component1>();

            Assert.AreEqual(true, g1.removed.Contains(e1.handle));
            Assert.AreEqual(false, g2.removed.Contains(e1.handle));
            Assert.AreEqual(true, g3.removed.Contains(e1.handle));
            Assert.AreEqual(false, g4.removed.Contains(e1.handle));
            Assert.AreEqual(true, g5.removed.Contains(e1.handle));
            Assert.AreEqual(false, g6.removed.Contains(e1.handle));
            Assert.AreEqual(false, g7.removed.Contains(e1.handle));
            Assert.AreEqual(false, g8.removed.Contains(e1.handle));

            Assert.AreEqual(false, g1.removed.Contains(e2.handle));
            Assert.AreEqual(true, g2.removed.Contains(e2.handle));
            Assert.AreEqual(true, g3.removed.Contains(e2.handle));
            Assert.AreEqual(false, g4.removed.Contains(e2.handle));
            Assert.AreEqual(false, g5.removed.Contains(e2.handle));
            Assert.AreEqual(false, g6.removed.Contains(e2.handle));
            Assert.AreEqual(true, g7.removed.Contains(e2.handle));
            Assert.AreEqual(false, g8.removed.Contains(e2.handle));

            Assert.AreEqual(false, g1.removed.Contains(e3.handle));
            Assert.AreEqual(false, g2.removed.Contains(e3.handle));
            Assert.AreEqual(false, g3.removed.Contains(e3.handle));
            Assert.AreEqual(false, g4.removed.Contains(e3.handle));
            Assert.AreEqual(false, g5.removed.Contains(e3.handle));
            Assert.AreEqual(false, g6.removed.Contains(e3.handle));
            Assert.AreEqual(false, g7.removed.Contains(e3.handle));
            Assert.AreEqual(false, g8.removed.Contains(e3.handle));


            e3.unsetTag<Tag1>();
            e3.removeComponent<Component2>();

            Assert.AreEqual(true, g1.removed.Contains(e1.handle));
            Assert.AreEqual(false, g2.removed.Contains(e1.handle));
            Assert.AreEqual(true, g3.removed.Contains(e1.handle));
            Assert.AreEqual(false, g4.removed.Contains(e1.handle));
            Assert.AreEqual(true, g5.removed.Contains(e1.handle));
            Assert.AreEqual(false, g6.removed.Contains(e1.handle));
            Assert.AreEqual(false, g7.removed.Contains(e1.handle));
            Assert.AreEqual(false, g8.removed.Contains(e1.handle));

            Assert.AreEqual(false, g1.removed.Contains(e2.handle));
            Assert.AreEqual(true, g2.removed.Contains(e2.handle));
            Assert.AreEqual(true, g3.removed.Contains(e2.handle));
            Assert.AreEqual(false, g4.removed.Contains(e2.handle));
            Assert.AreEqual(false, g5.removed.Contains(e2.handle));
            Assert.AreEqual(false, g6.removed.Contains(e2.handle));
            Assert.AreEqual(true, g7.removed.Contains(e2.handle));
            Assert.AreEqual(false, g8.removed.Contains(e2.handle));

            Assert.AreEqual(true, g1.removed.Contains(e3.handle));
            Assert.AreEqual(false, g2.removed.Contains(e3.handle));
            Assert.AreEqual(false, g3.removed.Contains(e3.handle));
            Assert.AreEqual(true, g4.removed.Contains(e3.handle));
            Assert.AreEqual(false, g5.removed.Contains(e3.handle));
            Assert.AreEqual(false, g6.removed.Contains(e3.handle));
            Assert.AreEqual(false, g7.removed.Contains(e3.handle));
            Assert.AreEqual(true, g8.removed.Contains(e3.handle));
        }
        [Test]
        public void WhenGroupIsSteppedRemovedListIsCleared()
        {
            //Setup
            //Init both entities
            Pool pool = new Pool();

            Group g1 = pool.getGroup(Signature.Get(typeof(Tag1)));
            Group g2 = pool.getGroup(Signature.Get(typeof(Tag2)));
            Group g3 = pool.getGroup(Signature.Get(typeof(Component1)));
            Group g4 = pool.getGroup(Signature.Get(typeof(Component2)));
            Group g5 = pool.getGroup(Signature.Get(typeof(Tag1), typeof(Component1)));
            Group g6 = pool.getGroup(Signature.Get(typeof(Tag2), typeof(Component2)));
            Group g7 = pool.getGroup(Signature.Get(typeof(Tag2), typeof(Component1)));
            Group g8 = pool.getGroup(Signature.Get(typeof(Tag1), typeof(Component2)));

            Entity e1 = pool.newEntity;
            Entity e2 = pool.newEntity;
            Entity e3 = pool.newEntity;

            e1.addComponent<Component1>();
            e1.setTag<Tag1>();
            e2.setTag<Tag2>();
            e2.addComponent<Component1>();
            e3.setTag<Tag1>();
            e3.addComponent<Component2>();

            pool.incrementGroups();

            e1.removeComponent<Component1>();
            e1.unsetTag<Tag1>();

            Assert.AreEqual(true, g1.removed.Contains(e1.handle));
            Assert.AreEqual(false, g2.removed.Contains(e1.handle));
            Assert.AreEqual(true, g3.removed.Contains(e1.handle));
            Assert.AreEqual(false, g4.removed.Contains(e1.handle));
            Assert.AreEqual(true, g5.removed.Contains(e1.handle));
            Assert.AreEqual(false, g6.removed.Contains(e1.handle));
            Assert.AreEqual(false, g7.removed.Contains(e1.handle));
            Assert.AreEqual(false, g8.removed.Contains(e1.handle));

            Assert.AreEqual(false, g1.removed.Contains(e2.handle));
            Assert.AreEqual(false, g2.removed.Contains(e2.handle));
            Assert.AreEqual(false, g3.removed.Contains(e2.handle));
            Assert.AreEqual(false, g4.removed.Contains(e2.handle));
            Assert.AreEqual(false, g5.removed.Contains(e2.handle));
            Assert.AreEqual(false, g6.removed.Contains(e2.handle));
            Assert.AreEqual(false, g7.removed.Contains(e2.handle));
            Assert.AreEqual(false, g8.removed.Contains(e2.handle));

            Assert.AreEqual(false, g1.removed.Contains(e3.handle));
            Assert.AreEqual(false, g2.removed.Contains(e3.handle));
            Assert.AreEqual(false, g3.removed.Contains(e3.handle));
            Assert.AreEqual(false, g4.removed.Contains(e3.handle));
            Assert.AreEqual(false, g5.removed.Contains(e3.handle));
            Assert.AreEqual(false, g6.removed.Contains(e3.handle));
            Assert.AreEqual(false, g7.removed.Contains(e3.handle));
            Assert.AreEqual(false, g8.removed.Contains(e3.handle));

            pool.incrementGroups();

            Assert.AreEqual(false, g1.removed.Contains(e1.handle));
            Assert.AreEqual(false, g2.removed.Contains(e1.handle));
            Assert.AreEqual(false, g3.removed.Contains(e1.handle));
            Assert.AreEqual(false, g4.removed.Contains(e1.handle));
            Assert.AreEqual(false, g5.removed.Contains(e1.handle));
            Assert.AreEqual(false, g6.removed.Contains(e1.handle));
            Assert.AreEqual(false, g7.removed.Contains(e1.handle));
            Assert.AreEqual(false, g8.removed.Contains(e1.handle));

            Assert.AreEqual(false, g1.removed.Contains(e2.handle));
            Assert.AreEqual(false, g2.removed.Contains(e2.handle));
            Assert.AreEqual(false, g3.removed.Contains(e2.handle));
            Assert.AreEqual(false, g4.removed.Contains(e2.handle));
            Assert.AreEqual(false, g5.removed.Contains(e2.handle));
            Assert.AreEqual(false, g6.removed.Contains(e2.handle));
            Assert.AreEqual(false, g7.removed.Contains(e2.handle));
            Assert.AreEqual(false, g8.removed.Contains(e2.handle));

            Assert.AreEqual(false, g1.removed.Contains(e3.handle));
            Assert.AreEqual(false, g2.removed.Contains(e3.handle));
            Assert.AreEqual(false, g3.removed.Contains(e3.handle));
            Assert.AreEqual(false, g4.removed.Contains(e3.handle));
            Assert.AreEqual(false, g5.removed.Contains(e3.handle));
            Assert.AreEqual(false, g6.removed.Contains(e3.handle));
            Assert.AreEqual(false, g7.removed.Contains(e3.handle));
            Assert.AreEqual(false, g8.removed.Contains(e3.handle));

            e2.unsetTag<Tag2>();
            e2.removeComponent<Component1>();

            Assert.AreEqual(false, g1.removed.Contains(e1.handle));
            Assert.AreEqual(false, g2.removed.Contains(e1.handle));
            Assert.AreEqual(false, g3.removed.Contains(e1.handle));
            Assert.AreEqual(false, g4.removed.Contains(e1.handle));
            Assert.AreEqual(false, g5.removed.Contains(e1.handle));
            Assert.AreEqual(false, g6.removed.Contains(e1.handle));
            Assert.AreEqual(false, g7.removed.Contains(e1.handle));
            Assert.AreEqual(false, g8.removed.Contains(e1.handle));

            Assert.AreEqual(false, g1.removed.Contains(e2.handle));
            Assert.AreEqual(true, g2.removed.Contains(e2.handle));
            Assert.AreEqual(true, g3.removed.Contains(e2.handle));
            Assert.AreEqual(false, g4.removed.Contains(e2.handle));
            Assert.AreEqual(false, g5.removed.Contains(e2.handle));
            Assert.AreEqual(false, g6.removed.Contains(e2.handle));
            Assert.AreEqual(true, g7.removed.Contains(e2.handle));
            Assert.AreEqual(false, g8.removed.Contains(e2.handle));

            Assert.AreEqual(false, g1.removed.Contains(e3.handle));
            Assert.AreEqual(false, g2.removed.Contains(e3.handle));
            Assert.AreEqual(false, g3.removed.Contains(e3.handle));
            Assert.AreEqual(false, g4.removed.Contains(e3.handle));
            Assert.AreEqual(false, g5.removed.Contains(e3.handle));
            Assert.AreEqual(false, g6.removed.Contains(e3.handle));
            Assert.AreEqual(false, g7.removed.Contains(e3.handle));
            Assert.AreEqual(false, g8.removed.Contains(e3.handle));

            pool.incrementGroups();

            Assert.AreEqual(false, g1.removed.Contains(e1.handle));
            Assert.AreEqual(false, g2.removed.Contains(e1.handle));
            Assert.AreEqual(false, g3.removed.Contains(e1.handle));
            Assert.AreEqual(false, g4.removed.Contains(e1.handle));
            Assert.AreEqual(false, g5.removed.Contains(e1.handle));
            Assert.AreEqual(false, g6.removed.Contains(e1.handle));
            Assert.AreEqual(false, g7.removed.Contains(e1.handle));
            Assert.AreEqual(false, g8.removed.Contains(e1.handle));

            Assert.AreEqual(false, g1.removed.Contains(e2.handle));
            Assert.AreEqual(false, g2.removed.Contains(e2.handle));
            Assert.AreEqual(false, g3.removed.Contains(e2.handle));
            Assert.AreEqual(false, g4.removed.Contains(e2.handle));
            Assert.AreEqual(false, g5.removed.Contains(e2.handle));
            Assert.AreEqual(false, g6.removed.Contains(e2.handle));
            Assert.AreEqual(false, g7.removed.Contains(e2.handle));
            Assert.AreEqual(false, g8.removed.Contains(e2.handle));

            Assert.AreEqual(false, g1.removed.Contains(e3.handle));
            Assert.AreEqual(false, g2.removed.Contains(e3.handle));
            Assert.AreEqual(false, g3.removed.Contains(e3.handle));
            Assert.AreEqual(false, g4.removed.Contains(e3.handle));
            Assert.AreEqual(false, g5.removed.Contains(e3.handle));
            Assert.AreEqual(false, g6.removed.Contains(e3.handle));
            Assert.AreEqual(false, g7.removed.Contains(e3.handle));
            Assert.AreEqual(false, g8.removed.Contains(e3.handle));

            e3.unsetTag<Tag1>();
            e3.removeComponent<Component2>();

            Assert.AreEqual(false, g1.removed.Contains(e1.handle));
            Assert.AreEqual(false, g2.removed.Contains(e1.handle));
            Assert.AreEqual(false, g3.removed.Contains(e1.handle));
            Assert.AreEqual(false, g4.removed.Contains(e1.handle));
            Assert.AreEqual(false, g5.removed.Contains(e1.handle));
            Assert.AreEqual(false, g6.removed.Contains(e1.handle));
            Assert.AreEqual(false, g7.removed.Contains(e1.handle));
            Assert.AreEqual(false, g8.removed.Contains(e1.handle));

            Assert.AreEqual(false, g1.removed.Contains(e2.handle));
            Assert.AreEqual(false, g2.removed.Contains(e2.handle));
            Assert.AreEqual(false, g3.removed.Contains(e2.handle));
            Assert.AreEqual(false, g4.removed.Contains(e2.handle));
            Assert.AreEqual(false, g5.removed.Contains(e2.handle));
            Assert.AreEqual(false, g6.removed.Contains(e2.handle));
            Assert.AreEqual(false, g7.removed.Contains(e2.handle));
            Assert.AreEqual(false, g8.removed.Contains(e2.handle));

            Assert.AreEqual(true, g1.removed.Contains(e3.handle));
            Assert.AreEqual(false, g2.removed.Contains(e3.handle));
            Assert.AreEqual(false, g3.removed.Contains(e3.handle));
            Assert.AreEqual(true, g4.removed.Contains(e3.handle));
            Assert.AreEqual(false, g5.removed.Contains(e3.handle));
            Assert.AreEqual(false, g6.removed.Contains(e3.handle));
            Assert.AreEqual(false, g7.removed.Contains(e3.handle));
            Assert.AreEqual(true, g8.removed.Contains(e3.handle));

            pool.incrementGroups();

            Assert.AreEqual(false, g1.removed.Contains(e1.handle));
            Assert.AreEqual(false, g2.removed.Contains(e1.handle));
            Assert.AreEqual(false, g3.removed.Contains(e1.handle));
            Assert.AreEqual(false, g4.removed.Contains(e1.handle));
            Assert.AreEqual(false, g5.removed.Contains(e1.handle));
            Assert.AreEqual(false, g6.removed.Contains(e1.handle));
            Assert.AreEqual(false, g7.removed.Contains(e1.handle));
            Assert.AreEqual(false, g8.removed.Contains(e1.handle));

            Assert.AreEqual(false, g1.removed.Contains(e2.handle));
            Assert.AreEqual(false, g2.removed.Contains(e2.handle));
            Assert.AreEqual(false, g3.removed.Contains(e2.handle));
            Assert.AreEqual(false, g4.removed.Contains(e2.handle));
            Assert.AreEqual(false, g5.removed.Contains(e2.handle));
            Assert.AreEqual(false, g6.removed.Contains(e2.handle));
            Assert.AreEqual(false, g7.removed.Contains(e2.handle));
            Assert.AreEqual(false, g8.removed.Contains(e2.handle));

            Assert.AreEqual(false, g1.removed.Contains(e3.handle));
            Assert.AreEqual(false, g2.removed.Contains(e3.handle));
            Assert.AreEqual(false, g3.removed.Contains(e3.handle));
            Assert.AreEqual(false, g4.removed.Contains(e3.handle));
            Assert.AreEqual(false, g5.removed.Contains(e3.handle));
            Assert.AreEqual(false, g6.removed.Contains(e3.handle));
            Assert.AreEqual(false, g7.removed.Contains(e3.handle));
            Assert.AreEqual(false, g8.removed.Contains(e3.handle));
        }
        #endregion //Remove Entity

        #region Null Change Entity
        [Test]
        public void WhenAddedAndRemovedNotInAnyLists()
        {
            //Setup
            //Init both entities
            Pool pool = new Pool();

            Group g1 = pool.getGroup(Signature.Get(typeof(Tag1)));
            Group g2 = pool.getGroup(Signature.Get(typeof(Tag2)));
            Group g3 = pool.getGroup(Signature.Get(typeof(Component1)));
            Group g4 = pool.getGroup(Signature.Get(typeof(Component2)));
            Group g5 = pool.getGroup(Signature.Get(typeof(Tag1), typeof(Component1)));
            Group g6 = pool.getGroup(Signature.Get(typeof(Tag2), typeof(Component2)));
            Group g7 = pool.getGroup(Signature.Get(typeof(Tag2), typeof(Component1)));
            Group g8 = pool.getGroup(Signature.Get(typeof(Tag1), typeof(Component2)));

            Entity e1 = pool.newEntity;
            Entity e2 = pool.newEntity;
            Entity e3 = pool.newEntity;

            e1.addComponent<Component1>();
            e1.setTag<Tag1>();
            e2.setTag<Tag2>();
            e2.addComponent<Component1>();
            e3.setTag<Tag1>();
            e3.addComponent<Component2>();

            e1.removeComponent<Component1>();
            e1.unsetTag<Tag1>();
            e2.unsetTag<Tag2>();
            e2.removeComponent<Component1>();
            e3.unsetTag<Tag1>();
            e3.removeComponent<Component2>();

            Assert.AreEqual(false, g1.removed.Contains(e1.handle));
            Assert.AreEqual(false, g2.removed.Contains(e1.handle));
            Assert.AreEqual(false, g3.removed.Contains(e1.handle));
            Assert.AreEqual(false, g4.removed.Contains(e1.handle));
            Assert.AreEqual(false, g5.removed.Contains(e1.handle));
            Assert.AreEqual(false, g6.removed.Contains(e1.handle));
            Assert.AreEqual(false, g7.removed.Contains(e1.handle));
            Assert.AreEqual(false, g8.removed.Contains(e1.handle));

            Assert.AreEqual(false, g1.removed.Contains(e2.handle));
            Assert.AreEqual(false, g2.removed.Contains(e2.handle));
            Assert.AreEqual(false, g3.removed.Contains(e2.handle));
            Assert.AreEqual(false, g4.removed.Contains(e2.handle));
            Assert.AreEqual(false, g5.removed.Contains(e2.handle));
            Assert.AreEqual(false, g6.removed.Contains(e2.handle));
            Assert.AreEqual(false, g7.removed.Contains(e2.handle));
            Assert.AreEqual(false, g8.removed.Contains(e2.handle));

            Assert.AreEqual(false, g1.removed.Contains(e3.handle));
            Assert.AreEqual(false, g2.removed.Contains(e3.handle));
            Assert.AreEqual(false, g3.removed.Contains(e3.handle));
            Assert.AreEqual(false, g4.removed.Contains(e3.handle));
            Assert.AreEqual(false, g5.removed.Contains(e3.handle));
            Assert.AreEqual(false, g6.removed.Contains(e3.handle));
            Assert.AreEqual(false, g7.removed.Contains(e3.handle));
            Assert.AreEqual(false, g8.removed.Contains(e3.handle));
        }
        #endregion //Null Change Entity
        #region Replaced Entity
        [Test]
        public void WhenReplacedEntityIsInReplacedList()
        {
            //Setup
            //Init both entities
            Pool pool = new Pool();

            Group g1 = pool.getGroup(Signature.Get(typeof(Tag1)));
            Group g2 = pool.getGroup(Signature.Get(typeof(Tag2)));
            Group g3 = pool.getGroup(Signature.Get(typeof(Component1)));
            Group g4 = pool.getGroup(Signature.Get(typeof(Component2)));
            Group g5 = pool.getGroup(Signature.Get(typeof(Tag1), typeof(Component1)));
            Group g6 = pool.getGroup(Signature.Get(typeof(Tag2), typeof(Component2)));
            Group g7 = pool.getGroup(Signature.Get(typeof(Tag2), typeof(Component1)));
            Group g8 = pool.getGroup(Signature.Get(typeof(Tag1), typeof(Component2)));

            Entity e1 = pool.newEntity;
            Entity e2 = pool.newEntity;
            Entity e3 = pool.newEntity;

            e1.addComponent<Component1>();
            e1.setTag<Tag1>();
            e2.setTag<Tag2>();
            e2.addComponent<Component1>();
            e3.setTag<Tag1>();
            e3.addComponent<Component2>();

            pool.incrementGroups();

            e1.removeComponent<Component1>();
            e1.unsetTag<Tag1>();

            Assert.AreEqual(true, g1.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e1.handle));
            Assert.AreEqual(true, g3.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e1.handle));
            Assert.AreEqual(true, g5.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e1.handle));

            Assert.AreEqual(false, g1.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g3.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e2.handle));

            Assert.AreEqual(false, g1.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g3.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e3.handle));


            e2.unsetTag<Tag2>();
            e2.removeComponent<Component1>();

            Assert.AreEqual(true, g1.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e1.handle));
            Assert.AreEqual(true, g3.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e1.handle));
            Assert.AreEqual(true, g5.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e1.handle));

            Assert.AreEqual(false, g1.replaced.Contains(e2.handle));
            Assert.AreEqual(true, g2.replaced.Contains(e2.handle));
            Assert.AreEqual(true, g3.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e2.handle));
            Assert.AreEqual(true, g7.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e2.handle));

            Assert.AreEqual(false, g1.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g3.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e3.handle));


            e3.unsetTag<Tag1>();
            e3.removeComponent<Component2>();

            Assert.AreEqual(true, g1.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e1.handle));
            Assert.AreEqual(true, g3.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e1.handle));
            Assert.AreEqual(true, g5.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e1.handle));

            Assert.AreEqual(false, g1.replaced.Contains(e2.handle));
            Assert.AreEqual(true, g2.replaced.Contains(e2.handle));
            Assert.AreEqual(true, g3.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e2.handle));
            Assert.AreEqual(true, g7.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e2.handle));

            Assert.AreEqual(true, g1.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g3.replaced.Contains(e3.handle));
            Assert.AreEqual(true, g4.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e3.handle));
            Assert.AreEqual(true, g8.replaced.Contains(e3.handle));
        }
        [Test]
        public void WhenGroupIsSteppedReplacedListIsCleared()
        {
            //Setup
            //Init both entities
            Pool pool = new Pool();

            Group g1 = pool.getGroup(Signature.Get(typeof(Tag1)));
            Group g2 = pool.getGroup(Signature.Get(typeof(Tag2)));
            Group g3 = pool.getGroup(Signature.Get(typeof(Component1)));
            Group g4 = pool.getGroup(Signature.Get(typeof(Component2)));
            Group g5 = pool.getGroup(Signature.Get(typeof(Tag1), typeof(Component1)));
            Group g6 = pool.getGroup(Signature.Get(typeof(Tag2), typeof(Component2)));
            Group g7 = pool.getGroup(Signature.Get(typeof(Tag2), typeof(Component1)));
            Group g8 = pool.getGroup(Signature.Get(typeof(Tag1), typeof(Component2)));

            Entity e1 = pool.newEntity;
            Entity e2 = pool.newEntity;
            Entity e3 = pool.newEntity;

            e1.addComponent<Component1>();
            e1.setTag<Tag1>();
            e2.setTag<Tag2>();
            e2.addComponent<Component1>();
            e3.setTag<Tag1>();
            e3.addComponent<Component2>();

            pool.incrementGroups();

            e1.removeComponent<Component1>();
            e1.unsetTag<Tag1>();

            Assert.AreEqual(true, g1.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e1.handle));
            Assert.AreEqual(true, g3.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e1.handle));
            Assert.AreEqual(true, g5.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e1.handle));

            Assert.AreEqual(false, g1.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g3.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e2.handle));

            Assert.AreEqual(false, g1.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g3.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e3.handle));

            pool.incrementGroups();

            Assert.AreEqual(false, g1.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g3.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e1.handle));

            Assert.AreEqual(false, g1.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g3.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e2.handle));

            Assert.AreEqual(false, g1.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g3.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e3.handle));

            e2.unsetTag<Tag2>();
            e2.removeComponent<Component1>();

            Assert.AreEqual(false, g1.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g3.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e1.handle));

            Assert.AreEqual(false, g1.replaced.Contains(e2.handle));
            Assert.AreEqual(true, g2.replaced.Contains(e2.handle));
            Assert.AreEqual(true, g3.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e2.handle));
            Assert.AreEqual(true, g7.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e2.handle));

            Assert.AreEqual(false, g1.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g3.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e3.handle));

            pool.incrementGroups();

            Assert.AreEqual(false, g1.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g3.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e1.handle));

            Assert.AreEqual(false, g1.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g3.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e2.handle));

            Assert.AreEqual(false, g1.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g3.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e3.handle));

            e3.unsetTag<Tag1>();
            e3.removeComponent<Component2>();

            Assert.AreEqual(false, g1.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g3.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e1.handle));

            Assert.AreEqual(false, g1.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g3.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e2.handle));

            Assert.AreEqual(true, g1.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g3.replaced.Contains(e3.handle));
            Assert.AreEqual(true, g4.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e3.handle));
            Assert.AreEqual(true, g8.replaced.Contains(e3.handle));

            pool.incrementGroups();

            Assert.AreEqual(false, g1.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g3.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e1.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e1.handle));

            Assert.AreEqual(false, g1.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g3.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e2.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e2.handle));

            Assert.AreEqual(false, g1.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g2.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g3.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g4.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g5.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g6.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g7.replaced.Contains(e3.handle));
            Assert.AreEqual(false, g8.replaced.Contains(e3.handle));
        }
        #endregion //Replace Entity
    }
}