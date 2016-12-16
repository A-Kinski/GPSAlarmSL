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
        private Alarm alarm;

        public GpsAlarm()
        {
            alarm = new Alarm("destinationAlarm");
            alarm.Content = "Приехали";
            alarm.Sound = new Uri("/Assets/Media/Ring01.wma", UriKind.Relative);
            //alarm.BeginTime = DateTime.Now.AddSeconds(5);
            //alarm.ExpirationTime = alarm.BeginTime.AddMinutes(2);
            alarm.RecurrenceType = RecurrenceInterval.None;

            //ScheduledActionService.Add(alarm);
        }

        public void createAlarm()
        {
            //Alarm alarm = new Alarm("destinationAlarm");
            //alarm.Content = "Приехали";
            //alarm.Sound = new Uri("/Assets/Media/Ring01.wma", UriKind.Relative);
            //alarm.BeginTime = DateTime.Now.AddSeconds(5);
            //alarm.ExpirationTime = alarm.BeginTime.AddMinutes(2);
            //alarm.RecurrenceType = RecurrenceInterval.None;

            //if (ScheduledActionService.Find("destinationAlarm") != null)
            //{
            //    var oldAlarm = ScheduledActionService.Find("destinationAlarm");
            //    if (oldAlarm.BeginTime < DateTime.Now)
            //    {
            //        ScheduledActionService.Remove("destinationAlarm");
            //        oldAlarm.BeginTime = DateTime.Now.AddSeconds(5);                    
            //        ScheduledActionService.Add(oldAlarm);
            //    }

            //}
            //else
            //{
            // //   ScheduledActionService.Add(alarm);
            //}

            alarm.BeginTime = DateTime.Now.AddSeconds(20);
            alarm.ExpirationTime = alarm.BeginTime.AddMinutes(2);

            if (ScheduledActionService.Find("destinationAlarm") != null)
            {
                if (ScheduledActionService.Find("destinationAlarm").BeginTime < DateTime.Now)
                {
                    Alarm bufferAlarm = ScheduledActionService.Find("destinationAlarm") as Alarm;
                    bufferAlarm.BeginTime = DateTime.Now.AddSeconds(20);
                    bufferAlarm.ExpirationTime = alarm.BeginTime.AddMinutes(2);
                    bufferAlarm.Content = "123";
                    ScheduledActionService.Replace(bufferAlarm);
                }
            }
            else
            {
                ScheduledActionService.Add(alarm);
            }
        }
    }
}
