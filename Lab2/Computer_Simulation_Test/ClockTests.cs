using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading;

namespace Computer_Simulation.Tests
{
    [TestClass]
    public class ClockTests
    {
        private Clock clock;

        [TestInitialize]
        public void TestInitialize()
        {
            clock = new Clock();
        }

        [TestMethod]
        public void LaunchApp_Test()
        {
            bool eventTriggered = false;
            clock.appStateUpdated += () => eventTriggered = true;
            clock.LaunchApp();
            clock.appStateUpdated?.Invoke();
            Assert.IsTrue(eventTriggered);
        }

        [TestMethod]
        public void CloseApp_Test()
        {
            clock.CloseApp();
        }

        [TestMethod]
        public void HandleInputTime_Test()
        {
            clock.HandleInput("time");
            var clockMode = clock.GetType().GetField("clockMode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(clock);
            Assert.AreEqual(1, clockMode);
        }

        [TestMethod]
        public void HandleInputStopwatchStart_Test()
        {
            clock.HandleInput("stopwatch start");
            var stopwatch = (Stopwatch)clock.GetType().GetField("stopwatch", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(clock);
            Assert.IsTrue(stopwatch.IsRunning);
        }

        [TestMethod]
        public void HandleInputStopwatchStop_Test()
        {
            clock.HandleInput("stopwatch start");
            var stopwatch = (Stopwatch)clock.GetType().GetField("stopwatch", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(clock);
            Assert.IsTrue(stopwatch.IsRunning);
            clock.HandleInput("stopwatch stop");
            Assert.IsFalse(stopwatch.IsRunning);
            Assert.AreEqual(0, stopwatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void HandleInputTimerStart_Test()
        {
            clock.HandleInput("timer start 5");
            var timer = (CustomTimer)clock.GetType().GetField("timer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(clock);
            Assert.IsNotNull(timer);
            var remainingTime = timer.GetRemainingTime();
            Assert.IsTrue(remainingTime.TotalSeconds <= 5 && remainingTime.TotalSeconds > 0);
        }

        [TestMethod]
        public void HandleInputTimerStop_Test()
        {
            clock.HandleInput("timer start 5");
            var timer = (CustomTimer)clock.GetType().GetField("timer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(clock);
            Assert.IsNotNull(timer);
            clock.HandleInput("timer stop");
            string remainingTime = timer.GetRemainingTime().ToString();
            Assert.AreEqual(remainingTime, timer.GetRemainingTime().ToString());
        }

        [TestMethod]
        public void UpdateAppRepresentation_Test()
        {
            string expectedTimeData = "Test Time";
            var timeDataField = clock.GetType().GetField("timeData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            timeDataField.SetValue(clock, expectedTimeData);
            var updateAppRepresentationMethod = clock.GetType().GetMethod("UpdateAppRepresentation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            updateAppRepresentationMethod.Invoke(clock, null);
        }
    }
}
