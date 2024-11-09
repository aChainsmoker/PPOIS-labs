using MusicAppNS;

namespace MusicAppTesting
{
    [TestClass]
    public class VisualInterfaceTests
    {
        private VisualInterface visualInterface;
        private LoginSystem loginSystem;
        private MusicPlayer musicPlayer;
        private Library library;

        [TestInitialize]
        public void TestInitialize()
        {
            loginSystem = new LoginSystem();
            musicPlayer = new MusicPlayer();
            library = new Library();
            visualInterface = new VisualInterface(loginSystem, musicPlayer, library);
            VisualInterface.IntefaceLines.Clear();
            File.WriteAllLines("Profiles.txt", new[] { "testUser", "testPass", "listener" });
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists("Profiles.txt"))
                File.Delete("Profiles.txt");
            if (File.Exists("newUser.txt"))
                File.Delete("newUser.txt");

            VisualInterface.IntefaceLines.Clear();
        }

        [TestMethod]
        public void UpdateVisualRepresentationTest()
        {
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            List<string> visuals = new List<string> { "Line 1", "Line 2" };
            List<string> result = VisualInterface.UpdateVisualRepresentation(visuals);
            StringAssert.Contains(stringWriter.ToString(), visuals[0]);
            StringAssert.Contains(stringWriter.ToString(), visuals[1]);
        }

        [TestMethod]
        public void VisualInterfaceInputTest()
        {
            Console.SetIn(new StringReader("open\nplay\nlib\ndp\nap\nadd\ndelete\nfind\naa\nls\nch\nsh\nrelease\nsub\nunsib\nexit"));
            Assert.AreEqual("exit", visualInterface.TakeInput());
        }

        [TestMethod]
        public void RemoveVisualsFromInterfaceLinesTest()
        {
            List<string> initialVisuals = new List<string> { "Line 1", "Line 2", "Line 3" };
            VisualInterface.IntefaceLines.AddRange(initialVisuals);
            List<string> visualsToRemove = new List<string> { "Line 1", "Line 2" };
            List<string> result = VisualInterface.RemoveVisualsFromInterfaceLines(visualsToRemove);
            CollectionAssert.AreEqual(new List<string> { "Line 3" }, result);
        }

        [TestMethod]
        public void AddVisualsToInterfaceLinesTest()
        {
            List<string> initialVisuals = new List<string> { "Line 1" };
            VisualInterface.IntefaceLines.AddRange(initialVisuals);
            List<string> visualsToAdd = new List<string> { "Line 2", "Line 3" };
            List<string> result = VisualInterface.AddVisualsToInterfaceLines(visualsToAdd);
            CollectionAssert.AreEqual(new List<string> { "Line 1", "Line 2", "Line 3" }, result);
        }

        [TestMethod]
        public void AddVisualsToInterfaceLinesWithIndexTest()
        {
            List<string> initialVisuals = new List<string> { "Line 1", "Line 2" };
            VisualInterface.IntefaceLines.AddRange(initialVisuals);
            List<string> visualsToAdd = new List<string> { "Inserted Line 1", "Inserted Line 2" };
            List<string> result = VisualInterface.AddVisualsToInterfaceLines(visualsToAdd, 1);
            CollectionAssert.AreEqual(new List<string> { "Line 1", "Inserted Line 1", "Inserted Line 2", "Line 2" }, result);
        }


        [TestMethod]
        public void HandleLoginInputSuccessTest()
        {
            string command = "login testUser testPass";
            bool result = loginSystem.HandleInput(command);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HandlRegisterInputSuccessTest()
        {
            string command = "register newUser newPass listener";
            bool result = loginSystem.HandleInput(command);
            Assert.IsTrue(result);
            Assert.IsTrue(File.Exists("newUser.txt"));
        }

        [TestMethod]
        public void HandlePlayInputTest()
        {
            Playlist playlist = new Playlist("My Playlist", new Profile("testUser", "testPass"));
            playlist.Songs.Add(new Song("Song 1", "Artist 1"));
            playlist.Songs.Add(new Song("Song 2", "Artist 2"));
            MusicApp.CurrentPlaylist = playlist;

            string command = "login testUser testPass";
            bool result = loginSystem.HandleInput(command);
            library.AddPlaylist(playlist);
            command = "play 1";
            musicPlayer.HandleInput(command);

            StringAssert.Contains(musicPlayer.ChangeSongName()[1].Trim(), "Song 1");
            StringAssert.Contains(musicPlayer.ChangeSongName()[2].Trim(), "Artist 1");
        }

        [TestMethod]
        public void HandleInputSetLibraryVisualsTest()
        {
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            Playlist playlist1 = new Playlist("Playlist 1", new Profile("testUser", "testPass"));
            library.AddPlaylist(playlist1);
            library.SetLibraryVisuals();
            StringAssert.Contains(stringWriter.ToString(), "Playlist 1");
        }


    }
}
