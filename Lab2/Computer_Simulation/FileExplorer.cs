using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer_Simulation
{
    public class FileExplorer : Application 
    {
        private string appName = "File Explorer";
        private string parentDir;
        private string externalSpace;
        private bool isFileEditorInstalled;
        private OS os;
        public override string AppName { get => appName; set => appName = value; }

        public FileExplorer(OS os)
        {
            this.os = os;
            externalSpace = Directory.GetCurrentDirectory();
        }
        public override void CloseApp()
        {
            appStateUpdated -= UpdateAppRepresentation;
            Directory.SetCurrentDirectory(parentDir);
        }

        public override void LaunchApp()
        {
            appStateUpdated += UpdateAppRepresentation;
            parentDir = Path.Combine(externalSpace, os.FileSystemPath);
            Directory.SetCurrentDirectory(parentDir);
            isFileEditorInstalled = CheckOnFileEditor();
        }

        public override string HandleInput(string command)
        {
            FileStream fs;
            string[] commandParts = command.Split();
            switch (commandParts[0])
            {
                case "open":
                    string content = File.ReadAllText(commandParts[1]);
                    Console.WriteLine(content);
                    if(Console.IsOutputRedirected == false)
                        Console.ReadKey();
                    break;
                case "edit":
                    if (isFileEditorInstalled == false)
                        break;
                    FileEditor fileEditor = new FileEditor(commandParts[1]);
                    fileEditor.LaunchApp();
                    fileEditor.TakeTheInput();
                    break;
                case "add":
                    fs = File.Create(commandParts[1]);
                    fs.Close();
                    break;
                case "delete":
                    File.Delete(commandParts[1]);
                    break;
                case "adir":
                    Directory.CreateDirectory(commandParts[1]);
                    break;
                case "ddir":
                    Directory.Delete(commandParts[1], true);
                    break;
                case "dir":
                    Directory.SetCurrentDirectory(commandParts[1]);
                    break;
                case "updir":
                    if(Directory.GetCurrentDirectory() != parentDir)
                        Directory.SetCurrentDirectory("..\\");
                    break;

            }
            return command;

        }

        protected override void UpdateAppRepresentation()
        {
            base.UpdateAppRepresentation();

            string[] dirs = Directory.GetDirectories(Directory.GetCurrentDirectory());
            foreach (string dir in dirs)
            {
                Console.WriteLine("  " + dir);
            }

            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
            foreach (string file in files)
            {
                Console.WriteLine("  " + file);
            }
        }

        private bool CheckOnFileEditor()
        {
            for(int i =0; i<os.InstalledApps.Count; i++)
            {
                if (os.InstalledApps[i] is FileEditor fileEditor)
                    return true;
            }
            return false;
        }

    }
}
