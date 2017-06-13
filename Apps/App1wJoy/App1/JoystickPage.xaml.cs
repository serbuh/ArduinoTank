using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace JoystickApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class JoystickPage : Page
    {
        private bool isJoystickPressed;
        private bool isBridgePressed;
        private BT_Connect bt_connect;

        public JoystickPage()
        {
            bt_connect = BT_Connect.getBT_Conn();
            this.InitializeComponent();
        }

        public double Map(double x, double in_min, double in_max, double out_min, double out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        private async void Joystick_Loaded(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var strBuilder = new StringBuilder();
            while (true)
            {
                strBuilder.Append("*").Append(isBridgePressed ? "U," : "D,");
                if (isJoystickPressed)
                {
                    double x_norm = Map(Joystick.XValue, 1, -1, 0, 1023);
                    double y_norm = Map(Joystick.YValue, -1, 1, 0, 1023);
                    strBuilder.Append(Math.Round(x_norm)).Append(",").Append(Math.Round(y_norm)).Append("#");
                }
                else
                {
                    strBuilder.Append("512,512#");
                }
                await bt_connect.BTSendAsync(strBuilder.ToString());
                strBuilder.Clear();
                await Task.Delay(TimeSpan.FromMilliseconds(150));
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
    }
}
