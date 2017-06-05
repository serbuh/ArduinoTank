using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WinPhoneJoystick
{
    /// <summary>
    /// Main joystick page
    /// </summary>
    public sealed partial class MainPage : Page
    {
	    private bool isJoystickPressed;
	    private bool isBridgePressed;
	    private StreamSocket socket;

	    private RfcommDeviceServicesResult service;
		public MainPage()
        {
            this.InitializeComponent();
	        Joystick.OnJoystickReleased += OnRelease;
	        Joystick.OnJoystickPressed += OnPress;
		}

	    private async Task<uint> Send(string msg)
	    {
		    try
		    {
			    var writer = new DataWriter(socket.OutputStream);

			    writer.WriteString(msg);

			    // Launch an async task to 
			    //complete the write operation
			    var store = writer.StoreAsync().AsTask();

			    return await store;
		    }
		    catch (Exception ex)
		    {
				Debug.WriteLine(ex.StackTrace);

				return 0;
		    }
	    }

		private void OnRelease(object sender, EventArgs e)
	    {
		    isJoystickPressed = false;
	    }

	    private void OnPress(object sender, EventArgs e)
	    {
		    isJoystickPressed = true;
	    }

		private async void Joystick_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				var selector = BluetoothDevice.GetDeviceSelector();
				var avdevices = await DeviceInformation.FindAllAsync(selector);

				var device = avdevices.Single(x => x.Name == "Lenovo A859");
				var bluetoothDevice = await BluetoothDevice.FromIdAsync(device.Id);
				/// Look, you should get your arduino UUID and rewrite the line below something like this:
				/// service = await bluetoothDevice.GetRfcommServicesForIdAsync(RfcommServiceId.FromUuid(myUUID));
				/// It will return required service for socket creation. Also now it's getting an array of services, so change the type of the field
				service = await bluetoothDevice.GetRfcommServicesAsync();

				socket = new StreamSocket();

				await socket.ConnectAsync(
					service.Services[0].ConnectionHostName,
					service.Services[0].ConnectionServiceName,
					SocketProtectionLevel.
						BluetoothEncryptionAllowNullAuthentication);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.StackTrace);
				return;
			}
			var strBuilder = new StringBuilder();
			while (true)
			{
				strBuilder.Append(isBridgePressed ? "A " : "B ");
				if (isJoystickPressed)
				{
					strBuilder.Append(Joystick.XValue * 1023).Append(" ").Append(Joystick.YValue * 1023);
				}
				else
				{
					strBuilder.Append(0).Append(" ").Append(0);
				}
				await Send(strBuilder.ToString());
				strBuilder.Clear();
			}
		}

		private void bridgeBtn_Click(object sender, RoutedEventArgs e)
		{
			isBridgePressed = !isBridgePressed;
		}
	}
}
