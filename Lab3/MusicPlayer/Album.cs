namespace MusicAppNS
{
    public class Album : Playlist //Разница в том, что в альбом можно будет добавлять новые треки, а в обычные плейлисты лишь существующие
    {
        public Album(string name, Profile author):base(name, author) { }


        public override void HandleInput(string input)
        {
            base.HandleInput(input);

            string[] inputParts = input.Trim().Split();

            switch (inputParts[0])
            {
                case "ls":
                    Song song = new Song(inputParts[1], MusicApp.Profile.Login);
                    AddSong(song);
                    File.AppendAllLines("Songs.txt",new string[]{ song.SongName + " " + song.Artist } );
                    MusicApp.Profile.UploadProfileData();
                    VisualInterface.UpdateVisualRepresentation(GetSongsNames());
                    break;
                case "release":
                    ReleaseAlbum();
                    break;
            }

        }
        private void ReleaseAlbum()
        {
            List<string> album = new List<string>();
            album.Add(playlistName);
            album.Add(songs.Count.ToString());
            for(int i =0; i<songs.Count; i++)
            {
                album.Add(songs[i].SongName + " " + songs[i].Artist);
            }

            for(int i =0; i < (MusicApp.Profile as Artist).SubscribersLogins.Count; i++)
            {
                SendAlbum((MusicApp.Profile as Artist).SubscribersLogins[i], album);
            }
        }

        private void SendAlbum(string subscribersLogin, List<string> album)
        {
            string pathToSubscriberFile = subscribersLogin + ".txt";
            if (File.Exists(pathToSubscriberFile))
            {
                StateReader stateReader = new StateReader();
                List<string> stringData = stateReader.ReadData(pathToSubscriberFile);
                
                if(stringData.Contains("subscribers"))
                {
                    int indexToInsert = stringData.IndexOf("subscribers");
                    stringData.InsertRange(indexToInsert, album);
                }
                else
                {
                    stringData.AddRange(album);
                }
                StateWriter stateWriter = new StateWriter();
                stateWriter.WriteState(stringData, pathToSubscriberFile);
            }
        }
    }
}
