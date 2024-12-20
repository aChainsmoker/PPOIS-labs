﻿namespace PostMachineNS
{
    public class Reader
    {
        private const int pointerDownUnicode = 8595;
        private string? pathToFile;

        public string? tapeData;
        public List<string?> behaviourData = new List<string?>();
        public int pointerPlace = 0;

        public void AskForFilePath(UserInterface userInterface)
        {
            userInterface.AskForFilePath();
        }

        public List<string> ReadTheHelpDocument(string pathToFile)
        {
            List<string> lines = new List<string>();
            StreamReader streamReader;
            streamReader = new StreamReader(pathToFile);

            while (streamReader.Peek() > -1)
            {
                lines.Add(streamReader.ReadLine());
            }

            return lines;

        }

        public void ReadTheTape(string pathToFile)
        {
            StreamReader streamReader;

            streamReader = new StreamReader(pathToFile);

            if (streamReader.Peek() > -1)
            {
                while (streamReader.Peek() != pointerDownUnicode)
                {
                    streamReader.Read();
                    pointerPlace++;
                }

                streamReader.ReadLine();
                tapeData = streamReader.ReadLine();

                while (streamReader.Peek() > -1)
                {
                    behaviourData.Add(streamReader.ReadLine());
                }
            }
            streamReader.Close();
        }
    }
}