using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DatabaseManipulatorTest
{
    [TestClass]
    public class UpdatedDataTest
    {
        [TestMethod]
        public void Update_Test()
        {
            int expectedAge = 30;

            var actual = new DatabaseManipulator.RootObject();
            actual.ConnectionString = "";
            

        }
    }
}
