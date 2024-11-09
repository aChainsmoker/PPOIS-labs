namespace MusicAppNS
{
    public class StateReader
    {
        private List<string> data;

        public StateReader()
        {
            data = new List<string>();
        }

        public List<string> ReadData(string pathToDataFile)
        {
            data.Clear();

            if(!File.Exists(pathToDataFile))
            {
                FileStream fs;
                fs = File.Create(pathToDataFile);
                fs.Close();
            }

            StreamReader streamReader = new StreamReader(pathToDataFile);
            while (streamReader.Peek() > -1)
            {
                data.Add(streamReader.ReadLine());
            }

            streamReader.Close();
            return data;
        }
    }
}
