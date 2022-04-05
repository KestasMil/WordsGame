using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace WordsGameCore.Services
{
    public class Scheduler : IScheduler
    {
        public virtual event EventHandler OnSchedule;
        private Timer timer;
        private DateTime ScheduledFor;

        public void ScheduleFor(DateTime shceduleFor)
        {
            // dispose old timer if exists
            if (timer != null)
            {
                timer.Dispose();
            }

            int triggerIn = (int)(shceduleFor - DateTime.Now).TotalMilliseconds;

            timer = new Timer((_) => {
                OnSchedule?.Invoke(this, EventArgs.Empty);
            }, null, triggerIn, Timeout.Infinite);

            ScheduledFor = shceduleFor;
        }

        public int SecondsRemaining()
        {
            if (ScheduledFor == null)
                throw new NullReferenceException("Nothing has been scheduled.");

            var remaining = (int)(ScheduledFor - DateTime.Now).TotalSeconds;
            return remaining > 0 ? remaining : 0;
        }
    }
}