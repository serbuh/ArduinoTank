﻿#pragma checksum "C:\Users\ykuznetc\Documents\YULIA\Technion Studies\Spring 2017\Arduino Project\workspace\ArduinoTank\WelcomePage\WelcomePage\Joystick.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "148910305A37BFB59C544A4BD23785BD"
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
                    global::Windows.UI.Xaml.Controls.Button element2 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 20 "..\..\..\Joystick.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element2).Click += this.BridgeButton_Click;
                    #line default
                }
                break;
            case 3:
                {
                    this.Time = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 4:
                {
                    global::Windows.UI.Xaml.Controls.Button element4 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 30 "..\..\..\Joystick.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element4).Click += this.StopButton_Click;
                    #line default
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

