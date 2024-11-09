namespace MusicAppNS
{
    public class Profile
    {
        protected string login;
        protected string password;
        protected ListeningHistory listeningHistory;
        protected Library library;

        public string Login { get => login; }
        public Library Library { get => library; }
        public ListeningHistory ListeningHistory { get => listeningHistory; set => listeningHistory = value; }

        public Profile(string login, string password)
        {
            this.login = login;
            this.password = password;
            library = new Library();
            listeningHistory = new ListeningHistory();
            DownloadProfileData();
        }

        protected virtual void DownloadProfileData()
        {
            StateReader stateReader = new StateReader();
            List<string> stringData = stateReader.ReadData(login + ".txt");
            for(int i = 0; i< stringData.Count; i++)
            {
                Playlist playlist = new Playlist(stringData[i], this);
                int amountOfSongs = Convert.ToInt32(stringData[i + 1]);
                for(int j = i+2; j < amountOfSongs+i+2; j++)
                {
                    Song song = new Song(stringData[j].Split()[0], stringData[j].Split()[1]);
                    playlist.AddSong(song);
                }
                i = i + 1 + amountOfSongs;
                library.AddPlaylist(playlist);
            }   
        }

        public virtual void UploadProfileData()
        {
            StateWriter stateWriter = new StateWriter();
            List<string> stringData = new List<string>();
            for(int i = 0; i<library.Playlists.Count; i++)
            {
                stringData.Add(library.Playlists[i].PlaylistName);
                stringData.Add(library.Playlists[i].Songs.Count.ToString());
                for(int j =0; j< library.Playlists[i].Songs.Count; j++)
                {
                    stringData.Add(library.Playlists[i].Songs[j].SongName.ToString() + " " + library.Playlists[i].Songs[j].Artist.ToString());
                }
            }
            stateWriter.WriteState(stringData, Login + ".txt");
        }

        public bool SubscribeToArtist(string artistName)
        {
            string artistFilePath = artistName + ".txt";
            if (File.Exists(artistFilePath))
            {
                StateReader stateReader = new StateReader();
                List<string> stringData = stateReader.ReadData(artistFilePath);
                if(!stringData.Contains(login) && login != artistName)
                {
                    stringData.Add(login);
                    StateWriter stateWriter = new StateWriter();
                    stateWriter.WriteState(stringData, artistFilePath);
                    return true;
                }
            }
            return false;
        }


        public bool UnSubscribeFromArtist(string artistName)
        {
            string artistFilePath = artistName + ".txt";
            if (File.Exists(artistFilePath))
            {
                StateReader stateReader = new StateReader();
                List<string> stringData = stateReader.ReadData(artistFilePath);
                if (stringData.Contains(login))
                {
                    stringData.Remove(login);
                    StateWriter stateWriter = new StateWriter();
                    stateWriter.WriteState(stringData, artistFilePath);
                    return true;
                }
            }
            return false;
        }

    }
}
