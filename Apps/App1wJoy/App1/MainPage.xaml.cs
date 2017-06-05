using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;
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
using System.Threading.Tasks;
using System.Text;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace JoystickApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainPage : Page
    {
        internal BT_Connect bt_connect;
        public MainPage()
        {
            //bt_connect = new BT_Connect(tbStatus, tbError);
            bt_connect = BT_Connect.getBT_Conn();
            this.InitializeComponent();
        }



        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            //tbError.Text = string.Empty;

           
            try
            {
                DeviceInformationCollection devices = await bt_connect.FindPairedDevicesAsync();

                var my_device = devices.Single(x => x.Name == "HC-06");

                bt_connect.print_devices(devices);

                bt_connect.IsConnected = await bt_connect.ConnectAsync(my_device);
                if (bt_connect.IsConnected)
                    bt_connect.printStatusAppend("Connected to " + my_device.Name.ToString());
                else
                    bt_connect.printStatusAppend("NOT connected to " + my_device.Name.ToString() + " :(") ;
            }
            catch (Exception ex)
            {
                bt_connect.printError("btnConnect_Click", ex.Message);
            }
        }

        private async void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            //tbError.Text = string.Empty;

            try
            {
                await bt_connect.DisconnectAsync();
            }
            catch (Exception ex)
            {
                bt_connect.printError("btnDisconnect_Click", ex.Message);
            }
        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            await bt_connect.BTSendAsync(tbInput.Text);
        }

        private void btnJoystickPage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(JoystickApp.JoystickPage));
        }
    }
}

