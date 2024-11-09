namespace MusicAppNS
{
    public class ListeningHistory
    {
        private List<Song> history;

        public List<Song> History { get => history; set => history = value; }

        public ListeningHistory()
        {
            history = new List<Song>();
            DownloadHistory();
        }
        
        public void ClearHistory()
        {
            history.Clear();
        }

        public void HandleInput(string input)
        {
            string[] inputParts = input.Trim().Split();

            switch (inputParts[0])
            {
                case "ch":
                    ClearHistory();
                    UploadHistory();
                    break;
                case "sh":
                    Playlist playlist = new Playlist("History", MusicApp.Profile);
                    playlist.FulfillPlaylist(history);
                    MusicApp.CurrentPlaylist = playlist;
                    VisualInterface.UpdateVisualRepresentation(playlist.GetSongsNames());
                    break;
            }
        }

        public void DownloadHistory()
        {
            StateReader stateReader = new StateReader();
            List<string> stringData = stateReader.ReadData("ListeningHistory.txt");
            for (int i = 0; i < stringData.Count; i++)
            {
                history.Add(new Song(stringData[i].Split()[0], stringData[i].Split()[1]));
            }
        }

        public void UploadHistory()
        {
            StateWriter stateWriter = new StateWriter();
            List<string> stringData = new List<string>();
            for(int i =0; i< history.Count; i++)
            {
                stringData.Add(history[i].SongName + " " + history[i].Artist);
            }
            stateWriter.WriteState(stringData, "ListeningHistory.txt");
        }
    }
}
