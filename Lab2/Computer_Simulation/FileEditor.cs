using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer_Simulation
{
    public class FileEditor: Application
    {
        private string appName = "File Editor";
        private string fileBeingEdited;

        public override string AppName { get => appName; set => appName = value; }

        public FileEditor(string pathToFile)
        {
            fileBeingEdited = pathToFile;
        }

        public FileEditor()
        {

        }

        public override void CloseApp()
        {
            appStateUpdated -= UpdateAppRepresentation;
        }

        public override void LaunchApp()
        {
            appStateUpdated += UpdateAppRepresentation;
        }

        protected override string HandleInput(string command)
        {
            string[] commandParts = command.Split();

            if (fileBeingEdited == null)
                return command;

            switch(commandParts[0])
            {
                case "rewrite":
                    File.WriteAllText(fileBeingEdited, ReadInput());
                    break;
                case "line":
                    string[] lines = File.ReadAllLines(fileBeingEdited);
                    lines[Convert.ToInt32(commandParts[1]) - 1] = String.Empty;
                    for (int i =2; i<commandParts.Length; i++)
                        lines[Convert.ToInt32(commandParts[1])-1] += (commandParts[i] + " ");
                    File.WriteAllLines(fileBeingEdited, lines);
                    break;
                case "append":
                    File.AppendAllText(fileBeingEdited, ReadInput());
                    break;
            }
            return command;
        }


        private string ReadInput()
        {
            string line;
            string newContent = String.Empty;
            while ((line = Console.ReadLine()) != "end")
            {
                newContent += line + Environment.NewLine;
            }
            return newContent;
        }
        protected override void UpdateAppRepresentation()
        {
            base.UpdateAppRepresentation();

            if (fileBeingEdited == null)
            {
                Console.Write("This app does nothing by itself, use it through File Explorer\n");
                return;
            }

            string[] content = File.ReadAllLines(fileBeingEdited);
            for(int i = 0; i<content.Length; i++)
            {
                Console.WriteLine(i+1 + " " + content[i] );
            }
           
        }

    }
}
