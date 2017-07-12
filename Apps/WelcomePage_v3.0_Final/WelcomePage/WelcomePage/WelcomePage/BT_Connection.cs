using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace WelcomePage
{
    class BT_Connect
    {
        private RfcommDeviceService _rfcommService;
        private StreamSocket _socket;
        public bool IsConnected;
        private DataReader _btReader;
        private DataWriter _btWriter;

        private static bool isInitialized = false;
        private static BT_Connect self;

        //private BT_Connect(TextBlock tbStatus, TextBlock tbError)
        private BT_Connect()
        {
            /*
            _tbStatus = tbStatus;
            _tbError = tbError;
            */
            BT_Connect.self = this;
        }

        public static BT_Connect getBT_Conn()
        {
            if (!isInitialized)
            {
                new BT_Connect();
                isInitialized = true;
            }
            return self;
        }

        public async Task<DeviceInformationCollection> FindPairedDevicesAsync()
        {
            var aqsDevices = RfcommDeviceService.GetDeviceSelector(RfcommServiceId.SerialPort);
            DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(aqsDevices);
            return devices;
        }


        public async Task<bool> ConnectAsync(DeviceInformation device)
        {
            _rfcommService = await RfcommDeviceService.FromIdAsync(device.Id);
            if (_rfcommService == null)
            {
                throw new System.Exception("_rfcommService is NULL for the device (Name): " + device.Name + " (Consider enabling Blutooth in app manifest)");
            }
            this._socket = new StreamSocket();
            try
            {
                await _socket.ConnectAsync(
                    _rfcommService.ConnectionHostName,
                    _rfcommService.ConnectionServiceName,
                    SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            _btReader = new DataReader(_socket.InputStream);
            _btWriter = new DataWriter(_socket.OutputStream);
            _btWriter.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
            return true;
        }

        public async Task<bool> DisconnectAsync()
        {
            try
            {
                await _socket.CancelIOAsync();
                if (_btReader != null)
                {
                    _btReader.DetachStream();
                    _btReader.Dispose();
                    _btReader = null;
                }
                if (_btWriter != null)
                {
                    _btWriter.DetachStream();
                    _btWriter.Dispose();
                    _btWriter = null;
                }
                if (_socket != null)
                {
                    _socket.Dispose();
                    _socket = null;
                }
                if (_rfcommService != null)
                {
                    _rfcommService.Dispose();
                    _rfcommService = null;
                }
                IsConnected = false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<uint> BTSendAsync(string msg)
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
                throw ex;
            }
        }

        public async Task<char> BTRecieveAsync()
        {
            if (!IsConnected) return '0';
            try
            {
                await _btReader.LoadAsync(1);
                byte b = _btReader.ReadByte();
                return (char)b;
            }
            catch (Exception ex)
            {
                return '0';
                //throw ex;
            }
        }

        public void print_devices(DeviceInformationCollection devices)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Count: " + devices.Count + Environment.NewLine);
            foreach (var _device in devices)
            {
                builder.Append(_device.Name + Environment.NewLine);
            }
            printStatusAppend(builder.ToString());
        }

        public void printError(string whoSent, string error)
        {
            //_tbError.Text += whoSent + ": " + error + Environment.NewLine;
        }

        public void printStatus(string msg)
        {
            //_tbStatus.Text += msg + Environment.NewLine;
        }

        public void printStatusAppend(string msg)
        {
            //_tbStatus.Text += msg + Environment.NewLine;
        }
    }
}
