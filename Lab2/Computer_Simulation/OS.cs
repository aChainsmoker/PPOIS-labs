using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer_Simulation
{
    public class OS
    {
        private string fileSystemPath;
        private ProfileSelector profileSelector;
        private FileExplorer fileExplorer;
        private Desktop desktop; 
        private AppStore store;
        private List<Application> installedApplications;

        public List<Application> InstalledApps {  get => installedApplications; set => installedApplications = value; }
        public Desktop Desktop { get => desktop; }
        public Action loadProfilesApps;
        public string FileSystemPath 
        {
            get => fileSystemPath;
            set 
            { 
                fileSystemPath = value; 

                if(!Directory.Exists(fileSystemPath))
                    Directory.CreateDirectory(fileSystemPath);

                Directory.SetCurrentDirectory(fileSystemPath);
                
            }  
        }

        public OS()
        {
            profileSelector = new ProfileSelector(this);
            fileExplorer = new FileExplorer(this);
            desktop = new Desktop(this);
            store = new AppStore(this);

            loadProfilesApps += store.LoadProfilesApps;
            loadProfilesApps += desktop.CheckOnWallpapersApp;
            loadProfilesApps += desktop.SetProfileActivation;

            installedApplications = new List<Application>();
            installedApplications.Add(profileSelector);
            installedApplications.Add(fileExplorer);
            installedApplications.Add(store);

            if (!Console.IsOutputRedirected)
            {
                desktop.LaunchApp();
                desktop.TakeTheInput();
            }
        }
    }
}
