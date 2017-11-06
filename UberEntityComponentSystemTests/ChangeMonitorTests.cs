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
    using System.Reflection;

    [TestClass()]
    public class ChangeMonitorTests
    {
        //TODO: add this to components bubble up was changed to entities
        #region Classes

        public class IgnoreChanges : Attribute { }

        public class Wub
        {
            protected Dictionary<PropertyInfo, object> _propertyChangelog = new Dictionary<PropertyInfo, object>();
            //protected Dictionary<PropertyInfo, bool> _propertyChanges = new Dictionary<PropertyInfo, bool>();
            protected Dictionary<FieldInfo, object> _fieldChangelog = new Dictionary<FieldInfo, object>();
            protected Dictionary<MemberInfo, bool> _changes = new Dictionary<MemberInfo, bool>();

            public Wub()
            {
                //Fields
                FieldInfo[] fi = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
                foreach (FieldInfo f in fi)
                {
                    if (f.GetCustomAttribute<IgnoreChanges>() != null)
                        continue; //Skip this one and go to the next because its marked as ignore
                    _fieldChangelog.Add(f, f.GetValue(this));
                    _changes.Add(f, true);
                }

                //Properties
                PropertyInfo[] pi = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo p in pi)
                {
                    if (p.GetCustomAttribute<IgnoreChanges>() != null)
                        continue; //Skip this one and go to the next because its marked as ignore
                    _propertyChangelog.Add(p, p.GetValue(this));
                    _changes.Add(p, true);
                }

                //Set Default
                wasChanged = true;
            }

            [IgnoreChanges]
            public bool wasChanged { get; private set; }

            public bool checkForChanges()
            {
                bool changed = false;
                
                KeyValuePair<FieldInfo, object> f;
                for (int i = 0; i < _fieldChangelog.Count; i++)
                {
                    f = _fieldChangelog.ElementAt(i); // Get Pair
                    if (f.Value.Equals(f.Key.GetValue(this)))
                    {
                        if (_changes[f.Key] == true) _changes[f.Key] = false;
                    }
                    else
                    {
                        //looks to have changed
                        _fieldChangelog[f.Key] = f.Key.GetValue(this);
                        if (_changes[f.Key] == false) _changes[f.Key] = true;
                        if (changed == false) changed = true;
                    }
                }

                KeyValuePair<PropertyInfo, object> p;
                for (int i = 0; i < _propertyChangelog.Count; i++)
                {
                    p = _propertyChangelog.ElementAt(i); // Get Pair
                    if (p.Value.Equals(p.Key.GetValue(this)))
                    {
                        if (_changes[p.Key] == true) _changes[p.Key] = false;
                    }
                    else
                    {
                        //looks to have changed
                        _propertyChangelog[p.Key] = p.Key.GetValue(this);
                        if (_changes[p.Key] == false) _changes[p.Key] = true;
                        if (changed == false) changed = true;
                    }
                }

                wasChanged = changed;
                return changed;
            }
        }

        public class Dub: Wub
        {
            public float a { get; set; }
            public float b;
            public string yolo = "yolo";
        }


        #endregion // Classes

        #region Tests

        [Theory]
        public void Test()
        {
            Dub[] dubs = { new Dub(), new Dub(), new Dub(), new Dub(), new Dub(), new Dub() };

            for (int d = 0; d < dubs.Length; d++)
            {
                Assert.IsTrue(dubs[d].wasChanged);
                dubs[d].checkForChanges();
            }

            float[] a = { 5, 3, 0, 6, 7, 8 };
            bool[] changes = { true, true, false, true, true, true };
            for (int d = 0; d < dubs.Length; d++)
            {
                Assert.IsFalse(dubs[d].wasChanged);
                dubs[d].a = a[d];
                dubs[d].checkForChanges();
                Assert.AreEqual(changes[d], dubs[d].wasChanged);
            }

            for (int d = 0; d < dubs.Length; d++)
            {
                Assert.AreEqual(changes[d], dubs[d].wasChanged);
            }



            for (int d = 0; d < dubs.Length; d++)
            {
                dubs[d].checkForChanges();
                Assert.IsFalse(dubs[d].wasChanged);
            }



            float[] b = { 0, 15, 6213, 15, 0, 0 };
            changes = new bool[] { false, true, true, true, false, false };
            for (int d = 0; d < dubs.Length; d++)
            {
                dubs[d].b = b[d];
                dubs[d].checkForChanges();
                Assert.AreEqual(changes[d], dubs[d].wasChanged);
            }

            for (int d = 0; d < dubs.Length; d++)
            {
                Assert.AreEqual(changes[d], dubs[d].wasChanged);
            }

            string[] y = { "y", "yolo", "as", "yolo", "7", "8" };
            changes = new bool[]{ true, false, true, false, true, true };
            for (int d = 0; d < dubs.Length; d++)
            {
                dubs[d].yolo = y[d];
                dubs[d].checkForChanges();
                Assert.AreEqual(changes[d], dubs[d].wasChanged);
            }

            for (int d = 0; d < dubs.Length; d++)
            {
                Assert.AreEqual(changes[d], dubs[d].wasChanged);
            }
        }

        #endregion // Tests

    }
}