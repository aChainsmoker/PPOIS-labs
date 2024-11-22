using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer_Simulation
{
    public class AppStore : Application, ILoadable
    {
        private string appName = "AppStore";
        private List<Application> installedApps;
        private List<string> installedAppsNames;
        private OS os;

        public List<Application> availableApps;

        public override string AppName { get => appName; set => appName = value; }

        public AppStore(OS os)
        {
            this.os = os;

            installedAppsNames = new List<string>();
            availableApps = new List<Application>();
            installedApps = new List<Application>();
            AddBasicAppLibrary();
        }

        public void LoadProfilesApps()
        {
            if(installedAppsNames!=null)
                ClearAppsLists();

            ReadData();
            LoadInstalledApps();
        }

        
        private void ClearAppsLists()
        {
            os.InstalledApps = os.InstalledApps.Except(installedApps).ToList();
            installedApps.Clear();
            installedAppsNames.Clear();
            availableApps.Clear();
            AddBasicAppLibrary();
        }

        private void AddBasicAppLibrary()
        {
            availableApps.Add(new FileEditor());
            availableApps.Add(new Clock());
            availableApps.Add(new Wallpapers());
            availableApps.Add(new Snakey());
        }

        public void ReadData()
        {
            List<string> stringData = stateReader.ReadData("InstalledApps.txt");

            for (int i = 0; i < stringData.Count; i++)
            {
                installedAppsNames.Add(stringData[i]);
            }
        }

        public void WriteData()
        {
            List<string> stringData = new List<string>();

            for (int i = 0; i < installedAppsNames.Count; i += 1)
            {
                stringData.Add(installedAppsNames[i]);
            }

            stateWriter.WriteState(stringData, "InstalledApps.txt");

        }

        public override void LaunchApp()
        {
            appStateUpdated += UpdateAppRepresentation;
        }

        public override void CloseApp()
        {
            appStateUpdated -= UpdateAppRepresentation;
            WriteData();
            os.loadProfilesApps?.Invoke();
        }

        public override string HandleInput(string command)
        {
            string[] commandParts = command.Split();

            switch (commandParts[0])
            {
                case "install":
                    InstallApp(availableApps[Convert.ToInt32(commandParts[1])-1]);
                    break;
                case "uninstall":
                    UninstallApp(installedApps[Convert.ToInt32(commandParts[1]) - 1]);
                    break;

            }


            return command;
        }


        protected override void UpdateAppRepresentation()
        {
            base.UpdateAppRepresentation();

            for(int i =0; i<availableApps.Count; i++)
            {
                Console.WriteLine(i+1 + " " + availableApps[i].AppName);
            }
            Console.WriteLine(new string('-', 20));
            for(int i=0; i< installedApps.Count; i++)
            {
                Console.WriteLine(i + 1 + " " + installedApps[i].AppName);
            }
        }


        private void HideInstalledApp(Application installedApp)
        {
            availableApps.Remove(installedApp);
        }

        private void LoadInstalledApps()
        {
            for(int i = 0; i< installedAppsNames.Count; i++)
            {
                for(int j =0; j < availableApps.Count; j++)
                {
                    if (installedAppsNames[i] == availableApps[j].AppName)
                    {
                        InstallApp(availableApps[j]);
                        break;
                    }

                }
            }
        }


        private void InstallApp(Application app)
        {
            os.InstalledApps.Add(app);
            installedApps.Add(app);
            if(installedAppsNames.Contains(app.AppName) == false)
                installedAppsNames.Add(app.AppName);
            HideInstalledApp(app);
        }

        private void UninstallApp(Application app)
        {
            availableApps.Add(app);
            os.InstalledApps.Remove(app);
            installedApps.Remove(app);
            installedAppsNames.Remove(app.AppName);
        }
    }
}
