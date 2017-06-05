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
using JoystickUserControl;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private RfcommDeviceService _service;
        private StreamSocket _socket;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            tbError.Text = string.Empty;

            try
            {
                var devices =
                      await DeviceInformation.FindAllAsync(
                        RfcommDeviceService.GetDeviceSelector(
                          RfcommServiceId.SerialPort));

                var device = devices.Single(x => x.Name == "HC-06");

                _service = await RfcommDeviceService.FromIdAsync(
                                                        device.Id);

                _socket = new StreamSocket();

                await _socket.ConnectAsync(
                      _service.ConnectionHostName,
                      _service.ConnectionServiceName,
                      SocketProtectionLevel.
                      BluetoothEncryptionAllowNullAuthentication);
            }
            catch (Exception ex)
            {
                tbError.Text = ex.Message;
            }
        }

        private async void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            tbError.Text = string.Empty;

            try
            {
                await _socket.CancelIOAsync();
                _socket.Dispose();
                _socket = null;
                _service.Dispose();
                _service = null;
            }
            catch (Exception ex)
            {
                tbError.Text = ex.Message;
            }
        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            int dummy;

            if (!int.TryParse(tbInput.Text, out dummy))
            {
                tbError.Text = "Invalid input";
            }

            var noOfCharsSent = await Send(tbInput.Text);

            if (noOfCharsSent != 0)
            {
                tbError.Text = noOfCharsSent.ToString();
            }

        }

        private async Task<uint> Send(string msg)
        {
            tbError.Text = string.Empty;

            try
            {
                var writer = new DataWriter(_socket.OutputStream);

                writer.WriteString(msg);

                // Launch an async task to 
                //complete the write operation
                var store = writer.StoreAsync().AsTask();

                return await store;
            }
            catch (Exception ex)
            {
                tbError.Text = ex.Message;

                return 0;
            }
        }

        private void tbInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            return;
        }
    }
    public sealed partial class Joystick : UserControl
    {
        /// <summary>  
        ///  Gets the current x-axis value of joystick between -1 to 1.  
        /// </summary>  
        public double XValue
        {
            get { return _joystickX / _controlRadius; }
            private set
            { _joystickX = value; }
        }
        private double _joystickX;

        /// <summary>  
        ///  Gets the current y-axis value of joystick between -1 to 1.  
        /// </summary>  
        public double YValue
        {
            get
            { return -_joystickY / _controlRadius; }
            private set
            { _joystickY = value; }
        }
        private double _joystickY;

        private bool _controllerPressed;
        private double _controlRadius;
        private ResourceDictionary _resources;


        // Generated automatically:
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", " 14.0.0.0")]
        private global::Windows.UI.Xaml.Controls.UserControl JoystickControl;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", " 14.0.0.0")]
        private global::Windows.UI.Xaml.Shapes.Ellipse ControlArea;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", " 14.0.0.0")]
        private global::Windows.UI.Xaml.Shapes.Ellipse Controller;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", " 14.0.0.0")]
        private global::Windows.UI.Xaml.Media.TranslateTransform JoystickTransform;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", " 14.0.0.0")]
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", " 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]



        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;

            global::System.Uri resourceLocator = new global::System.Uri("ms-appx:///Joystick.xaml");
            global::Windows.UI.Xaml.Application.LoadComponent(this, resourceLocator, global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
        }
        public Joystick()
        {
            InitializeComponent();
            _resources = JoystickControl.Resources;
            Loaded += OnLoaded;
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ConfigureEvents();
            SetTheme();
            SetDefaults();
        }

        void ConfigureEvents()
        {
            Window.Current.Content.PointerMoved += OnPointerMoved;
            Window.Current.Content.PointerReleased += OnPointerReleased;
        }

        void SetDefaults()
        {
            InnerDiameter = InnerDiameter == 0 ? 60 : InnerDiameter;
            OuterDiameter = OuterDiameter == 0 ? 100 : OuterDiameter;
            InnerStrokeThickness = (InnerStrokeThickness == 0) ? 1 : InnerStrokeThickness;
            OuterStrokeThickness = (OuterStrokeThickness == 0) ? 1 : OuterStrokeThickness;
        }

        void SetTheme()
        {
            switch (Theme)
            {
                case JoystickTheme.AccentTheme:
                    var x = Application.Current.Resources["SystemControlHighlightAccentBrush"] as SolidColorBrush;
                    InnerFill = InnerFill ?? x;
                    OuterStroke = OuterStroke ?? x;
                    break;

                case JoystickTheme.Dark:
                    OuterFill = OuterFill ?? (Brush)Resources["DarkA"];
                    InnerFill = (Brush)Resources["DarkB"];
                    break;
                case JoystickTheme.Light:
                    OuterFill = OuterFill ?? (Brush)Resources["LightA"];
                    InnerFill = InnerFill ?? (Brush)Resources["LightB"];
                    break;

            }
        }

        private void OnPointerMoved(object sender, PointerRoutedEventArgs eventArgs)
        {
            if (!_controllerPressed) return;

            var x = eventArgs.GetCurrentPoint(ControlArea).Position.X - _controlRadius;
            var y = eventArgs.GetCurrentPoint(ControlArea).Position.Y - _controlRadius;
            var disp = Math.Sqrt(x * x + y * y);
            if (disp < _controlRadius)
            {

                JoystickTransform.X = XValue = x;
                JoystickTransform.Y = YValue = y;

            }
            else
            {
                JoystickTransform.X = XValue = _controlRadius * (x / disp);       //A*cos(x)
                JoystickTransform.Y = YValue = _controlRadius * (y / disp);       //A*sin(x)
            }

            OnJoystickMoved?.Invoke(this, new JoystickEventArgs(XValue, YValue));

        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            _controllerPressed = false;
            JoystickTransform.X = 0;
            JoystickTransform.Y = 0;
            XValue = 0;
            YValue = 0;
            OnJoystickReleased?.Invoke(this, new JoystickEventArgs(XValue, YValue));
        }

        private void Controller_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _controlRadius = OuterDiameter / 2;
            _controllerPressed = true;
            OnJoystickPressed?.Invoke(this, new JoystickEventArgs(XValue, YValue));
        }

        /// <summary>  
        ///  Gets or sets the outer diameter of joystick.  
        /// </summary>  
        public double OuterDiameter
        {
            get
            { return (double)GetValue(OuterDiameterProperty); }
            set
            {
                SetValue(OuterDiameterProperty, value);
            }
        }
        public static readonly DependencyProperty OuterDiameterProperty =
            DependencyProperty.Register("OuterDiameter", typeof(double), typeof(Joystick), null);

        /// <summary> 
        ///  Gets or sets inner diameter of the joystick.  
        /// </summary>
        public double InnerDiameter
        {
            get
            {
                return (double)GetValue(InnerDiameterProperty);
            }
            set
            {
                SetValue(InnerDiameterProperty, value);
            }
        }
        public static readonly DependencyProperty InnerDiameterProperty =
            DependencyProperty.Register("InnerDiameter", typeof(double), typeof(Joystick), null);

        /// <summary>  
        ///  Gets or sets the color of joystick i.e color of inner circle.  
        /// </summary>
        public Brush InnerFill
        {
            get
            {
                return (Brush)GetValue(InnerFillProperty);
            }
            set
            {
                SetValue(InnerFillProperty, value);
            }
        }
        public static readonly DependencyProperty InnerFillProperty =
            DependencyProperty.Register("InnerFill", typeof(Brush), typeof(Joystick), null);

        /// <summary>  
        ///  Gets or sets the inner border color of joystick i.e border of inner circle.  
        /// </summary>
        public Brush InnerStroke
        {
            get
            {
                return (Brush)GetValue(InnerStrokeProperty);
            }
            set
            {
                SetValue(InnerStrokeProperty, value);
            }
        }
        public static readonly DependencyProperty InnerStrokeProperty =
            DependencyProperty.Register("InnerStroke", typeof(Brush), typeof(Joystick), null);


        /// <summary>  
        ///  Gets or sets the thickness of inner border of joystick.  
        /// </summary>s
        public double InnerStrokeThickness
        {
            get { return (double)GetValue(InnerStrokeThicknessProperty); }
            set { SetValue(InnerStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty InnerStrokeThicknessProperty =
            DependencyProperty.Register("InnerStrokeThickness", typeof(double), typeof(Joystick), null);


        /// <summary>  
        ///  Gets or sets a Double that specifies the distance within the dash pattern where a dash begins.
        /// </summary>s
        public double InnerStrokeDashOffset
        {
            get { return (double)GetValue(InnerStrokeDashOffsetProperty); }
            set { SetValue(InnerStrokeDashOffsetProperty, value); }
        }
        public static readonly DependencyProperty InnerStrokeDashOffsetProperty =
            DependencyProperty.Register("InnerStrokeDashOffset", typeof(double), typeof(Joystick), null);


        /// <summary>  
        ///  Gets or sets a collection of Double values that indicate the length of dashes and gaps that is used to outline inner circle.
        /// </summary>
        public DoubleCollection InnerStrokeDashArray
        {
            get { return (DoubleCollection)GetValue(InnerStrokeDashArrayProperty); }
            set { SetValue(InnerStrokeDashArrayProperty, value); }
        }
        public static readonly DependencyProperty InnerStrokeDashArrayProperty =
            DependencyProperty.Register("InnerStrokeDashArray", typeof(DoubleCollection), typeof(Joystick), null);


        /// <summary>  
        ///  It is used to describe the shape at edges of every dash in a dashed border of inner circle.  
        /// </summary>
        public PenLineCap InnerStrokeDashCap
        {
            get { return (PenLineCap)GetValue(InnerStrokeDashCapProperty); }
            set { SetValue(InnerStrokeDashCapProperty, value); }
        }
        public static readonly DependencyProperty InnerStrokeDashCapProperty =
            DependencyProperty.Register("InnerStrokeDashCap", typeof(PenLineCap), typeof(Joystick), null);



        /// <summary>  
        ///  Gets or sets the color of joystick control area.  
        /// </summary>
        public Brush OuterFill
        {
            get
            {
                return (Brush)GetValue(OuterFillProperty);
            }
            set
            {
                SetValue(OuterFillProperty, value);
            }
        }
        public static readonly DependencyProperty OuterFillProperty =
            DependencyProperty.Register("OuterFill", typeof(Brush), typeof(Joystick), null);


        /// <summary>  
        ///  Gets or sets the border color of joystick control area.  
        /// </summary>
        public Brush OuterStroke
        {
            get
            {
                return (Brush)GetValue(OuterStrokeProperty);
            }
            set
            {
                SetValue(OuterStrokeProperty, value);
            }
        }
        public static readonly DependencyProperty OuterStrokeProperty =
            DependencyProperty.Register("OuterStroke", typeof(Brush), typeof(Joystick), null);


        /// <summary>  
        ///  Gets or sets the thickness of outer border of joystick.  
        /// </summary>
        public double OuterStrokeThickness
        {
            get { return (double)GetValue(OuterStrokeThicknessProperty); }
            set { SetValue(OuterStrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty OuterStrokeThicknessProperty =
            DependencyProperty.Register("OuterStrokeThickness", typeof(double), typeof(Joystick), null);


        /// <summary>  
        ///  Gets or sets a collection of Double values that indicate the length of dashes and gaps that is used to outline outer circle.
        /// </summary>
        public DoubleCollection OuterStrokeDashArray
        {
            get { return (DoubleCollection)GetValue(OuterStrokeDashArrayProperty); }
            set { SetValue(OuterStrokeDashArrayProperty, value); }
        }
        public static readonly DependencyProperty OuterStrokeDashArrayProperty =
            DependencyProperty.Register("OuterStrokeDashArray", typeof(DoubleCollection), typeof(Joystick), null);

        /// <summary>  
        ///  Gets or sets a Double that specifies the distance within the dash pattern where a dash begins.
        /// </summary>s
        public double OuterStrokeDashOffset
        {
            get { return (double)GetValue(OuterStrokeDashOffsetProperty); }
            set { SetValue(OuterStrokeDashOffsetProperty, value); }
        }
        public static readonly DependencyProperty OuterStrokeDashOffsetProperty =
            DependencyProperty.Register("OuterStrokeDashOffset", typeof(double), typeof(Joystick), null);


        /// <summary>  
        ///  It is used to describe the shape at edges of every dash in a dashed border in outer circle.  
        /// </summary>
        public PenLineCap OuterStrokeDashCap
        {
            get { return (PenLineCap)GetValue(OuterStrokeDashCapProperty); }
            set { SetValue(OuterStrokeDashCapProperty, value); }
        }
        public static readonly DependencyProperty OuterStrokeDashCapProperty =
            DependencyProperty.Register("OuterStrokeDashCap", typeof(PenLineCap), typeof(Joystick), null);


        public enum JoystickTheme
        {
            AccentTheme,
            Dark,
            Light
        }


        public JoystickTheme Theme
        {
            get { return (JoystickTheme)GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Theme.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.Register("Theme", typeof(JoystickTheme), typeof(Joystick), new PropertyMetadata(0));


        public event EventHandler<JoystickEventArgs> OnJoystickPressed;
        public event EventHandler<JoystickEventArgs> OnJoystickMoved;
        public event EventHandler<JoystickEventArgs> OnJoystickReleased;


        

    }
    }

