using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GPSAlarmSL.Resources;
using System.Device.Location;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace GPSAlarmSL
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Coordinate coord;
        private Geolocator myGeolocator;
       // private Geoposition pos = null;
        private MapLayer currentPositionLayer = new MapLayer();
        private MapLayer destinationPositionLayer = new MapLayer();
        private static Queue<GeoCoordinate> oldDestinationQueue = new Queue<GeoCoordinate>();


        // Конструктор
        public MainPage()
        {
            InitializeComponent();

            myGeolocator = new Geolocator();
            myGeolocator.MovementThreshold = 5;
            myGeolocator.PositionChanged += MyGeolocator_PositionChanged;

            mainMap.Layers.Add(currentPositionLayer);
            mainMap.Layers.Add(destinationPositionLayer);
        }

        private void MyGeolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            Dispatcher.BeginInvoke(() =>
            {
                coord.setCurrentCoordinate(args.Position.Coordinate.Latitude, args.Position.Coordinate.Longitude);
                drawPoint("current", args.Position.Coordinate.Latitude, args.Position.Coordinate.Longitude);
            });
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            coord = new Coordinate();

            mapSeetings();
        }

        private async void mapSeetings()
        {
            Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
            Geocoordinate myGeocoordinate = myGeoposition.Coordinate;
            GeoCoordinate myGeoCoordinate =
                CoordinateConverter.ConvertGeocoordinate(myGeocoordinate);

            mainMap.Center = myGeoCoordinate;
            mainMap.ZoomLevel = 17;
        }


        private void mainMap_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "Application ID";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "AuthenticationToken";
        }

        private void mainMap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //GeoCoordinate tappedCoordinate = this.mainMap.ConvertViewportPointToGeoCoordinate(e.GetPosition(this.mainMap));
            //coord.setDestinationCoordinate(tappedCoordinate.Latitude, tappedCoordinate.Longitude);
            //drawPoint("destination", tappedCoordinate.Latitude, tappedCoordinate.Longitude);
            //OldDestinationQueueWrite(tappedCoordinate.Latitude, tappedCoordinate.Longitude);
        }

        public void drawPoint(String t, double latitude, double longitude)
        {
            Color color = new Color();
            color = t == "current" ? Colors.Green : Colors.Red;

            MapOverlay overlay = new MapOverlay
            {
                
                GeoCoordinate = new GeoCoordinate
                {
                    Latitude = latitude,
                    Longitude = longitude
                },
                Content = new Ellipse
                {
                    Fill = new SolidColorBrush(color),
                    Width = 20,
                    Height = 20
                }
            };

            if (t == "current")
            {
                if (currentPositionLayer.Count != 0) currentPositionLayer.RemoveAt(0);
                currentPositionLayer.Add(overlay);
            }

            if (t == "destination")
            {
                if (destinationPositionLayer.Count != 0) destinationPositionLayer.RemoveAt(0);
                destinationPositionLayer.Add(overlay);
            }
        }

        private void ZoomIn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                mainMap.ZoomLevel++;
            }
            catch (ArgumentOutOfRangeException ex)
            {

            }            
        }

        private void ZoomOut_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                mainMap.ZoomLevel--;
            }
            catch (ArgumentOutOfRangeException ex)
            {

            }
        }

        private void MyPosition_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            mainMap.Center = coord.GetCoordinate();
        }

        private void SearchButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            String oldCoordinateParametr = HitchCoordinateInString();
            NavigationService.Navigate(new Uri(string.Format("/SearchPage.xaml?oldDestination={0}",oldCoordinateParametr), UriKind.Relative));
        }

        private string HitchCoordinateInString()
        {
            string result = "";
            List<GeoCoordinate> tmpGeoCoordList = oldDestinationQueue.ToList();

            for (int i = 0; i < tmpGeoCoordList.Count; i++)
            {
                result += tmpGeoCoordList[i].Latitude + ":" + tmpGeoCoordList[i].Longitude;

                if (i != tmpGeoCoordList.Count - 1) result += "@";
            }

            return result;
        }

        internal static void OldDestinationQueueWrite(double oldLatitude, double oldLongitude)
        {
            oldDestinationQueue.Enqueue(new GeoCoordinate(oldLatitude, oldLongitude));

            if (oldDestinationQueue.Count > 10)
                oldDestinationQueue.Dequeue();
        }

        internal static Queue<GeoCoordinate> GetOldDestinationCoordinateQueue()
        {
            return oldDestinationQueue;
        }

        private void mainMap_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            GeoCoordinate tappedCoordinate = this.mainMap.ConvertViewportPointToGeoCoordinate(e.GetPosition(this.mainMap));
            coord.setDestinationCoordinate(tappedCoordinate.Latitude, tappedCoordinate.Longitude);
            drawPoint("destination", tappedCoordinate.Latitude, tappedCoordinate.Longitude);
            OldDestinationQueueWrite(tappedCoordinate.Latitude, tappedCoordinate.Longitude);
        }
    }
}