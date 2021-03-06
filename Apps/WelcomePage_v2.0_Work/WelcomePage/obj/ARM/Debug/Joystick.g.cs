﻿#pragma checksum "D:\Arduino\ArduinoTank\Apps\WelcomePage\WelcomePage\Joystick.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A6580AB039D5FB68FC063B66F4D71B15"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WelcomePage
{
    partial class Joystick : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.myJoystick = (global::JoystickUserControl.Joystick)(target);
                    #line 15 "..\..\..\Joystick.xaml"
                    ((global::JoystickUserControl.Joystick)this.myJoystick).Loaded += this.Joystick_Loaded;
                    #line 18 "..\..\..\Joystick.xaml"
                    ((global::JoystickUserControl.Joystick)this.myJoystick).OnJoystickPressed += this.Joystick_OnJoystickPressed;
                    #line 18 "..\..\..\Joystick.xaml"
                    ((global::JoystickUserControl.Joystick)this.myJoystick).OnJoystickReleased += this.Joystick_OnJoystickReleased;
                    #line default
                }
                break;
            case 2:
                {
                    this.Time = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 3:
                {
                    global::Windows.UI.Xaml.Controls.Button element3 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 28 "..\..\..\Joystick.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element3).Click += this.StopButton_Click;
                    #line default
                }
                break;
            case 4:
                {
                    global::Windows.UI.Xaml.Controls.Button element4 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 29 "..\..\..\Joystick.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element4).Click += this.ConnectButton_Click;
                    #line default
                }
                break;
            case 5:
                {
                    global::Windows.UI.Xaml.Controls.Button element5 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 30 "..\..\..\Joystick.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element5).Click += this.DisconnectButton_Click;
                    #line default
                }
                break;
            case 6:
                {
                    global::Windows.UI.Xaml.Controls.Button element6 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 31 "..\..\..\Joystick.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element6).Click += this.ClearButton_Click;
                    #line default
                }
                break;
            case 7:
                {
                    this.BridgeUp = (global::Windows.UI.Xaml.Controls.Button)(target);
                }
                break;
            case 8:
                {
                    this.BridgeDown = (global::Windows.UI.Xaml.Controls.Button)(target);
                }
                break;
            case 9:
                {
                    this.tbStatus = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

