namespace MusicAppNS
{
    public class Playlist
    {
        protected List<Song> songs;
        protected string playlistName;
        protected Profile author;

        public List<Song> Songs { get { return songs; } }
        public Profile Author { get { return author; } }
        public string PlaylistName { get { return playlistName; } }

        public Playlist(string name, Profile Author)
        {
            songs = new List<Song>();
            this.playlistName = name;
            this.author = Author;
        }

        public Song AddSong(Song song)
        {
            songs.Add(song);
            return song;
        }

        public void RemoveSong(Song song)
        {
            songs.Remove(song);
        }

        public Playlist FulfillPlaylist(List<Song> songs)
        {
            for (int i = 0; i < songs.Count; i++)
            {
                this.songs.Add(songs[i]);
            }
            return this;
        }

        public List<string> GetSongsNames()
        {
            List<string> songNames = new List<string>();
            for (int i = 0; i < songs.Count; i++)
            {
                songNames.Add(songs[i].SongName);
            }
            return songNames;
        }

        public virtual void HandleInput(string input)
        {
            string[] inputParts = input.Trim().Split();

            switch (inputParts[0])
            {
                case "delete":
                    RemoveSong(songs[Convert.ToInt32(inputParts[1])-1]);
                    VisualInterface.UpdateVisualRepresentation(GetSongsNames());
                    MusicApp.Profile.UploadProfileData();
                    break;
            }

        }

    }
}
