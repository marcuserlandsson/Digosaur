// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.Validations
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

#nullable disable
namespace Microsoft.Surface.Core
{
  internal static class Validations
  {
    public static void CheckFinite(float value, string property, string paramName)
    {
      if (float.IsNaN(value) || float.IsInfinity(value))
        throw SurfaceCoreExceptions.FiniteExpected(property, paramName, value);
    }

    public static void CheckFiniteOrNaN(float value, string property, string paramName)
    {
      if (float.IsInfinity(value))
        throw SurfaceCoreExceptions.FiniteOrNaNExpected(property, paramName, value);
    }

    public static void CheckFinitePositive(float value, string property, string paramName)
    {
      if ((double) value <= 0.0 || float.IsInfinity(value) || float.IsNaN(value))
        throw SurfaceCoreExceptions.FinitePositiveExpected(property, paramName, value);
    }

    public static void CheckFiniteNonNegativeOrNaN(float value, string property, string paramName)
    {
      if ((double) value < 0.0 || float.IsInfinity(value))
        throw SurfaceCoreExceptions.FiniteNonNegativeOrNaNExpected(property, paramName, value);
    }

    public static void CheckFiniteNonNegative(float value, string property, string paramName)
    {
      if ((double) value < 0.0 || float.IsInfinity(value) || float.IsNaN(value))
        throw SurfaceCoreExceptions.FiniteNonNegativeExpected(property, paramName, value);
    }
  }
}
