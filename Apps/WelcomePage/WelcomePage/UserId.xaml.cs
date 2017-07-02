﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.Devices.Enumeration;
using Windows.Devices.Bluetooth.Rfcomm;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WelcomePage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserId : Page
    {
        //private BT_Connect bt_connect;
        //private bool connected = false;

        public UserId()
        {
            this.InitializeComponent();
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            //ConnectToBT();
            if (!string.IsNullOrWhiteSpace(App.CurrentNick))
            {
                App.StartTime = TimeUtils.CurrentTimeMillis();
                this.Frame.Navigate(typeof(Joystick), null);
            }
        }

        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Statistics), null);
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentNick = Nickname.Text;
            if (string.IsNullOrWhiteSpace(App.CurrentNick))
            {
                Error.Text = "      Please enter your name";
                Error.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
            }
            else
            {
                Error.Text = "";
            }
        }

        private void TextInput_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) {
                OK.Focus(FocusState.Programmatic);
              
            }
        }

        //private async void ConnectToBT()
        //{
        //    try
        //    {
        //        DeviceInformationCollection devices = await bt_connect.FindPairedDevicesAsync();

        //        var my_device = devices.Single(x => x.Name == "HC-06");

        //        bt_connect.IsConnected = await bt_connect.ConnectAsync(my_device);

        //        if (bt_connect.IsConnected)
        //        {
        //            connected = true;
        //            return;
        //        }
        //        else
        //            this.Frame.Navigate(typeof(MainPage), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        this.Frame.Navigate(typeof(MainPage), null); 
        //    }
        //}
    }
}
