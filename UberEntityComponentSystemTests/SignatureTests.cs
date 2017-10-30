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
    public class SignatureTests
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

        #endregion //Object Setup

        #region Tests
        [Test]
        public void Generics()
        {
        }
        #endregion //Tests

    }
}