using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public static class DateTimeExtension
    {
        public static bool IsSameDay(this DateTime dt1, DateTime dt2)
        {
            return dt1.Date == dt2.Date;
        }
    }
}