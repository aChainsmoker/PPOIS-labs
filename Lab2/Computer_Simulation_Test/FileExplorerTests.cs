using Computer_Simulation;
using System.Reflection;

namespace Computer_Simulation.Tests
{
    [TestClass]
    public class FileExplorerTests
    {
        private FileExplorer fileExplorer;
        private string tempDir;
        private OS os;
        private string mainDir;
        private string parentDir;

        [TestInitialize]
        public void TestInitialize()
        {
            mainDir = Directory.GetCurrentDirectory();
            tempDir = "TestDir";
            Directory.CreateDirectory(tempDir);
            parentDir = "A";

            os = new OS();
            os.FileSystemPath = parentDir;

            fileExplorer = (FileExplorer)os.InstalledApps.Find(app => app is FileExplorer);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Directory.SetCurrentDirectory(mainDir);
            if (Directory.Exists(parentDir))
            {
                Directory.Delete(parentDir, true);
            }
        }

        [TestMethod]
        public void HandleInput_AddCommand_Test()
        {
            string fileName = "testFile.txt";
            fileExplorer.LaunchApp();
            fileExplorer.HandleInput($"add {fileName}");
            Assert.IsTrue(File.Exists(fileName));
        }

        [TestMethod]
        public void HandleInput_DeleteCommand_Test()
        {
            string fileName = "testFile.txt";
            fileExplorer.LaunchApp();
            fileExplorer.HandleInput($"delete {fileName}");
            Assert.IsFalse(File.Exists(fileName));
        }

        [TestMethod]
        public void RepresentationUpdate_Test()
        {
            Console.SetIn(new StringReader("exit"));
            fileExplorer.LaunchApp();
            fileExplorer.TakeTheInput();
        }

        [TestMethod]
        public void HandleInput_AdirCommand_Test()
        {
            string dirName = "testDir";
            fileExplorer.LaunchApp();
            fileExplorer.HandleInput($"adir {dirName}");
            Assert.IsTrue(Directory.Exists(dirName));
        }

        [TestMethod]
        public void HandleInput_DdirCommand_Test()
        {
            string dirName = "testDir";
            string dirPath = Path.Combine(tempDir, dirName);
            Directory.CreateDirectory(dirPath);
            fileExplorer.LaunchApp();
            fileExplorer.HandleInput($"ddir {dirName}");
            Assert.IsFalse(Directory.Exists(dirPath));
        }

        [TestMethod]
        public void HandleInput_DirCommand_Test()
        {
            string dirName = "testDir";
            string dirPath = Path.Combine(Directory.GetCurrentDirectory(), dirName);
            Directory.CreateDirectory(dirPath);
            fileExplorer.LaunchApp();
            fileExplorer.HandleInput($"dir {dirName}");
            Assert.AreEqual(dirPath, Directory.GetCurrentDirectory());
        }

        [TestMethod]
        public void HandleInput_UpdirCommand_Test()
        {
            string dirName = "testDir";
            string dirPath = Path.Combine(tempDir, dirName);
            Directory.CreateDirectory(dirPath);
            fileExplorer.LaunchApp();
            fileExplorer.HandleInput($"dir {dirPath}");
            fileExplorer.HandleInput("updir");
            Directory.SetCurrentDirectory("..\\");
            tempDir = Path.Combine(Directory.GetCurrentDirectory(), tempDir);
            Directory.SetCurrentDirectory(tempDir);
            Assert.AreEqual(tempDir, Directory.GetCurrentDirectory());
        }

        private T GetPrivateField<T>(object obj, string fieldName)
        {
            var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)field.GetValue(obj);
        }
    }
}
