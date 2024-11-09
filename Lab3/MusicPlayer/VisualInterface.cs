namespace MusicAppNS
{
    public class VisualInterface
    {
        private static List<string> intefaceLines = new List<string>();
        private LoginSystem loginSystem;
        private MusicPlayer musicPlayer;
        private Library library;
        private Search search;

        public static List<string> IntefaceLines { get => intefaceLines; set => intefaceLines = value; }
        public VisualInterface(LoginSystem loginSystem, MusicPlayer musicPlayer, Library library) 
        {
            search = new Search();
            this.loginSystem = loginSystem;
            this.musicPlayer = musicPlayer; 
            this.library = library;

            MusicPlayer.SetPlayer();
        }

        public string TakeInput()
        {
            library.SetLibraryVisuals();

            string input = String.Empty;
            while (input.Trim() != "exit")
            {
                input = Console.ReadLine();

                try { HandleInput(input); } 
                catch { Console.WriteLine("Incorrect input. Try again");}
            }
            return input;
        }

        private void HandleInput(string input)
        {
            string[] inputParts = input.Trim().Split();
            switch (inputParts[0])
            {
                case "open":
                    library.HandleInput(input);
                    break;
                case "play":
                    musicPlayer.HandleInput(input);
                    break;
                case "lib":
                    library.HandleInput(input);
                    break;
                case "dp":
                    library.HandleInput(input);
                    break;
                case "ap":
                    library.HandleInput(input);
                    break;
                case "add":
                    search.HandleInput(input);
                    break;
                case "delete":
                    MusicApp.CurrentPlaylist.HandleInput(input);
                    break;
                case "find":
                    Playlist playlist = new Playlist("search result", MusicApp.Profile);
                    string searchPattern = String.Empty;
                    for (int i = 1; i < inputParts.Length; i++)
                        searchPattern += inputParts[i];
                    playlist.FulfillPlaylist(search.FindSongs(searchPattern));
                    UpdateVisualRepresentation(playlist.GetSongsNames());
                    MusicApp.CurrentPlaylist = playlist;
                    break;
                case "aa":
                    library.HandleInput(input);
                    break;
                case "ls":
                    if(MusicApp.CurrentPlaylist is Album album)
                        album.HandleInput(input);
                    break;
                case "ch":
                    MusicApp.Profile.ListeningHistory.HandleInput(input);
                    break;
                case "sh":
                    MusicApp.Profile.ListeningHistory.HandleInput(input);
                    break;
                case "release":
                    MusicApp.CurrentPlaylist.HandleInput(input);
                    break;
                case "sub":
                    if (MusicApp.Profile.SubscribeToArtist(inputParts[1]) == false)
                        UpdateVisualRepresentation(new List<string> {"There's no such Artist"});
                    break;
                case "unsub":
                    if (MusicApp.Profile.UnSubscribeFromArtist(inputParts[1]) == false)
                        UpdateVisualRepresentation(new List<string> { "There's no such Artist" });
                    break;
            }
        }

        public static List<string> UpdateVisualRepresentation(List<string> visuals)
        {
            intefaceLines.AddRange(visuals);
            if(!Console.IsOutputRedirected)
                Console.Clear();
            for(int i = 0; i<intefaceLines.Count; i++)
                Console.WriteLine(intefaceLines[i]);
            intefaceLines.RemoveRange(intefaceLines.Count-(visuals.Count), visuals.Count);
            return visuals;
        }

        public static List<string> RemoveVisualsFromInterfaceLines(List<string> visuals)
        {
            if (visuals.Count == 0)
                return intefaceLines;

            int startIndex = intefaceLines.IndexOf(visuals[0]);
            if (startIndex == -1)
                return intefaceLines;
            intefaceLines.RemoveRange(startIndex, visuals.Count);
            return intefaceLines;
        }

        public static List<string> AddVisualsToInterfaceLines(List<string> visuals, int startIndex)
        {
            intefaceLines.InsertRange(startIndex, visuals);
            return intefaceLines;
        }
        public static List<string> AddVisualsToInterfaceLines(List<string> visuals)
        {
            intefaceLines.AddRange(visuals);
            return intefaceLines;
        }
    }
}
