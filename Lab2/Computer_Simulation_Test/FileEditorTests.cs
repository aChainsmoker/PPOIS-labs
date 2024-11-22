using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Computer_Simulation.Tests
{
    [TestClass]
    public class FileEditorTests
    {
        private FileEditor fileEditor;
        private string testFile;

        [TestInitialize]
        public void TestInitialize()
        {
            testFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".txt");
            File.WriteAllText(testFile, "Line 1\nLine 2\nLine 3");
            fileEditor = new FileEditor(testFile);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }
        }

        [TestMethod]
        public void LaunchApp_Test()
        {
            fileEditor.LaunchApp();
        }

        [TestMethod]
        public void CloseApp_Test()
        {
            fileEditor.LaunchApp();
            fileEditor.CloseApp();
        }

        [TestMethod]
        public void TakeInputTest()
        {
            Console.SetIn(new StringReader("exit"));
            fileEditor.LaunchApp();
            fileEditor.TakeTheInput();
        }

        [TestMethod]
        public void HandleInput_RewriteCommand_Test()
        {
            string input = "New content\nend";
            SimulateConsoleInput(input);
            fileEditor.HandleInput("rewrite");
            string fileContent = File.ReadAllText(testFile);
            Assert.AreEqual("New content\r\n", fileContent);
        }

        [TestMethod]
        public void HandleInput_LineCommand_Test()
        {
            fileEditor.HandleInput("line 2 This is updated line");
            string[] lines = File.ReadAllLines(testFile);
            Assert.AreEqual("This is updated line ", lines[1]);
        }

        [TestMethod]
        public void HandleInput_AppendCommand_Test()
        {
            string input = "Appended content\nend";
            SimulateConsoleInput(input);
            fileEditor.HandleInput("append");
            string fileContent = File.ReadAllText(testFile);
            Assert.IsTrue(fileContent.EndsWith("Appended content\r\n"));
        }

        private void SimulateConsoleInput(string input)
        {
            var inputStream = new MemoryStream();
            var writer = new StreamWriter(inputStream);
            writer.Write(input);
            writer.Flush();
            inputStream.Position = 0;
            Console.SetIn(new StreamReader(inputStream));
        }
    }
}
