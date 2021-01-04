using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace InputRecorder
{
    public class EventTimerArgs : EventArgs
    {
        public long ElapsedMilliSeconds;
        public long Delay;
        public EventTimerArgs(long elapsedMilliSeconds,long delay)
        {
            ElapsedMilliSeconds = elapsedMilliSeconds;
            Delay = delay;
        }
    }

    public delegate void EventTimerElapsedHandler(EventTimerArgs e);

    class EventTimer
    {
        public event EventTimerElapsedHandler Elapsed;
        Thread timerThread;
        Stopwatch stopWatch;
        public int Interval;
        private bool isEnabled;

        public EventTimer(int interval)
        {
            Interval = interval;
            stopWatch = new Stopwatch();
        }

        public void Start()
        {
            isEnabled = true;
            timerThread = new Thread(Tick);
            timerThread.Start();
        }

        public void Stop()
        {
            isEnabled = false;
            timerThread.Abort();
        }

        private void Tick()
        {
           stopWatch.Reset();
           stopWatch.Start();
           long lastElapsedTime = 0;
           while (isEnabled)
           {
               if ((stopWatch.ElapsedMilliseconds - lastElapsedTime >= (long)Interval))
               {
                   EventTimerArgs args = new EventTimerArgs(stopWatch.ElapsedMilliseconds, stopWatch.ElapsedMilliseconds - lastElapsedTime - Interval);
                   Elapsed(args);
                   lastElapsedTime = stopWatch.ElapsedMilliseconds;
               }
           }
        }
    }
}
