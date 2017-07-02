using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberEntitySystemCore;

namespace UberEntitySystemCoreTests.Tutorial_3
{
    using NUnit.Framework;

    public class Component1 : IComponent { }

    public class Component2 : IComponent { }
    public class Component3 : IComponent { }
    public class Component4 : IComponent { }
    public class NotComponent { }

    [TestFixture]
    public class ComponentInfoTests
    {
        [Test]
        public void GetIdGeneric()
        {
            //Setup
            int c1 = ComponentInfo.getID<Component1>();
            int c2 = ComponentInfo.getID<Component2>();
            int c3 = ComponentInfo.getID<Component3>();
            int c4 = ComponentInfo.getID<Component4>();

            //Test
            Assert.AreNotEqual(c1, c2);
            Assert.AreNotEqual(c1, c3);
            Assert.AreNotEqual(c1, c4);

            Assert.AreNotEqual(c2, c3);
            Assert.AreNotEqual(c2, c4);

            Assert.AreNotEqual(c3, c4);
        }

        [Test]
        public void GetIdDynamic()
        {
            //Setup
            int c1 = ComponentInfo.getID(typeof(Component1));
            int c2 = ComponentInfo.getID(typeof(Component2));
            int c3 = ComponentInfo.getID(typeof(Component3));
            int c4 = ComponentInfo.getID(typeof(Component4));

            //Test
            Assert.AreNotEqual(c1, c2);
            Assert.AreNotEqual(c1, c3);
            Assert.AreNotEqual(c1, c4);

            Assert.AreNotEqual(c2, c3);
            Assert.AreNotEqual(c2, c4);

            Assert.AreNotEqual(c3, c4);
        }

        [Test]
        public void GetIdDynamicThrowErrorForNoncomponent()
        {
            //Test
            Assert.Throws(
                typeof(KeyNotFoundException),
                () => { ComponentInfo.getID(typeof(NotComponent)); }
            );
        }



        [Test]
        public void GetType()
        {
            //Setup
            int c1 = ComponentInfo.getID<Component1>();
            int c2 = ComponentInfo.getID<Component2>();
            int c3 = ComponentInfo.getID<Component3>();
            int c4 = ComponentInfo.getID<Component4>();

            //validate State
            Assert.AreNotEqual(c1, c2, "Setup Failed", null);
            Assert.AreNotEqual(c1, c3, "Setup Failed", null);
            Assert.AreNotEqual(c1, c4, "Setup Failed", null);

            Assert.AreNotEqual(c2, c3, "Setup Failed", null);
            Assert.AreNotEqual(c2, c4, "Setup Failed", null);

            Assert.AreNotEqual(c3, c4, "Setup Failed", null);

            //Finish test setup
            Type t1 = ComponentInfo.getType(c1);
            Type t2 = ComponentInfo.getType(c2);
            Type t3 = ComponentInfo.getType(c3);
            Type t4 = ComponentInfo.getType(c4);

            //Run Test
            Assert.AreEqual(typeof(Component1), t1);
            Assert.AreEqual(typeof(Component2), t2);
            Assert.AreEqual(typeof(Component3), t3);
            Assert.AreEqual(typeof(Component4), t4);
        }

        [Test]
        public void Export()
        {
            //Setup
            int c1 = ComponentInfo.getID<Component1>();
            int c2 = ComponentInfo.getID<Component2>();
            int c3 = ComponentInfo.getID<Component3>();
            int c4 = ComponentInfo.getID<Component4>();

            //validate State
            Assert.AreNotEqual(c1, c2, "Setup Failed", null);
            Assert.AreNotEqual(c1, c3, "Setup Failed", null);
            Assert.AreNotEqual(c1, c4, "Setup Failed", null);

            Assert.AreNotEqual(c2, c3, "Setup Failed", null);
            Assert.AreNotEqual(c2, c4, "Setup Failed", null);

            Assert.AreNotEqual(c3, c4, "Setup Failed", null);

            //Get State
            KeyValuePair<int, Type>[] export = ComponentInfo.export;

            //Test
            foreach(KeyValuePair<int, Type> pair in export)
            {
                if (pair.Value == typeof(Component1))
                    Assert.AreEqual(c1, ComponentInfo.getID<Component1>());
                if (pair.Value == typeof(Component2))
                    Assert.AreEqual(c2, ComponentInfo.getID<Component2>());
                if (pair.Value == typeof(Component3))
                    Assert.AreEqual(c3, ComponentInfo.getID<Component3>());
                if (pair.Value == typeof(Component4))
                    Assert.AreEqual(c4, ComponentInfo.getID<Component4>());
            }
        }

        [Test]
        public void ImportReorder()
        {
            //Setup
            int c1 = ComponentInfo.getID<Component4>();
            int c2 = ComponentInfo.getID<Component3>();
            int c3 = ComponentInfo.getID<Component2>();
            int c4 = ComponentInfo.getID<Component1>();

            //validate State
            Assert.AreNotEqual(c1, c2, "Setup Failed", null);
            Assert.AreNotEqual(c1, c3, "Setup Failed", null);
            Assert.AreNotEqual(c1, c4, "Setup Failed", null);

            Assert.AreNotEqual(c2, c3, "Setup Failed", null);
            Assert.AreNotEqual(c2, c4, "Setup Failed", null);

            Assert.AreNotEqual(c3, c4, "Setup Failed", null);

            //Create New State
            KeyValuePair<int, Type>[] state = new KeyValuePair<int, Type>[4];
            state[0] = new KeyValuePair<int, Type>(c1, typeof(Component1));
            state[1] = new KeyValuePair<int, Type>(c2, typeof(Component2));
            state[2] = new KeyValuePair<int, Type>(c3, typeof(Component3));
            state[3] = new KeyValuePair<int, Type>(c4, typeof(Component4));

            //Setup New State
            ComponentInfo.import = state;

            //Check the correct IDs are now set
            Assert.AreEqual(c1, ComponentInfo.getID<Component1>());
            Assert.AreEqual(c2, ComponentInfo.getID<Component2>());
            Assert.AreEqual(c3, ComponentInfo.getID<Component3>());
            Assert.AreEqual(c4, ComponentInfo.getID<Component4>());

            //Check all the tests still work as expected.
            //Validate IDs from Generic
            GetIdGeneric();
            //Validate IDs from Dynamic
            GetIdDynamic();
            //Validate Types from Ids
            GetType();
        }

        [Test]
        public void Reset()
        {
            //setup
            //Get Defaults
            int c1 = ComponentInfo.getID<Component1>();
            int c2 = ComponentInfo.getID<Component2>();
            int c3 = ComponentInfo.getID<Component3>();
            int c4 = ComponentInfo.getID<Component4>();
            
            //Corrupt the state
            KeyValuePair<int, Type>[] state = new KeyValuePair<int, Type>[4];
            state[0] = new KeyValuePair<int, Type>(c4, typeof(Component1));
            state[1] = new KeyValuePair<int, Type>(c3, typeof(Component2));
            state[2] = new KeyValuePair<int, Type>(c2, typeof(Component3));
            state[3] = new KeyValuePair<int, Type>(c1, typeof(Component4));

            //Set Corrupted State
            ComponentInfo.import = state;
            
            //Check the IDs are now "corrupted"
            Assert.AreEqual(c4, ComponentInfo.getID<Component1>());
            Assert.AreEqual(c3, ComponentInfo.getID<Component2>());
            Assert.AreEqual(c2, ComponentInfo.getID<Component3>());
            Assert.AreEqual(c1, ComponentInfo.getID<Component4>());

            //Reset state
            ComponentInfo.reset();

            //Test
            //Check the IDs are now "fixed"
            Assert.AreEqual(c1, ComponentInfo.getID<Component1>());
            Assert.AreEqual(c2, ComponentInfo.getID<Component2>());
            Assert.AreEqual(c3, ComponentInfo.getID<Component3>());
            Assert.AreEqual(c4, ComponentInfo.getID<Component4>());
        }

        [TearDown]
        public void cleanup()
        {
            ComponentInfo.reset();
        }
    }
}
