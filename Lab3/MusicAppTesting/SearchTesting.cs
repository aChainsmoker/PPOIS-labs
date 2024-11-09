using MusicAppNS;

namespace MusicAppTesting
{
    [TestClass]
    public class SearchTests
    {
        private Search search;
        private Profile testProfile;

        [TestInitialize]
        public void Setup()
        {
            testProfile = new Profile("testUser", "password");
            search = new Search();

            List<string> testSongs = new List<string> { "Song1 Artist1", "Song2 Artist2", "TestSong Artist3" };
            File.WriteAllLines("Songs.txt", testSongs);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists("Songs.txt"))
                File.Delete("Songs.txt");
            if (File.Exists("testUser.txt"))
                File.Delete("testUser.txt");
        }

        [TestMethod]
        public void FindSongsTest()
        {
            List<Song> foundSongs = search.FindSongs("Song");

            Assert.AreEqual(3, foundSongs.Count);
            Assert.IsTrue(foundSongs.Exists(song => song.SongName == "Song1" && song.Artist == "Artist1"));
            Assert.IsTrue(foundSongs.Exists(song => song.SongName == "Song2" && song.Artist == "Artist2"));
        }


        [TestMethod]
        public void HandleInputTest()
        {
            MusicApp.Profile = testProfile;
            Playlist testPlaylist = new Playlist("TestPlaylist", testProfile);
            testProfile.Library.AddPlaylist(testPlaylist);
            MusicApp.CurrentPlaylist = new Playlist("CurrentPlaylist", testProfile);
            Song songToAdd = new Song("CurrentSong", "CurrentArtist");
            MusicApp.CurrentPlaylist.AddSong(songToAdd);

            Console.SetIn(new StringReader("1\n exit"));
            search.HandleInput("add 1");

            Assert.AreEqual(1, testPlaylist.Songs.Count);
            Assert.AreEqual("CurrentSong", testPlaylist.Songs[0].SongName);
            Assert.AreEqual("CurrentArtist", testPlaylist.Songs[0].Artist);
        }
    }
}
