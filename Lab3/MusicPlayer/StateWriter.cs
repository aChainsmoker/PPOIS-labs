namespace MusicAppNS
{
    public class StateWriter
    {

        public void WriteState(List<string> data, string pathToFile)
        {

            StreamWriter streamWriter = new StreamWriter(pathToFile);

            for (int i = 0; i < data.Count; i++)
            {
                streamWriter.WriteLine(data[i]);
            }


            streamWriter.Close();
        }
    }
        
}
