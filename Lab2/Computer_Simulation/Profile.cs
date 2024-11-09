using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer_Simulation
{
    public class Profile
    {
        private string name;
        private string password;
        private string fileSpacePath;

        
        public string Password { get => password; set => password = value; }
        public string Name { get => name; set => name = value; }
        public string FileSpacePath { get => fileSpacePath; set => fileSpacePath = value; }


        public Profile(string name, string password, string fileSpacePath) 
        {
            this.name = name;
            this.password = password;
            this.fileSpacePath = fileSpacePath;
        }
    }
}
