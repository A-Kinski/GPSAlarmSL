using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;
using Windows.Storage;
using Windows.UI.Popups;

namespace BackgroundGPS
{
    public sealed class BackgroundGPS : IBackgroundTask
    {
        private Geolocator myGeolocator;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            //TODO прсомотр обновления координат в geolocator и после этого Run application?
            myGeolocator = new Geolocator();
            myGeolocator.MovementThreshold = 5;
            myGeolocator.PositionChanged += MyGeolocator_PositionChanged;
            MessageDialog dialog = new MessageDialog(String.Format("12345"));
            dialog.ShowAsync();
        }

        private async void MyGeolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            Double lat = args.Position.Coordinate.Latitude;
            Double longit = args.Position.Coordinate.Longitude;

            MessageDialog dialog = new MessageDialog(String.Format("{0}, {1}", lat, longit));
            await dialog.ShowAsync();
            //Dispatcher.BeginInvoke(() =>
            //{
            //    coord.setCurrentCoordinate(args.Position.Coordinate.Latitude, args.Position.Coordinate.Longitude);
            //    drawPoint("current", args.Position.Coordinate.Latitude, args.Position.Coordinate.Longitude);
            //});
        }
    }
}
