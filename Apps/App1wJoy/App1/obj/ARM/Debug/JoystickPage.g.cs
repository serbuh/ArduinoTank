﻿#pragma checksum "D:\Arduino\ArduinoTank\Apps\App1wJoy\App1\JoystickPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6D81FE69F5064418C0DBC09B01C179A4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JoystickApp
{
    partial class JoystickPage : 
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
                    this.Joystick = (global::JoystickUserControl.Joystick)(target);
                    #line 12 "..\..\..\JoystickPage.xaml"
                    ((global::JoystickUserControl.Joystick)this.Joystick).Loaded += this.Joystick_Loaded;
                    #line 15 "..\..\..\JoystickPage.xaml"
                    ((global::JoystickUserControl.Joystick)this.Joystick).OnJoystickPressed += this.Joystick_OnJoystickPressed;
                    #line 15 "..\..\..\JoystickPage.xaml"
                    ((global::JoystickUserControl.Joystick)this.Joystick).OnJoystickReleased += this.Joystick_OnJoystickReleased;
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
