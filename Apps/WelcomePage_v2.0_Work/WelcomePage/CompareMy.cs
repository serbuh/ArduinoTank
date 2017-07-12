using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WelcomePage
{
    public class CompareMy : Comparer<TodoItem>
    {
        public override int Compare(TodoItem i1, TodoItem i2)
        {
            if (TimeUtils.TimeToDouble(i1.Time) > TimeUtils.TimeToDouble(i2.Time))
            {
                return 1;
            }
            else
            {
                if (TimeUtils.TimeToDouble(i1.Time) == TimeUtils.TimeToDouble(i2.Time))
                {
                    if (String.Compare(i1.Nickname, i2.Nickname) < 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            }
        }
    }
}
