using System;
using System.Text;
using UnityEngine;

public static class HelperMethods
{
    public static string StopWatchFormattedTime(float currentTime)
    {
        TimeSpan t = TimeSpan.FromSeconds(currentTime);

        var sb = new StringBuilder();

        return sb.Append(string.Format
            (
                "{0:00}:{1:00}:{2:000}",
                 t.Minutes,
                 t.Seconds,
                 Mathf.FloorToInt(t.Milliseconds) / 10f
            )).ToString();
    }
}