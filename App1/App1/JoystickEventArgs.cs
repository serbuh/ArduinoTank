using System;

namespace JoystickUserControl
{
    public class JoystickEventArgs : EventArgs
    {
        public double XValue { get; private set; }
        public double YValue { get; private set; }

        public JoystickEventArgs(double xValue, double yValue)
        {
            XValue = xValue;
            YValue = yValue;
        }
    }
}