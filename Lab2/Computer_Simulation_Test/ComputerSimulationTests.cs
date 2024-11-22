//using Computer_Simulation;
//using Microsoft.VisualBasic;
//using System;
//using System.Linq;

//namespace Computer_Simulation_Test
//{

//    [TestClass]
//    public class ComputerSimulationTests
//    {
//        string mainDir;

//        [TestInitialize]
//        public void ComputerSimulationInitialize()
//        {

//            FileStream fs;
//            fs = File.Create("Profiles.txt");
//            fs.Close();

//            File.WriteAllLines("Profiles.txt", new string[] { "Haron", "123", "HaronFiles"});
//            mainDir = Directory.GetCurrentDirectory();
//            Desktop.testKeySequence.Add(ConsoleKey.Escape);
//        }
//        [TestCleanup]
//        public void ComputerSimulationCleanup()
//        {
//            Desktop.testKeySequence.Clear();
//            Directory.SetCurrentDirectory(mainDir);
//            if (File.Exists("Profiles.txt"))
//                File.Delete("Profiles.txt");
//        }

//        [TestMethod]
//        public void ProfileTests()
//        {
//            Profile profile = new Profile("Lesha", "123", "LeshaFiles");
//            Assert.AreEqual("Lesha", profile.Name);
//            Assert.AreEqual("123", profile.Password);
//            Assert.AreEqual("LeshaFiles", profile.FileSpacePath);
//        }

//        [TestMethod]
//        public void ComputerTests()
//        {
//            Computer computer = new Computer();
//        }

//        [TestMethod]
//        public void OSTests()
//        {
//            OS os  = new OS();
//            os.FileSystemPath = "LolaFiles";
//            StringAssert.Equals("LolaFiles", os.FileSystemPath);
//            StringAssert.Equals("LolaFiles", Directory.GetCurrentDirectory());
//            Directory.SetCurrentDirectory("..\\");
//            Directory.Delete("LolaFiles");


//        }

//        [TestMethod]
//        public void ClockTests()
//        {
//            Clock clock = new Clock();
//            Console.SetIn(new StringReader("time\nstopwatch start\nstopwatch stop\ntimer start 1\ntimer stop\nexit"));

//            clock.LaunchApp();
//            clock.TakeTheInput();
//        }

//        [TestMethod]
//        public void ProfileSelectorTests()
//        {
//            OS os = new OS();
//            ProfileSelector profileSelector = new ProfileSelector(os);
//            profileSelector.CreateNewProfile("a", "b", "c");
//            Assert.AreEqual("a", profileSelector.Profiles[profileSelector.Profiles.Count-1].Name);
//            profileSelector.SelectProfile(profileSelector.Profiles.Count, os);
//            Profile profile = profileSelector.Profiles[profileSelector.Profiles.Count - 1];
//            Assert.AreEqual(profile, profileSelector.SelectedProfile);
//            profileSelector.SelectProfile(1, os);
//            profileSelector.DeleteProfile(profile);
//            CollectionAssert.DoesNotContain(profileSelector.Profiles, profile);
//        }

//        [TestMethod]
//        public void FileExplorerTests()
//        {
//            OS os = new OS();
//            FileExplorer fileExplorer = new FileExplorer(os);
//            ProfileSelector profileSelector = new ProfileSelector(os);
//            profileSelector.SelectProfile(1,os);
//            fileExplorer.LaunchApp();
//            Console.SetIn(new StringReader("open hachapuri.txt\nadd x.txt\nedit x.txt\ndelete x.txt\nadir kl\ndir kl\nupdir\nddir kl\nexit"));
//            StringWriter outputString = new StringWriter();
//            Console.SetOut(outputString);
//            fileExplorer.TakeTheInput();

//            StringAssert.Contains(outputString.ToString(), "asd");

//            fileExplorer.CloseApp();

//            Directory.SetCurrentDirectory("..\\");
//            os.InstalledApps.Add(new FileEditor());
//            fileExplorer = new FileExplorer(os);
//            fileExplorer.LaunchApp();
//            Console.SetIn(new StringReader("add x.txt\nedit x.txt\nexit\ndelete x.txt\nexit"));
//            fileExplorer.TakeTheInput();
//            fileExplorer.CloseApp();

//        }

//        [TestMethod]
//        public void WallpapersTests()
//        {
//            Directory.SetCurrentDirectory("HaronFiles");
//            Wallpapers wallpapers = new Wallpapers();
//            wallpapers.LaunchApp();
//            StateReader stateReader = new StateReader();
//            List<string> stringData = stateReader.ReadData("Wallpapers.txt");
//            CollectionAssert.Contains(stringData, wallpapers.CurrentWallpapers[0]);

//            Console.SetIn(new StringReader("prev\nprev\nnext\nnext\nnext\nexit"));
//            wallpapers.TakeTheInput();
//            wallpapers.CloseApp();

//            File.Delete("Wallpapers.txt");

//        }

//        [TestMethod]
//        public void SnakeyTests()
//        {
//            Snakey snakey = new Snakey();
//            snakey.LaunchApp();
//            snakey.CloseApp();
//        }
//            [TestMethod]
//        public void FileEditorTests()
//        {
//            Directory.SetCurrentDirectory("HaronFiles");
//            FileEditor fileEditor = new FileEditor("test.txt");
//            fileEditor.LaunchApp();
//            Console.SetIn(new StringReader("rewrite\nhello\nend\nappend\nhi\nend\nline 1 lol\nexit"));
//            fileEditor.TakeTheInput();


//            fileEditor.CloseApp();

                
//        }

//        [TestMethod]
//        public void DesktopTests()
//        {
//            Desktop.testKeySequence.Clear();
//            Desktop.testKeySequence = Enumerable.Repeat(ConsoleKey.DownArrow, 3).ToList();
//            Desktop.testKeySequence.AddRange(Enumerable.Repeat(ConsoleKey.UpArrow, 3).ToList());
//            Desktop.testKeySequence.AddRange(new List<ConsoleKey> { ConsoleKey.DownArrow, ConsoleKey.Enter, 
//                ConsoleKey.UpArrow, ConsoleKey.Enter, ConsoleKey.DownArrow, ConsoleKey.DownArrow, 
//                ConsoleKey.Enter, ConsoleKey.UpArrow, ConsoleKey.UpArrow, ConsoleKey.Enter, ConsoleKey.Enter});
//            Desktop.testKeySequence.Add(ConsoleKey.Escape);


//            Console.SetIn(new StringReader("select 1\n123\nexit\ninstall 3\nuninstall 1\nexit\nadd Lola 123 LolaFiles" +
//                "\nselect 2\n123\nexit\ndelete 2\nselect 1\n123\ndelete 2\n123\nselect 1\n34\nexit\n"));


//            Desktop desktop = new Desktop(new OS());

//            Assert.AreEqual("Desktop", desktop.AppName);
            

//        }
      
//    }
//}