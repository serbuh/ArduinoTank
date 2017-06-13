using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WelcomePage
{
    class TimeUtils
    {
        private static readonly DateTime Jan1st1970 = new DateTime
                                (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }
        public static string CurrentTimeToShow()
        {
            int minuites = Convert.ToInt32(Math.Floor(App.CurrentTime / 60));
            int seconds = Convert.ToInt32(Math.Floor(App.CurrentTime % 60));
            var strBuilder = new StringBuilder();
            if (minuites < 10)
            {
                strBuilder.Append("0");
            }
            strBuilder.Append(minuites.ToString()).Append(":");
            if (seconds < 10)
            {
                strBuilder.Append("0");
            }
            strBuilder.Append(seconds.ToString());
            string timeToShow = strBuilder.ToString();
            return timeToShow;
        }
    }
}
