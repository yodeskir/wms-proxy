using System;
using WMSTools.Interfaces;

namespace WMSTools
{
    public class DateTimeHelper : IDateTimeHelper
    {
        public DateTime GetDateTimeNow()
        {
            return DateTime.Now;
        }
    }
}