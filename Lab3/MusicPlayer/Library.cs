namespace MusicAppNS
{
    public class Library
    {
        private List<Playlist> playlists;

        public List<Playlist> Playlists { get => playlists; }

        public Library()
        {
            playlists = new List<Playlist>();
        }

        public List<Playlist> AddPlaylist(Playlist playlist)
        {
            playlists.Add(playlist);
            return playlists;
        }


        public List<Playlist> DeletePlaylist(Playlist playlist)
        {
            playlists.Remove(playlist);
            return playlists;
        }

        public void SetLibraryVisuals()
        {
            List<string> playlistNames = new List<string>();
            for(int i = 0; i<playlists.Count;i ++)
                playlistNames.Add(playlists[i].PlaylistName);
            VisualInterface.UpdateVisualRepresentation(playlistNames);
        }

        public Playlist CreateNewPlaylist(string name, Profile profile)
        {
            Playlist playlist = new Playlist(name, profile);
            return playlist;
        }

        public void HandleInput(string input)
        {
            string[] inputParts = input.Trim().Split();
            switch (inputParts[0])
            {
                case "open":
                    List<string> songNames = playlists[Convert.ToInt32(inputParts[1]) - 1].GetSongsNames();
                    VisualInterface.AddVisualsToInterfaceLines(songNames);
                    VisualInterface.UpdateVisualRepresentation(new List<string> { });
                    MusicApp.CurrentPlaylist = playlists[Convert.ToInt32(inputParts[1]) - 1];
                    VisualInterface.RemoveVisualsFromInterfaceLines(songNames);
                    break;
                case "lib":
                    SetLibraryVisuals();
                    MusicApp.CurrentPlaylist = null;
                    break;
                case "dp":
                    DeletePlaylist(playlists[Convert.ToInt32(inputParts[1])-1]);
                    MusicApp.Profile.UploadProfileData();
                    SetLibraryVisuals();
                    break;
                case "ap":
                    Playlist playlist = new Playlist(inputParts[1], MusicApp.Profile);
                    AddPlaylist(playlist);
                    MusicApp.Profile.UploadProfileData();
                    SetLibraryVisuals();
                    break;
                case "aa":
                    Playlist album;
                    if (MusicApp.Profile is Artist artist)
                    {
                        album = new Album(inputParts[1], artist);
                        AddPlaylist(album);
                        SetLibraryVisuals();
                        MusicApp.Profile.UploadProfileData();
                    }
                    break;
            }
        }



    }
}
