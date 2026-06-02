using System;
using UnityEngine;

public class DateTimeUtil
{
    public static float GetNistTimeInMilliseconds()
    {
        // Return Utc milliseconds since Unix Epoch to keep consistency with previous saves
        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return (float)(DateTime.UtcNow - epoch).TotalMilliseconds;
    }

    public static DateTime NistTimeToDatetime(float nistTimeInMilliseconds)
    {
        if (nistTimeInMilliseconds > 0)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(nistTimeInMilliseconds).ToLocalTime();
        }
        return DateTime.Now;
    }

    public static DateTime GetCurrentTime()
    {
        return DateTime.Now;
    }
}
