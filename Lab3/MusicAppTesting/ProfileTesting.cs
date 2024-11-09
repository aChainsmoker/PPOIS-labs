using MusicAppNS;

namespace MusicAppTesting
{
    [TestClass]
    public class ProfileTests
    {
        private Profile profile;

        [TestInitialize]
        public void TestInitialize()
        {
            File.WriteAllLines("testUser.txt", new string[] { "testAlbum", "1", "lol Oli" });
            profile = new Profile("testUser", "password123");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists("testUser.txt"))
                File.Delete("testUser.txt");

            if (File.Exists("ListeningHistory.txt"))
                File.Delete("ListeningHistory.txt");
        }

        [TestMethod]
        public void UploadProfileDataTest()
        {
            File.WriteAllText("testUser.txt", String.Empty);
            profile.Library.Playlists.Clear();

            var playlist = new Playlist("Playlist1", profile);
            playlist.AddSong(new Song("Song1", "Artist1"));
            profile.Library.AddPlaylist(playlist);

            profile.UploadProfileData();

            var savedData = File.ReadAllLines("testUser.txt");
            Assert.AreEqual("Playlist1", savedData[0]);
            Assert.AreEqual("1", savedData[1]);
            Assert.AreEqual("Song1 Artist1", savedData[2]);
        }

        [TestMethod]
        public void SubscribeToArtistSuccessTest()
        {
            File.WriteAllLines("artistUser.txt", new List<string>());
            bool result = profile.SubscribeToArtist("artistUser");
            Assert.IsTrue(result);

            var artistData = File.ReadAllLines("artistUser.txt");
            Assert.IsTrue(artistData.Contains("testUser"));
        }

        [TestMethod]
        public void UnSubscribeFromArtistSuccessTest()
        {
            File.WriteAllLines("artistUser.txt", new List<string> { "testUser" });
            bool result = profile.UnSubscribeFromArtist("artistUser");
            Assert.IsTrue(result);

            var artistData = File.ReadAllLines("artistUser.txt");
            Assert.IsFalse(artistData.Contains("testUser"));
        }

        [TestMethod]
        public void UnSubscribeFromArtistFailTest()
        {
            File.WriteAllLines("artistUser.txt", new List<string> { "otherUser" });
            bool result = profile.UnSubscribeFromArtist("artistUser");
            Assert.IsFalse(result);

            var artistData = File.ReadAllLines("artistUser.txt");
            Assert.AreEqual(1, artistData.Length);
            Assert.IsTrue(artistData.Contains("otherUser"));
        }
    }
}
