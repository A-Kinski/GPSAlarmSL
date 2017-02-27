using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Device.Location;

namespace GPSAlarmSL
{
    static class Coordinate
    {
        private const Int32 R = 6371210; //радиус Земли

        public static double GetDistance(double currentLatitude, double currentLongitude, double destinationLatitude, double destinationLongitude)
        {
            double distance = Math.Round(R * Math.Acos(Math.Sin(degreToRadian(currentLatitude)) * Math.Sin(degreToRadian(destinationLatitude)) +
                 Math.Cos(degreToRadian(currentLatitude)) * Math.Cos(degreToRadian(destinationLatitude)) * Math.Cos(degreToRadian(destinationLongitude) - degreToRadian(currentLongitude))));

            return distance;
        }

        private static double degreToRadian(double coordinate)
        {
            return (coordinate * Math.PI) / 180;
        }

    }
}
