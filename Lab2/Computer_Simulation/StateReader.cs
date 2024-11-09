using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer_Simulation
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

            FileStream fs;
            if (!File.Exists(pathToDataFile))
            {
                fs = File.Create(pathToDataFile);
                fs.Close();
            }

            StreamReader streamReader = new StreamReader(pathToDataFile);
            while(streamReader.Peek() > -1)
            {
                data.Add(streamReader.ReadLine());
            }

            streamReader.Close();
            return data;   
        }
    }
}
