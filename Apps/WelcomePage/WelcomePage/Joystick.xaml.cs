using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
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
    public sealed partial class Joystick : Page
    {
        private bool isJoystickPressed;
        private bool isBridgePressed;
        private BT_Connect bt_connect;


        public Joystick()
        {
            App.CurrentTime = TimeUtils.CurrentTimeMillis();//DateTime.UtcNow.Second;
            this.InitializeComponent();
            bt_connect = BT_Connect.getBT_Conn();
        }


        public double Map(double x, double in_min, double in_max, double out_min, double out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        private async void Joystick_Loaded(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var strBuilder = new StringBuilder();
            while (true) // form a string. (E.g. *S,512,512# or *U,512,512# or *S,1023,512#)
            {
                if (BridgeDown.IsPressed)
                    strBuilder.Append("*D,");
                else if (BridgeUp.IsPressed)
                    strBuilder.Append("*U,");
                else
                    strBuilder.Append("*S,");

                if (isJoystickPressed)
                {
                    double x_norm = Map(myJoystick.XValue, 1, -1, 0, 1023);
                    double y_norm = Map(myJoystick.YValue, -1, 1, 0, 1023);
                    strBuilder.Append(Math.Round(x_norm)).Append(",").Append(Math.Round(y_norm)).Append("#");
                }
                else
                {
                    strBuilder.Append("512,512#");
                }
                await bt_connect.BTSendAsync(strBuilder.ToString());
                strBuilder.Clear();
                await Task.Delay(TimeSpan.FromMilliseconds(150));
                App.CurrentTime = (TimeUtils.CurrentTimeMillis() - App.StartTime) / 1000.0;
                Time.Text = TimeUtils.CurrentTimeToShow();
            }
        }

        private void Joystick_OnJoystickPressed(object sender, JoystickUserControl.JoystickEventArgs e)
        {
            isJoystickPressed = true;
        }

        private void Joystick_OnJoystickReleased(object sender, JoystickUserControl.JoystickEventArgs e)
        {
            isJoystickPressed = false;
        }



        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            this.AddUser();
            this.Frame.Navigate(typeof(Score), null);
        }

        private async void AddUser()
        {
            TodoItem todoItem = new TodoItem { Nickname = App.CurrentNick, Time = TimeUtils.TimeToShow(App.CurrentTime) };
            await App.usersTable.InsertAsync(todoItem);
        }



        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DeviceInformationCollection devices = await bt_connect.FindPairedDevicesAsync();

                var my_device = devices.Single(x => x.Name == "HC-06");

                print_devices(devices);

                PrintStatusAppend("Connecting to " + my_device.Name);
                bt_connect.IsConnected = await bt_connect.ConnectAsync(my_device);

                if (bt_connect.IsConnected)
                    PrintStatusAppend("Connected to " + my_device.Name);
                else
                    PrintStatusAppend("NOT connected to " + my_device.Name + " :(");
            }
            catch (Exception ex)
            {
                PrintStatusAppend("Exception! " + ex.Message);
            }
        }

        private async void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await bt_connect.DisconnectAsync();
                PrintStatusAppend("Disconnected!");
            }
            catch (Exception ex)
            {
                PrintStatusAppend(ex.Message);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // For debug uncomment
            //tbStatus.Text = "";
        }

        private void PrintStatusAppend(string msg)
        {
            // For debug uncomment
            //tbStatus.Text += msg + Environment.NewLine;
        }

        private void print_devices(DeviceInformationCollection devices)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("#(Paired BT devices) = " + devices.Count);
            foreach (var _device in devices)
            {
                builder.Append(Environment.NewLine + _device.Name);
            }
            PrintStatusAppend(builder.ToString());
        }
        //TODO: Clock; Getting string from Arduino
    }
}
