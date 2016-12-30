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
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation.Geofencing;
using Windows.UI.Core;
using Windows.UI.Popups;


namespace GPSAlarmSL
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Coordinate coord;
        private Geolocator myGeolocator;

        private MapLayer currentPositionLayer = new MapLayer();
        private MapLayer destinationPositionLayer = new MapLayer();
        private static Queue<GeoCoordinate> oldDestinationQueue = new Queue<GeoCoordinate>();

        private IBackgroundTaskRegistration _geolocTask = null;
        private const string SampleBackgroundTaskName = "BackgroundGPSTask";
        private const string SampleBackgroundTaskEntryPoint = "BackgroundGPS.BackgroundGPS";

        GeofenceMonitor _monitor = GeofenceMonitor.Current;

        // Конструктор
        public MainPage()
        {
            InitializeComponent();

            _monitor.GeofenceStateChanged += _monitor_GeofenceStateChanged;

            myGeolocator = new Geolocator();
            myGeolocator.MovementThreshold = 5;
            myGeolocator.PositionChanged += MyGeolocator_PositionChanged;

            mainMap.Layers.Add(currentPositionLayer);
            mainMap.Layers.Add(destinationPositionLayer);

            RegisterBackgroundTask();
        }

        private void _monitor_GeofenceStateChanged(GeofenceMonitor sender, object args)
        {
            var fences = sender.ReadReports();

            foreach (var report in fences)
            {
                if (report.Geofence.Id != "destinationCircle") continue;


                switch (report.NewState)
                {
                    case GeofenceState.Entered:
                        Dispatcher.BeginInvoke(async () =>
                        {
                            string message = "Hello, MessageBox!";
                            string caption = "Caption text";
                            MessageBoxButton buttons = MessageBoxButton.OKCancel;
                            // Show message box
                            MessageBoxResult result = MessageBox.Show(message, caption, buttons);

                            //MessageDialog dialog = new MessageDialog("Welcome to building 9");
                            //await dialog.ShowAsync();
                        });
                        break;
                }
            }
        }

        async private void RegisterBackgroundTask()
        {
            BackgroundAccessStatus backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();

            BackgroundTaskBuilder geolocTaskBuilder = new BackgroundTaskBuilder();

            geolocTaskBuilder.Name = SampleBackgroundTaskName;
            geolocTaskBuilder.TaskEntryPoint = SampleBackgroundTaskEntryPoint;

            var trigger = new LocationTrigger(LocationTriggerType.Geofence);

            geolocTaskBuilder.SetTrigger(trigger);

            _geolocTask = geolocTaskBuilder.Register();

            _geolocTask.Completed += new BackgroundTaskCompletedEventHandler(OnCompleted);
        }

        async private void OnCompleted(IBackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs e)
        {
            string str = "string";
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

            //создание geofence
            BasicGeoposition pos = new BasicGeoposition { Latitude = tappedCoordinate.Latitude, Longitude = tappedCoordinate.Longitude };
            Geofence destinationFence = new Geofence("destinationCircle", new Geocircle(pos, 100), MonitoredGeofenceStates.Entered, true);

            try
            {
                _monitor.Geofences.Add(destinationFence);
            }
            catch { }
        }
    }
}