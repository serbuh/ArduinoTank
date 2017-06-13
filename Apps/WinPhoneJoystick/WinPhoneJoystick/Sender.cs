using System;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.ViewManagement;

namespace WinPhoneJoystick
{
	class Sender
	{
		public RfcommDeviceService Service { get; private set; }

		public StreamSocket Socket { get; private set; }

		// This App relies on CRC32 checking available in version 2.0 of the service.
		private const uint SERVICE_VERSION_ATTRIBUTE_ID = 0x0300;
		private const byte SERVICE_VERSION_ATTRIBUTE_TYPE = 0x0A;   // UINT32
		private const uint MINIMUM_SERVICE_VERSION = 200;
		// For finding right device in returned list
		private const string GUID = "";

		public async void Send(string dataStr)
		{
			var bytes = Encoding.ASCII.GetBytes(dataStr);
			await Socket.OutputStream.WriteAsync(bytes.AsBuffer());
		}

		public async void Initialize()
		{
			// Enumerate devices with the object push service
			var selector = BluetoothDevice.GetDeviceSelector();
			var devices = await DeviceInformation.FindAllAsync(selector);

			if (devices.Count > 0)
			{
				// Initialize the target Bluetooth BR device
				
				var targetService = await RfcommDeviceService.FromIdAsync(devices[0].Id);

				// Check that the service meets this App's minimum requirement
				if (SupportsProtection(targetService) && IsCompatibleVersion(targetService).Result)
				{
					Service = targetService;

					// Create a socket and connect to the target
					Socket = new StreamSocket();
					await Socket.ConnectAsync(
						Service.ConnectionHostName,
						Service.ConnectionServiceName,
						SocketProtectionLevel
							.BluetoothEncryptionAllowNullAuthentication);

					// The socket is connected. At this point the App can wait for
					// the user to take some action, e.g. click a button to send a
					// file to the device, which could invoke the Picker and then
					// send the picked file. The transfer itself would use the
					// Sockets API and not the Rfcomm API, and so is omitted here for
					// brevity.
				}
			}
		}

		// This App requires a connection that is encrypted but does not care about
		// whether its authenticated.
		private bool SupportsProtection(RfcommDeviceService service)
		{
			switch (service.ProtectionLevel)
			{
				case SocketProtectionLevel.PlainSocket:
					if ((service.MaxProtectionLevel == SocketProtectionLevel
						     .BluetoothEncryptionWithAuthentication)
					    || (service.MaxProtectionLevel == SocketProtectionLevel
						        .BluetoothEncryptionAllowNullAuthentication))
					{
						// The connection can be upgraded when opening the socket so the
						// App may offer UI here to notify the user that Windows may
						// prompt for a PIN exchange.
						return true;
					}
					else
					{
						// The connection cannot be upgraded so an App may offer UI here
						// to explain why a connection won't be made.
						return false;
					}
					case SocketProtectionLevel.BluetoothEncryptionWithAuthentication:
						return true;
					case SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication:
						return true;
			}
			return false;
		}
		
		private async Task<bool> IsCompatibleVersion(RfcommDeviceService service)
		{
			var attributes = await service.GetSdpRawAttributesAsync(
				BluetoothCacheMode.Uncached);
			var attribute = attributes[SERVICE_VERSION_ATTRIBUTE_ID];
			var reader = DataReader.FromBuffer(attribute);

			// The first byte contains the attribute' s type
			byte attributeType = reader.ReadByte();
			if (attributeType == SERVICE_VERSION_ATTRIBUTE_TYPE)
			{
				// The remainder is the data
				uint version = reader.ReadUInt32();
				return version >= MINIMUM_SERVICE_VERSION;
			}
			return false;
		}
	}
}
