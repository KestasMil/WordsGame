using System;
using System.Collections.Generic;
using System.Text;

namespace WordsGameCore.Services
{
    public interface IScheduler
    {
        event EventHandler OnSchedule;

        void ScheduleFor(DateTime dateTime);
        int SecondsRemaining();
    }
}
