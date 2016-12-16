using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Scheduler;


namespace GPSAlarmSL
{
    class GpsAlarm
    {
        public GpsAlarm()
        {
        }

        public void createAlarm()
        {
            Alarm alarm = new Alarm("destinationAlarm");
            alarm.Content = "Приехали";
            alarm.Sound = new Uri("/Assets/Media/Ring01.wma", UriKind.Relative);
            alarm.BeginTime = DateTime.Now.AddSeconds(5);
            alarm.ExpirationTime = alarm.BeginTime.AddMinutes(2);
            alarm.RecurrenceType = RecurrenceInterval.None;

            if (ScheduledActionService.Find("destinationAlarm") != null)
            {
                var oldAlarm = ScheduledActionService.Find("destinationAlarm");
                if (oldAlarm.BeginTime < DateTime.Now) oldAlarm.BeginTime = DateTime.Now.AddSeconds(5);
            }
            else
            {
                ScheduledActionService.Add(alarm);
            }
        }
    }
}
