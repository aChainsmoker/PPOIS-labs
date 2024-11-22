using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Computer_Simulation.Tests
{
    [TestClass]
    public class DesktopTests
    {
        private OS os;
        private Desktop desktop;
        private Wallpapers wallpapersApp;
        private string mainDir;

        [TestInitialize]
        public void TestInitialize()
        {
            os = new OS();
            desktop = os.Desktop;
            wallpapersApp = new Wallpapers();
            mainDir = Directory.GetCurrentDirectory();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Desktop.testKeySequence.Clear();
            Directory.SetCurrentDirectory(mainDir);
        }

        [TestMethod]
        public void CheckOnWallpapersApp_Test()
        {
            os.InstalledApps.Add(wallpapersApp);
            desktop.CheckOnWallpapersApp();
            Assert.IsNotNull(desktop.GetType().GetField("wallpapersApp", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(desktop));
        }

        [TestMethod]
        public void HandleInputChangeSelectedAppIndex_Test()
        {
            Desktop.testKeySequence = new List<ConsoleKey> { ConsoleKey.DownArrow, ConsoleKey.UpArrow };
            os.InstalledApps.Add(wallpapersApp);

            desktop.HandleInput(null);
            var indexAfterDown = desktop.GetType().GetField("selectedAppIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(desktop);

            desktop.HandleInput(null);
            var indexAfterUp = desktop.GetType().GetField("selectedAppIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(desktop);

            Assert.AreEqual(1, indexAfterDown);
            Assert.AreEqual(0, indexAfterUp);
        }

        [TestMethod]
        public void WallpaperChanging_Test()
        {
            Console.SetIn(new StringReader("next\nprev\nexit"));
            wallpapersApp.LaunchApp();
            wallpapersApp.TakeTheInput();
        }

        [TestMethod]
        public void HandleInputLaunchSelectedApp_Test()
        {
            bool appLaunched = false;
            wallpapersApp.appStateUpdated += () => appLaunched = true;
            os.InstalledApps.Add(wallpapersApp);
            Console.SetIn(new StringReader("add A 123 A\nselect 1\n123\nexit\nexit\n"));
            Desktop.testKeySequence = new List<ConsoleKey> { ConsoleKey.Enter, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.Enter, ConsoleKey.Escape };

            desktop.TakeTheInput();

            Assert.IsTrue(appLaunched);
            Directory.SetCurrentDirectory(mainDir);
            if (Directory.Exists("A"))
                Directory.Delete("A", true);
            File.WriteAllText("Profiles.txt", String.Empty);
        }

        [TestMethod]
        public void LaunchApp_Test()
        {
            bool eventTriggered = false;
            desktop.appStateUpdated += () => eventTriggered = true;

            desktop.LaunchApp();
            desktop.appStateUpdated?.Invoke();

            Assert.IsTrue(eventTriggered);
        }

        [TestMethod]
        public void CloseApp_Test()
        {
            desktop.CloseApp();
        }

        [TestMethod]
        public void SetProfileActivation_Test()
        {
            desktop.SetProfileActivation();
            var isProfileSelected = desktop.GetType().GetField("isProfileSelected", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(desktop);
            Assert.IsTrue((bool)isProfileSelected);
        }
    }
}
