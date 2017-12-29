using System;
using System.Collections.Generic;
using System.Text;

namespace WebBoardAuth.Common.Utils
{
    public class DateTimeUtils
    {
        // Get this datetime as a Unix epoch timestamp (seconds since Jan 1, 1970, midnight UTC).
        public static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
