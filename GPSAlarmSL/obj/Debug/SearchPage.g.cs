﻿#pragma checksum "C:\Users\Anton\Source\Repos\GPSAlarmSL\GPSAlarmSL\SearchPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "723869F29CC2E252D55D9DF56B0F37FB"
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
    
    
    public partial class SearchPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBox searchTextBox;
        
        internal System.Windows.Controls.StackPanel oldDestinationStackPanel;
        
        internal System.Windows.Controls.Button goBackButton;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/GPSAlarmSL;component/SearchPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.searchTextBox = ((System.Windows.Controls.TextBox)(this.FindName("searchTextBox")));
            this.oldDestinationStackPanel = ((System.Windows.Controls.StackPanel)(this.FindName("oldDestinationStackPanel")));
            this.goBackButton = ((System.Windows.Controls.Button)(this.FindName("goBackButton")));
        }
    }
}

