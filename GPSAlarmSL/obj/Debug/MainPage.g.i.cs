﻿#pragma checksum "C:\Users\Anton\Documents\Visual Studio 2015\Projects\GPSAlarmSL\GPSAlarmSL\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "96326728BAD743A377812980EB739B47"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace GPSAlarmSL {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal Microsoft.Phone.Maps.Controls.Map mainMap;
        
        internal System.Windows.Controls.Button ZoomIn;
        
        internal System.Windows.Controls.Button ZoomOut;
        
        internal System.Windows.Controls.Button MyPosition;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/GPSAlarmSL;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.mainMap = ((Microsoft.Phone.Maps.Controls.Map)(this.FindName("mainMap")));
            this.ZoomIn = ((System.Windows.Controls.Button)(this.FindName("ZoomIn")));
            this.ZoomOut = ((System.Windows.Controls.Button)(this.FindName("ZoomOut")));
            this.MyPosition = ((System.Windows.Controls.Button)(this.FindName("MyPosition")));
        }
    }
}

