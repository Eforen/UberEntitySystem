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
    public class PoolTests
    {
        [Test]
        public void InstantiationTest()
        {
            new Pool();
        }

        #region Entity

        [Test]
        public void GetEntity()
        {
            //Setup
            //Init both entities
            Pool pool = new Pool();
            Entity e1 = pool.newEntity;
            Entity e2 = pool.newEntity;

            //Test
            // Check not same
            Assert.NotNull(e1);
            Assert.NotNull(e2);
            Assert.AreNotSame(e1, e2);
        }

        [Test]
        public void GetEntityPool()
        {
            //Setup
            //Init both entities
            Pool pool = new Pool();
            Entity e1 = pool.newEntity;
            Entity e2 = pool.newEntity;

            //Test
            Assert.AreSame(pool, e1.pool);
            Assert.AreSame(pool, e2.pool);
        }

        [Test]
        public void SetEntityPool()
        {
            //Setup
            //Init both entities
            Pool pool1 = new Pool();
            Pool pool2 = new Pool();
            Entity e1 = pool1.newEntity;
            Entity e2 = pool1.newEntity;

            //assure Test
            Assert.AreSame(pool1, e1.pool);
            Assert.AreSame(pool1, e2.pool);

            //Set & Check
            e1.setPool(pool2);

            Assert.AreSame(pool2, e1.pool);
            Assert.AreSame(pool1, e2.pool);

            e2.setPool(pool2);
            Assert.AreSame(pool2, e1.pool);
            Assert.AreSame(pool2, e2.pool);
        }

        [Test]
        public void HasEntity()
        {
            //Setup
            //Init both entities
            Pool pool = new Pool();
            Entity e1 = pool.newEntity;
            Entity e2 = pool.newEntity;

            //Test
            // Check not same
            Assert.NotNull(e1);
            Assert.NotNull(e2);
            Assert.AreNotSame(e1, e2);

            // Check count
            Assert.AreEqual(2, pool.Count);
            Assert.IsTrue(pool.has(e1));
            Assert.IsTrue(pool.has(e2));
        }

        [Test]
        public void RemoveEntity()
        {
            //Setup
            //Init both entities
            Pool pool = new Pool();
            Entity e1 = pool.newEntity;
            Entity e2 = pool.newEntity;

            Assert.AreEqual(2, pool.Count);
            Assert.IsTrue(pool.has(e1));
            Assert.IsTrue(pool.has(e2));

            //Test
            Factory.Cache<Entity>(e1);

            Assert.AreEqual(1, pool.Count);
            Assert.IsFalse(pool.has(e1));
            Assert.IsTrue(pool.has(e2));
            Assert.IsNull(e1.pool);
        }

        [Test]
        public void GetEntityFromCache()
        {
            //Setup
            //Init both entities
            Pool pool = new Pool();
            Entity e1 = pool.newEntity;

            Factory.Cache<Entity>(e1); //cache the first entity

            Entity e2 = pool.newEntity;

            //Test
            // Check is the same
            Assert.NotNull(e1);
            Assert.NotNull(e2);
            Assert.AreSame(e1, e2);
        }

        [Test]
        public void Indexer()
        {
            Pool pool = new Pool();
            Handle[] entities = { pool.newEntity.handle, pool.newEntity.handle, pool.newEntity.handle, pool.newEntity.handle, pool.newEntity.handle, pool.newEntity.handle };

            Assert.AreEqual(6, pool.Count);
            for (int i = 0; i < pool.Count; i++)
            {
                Assert.AreSame(entities[i], pool[i]);
            }
        }

        #endregion //Entity

    }
}