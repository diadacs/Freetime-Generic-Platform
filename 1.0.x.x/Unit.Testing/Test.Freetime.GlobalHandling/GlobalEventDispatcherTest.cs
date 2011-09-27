using System;
using Freetime.GlobalHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Freetime.GlobalHandling
{
    /// <summary>
    /// Unit Test for Freetime.GlobalHandling.GlobalEventDispatcher
    /// </summary>
    /// <Assembly>Freetime.GlobalHandling</Assembly>
    /// <Class>Freetime.GlobalHandling.GlobalEventDispatcher</Class>
    [TestClass]
    public class GlobalEventDispatcherTest
    {
        private const string EVENT_NAME = "TestEvent";

        /// <summary>
        /// Expected : Added Event exists
        /// </summary>
        [TestMethod]
        public void AddEventHandlerTest()
        {
            GlobalEventDispatcher.AddEventHandler(EVENT_NAME, TestEvent);
            Assert.IsTrue(GlobalEventDispatcher.HasEvent(EVENT_NAME));
        }

        [TestMethod]
        public void RaiseEventTest()
        {
            if(GlobalEventDispatcher.HasEvent(EVENT_NAME))
                GlobalEventDispatcher.RaiseEvent(EVENT_NAME, this, EventArgs.Empty);
            
        }

        private void TestEvent(object sender, EventArgs e)
        {
            Assert.IsTrue(true);
        }
    }
}
