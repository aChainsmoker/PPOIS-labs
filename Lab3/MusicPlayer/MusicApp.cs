namespace MusicAppNS
{
    public class MusicApp
    {
        private LoginSystem loginSystem;
        private VisualInterface visualInterface;
        private MusicPlayer musicPlayer;
        private static Profile profile;
        private static Playlist currentPlaylist;

        public static Profile Profile { get { return profile; } set => profile = value; }
        public static Playlist CurrentPlaylist { get { return currentPlaylist; } set => currentPlaylist = value; }

        public MusicApp() 
        {
            loginSystem = new LoginSystem();
            loginSystem.EnterTheLoginData();
            musicPlayer = new MusicPlayer();
            visualInterface = new VisualInterface(loginSystem, musicPlayer, profile.Library);
            visualInterface.TakeInput();
        }


    }
}
