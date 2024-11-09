using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer_Simulation
{
    public class ProfileSelector : Application, ILoadable
    {
        private string appName = "Profile Selector";
        private List<Profile> profiles;
        private OS os;
        private Profile selectedProfile;
        private string profilesFileDirectory;

        public override string AppName { get => appName; set => appName = value; }
        public Profile SelectedProfile { get => selectedProfile; }
        public  List<Profile> Profiles { get => profiles; }


        public ProfileSelector(OS os)
        {
            profiles = new List<Profile>();
            profilesFileDirectory = Directory.GetCurrentDirectory();
            this.os = os;
            ReadData();
        }


        public void CreateNewProfile(string name, string password, string fileSpacePath)
        {
            profiles.Add(new Profile(name, password, fileSpacePath));
        }


        public void DeleteProfile(Profile profile)
        {
            profiles.Remove(profile);
            if (selectedProfile == null)
                Directory.Delete(profile.FileSpacePath, true);
            else
            {
                Directory.Delete("..\\"+profile.FileSpacePath, true);
            }
        }

        public void SelectProfile(int i, OS os)
        {
            if (selectedProfile != null)
                Directory.SetCurrentDirectory("..\\");

            os.FileSystemPath = profiles[i-1].FileSpacePath;
            selectedProfile = profiles[i-1];
            os.loadProfilesApps?.Invoke();
        }

        public void ReadData()
        {
            List<string> stringData = stateReader.ReadData("Profiles.txt");

            for (int i = 0; i < stringData.Count; i += 3)
            {
                CreateNewProfile(stringData[i], stringData[i + 1], stringData[i+2]);
            }
        }

        public void WriteData()
        {
            List<string> stringData = new List<string>();

            for(int i = 0; i < profiles.Count; i += 1)
            {
                stringData.Add(profiles[i].Name);
                stringData.Add(profiles[i].Password);
                stringData.Add(profiles[i].FileSpacePath);
            }

            stateWriter.WriteState(stringData, profilesFileDirectory+"\\Profiles.txt");

        }

        public override void LaunchApp()
        {
            appStateUpdated += UpdateAppRepresentation;
        }

        public override void CloseApp()
        {
            appStateUpdated -= UpdateAppRepresentation;
            WriteData();
        }

        protected override string HandleInput(string command)
        {
            string[] commandParts = command.Split();

            switch (commandParts[0])
            {
                case "select":
                    if (AskForPassword(Convert.ToInt32(commandParts[1])-1) == true)
                        SelectProfile(Convert.ToInt32(commandParts[1]), os);
                    break;
                case "add":
                    CreateNewProfile(commandParts[1], commandParts[2], commandParts[3]);
                    break;
                case "delete":
                    if (profiles[Convert.ToInt32(commandParts[1]) - 1] == selectedProfile)
                        break;
                    if (AskForPassword(Convert.ToInt32(commandParts[1]) - 1) == true)
                        DeleteProfile(profiles[Convert.ToInt32(commandParts[1])-1]);
                    break;

            }
            return command;
        }

        protected override void UpdateAppRepresentation()
        {
            base.UpdateAppRepresentation();

            for (int i = 0; i < profiles.Count; i++)
            {
                Console.WriteLine(i+1 + ". " + profiles[i].Name);
            }

            if (selectedProfile != null)
                Console.WriteLine("\nSelected Profile is " + selectedProfile.Name);
        }

        private bool AskForPassword(int i)
        {
            Console.WriteLine("Enter the profile's password");

            if (Console.ReadLine() == profiles[i].Password)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Incorrect password");
                Thread.Sleep(200);
                return false;
            }
        }
    }
}
