using System;
using System.Linq;
using DoStufLib.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoStufLibTest
{
    [TestClass]
    public class ActivityTest
    {
        [TestMethod]
        public void GetActivities_FindsActivities()
        {
            var activities = Activity.GetActivities();
            var count = activities.Count();
            Console.WriteLine($"COUNT: {count}");
            Assert.IsTrue(count > 0);
        }
    }
}