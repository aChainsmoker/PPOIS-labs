using MusicAppNS;

namespace MusicAppTesting
{
    [TestClass]
    public class PlaylistTests
    {
        private Album album;
        private Profile author;

        [TestInitialize]
        public void Setup()
        {
            author = new Artist("testAuthor", "password");
            MusicApp.Profile = author;
            album = new Album("TestAlbum", author);
        }


        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists("Songs.txt"))
                File.Delete("Songs.txt");
            if (File.Exists("testSubscriber.txt"))
                File.Delete("testSubscriber.txt");
        }

        [TestMethod]
        public void AddSongToAlbumTest()
        {
            album.HandleInput("ls TestSong");

            Assert.AreEqual(1, album.Songs.Count);
            Assert.AreEqual("TestSong", album.Songs[0].SongName);

            string[] songData = File.ReadAllLines("Songs.txt");
            Assert.IsTrue(songData[0].Contains("TestSong"));
            Assert.IsTrue(songData[0].Contains("testAuthor"));
        }

        [TestMethod]
        public void ReleaseAlbumTest()
        {
            album.AddSong(new Song("Song1", "testAuthor"));
            album.AddSong(new Song("Song2", "testAuthor"));
            author.UploadProfileData();

            File.AppendAllLines("testAuthor.txt", new string[] { "testSubscriber" });

            FileStream fs;
            fs = File.Create("testSubscriber.txt");
            fs.Close();

            album.HandleInput("release");

            List<string> subscriberData = new List<string>(File.ReadAllLines("testSubscriber.txt"));
            Assert.IsTrue(subscriberData.Contains("TestAlbum"));
            Assert.IsTrue(subscriberData.Contains("Song1 testAuthor"));
            Assert.IsTrue(subscriberData.Contains("Song2 testAuthor"));
        }

        [TestMethod]
        public void DeleteSongFromAlbumTest()
        {
            Song song = album.AddSong(new Song("SongToDelete", "testAuthor"));
            Assert.AreEqual(1, album.Songs.Count);

            album.HandleInput("delete 1");

            Assert.AreEqual(0, album.Songs.Count);
        }

    }
}
