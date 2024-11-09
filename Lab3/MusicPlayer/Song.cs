namespace MusicAppNS
{
    public class Song
    {
        private string songName;
        private List<string> lyrics;
        private string artist;

        public string SongName { get => songName; set => songName = value; }
        public List<string> Lyrics { get => lyrics; set => lyrics = value; }
        public string Artist { get => artist; set => artist = value; }


        public Song(string name, string artist)
        {
            lyrics = new List<string>();
            this.songName = name;
            this.artist = artist;
        }
    }
}
