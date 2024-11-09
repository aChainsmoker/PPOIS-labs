namespace MusicAppNS
{
    public class LoginSystem
    {
        private StateWriter stateWriter;
        private StateReader stateReader;
        public LoginSystem() 
        {
            stateReader = new StateReader();
            stateWriter = new StateWriter();

        }

        
        public bool LogInSystem(string login, string password)
        {
            List<string> profilesData = stateReader.ReadData("Profiles.txt");

            for(int i =0; i< profilesData.Count; i+=3)
            {
                if (profilesData[i] == login && profilesData[i+1]==password)
                {
                    LoadProfile(login, password, profilesData[i+2]);
                    return true;
                }
            }
            return false;
        }
        public bool RegisterInSystem(string login, string password, string rights)
        {
            List<string> profilesData = stateReader.ReadData("Profiles.txt");

            for (int i = 0; i < profilesData.Count; i += 3)
            {
                if (profilesData[i] == login)
                {
                    return false;
                }
            }
            profilesData.AddRange(new List<string> {login, password, rights});
            stateWriter.WriteState(profilesData, "Profiles.txt");
            CreateFileForProfile(login);
            LoadProfile(login, password, rights);
            return true;
        }

        private void  CreateFileForProfile(string login)
        {
            FileStream fs;
            fs = File.Create(login + ".txt");
            fs.Close();
        }
        private Profile LoadProfile(string login, string password, string rights)
        {
            Profile profile;
            if (rights == "artist")
                profile = new Artist(login, password);
            else
                profile = new Profile(login, password);
            MusicApp.Profile = profile;
            return profile;
        }
        

        public bool HandleInput(string command)
        {
            string[] commandParts = command.Trim().Split();
            switch (commandParts[0])
            {
                case "login":
                    if (LogInSystem(commandParts[1], commandParts[2]))
                        return true;
                    else
                        VisualInterface.UpdateVisualRepresentation(new List<string> { "Wrong login or password" });
                    break;
                case "register":
                    if (RegisterInSystem(commandParts[1], commandParts[2], commandParts[3]))
                        return true;
                    else
                        VisualInterface.UpdateVisualRepresentation(new List<string> { "The login has already taken, try another" });
                    break;
            }
            return false;
        }

        public void EnterTheLoginData()
        {
            while (true)
                if (HandleInput(Console.ReadLine())) break;
        }
    }
}
