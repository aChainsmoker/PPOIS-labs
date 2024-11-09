using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Computer_Simulation
{
    public class CustomTimer
    {
        private System.Timers.Timer timer;
        private DateTime endTime;
        private double durationInSeconds;

        public CustomTimer(double durationInSeconds)
        {
            this.durationInSeconds = durationInSeconds;
            timer = new System.Timers.Timer(1000); 
        }

        public void StartTimer()
        {
            endTime = DateTime.Now.AddSeconds(durationInSeconds);
            timer.Start();
        }

        public void StopTimer()
        {
            timer.Stop();
        }

        public TimeSpan GetRemainingTime()
        {
            TimeSpan remainingTime = endTime - DateTime.Now;
            return remainingTime < TimeSpan.Zero ? TimeSpan.Zero : remainingTime;
        }


    }
}
