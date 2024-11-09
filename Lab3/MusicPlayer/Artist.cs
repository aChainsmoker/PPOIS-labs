namespace MusicAppNS
{
    public class Artist : Profile 
    {
        private List<string> subscribersLogins = new List<string>();

        public List<string> SubscribersLogins { get => subscribersLogins; }
        public Artist(string login, string password) : base(login, password) { }

        protected override void DownloadProfileData()
        {
            StateReader stateReader = new StateReader();
            List<string> stringData = stateReader.ReadData(login + ".txt");
            for (int i = 0; i < stringData.Count; i++)
            {
                if (stringData[i] == "subscribers")
                {
                    for (int j = i + 1; j < stringData.Count; j++)
                    {
                        subscribersLogins.Add(stringData[j]);
                    }
                    break;
                }
                Playlist playlist;
                if (stringData[i].Trim().Split().Length > 1)
                    playlist = new Album(stringData[i].Trim().Split()[0],this );
                else
                    playlist = new Playlist(stringData[i], this);
                int amountOfSongs = Convert.ToInt32(stringData[i + 1]);
                for (int j = i + 2; j < amountOfSongs + i + 2; j++)
                {
                    Song song = new Song(stringData[j].Split()[0], stringData[j].Split()[1]);
                    playlist.AddSong(song);
                }
                i = i + 1 + amountOfSongs;
                library.AddPlaylist(playlist);
            }
        }

        public override void UploadProfileData()
        {
            StateWriter stateWriter = new StateWriter();
            List<string> stringData = new List<string>();
            for (int i = 0; i < library.Playlists.Count; i++)
            {
                stringData.Add(library.Playlists[i].PlaylistName);
                if (library.Playlists[i] is Album album)
                    stringData[stringData.Count-1] += " " + "album";
                stringData.Add(library.Playlists[i].Songs.Count.ToString());
                for (int j = 0; j < library.Playlists[i].Songs.Count; j++)
                {
                    stringData.Add(library.Playlists[i].Songs[j].SongName.ToString() + " " + library.Playlists[i].Songs[j].Artist.ToString());
                }
            }
            stringData.Add("subscribers");
            for(int i =0; i<subscribersLogins.Count; i++)
            {
                stringData.Add(subscribersLogins[i]);
            }

            stateWriter.WriteState(stringData, MusicApp.Profile.Login + ".txt");
        }

    }
}
