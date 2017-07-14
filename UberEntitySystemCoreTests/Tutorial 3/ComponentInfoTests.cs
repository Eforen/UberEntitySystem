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
    using System.Diagnostics;

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

        [Test]
        [Theory]
        public void TypeLookupVsStringLookup([Values(12857982)]int seed, [Values(5, 50, 500, 5000, 50000, 500000, 5000000)]int benchmarkCount)
        {
            Random rand = new Random(seed); //Get Specific Rand
            Type[] typeLookups = { typeof(Component1), typeof(Component2), typeof(Component3), typeof(Component4) };
            string[] stringLookups = { "Component1", "Component2", "Component3", "Component4" };
            int[] intLookups = { 0, 1, 2, 3 };

            Dictionary<Type, int> listByTypes = new Dictionary<Type, int>(); //Setup Type List
            listByTypes.Add(typeLookups[0], 0);
            listByTypes.Add(typeLookups[1], 0);
            listByTypes.Add(typeLookups[2], 0);
            listByTypes.Add(typeLookups[3], 0);

            GC.Collect(); //Force garbage collection right now!
            Stopwatch swTypeList = Stopwatch.StartNew(); //Start the watch
            for (int i = 0; i < benchmarkCount; i++)
            {
                listByTypes[typeLookups[rand.Next(3)]] = rand.Next();
            }
            swTypeList.Stop();





            rand = new Random(seed); //Reset to Specific Rand
            Dictionary<String, int> listByString = new Dictionary<String, int>(); //Setup Type List
            listByString.Add(stringLookups[0], 0);
            listByString.Add(stringLookups[1], 0);
            listByString.Add(stringLookups[2], 0);
            listByString.Add(stringLookups[3], 0);

            GC.Collect(); //Force garbage collection right now!
            Stopwatch swStringList = Stopwatch.StartNew(); //Start the watch
            for (int i = 0; i < benchmarkCount; i++)
            {
                listByString[stringLookups[rand.Next(3)]] = rand.Next();
            }
            swStringList.Stop();





            rand = new Random(seed); //Reset to Specific Rand
            Dictionary<int, int> listByInt = new Dictionary<int, int>(); //Setup Type List
            listByInt.Add(intLookups[0], 0);
            listByInt.Add(intLookups[1], 0);
            listByInt.Add(intLookups[2], 0);
            listByInt.Add(intLookups[3], 0);

            GC.Collect(); //Force garbage collection right now!
            Stopwatch swIntList = Stopwatch.StartNew(); //Start the watch
            for (int i = 0; i < benchmarkCount; i++)
            {
                listByInt[intLookups[rand.Next(3)]] = rand.Next();
            }
            swIntList.Stop();

            Assert.Pass("typeLookups: {0}\nstringLookups: {1}\nintLookups: {2}", swTypeList.ElapsedMilliseconds, swStringList.ElapsedMilliseconds, swIntList.ElapsedMilliseconds);
        }

        [TearDown]
        public void cleanup()
        {
            ComponentInfo.reset();
        }
    }
}
