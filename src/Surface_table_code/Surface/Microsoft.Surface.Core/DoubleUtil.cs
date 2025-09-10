// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.DoubleUtil
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using System;

#nullable disable
namespace Microsoft.Surface
{
  internal static class DoubleUtil
  {
    internal const double DBL_EPSILON = 2.2204460492503131E-16;

    public static bool AreClose(double value1, double value2)
    {
      if (value1 == value2)
        return true;
      double num1 = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * 2.2204460492503131E-16;
      double num2 = value1 - value2;
      return -num1 < num2 && num1 > num2;
    }

    public static bool LessThan(double value1, double value2)
    {
      return value1 < value2 && !DoubleUtil.AreClose(value1, value2);
    }

    public static bool GreaterThan(double value1, double value2)
    {
      return value1 > value2 && !DoubleUtil.AreClose(value1, value2);
    }

    public static bool LessThanOrClose(double value1, double value2)
    {
      return value1 < value2 || DoubleUtil.AreClose(value1, value2);
    }

    public static bool GreaterThanOrClose(double value1, double value2)
    {
      return value1 > value2 || DoubleUtil.AreClose(value1, value2);
    }

    public static bool IsDoubleFinite(double d) => !double.IsInfinity(d) && !double.IsNaN(d);

    public static bool IsDoubleFiniteNonZero(double d)
    {
      return DoubleUtil.IsDoubleFinite(d) && !DoubleUtil.IsZero(d);
    }

    public static bool IsZero(double d) => Math.Abs(d) <= 2.2204460492503131E-16;

    public static double Limit(double d, double min, double max)
    {
      if (!double.IsNaN(max) && d > max)
        return max;
      return !double.IsNaN(min) && d < min ? min : d;
    }

    public static float ConvertToFloat(double d)
    {
      float f = (float) d;
      if (!double.IsInfinity(d) && float.IsInfinity(f))
      {
        if (d > 3.4028234663852886E+38)
          f = float.MaxValue;
        else if (d < -3.4028234663852886E+38)
          f = float.MinValue;
      }
      return f;
    }
  }
}
