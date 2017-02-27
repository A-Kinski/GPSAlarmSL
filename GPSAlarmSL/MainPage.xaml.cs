using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Device.Location;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using System.Windows.Controls;

namespace GPSAlarmSL
{
    public partial class MainPage : PhoneApplicationPage
    {
        #region
        /// <summary>
        /// Конструктор и системные функции
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        //handler для геолокатора на изменение позиции
        private void MyGeolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            if (!App.RunningInBackground)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    currentLatitude = args.Position.Coordinate.Latitude;
                    currentLongitude = args.Position.Coordinate.Longitude;
                    drawPoint("current", currentLatitude, currentLongitude);
                    checkDistance(currentLatitude, currentLongitude);
                });
            }
            else
            {
                checkDistance(args.Position.Coordinate.Latitude, args.Position.Coordinate.Longitude);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (App.Geolocator == null)
            {
                App.Geolocator = new Geolocator();
                App.Geolocator.MovementThreshold = 5;
                App.Geolocator.PositionChanged += MyGeolocator_PositionChanged;
            }

            alarm.MediaEnded += AlarmLoopback;

            mapSeetings();


            //TODO отрисовка кнопки отключения музыки если PlayingMusic - true
            if (App.PlayingMusic)
            {
                musicOff.Visibility = Visibility.Visible;
            }
        }

        //закольцовка медиа-элемента
        private void AlarmLoopback(object sender, RoutedEventArgs e)
        {
            var alarmMedia = sender as System.Windows.Controls.MediaElement;

            if (alarmMedia != null)
            {
                alarmMedia.Position = new TimeSpan(0);
                alarmMedia.Play();
            }
        }

        protected override void OnRemovedFromJournal(System.Windows.Navigation.JournalEntryRemovedEventArgs e)
        {
            App.Geolocator.PositionChanged -= MyGeolocator_PositionChanged;
            App.Geolocator = null;
        }

        //Отрисовка всплывающего уведомления
        private void ShowMessageDialog()
        {
            ToastTemplateType toastTemplate = ToastTemplateType.ToastText02;

            XmlDocument toastXML = ToastNotificationManager.GetTemplateContent(toastTemplate);

            XmlNodeList toastTemplateText = toastXML.GetElementsByTagName("text");

            toastTemplateText[0].AppendChild(toastXML.CreateTextNode("GPS Alarm!!!"));
            toastTemplateText[1].AppendChild(toastXML.CreateTextNode("Мы приехали. Нажми меня, чтобы выключить музыку"));

            IXmlNode toastNode = toastXML.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("duration", "long");

            ToastNotification toast = new ToastNotification(toastXML);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        //Включение будильника
        private void PlayAlarm()
        {
            App.PlayingMusic = true;

            Dispatcher.BeginInvoke(() => { alarm.Play(); });

            if (App.RunningInBackground)
            {
                ShowMessageDialog();
            }
            else
            {
                musicOff.Visibility = Visibility.Visible;
            }
        }
        #endregion

        #region
        /// <summary>
        /// Секция для работы с координатами
        /// </summary>

        //текущие координаты
        private double currentLatitude { get; set; } = 0;
        private double currentLongitude { get; set; } = 0;

        //координаты точки назначения
        private double destinationLatitude { get; set; } = 1;
        private double destinationLongitude { get; set; } = 1;

        //расстояние для срабатывания будильника
        private Int16 alarmDistance = 500;

        //установка координат точки назначения
        private void WriteDestinationCoordinate(double latitude, double longitude)
        {
            destinationLatitude = latitude;
            destinationLongitude = longitude;
        }

        //расчет дистанции
        private void checkDistance(double latitude, double longitude)
        {
            //TODO если надо тащить координаты места назначения из памяти
            if (Coordinate.GetDistance(latitude, longitude, destinationLatitude, destinationLongitude) < alarmDistance)
            {
                PlayAlarm();                
            }
            
        }

        //Сброс координат места назначения
        private void SetDestionationCoordinateToZero()
        {
            destinationLatitude = 1;
            destinationLongitude = 1;
        }
        #endregion

        #region
        /// <summary>
        /// Дополнительные функции карты
        /// </summary>
        /// 
        private MapLayer currentPositionLayer = new MapLayer();
        private MapLayer destinationPositionLayer = new MapLayer();

        //первоначальная настройка карты
        private async void mapSeetings()
        {
            Geoposition myGeoposition = await App.Geolocator.GetGeopositionAsync();
            Geocoordinate myGeocoordinate = myGeoposition.Coordinate;
            GeoCoordinate myGeoCoordinate =
                CoordinateConverter.ConvertGeocoordinate(myGeocoordinate);

            mainMap.Center = myGeoCoordinate;
            mainMap.ZoomLevel = 17;
        }

        //отрисовка точек на карте
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
                    Width = 30,
                    Height = 30
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

        //Удаление точки назначения с карты
        private void DeleteDestinationPositionLayer()
        {
            if (destinationPositionLayer.Count != 0) destinationPositionLayer.RemoveAt(0);
        }
        #endregion

        #region
        /// <summary>
        /// Функции взаимодействия с UI
        /// </summary>
        /// 

        private void mainMap_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "Application ID";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "AuthenticationToken";

            mainMap.Layers.Add(currentPositionLayer);
            mainMap.Layers.Add(destinationPositionLayer);
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
            mainMap.Center = new GeoCoordinate(currentLatitude, currentLongitude);
        }

        private void SearchButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            String oldCoordinateParametr = HitchCoordinateInString();
            NavigationService.Navigate(new Uri(string.Format("/SearchPage.xaml?oldDestination={0}", oldCoordinateParametr), UriKind.Relative));
        }

        private void mainMap_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //GeoCoordinate tappedCoordinate = this.mainMap.ConvertViewportPointToGeoCoordinate(e.GetPosition(this.mainMap));

            //WriteDestinationCoordinate(tappedCoordinate.Latitude, tappedCoordinate.Longitude);

            //drawPoint("destination", tappedCoordinate.Latitude, tappedCoordinate.Longitude);
            //OldDestinationQueueWrite(tappedCoordinate.Latitude, tappedCoordinate.Longitude);
        }

        private void mainMap_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            GeoCoordinate tappedCoordinate = this.mainMap.ConvertViewportPointToGeoCoordinate(e.GetPosition(this.mainMap));

            WriteDestinationCoordinate(tappedCoordinate.Latitude, tappedCoordinate.Longitude);

            drawPoint("destination", tappedCoordinate.Latitude, tappedCoordinate.Longitude);
            OldDestinationQueueWrite(tappedCoordinate.Latitude, tappedCoordinate.Longitude);
        }

        private void musicOff_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            alarm.Stop();
            App.PlayingMusic = false;
            musicOff.Visibility = Visibility.Collapsed;

            SetDestionationCoordinateToZero();

            DeleteDestinationPositionLayer();

        }
        #endregion

        #region
        /// <summary>
        /// Работа с очередью координат места назначения
        /// </summary>

        private static Queue<GeoCoordinate> oldDestinationQueue = new Queue<GeoCoordinate>();

        private string HitchCoordinateInString()
        {
            string result = currentLatitude + ":" + currentLongitude;

            List<GeoCoordinate> tmpGeoCoordList = oldDestinationQueue.ToList();

            if (tmpGeoCoordList.Count != 0)
            {
                result += "@";

                for (int i = 0; i < tmpGeoCoordList.Count; i++)
                {
                    result += tmpGeoCoordList[i].Latitude + ":" + tmpGeoCoordList[i].Longitude;

                    if (i != tmpGeoCoordList.Count - 1) result += "@";
                }
            }

            return result;
        }

        public static void OldDestinationQueueWrite(double oldLatitude, double oldLongitude)
        {
            oldDestinationQueue.Enqueue(new GeoCoordinate(oldLatitude, oldLongitude));

            if (oldDestinationQueue.Count > 10)
                oldDestinationQueue.Dequeue();
        }

        public static Queue<GeoCoordinate> GetOldDestinationCoordinateQueue()
        {
            return oldDestinationQueue;
        }
        #endregion

    }
}