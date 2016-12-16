using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Device.Location;

namespace GPSAlarmSL
{
    class Coordinate
    {
        //Коллекция с координатами - 0 - latitude широта, 1 - longitude долгота
        private ObservableCollection<Double> currentCoordinate = new ObservableCollection<Double>() { 57, 35 };
        private ObservableCollection<Double> destinationCoordinate = new ObservableCollection<double>() { 1, 1 };

        private Int32 R = 6371210; //радиус Земли
        private Int16 alarmDistance = 500;
        

        public Coordinate()
        {
            currentCoordinate.CollectionChanged += coordinate_CoolectionChanged;
            destinationCoordinate.CollectionChanged += coordinate_CoolectionChanged;
        }

        private void coordinate_CoolectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            checkCoordinate();
        }

        internal void setCurrentCoordinate(double latitude, double longitude)
        {
            currentCoordinate[0] = degreToRadian(latitude);
            currentCoordinate[1] = degreToRadian(longitude);
        }

        private void checkCoordinate()
        {
            //пеерводим координаты в радианы
            //double current_x = degreToRadian(currentCoordinate[0]);
            //double current_y = degreToRadian(currentCoordinate[1]);
            //double destination_x = degreToRadian(destinationCoordinate[0]);
            //double destination_y = degreToRadian(destinationCoordinate[1]);
            //найдем расстояние между двумя точками на сфере
            //Double distance = Math.Round(R * Math.Acos(Math.Sin(current_x) * Math.Sin(destination_x) + 
            //    Math.Cos(current_x) * Math.Cos(destination_x) * Math.Cos(destination_y - current_y)));
            Double distance = Math.Round(R * Math.Acos(Math.Sin(currentCoordinate[0]) * Math.Sin(destinationCoordinate[0]) +
                 Math.Cos(currentCoordinate[0]) * Math.Cos(destinationCoordinate[0]) * Math.Cos(destinationCoordinate[1] - currentCoordinate[1])));

            //проверка - если дистанция меньше определенного расстояния - делаем alarm
            if (distance < alarmDistance) alarmLetsGo();

        }

        private void alarmLetsGo()
        {
            GpsAlarm gpsAlarm = new GpsAlarm();
            gpsAlarm.createAlarm();
            
        }

        public void setDestinationCoordinate(Double latitude, Double longitude)
        {
            destinationCoordinate[0] = degreToRadian(latitude);
            destinationCoordinate[1] = degreToRadian(longitude);
        }

        private double degreToRadian(double coordinate)
        {
            return (coordinate * Math.PI) / 180;
        }

        private double radianToDegree(double coordinate)
        {
            return (coordinate * 180) / Math.PI;
        }

        public GeoCoordinate GetCoordinate()
        {
            GeoCoordinate coordinate = new GeoCoordinate(radianToDegree(currentCoordinate[0]), radianToDegree(currentCoordinate[1]));
            return coordinate;
        }

    }
}
