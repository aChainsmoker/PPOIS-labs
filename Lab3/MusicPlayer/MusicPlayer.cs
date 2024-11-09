namespace MusicAppNS
{
    public class MusicPlayer
    {
        private Song currentlyPlayingSong;
        private Playlist currentlyPlayingPlaylist;
        private StateReader stateReader;
        private static List<string> playerRepresentation;

        private const int PLAYER_WIDTH = 70;
        private const int PLAYER_HEIGHT = 5;

        public MusicPlayer()
        {
            stateReader = new StateReader();
            playerRepresentation = stateReader.ReadData("Player.txt");
        }

        public static void SetPlayer()
        {
            VisualInterface.RemoveVisualsFromInterfaceLines(playerRepresentation);
            VisualInterface.AddVisualsToInterfaceLines(playerRepresentation, 0);
        }

        public List<string> ChangeSongName()
        {
            int middleIndexForName = (PLAYER_WIDTH - currentlyPlayingSong.SongName.Length)/2;
            int middleIndexForArtist = (PLAYER_WIDTH - currentlyPlayingSong.Artist.Length) / 2;

            playerRepresentation[1] = String.Empty;
            playerRepresentation[1] = "|" + new string(' ', 70) + "|";
            playerRepresentation[2] = String.Empty;
            playerRepresentation[2] = "|" + new string(' ', 70) + "|";

            playerRepresentation[1] = playerRepresentation[1].Substring(0, middleIndexForName) + currentlyPlayingSong.SongName + playerRepresentation[1].Substring(middleIndexForName+currentlyPlayingSong.SongName.Length);
            playerRepresentation[2] = playerRepresentation[2].Substring(0, middleIndexForArtist) + currentlyPlayingSong.Artist + playerRepresentation[2].Substring(middleIndexForArtist + currentlyPlayingSong.Artist.Length);

            SetPlayer();

            return playerRepresentation;
        }

        public void HandleInput(string input)
        {
            string[] inputParts = input.Trim().Split();
            switch (inputParts[0])
            {
                case "play":
                    currentlyPlayingPlaylist = MusicApp.CurrentPlaylist;
                    currentlyPlayingSong = currentlyPlayingPlaylist.Songs[Convert.ToInt32(inputParts[1]) - 1];
                    ChangeSongName();
                    MusicApp.Profile.ListeningHistory.History.Add(currentlyPlayingSong);
                    MusicApp.Profile.ListeningHistory.UploadHistory();
                    VisualInterface.UpdateVisualRepresentation(currentlyPlayingPlaylist.GetSongsNames());
                    break;

            }
        }

    }
}
