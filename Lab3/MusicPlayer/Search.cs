namespace MusicAppNS
{
    public class Search
    {
        public List<Song> FindSongs(string searchPattern)
        {
            StateReader stateReader = new StateReader();
            List<Song> songs = new List<Song>();
            List<string> stringData = stateReader.ReadData("Songs.txt");

            for(int i = 0; i<stringData.Count; i++)
            {
                if (stringData[i].Contains(searchPattern))
                {
                    Song song = new Song(stringData[i].Split()[0], stringData[i].Split()[1]);
                    songs.Add(song);
                }
            }  
            return songs;
        }

        public void HandleInput(string input)
        {
            string[] inputParts = input.Trim().Split();
            int indexOfTheSong;
            switch (inputParts[0])
            {
                case "add":
                    indexOfTheSong = Convert.ToInt32(inputParts[1])-1;
                    MusicApp.Profile.Library.SetLibraryVisuals();
                    MusicApp.Profile.Library.Playlists[Convert.ToInt32(Console.ReadLine()) - 1].AddSong(MusicApp.CurrentPlaylist.Songs[indexOfTheSong]);
                    MusicApp.Profile.UploadProfileData();
                    break;
            }
        }

    }
}
