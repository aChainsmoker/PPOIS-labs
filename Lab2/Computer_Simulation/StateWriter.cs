using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer_Simulation
{
    public class StateWriter
    {
        public void WriteState(List<string> data, string pathToFile)
        {       

            StreamWriter streamWriter = new StreamWriter(pathToFile);

            for(int i =0; i<data.Count; i++)
            {
                streamWriter.WriteLine(data[i]);
            }


            streamWriter.Close();
        }
    }
}
