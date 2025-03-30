using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Registration;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void AuthTest()
        {
            var page = new LoginWindow();
            Assert.IsTrue(page.Auth("Kostya", "Alex"));
            Assert.IsFalse(page.Auth("BB", "SS"));
            Assert.IsFalse(page.Auth("", ""));
            Assert.IsFalse(page.Auth(" ", " "));
            
        }
    }
}
