using System;
using Android.Content;

namespace LearnEnglish.XN.Droid.Extensions;

public static class ContextExtensions
{
    public static int ToPixels(this Context context, double dp)
    {
        using (var metrics = context.Resources.DisplayMetrics) return (int)Math.Ceiling(dp * metrics.Density);
    }
}
