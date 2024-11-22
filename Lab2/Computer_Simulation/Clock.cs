using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer_Simulation
{
    public class Clock : Application
    {
        private string appName = "Clock";
        private string timeData;
        private Stopwatch stopwatch;
        private CustomTimer timer;
        private bool updateMode;
        private int clockMode;

        public override string AppName { get => appName; set => appName = value; }

        public Clock()
        {
            stopwatch = new Stopwatch();
        }


        public override void LaunchApp()
        {
            appStateUpdated += UpdateAppRepresentation;
        }

        public override void CloseApp()
        {
            appStateUpdated -= UpdateAppRepresentation;
        }

        public override string HandleInput(string command)
        {
            while (updateMode == true)
            {
                if (Console.IsOutputRedirected == true)
                    updateMode = false;
                else if(Console.KeyAvailable)
                     if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                        updateMode = false;
                    
                Thread.Sleep(50);

                switch (clockMode)
                {
                    case 1:
                        timeData = DateTime.Now.ToString();
                        break;
                    case 2:
                        timeData = stopwatch.ToString();
                        break;
                    case 3:
                        timeData = timer.GetRemainingTime().ToString();
                        break;
                }

                UpdateAppRepresentation();
            }

            string[] commandParts = command.Trim().Split();

            switch (commandParts[0])
            {
                case "time":
                    clockMode = 1;
                    SetToUpdateMode();
                    break;
                case "stopwatch":
                    clockMode = 2;
                    if (commandParts[1] == "start")
                    {
                        stopwatch.Start();
                        SetToUpdateMode();
                    }
                    else if (commandParts[1] == "stop")
                    {
                        stopwatch.Stop();
                        stopwatch.Reset();
                        timeData = stopwatch.ToString();
                    }
                    break;
                case "timer":
                    clockMode = 3;
                    if (commandParts[1] == "start")
                    {
                        timer = new CustomTimer(Convert.ToDouble(commandParts[2]));
                        timer.StartTimer();
                        SetToUpdateMode();
                    }
                    else if (commandParts[1] == "stop")
                    {
                        timer.StopTimer();
                        timeData = timer.GetRemainingTime().ToString();
                    }

                    break;
            }

            return command;
        }

        private void SetToUpdateMode()
        {
            updateMode = true;
            HandleInput(String.Empty);
        }

        protected override void UpdateAppRepresentation()
        {
            base.UpdateAppRepresentation();
            Console.WriteLine(timeData);
        }
    }
}
