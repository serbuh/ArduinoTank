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
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using System.Text;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private RfcommDeviceService _rfcommService;
        private StreamSocket _socket;
        public bool IsConnected;
        private DataReader _btReader;
        private DataWriter _btWriter;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void printError(string whoSent, string error)
        {
            tbError.Text += whoSent + ": " + error + Environment.NewLine;
        }

        private void printStatus(string msg)
        {
            tbStatus.Text += msg + Environment.NewLine;
        }

        private void printStatusAppend(string msg)
        {
            tbStatus.Text += msg + Environment.NewLine;
        }

        private async Task<DeviceInformationCollection> FindPairedDevicesAsync()
        {
            var aqsDevices = RfcommDeviceService.GetDeviceSelector(RfcommServiceId.SerialPort);
            DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(aqsDevices);
            return devices;
        }

        private async Task<bool> ConnectAsync(DeviceInformation device)
        {
            _rfcommService = await RfcommDeviceService.FromIdAsync(device.Id);
            if (_rfcommService == null) tbError.Text += "_rfcommService is NULL :(";
            this._socket = new StreamSocket();
            try
            {
                await _socket.ConnectAsync(
                    _rfcommService.ConnectionHostName,
                    _rfcommService.ConnectionServiceName,
                    SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);
            }
            catch(Exception ex)
            {
                printError("ConnectAsync", ex.Message);
            }

            _btReader = new DataReader(_socket.InputStream);
            _btWriter = new DataWriter(_socket.OutputStream);
            _btWriter.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
            return true;
        }

        private void print_devices(DeviceInformationCollection devices)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Count: " + devices.Count + Environment.NewLine);
            foreach (var _device in devices)
            {
                builder.Append(_device.Name + Environment.NewLine);
            }
            printStatusAppend(builder.ToString());
        }


        public async Task<uint> WriteAsync(string msg)
        {
            if (!IsConnected) return 0;
            //tbError.Text = string.Empty;

            try
            {
                var n = _btWriter.WriteString(msg);
                // Launch an async task to complete the write operation
                await _btWriter.StoreAsync();
                return n;
            }
            catch (Exception ex)
            {
                printError("WriteAsync", ex.Message);
                return 0;
            }
        }


        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            //tbError.Text = string.Empty;

            try
            {
                DeviceInformationCollection devices = await FindPairedDevicesAsync();

                var my_device = devices.Single(x => x.Name == "HC-06");

                print_devices(devices);

                IsConnected = await ConnectAsync(my_device);
                if (IsConnected)
                    printStatusAppend("Connected to " + my_device.Name.ToString());
                else
                    printStatusAppend("NOT connected to " + my_device.Name.ToString() + " :(") ;
            }
            catch (Exception ex)
            {
                printError("btnConnect_Click", ex.Message);
            }
        }

        private async void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            //tbError.Text = string.Empty;

            try
            {
                await _socket.CancelIOAsync();
                _socket.Dispose();
                _socket = null;
                _rfcommService.Dispose();
                _rfcommService = null;
            }
            catch (Exception ex)
            {
                printError("btnDisconnect_Click", ex.Message);
            }
        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {

            int dummy;
            /*
            if (!int.TryParse(tbInput.Text, out dummy))
            {
                printError("btnSend_Click", "Invalid input");
            }
            */
            var noOfCharsSent = await WriteAsync(tbInput.Text);

            if (noOfCharsSent != 0)
            {
                printStatus("Sent " + noOfCharsSent.ToString() + " chars");
            }

        }
    }
}

