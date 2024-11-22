using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Computer_Simulation
{
    public class Wallpapers : Application, ILoadable
    {
        private string name = "Wallpapers";
        private int wallpaperIndex;
        private List<string> wallpapers = new List<string>();

        private const int WallpapersSize = 30;

        public override string AppName { get => name; set => name = value; }
        public List<string> CurrentWallpapers { get => wallpapers; private set => wallpapers = value; }
        public override void LaunchApp()
        {
            ReadData();
            appStateUpdated += UpdateAppRepresentation;
            SetWallpapers(); 
        }

        public override void CloseApp()
        {
            WriteData();
            appStateUpdated -= UpdateAppRepresentation;
        }

        public override string HandleInput(string command)
        {
            switch (command.Trim())
            {
                case "next":
                    List<string> stringData = stateReader.ReadData("Wallpapers.txt");
                    if (wallpaperIndex != (stringData.Count - 1) / 30)
                    {
                        wallpaperIndex++;
                        SetWallpapers();
                    }
                    break;
                case "prev":
                    if (wallpaperIndex - 1 > 0)
                    {
                        wallpaperIndex--;
                        SetWallpapers();
                    }
                    break;
            }

            return command;
        }
    

        public void ReadData()
        {
            List<string >stringData = stateReader.ReadData("Wallpapers.txt");

            if(stringData.Count == 0)
            {
                string destinationFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Wallpapers.txt");
                File.Copy("..\\Wallpapers.txt", destinationFilePath, true);
                stringData = stateReader.ReadData("Wallpapers.txt");
            }

            wallpaperIndex = Convert.ToInt32(stringData[0]);

        }

        public void SetWallpapers()
        {
            List<string> stringData = stateReader.ReadData("Wallpapers.txt");

            wallpapers.Clear();

            int wallpapersLinesStart = 1;

            wallpapersLinesStart = wallpaperIndex * WallpapersSize - WallpapersSize + 1;

            for (int i = wallpapersLinesStart; i < wallpapersLinesStart + WallpapersSize; i++)
            {
                wallpapers.Add(stringData[i]);
            }
        }

        public void WriteData()
        {
            List<string> stringData = stateReader.ReadData("Wallpapers.txt");

            stringData[0] = wallpaperIndex.ToString();

            stateWriter.WriteState(stringData, "Wallpapers.txt");
        }

        protected override void UpdateAppRepresentation()
        {
            base.UpdateAppRepresentation();

            for(int i = 0; i<wallpapers.Count; i++)
            {
                Console.WriteLine(wallpapers[i]);
            }
        }
    }
}
