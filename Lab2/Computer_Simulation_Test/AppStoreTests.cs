using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace Computer_Simulation.Tests
{
    [TestClass]
    public class AppStoreIntegrationTests
    {
        private OS os;
        private AppStore appStore;
        private string testFilePath = "InstalledApps.txt";

        [TestInitialize]
        public void TestInitialize()
        {
            if (File.Exists(testFilePath))
                File.Delete(testFilePath);

            os = new OS
            {
                FileSystemPath = Directory.GetCurrentDirectory()
            };

            appStore = (AppStore)os.InstalledApps.Find(app => app is AppStore);
            appStore.stateReader.ReadData(testFilePath);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(testFilePath))
                File.Delete(testFilePath);
        }

        [TestMethod]
        public void AppStore_AddBasicAppLibrary_Test()
        {
            var availableApps = appStore.availableApps;

            Assert.AreEqual(4, availableApps.Count);
            Assert.IsTrue(availableApps.Exists(app => app.AppName == "File Editor"));
            Assert.IsTrue(availableApps.Exists(app => app.AppName == "Clock"));
            Assert.IsTrue(availableApps.Exists(app => app.AppName == "Wallpapers"));
            Assert.IsTrue(availableApps.Exists(app => app.AppName == "Snakey"));
        }

        [TestMethod]
        public void AppStore_InstallApp_Test()
        {
            var appToInstall = appStore.availableApps[0];

            appStore.HandleInput("install 1");

            Assert.AreEqual(4, os.InstalledApps.Count);
            CollectionAssert.Contains(os.InstalledApps, appToInstall);
            Assert.IsFalse(appStore.availableApps.Contains(appToInstall));
        }

        [TestMethod]
        public void AppStore_UninstallApp_Test()
        {
            var appToInstall = appStore.availableApps[0];
            appStore.HandleInput("install 1");

            appStore.HandleInput("uninstall 1");

            Assert.AreEqual(3, os.InstalledApps.Count);
            Assert.IsTrue(appStore.availableApps.Contains(appToInstall));
        }

        [TestMethod]
        public void AppStore_WriteData_Test()
        {
            var appToInstall = appStore.availableApps[0];
            appStore.HandleInput("install 1");

            appStore.WriteData();

            var savedData = File.ReadAllLines(testFilePath);
            Assert.AreEqual(1, savedData.Length);
            Assert.AreEqual("File Editor", savedData[0]);
        }

        [TestMethod]
        public void LoadProfilesApps_Test()
        {
            File.WriteAllLines(testFilePath, new[] { "Clock", "Snakey" });

            appStore.LoadProfilesApps();

            Assert.AreEqual(5, os.InstalledApps.Count);
            Assert.IsTrue(os.InstalledApps.Exists(app => app.AppName == "Clock"));
            Assert.IsTrue(os.InstalledApps.Exists(app => app.AppName == "Snakey"));
        }

        [TestMethod]
        public void ReadData_Test()
        {
            File.WriteAllLines(testFilePath, new[] { "FileEditor", "Wallpapers" });

            appStore.ReadData();
        }

        [TestMethod]
        public void LaunchApp_Test()
        {
            var wasEventTriggered = false;
            appStore.appStateUpdated += () => wasEventTriggered = true;

            appStore.LaunchApp();
            appStore.appStateUpdated?.Invoke();

            Assert.IsTrue(wasEventTriggered);
        }

        [TestMethod]
        public void CloseApp_Test()
        {
            var appToInstall = appStore.availableApps[0];
            appStore.HandleInput("install 1");

            appStore.CloseApp();

            var savedData = File.ReadAllLines("InstalledApps.txt");
            Assert.AreEqual(1, savedData.Length);
            Assert.AreEqual("File Editor", savedData[0]);

            Assert.IsNull(appStore.appStateUpdated);
        }
    }
}
