using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Warehouse.Modules;
using WarehouseTest.Helpers;

namespace WarehouseTest.FunctionApps
{
    [TestClass]
    public class NextRunTest : GenericTest
    {

        /// <summary>
        /// An example on a module must that schall be called every day between 12:00 and 13:00.
        /// The functionApp starts every hour, but it is not certain when.
        /// Only run if time is between 12:00 - 13:00 AND last run is earlier than yesterday at 13:00
        /// Deduction:
        ///     Time between 12 and 13.
        ///     Last run must not have ben called earlier then 23 hours ago.
        /// </summary>
        [TestMethod]
        public void TestRunPerDay()
        {
            var expression = "0 12 * * *";
            var nextRun = new NextRun("Call_modules_each_hour");
            nextRun.PurgeLog();
            
            nextRun.SetLastRun(new DateTime(2021, 1, 9, 13, 01, 0));
            Assert.IsFalse(nextRun.DoRun(new DateTime(2021, 1, 10, 12, 55, 0), expression));

            nextRun.SetLastRun(new DateTime(2021, 1, 9, 12, 00, 0));
            Assert.IsTrue(nextRun.DoRun(new DateTime(2021, 1, 10, 12, 55, 0), expression));

            nextRun.SetLastRun(new DateTime(2021, 1, 8, 11, 10, 0));
            Assert.IsTrue(nextRun.DoRun(new DateTime(2021, 1, 10, 12, 55, 0), expression));

            nextRun.SetLastRun(new DateTime(2021, 1, 8, 12, 10, 0));
            Assert.IsTrue(nextRun.DoRun(new DateTime(2021, 1, 10, 12, 55, 0), expression));

            nextRun.SetLastRun(new DateTime(2021, 1, 9, 12, 10, 0));
            Assert.IsTrue( nextRun.DoRun(new DateTime(2021, 1, 10, 12, 55, 0), expression));

            Assert.IsFalse(nextRun.DoRun(new DateTime(2021, 1, 10, 11, 55, 0), expression));
        }

        [TestMethod]
        public void TestRunPerHour()
        {
            var expression = "0 * * * *";
            var nextRun = new NextRun("Call_modules_each_hour");
            nextRun.PurgeLog();

            nextRun.SetLastRun(new DateTime(2021, 1, 10, 11, 01, 0));
            Assert.IsTrue(nextRun.DoRun(new DateTime(2021, 1, 10, 12, 55, 0), expression));  //It's over an hour since last run

            nextRun.SetLastRun(new DateTime(2021, 1, 10, 12, 01, 0));
            Assert.IsFalse(nextRun.DoRun(new DateTime(2021, 1, 10, 12, 55, 0), expression));  //It's under an hour since last run

            nextRun.SetLastRun(new DateTime(2021, 1, 10, 11, 01, 0));
            Assert.IsFalse(nextRun.DoRun(new DateTime(2021, 1, 10, 12, 00, 0), expression));  //It's under an hour since last run
        }

        [TestMethod]
        public void TestBasicFunctionsOnLogToJsonFile()
        {
            var dateTime = DateTime.Now;
            var nextRun = new NextRun("Test");
            nextRun.PurgeLog();
            Assert.AreEqual(nextRun.GetLastRun(), DateTime.MinValue);
            nextRun.SetLastRun(dateTime);
            Assert.AreEqual(nextRun.GetLastRun(), dateTime);
            Assert.AreEqual(new NextRun("Test2").GetLastRun(), DateTime.MinValue);
            nextRun.PurgeLog();
            Assert.AreEqual(nextRun.GetLastRun(), DateTime.MinValue);
        }
    }
}
