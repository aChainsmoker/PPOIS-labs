using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer_Simulation
{
    public abstract class Application
    {
        public StateReader stateReader;
        public StateWriter stateWriter;
        public Action appStateUpdated;


        public Application()
        {
            stateReader = new StateReader();
            stateWriter = new StateWriter();   
        }
        public virtual void TakeTheInput()
        {
            while (true)
            {
                appStateUpdated?.Invoke();
                try
                {
                    if (HandleInput(Console.ReadLine()) == "exit")
                    {
                        CloseApp();
                        break;
                    }
                }
                catch(Exception e) { Console.WriteLine("Incorrect Input. Try Again.\n" + e.Message); Thread.Sleep(5000); }
            }
        }
        protected virtual void UpdateAppRepresentation()
        {
            if (Console.IsOutputRedirected == false)
                Console.Clear();
        }
        protected abstract string HandleInput(string command);
        public abstract void LaunchApp();
        public abstract void CloseApp();
        public abstract string AppName { get; set; }


    }
}
