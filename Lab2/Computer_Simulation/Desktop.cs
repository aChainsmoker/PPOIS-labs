using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer_Simulation
{
    public class Desktop : Application
    {
        private string appName = "Desktop";
        private OS os;
        private int selectedAppIndex;
        private Wallpapers wallpapersApp;
        private bool isProfileSelected;
        private static List<ConsoleKey> testingKeysSequence = new List<ConsoleKey>();
        private int testingKeysSequenceIndex = 0;

        public override string AppName { get => appName; set => appName = value; }
        public static List<ConsoleKey> testKeySequence { set => testingKeysSequence = value; get => testingKeysSequence; }
        public Desktop(OS os)
        {
            this.os = os;
        }


        public override void LaunchApp()
        {
            appStateUpdated += UpdateAppRepresentation;
        }

        public override void CloseApp()
        {
            appStateUpdated -= UpdateAppRepresentation;
        }

        public void CheckOnWallpapersApp()
        {
            for (int i = 0; i < os.InstalledApps.Count; i++)
                if (os.InstalledApps[i] is Wallpapers wallpapersApp)
                {
                    wallpapersApp.ReadData();
                    wallpapersApp.SetWallpapers();
                    this.wallpapersApp = wallpapersApp;
                    return;
                }
            wallpapersApp = null;
        }

        public override void TakeTheInput()
        {
            while (true)
            {
                appStateUpdated?.Invoke();
                if (HandleInput(null) == "exit")
                {
                    CloseApp();
                    break;
                }
            }
        }
        public override string HandleInput(string command)
        {
            ConsoleKey key;
            if (Console.IsOutputRedirected == false)
                key = Console.ReadKey(true).Key;
            else
            {
                key = testingKeysSequence[testingKeysSequenceIndex++];
            }


            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedAppIndex = (selectedAppIndex == 0) ? os.InstalledApps.Count - 1 : selectedAppIndex - 1;
                    break;
                case ConsoleKey.DownArrow:
                    selectedAppIndex = (selectedAppIndex == os.InstalledApps.Count - 1) ? 0 : selectedAppIndex + 1;
                    break;
                case ConsoleKey.Enter:
                    if (isProfileSelected == false && os.InstalledApps[selectedAppIndex] is not ProfileSelector profileSelector)
                        return null;
                    os.InstalledApps[selectedAppIndex].LaunchApp();
                    os.InstalledApps[selectedAppIndex].TakeTheInput();
                    break;
                case ConsoleKey.Escape:
                    return "exit";
            }
            return null;
        }

        private void PrintDesktop(List<string> wallpapers, List<Application> apps, int selectedAppIndex)
        {
            string wallpaperString = String.Empty;

            for (int i = 0; i<wallpapers.Count; i++)
            {
                int verticalMargin = (wallpapers.Count - apps.Count)/2;
                for (int j = 0; j < wallpapers[i].Length; j++)
                {
                    wallpaperString += wallpapers[i][j];
                }
                if (i > verticalMargin-1 && i <verticalMargin+apps.Count)
                {
                    int middleIndex = (wallpaperString.Length - apps[i-verticalMargin].AppName.Length)/2;
                    wallpaperString = wallpapers[i].Substring(0,middleIndex) + apps[i - verticalMargin].AppName + wallpapers[i].Substring(middleIndex + apps[i-verticalMargin].AppName.Length);
                }
                if(i - verticalMargin == selectedAppIndex)
                    Console.ForegroundColor = ConsoleColor.Magenta;

                Console.WriteLine(wallpaperString);
                Console.ResetColor();
                wallpaperString = String.Empty;
            }
        }

        protected override void UpdateAppRepresentation()
        {
            base.UpdateAppRepresentation();

            if(wallpapersApp != null)
                PrintDesktop(wallpapersApp.CurrentWallpapers, os.InstalledApps, selectedAppIndex);
            else
            {
                List<string> emptyBackground = Enumerable.Repeat(new string(' ', 100), 30).ToList();
                PrintDesktop(emptyBackground, os.InstalledApps, selectedAppIndex);
            }

        }

        public void SetProfileActivation()
        {
            isProfileSelected = true;
        }
    }
}
