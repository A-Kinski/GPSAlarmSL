﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace GPSAlarmSL
{
    public partial class SearchPage : PhoneApplicationPage
    {
        private double userLatitude { get; set; } = 0;
        private double userLongitude { get; set; } = 0;

        public SearchPage()
        {
            InitializeComponent();
        }

        private void goBackButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Первые две координаты в incomingString - текущие координаты пользователя
            string incomingString = NavigationContext.QueryString["oldDestination"].ToString();

            //string[] tmpCoordiantes = incomingString.Split('@');

            //for (int i = 0; i < tmpCoordiantes.Length; i++)
            //{
            //    string[] tmpCoordinate = tmpCoordiantes[i].Split(':');

            //    TextBlock oldDestination = new TextBlock();
            //    oldDestination.Text = tmpCoordinate[0] + " " + tmpCoordinate[1];
            //    oldDestinationStackPanel.Children.Add(oldDestination);
            //}
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Поиск по результатов по тексту
            String text = searchTextBox.Text;
        }
    }
}