using System.Linq;
using DoStufLib;
using DoStufLib.ActivityTasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoStufLibTest
{
    [TestClass]
    public class ActivityTaskListTest
    {
        [TestMethod]
        public void TestDefaultTypes()
        {
            var expectedTypes = new[] {typeof (CommandTask)};
            Assert.AreEqual(expectedTypes.Length, ActivityTaskList.Instance.Count());
            Assert.IsTrue(expectedTypes.All(ActivityTaskList.Instance.Contains));
            Assert.IsTrue(ActivityTaskList.Instance.All(expectedTypes.Contains));
        }
    }
}