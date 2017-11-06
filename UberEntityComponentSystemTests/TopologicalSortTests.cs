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
    public class TopologicalTests
    {
        #region Classes

        public class Item
        {
            public string Name { get; private set; }
            public Item[] Dependencies { get; private set; }

            public Item(string name, params Item[] dependencies)
            {
                Name = name;
                Dependencies = dependencies;
            }
        }

        #endregion // Classes

        #region Tests

        [Test]
        public void Test()
        {
            var a = new Item("A");
            var c = new Item("C");
            var f = new Item("F");
            var h = new Item("H");
            var d = new Item("D", a);
            var g = new Item("G", f, h);
            var e = new Item("E", d, g);
            var b = new Item("B", c, e);

            var unsorted = new[] { a, b, c, d, e, f, g, h };

            var sorted = TopologicalSort.Sort(unsorted, x => x.Dependencies);

            string output = "";
            foreach (Item i in sorted)
            {
                output += i.Name;
            }

            Assert.AreEqual("ACDFHGEB", output);
        }

        [Test]
        public void Test2()
        {
            var a = new Item("A");
            var b = new Item("C", a);
            var c = new Item("F", a, b);
            var d = new Item("H", a, b, c);
            var e = new Item("D", a, b, c, d);
            var f = new Item("G", a, b, c, d, e);
            var g = new Item("E", a, b, c, d, e, f);
            var h = new Item("B", a, b, c, d, e, f, g);

            var unsorted = new[] { a, c, d, f, h, g, e, b };

            var sorted = TopologicalSort.Sort(unsorted, x => x.Dependencies);

            string output = "";
            foreach (Item i in sorted)
            {
                output += i.Name;
            }

            Assert.AreEqual("ABCDEFGH", output);
        }

        #endregion // Tests

    }
}