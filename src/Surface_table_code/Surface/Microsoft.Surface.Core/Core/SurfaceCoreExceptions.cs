// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.SurfaceCoreExceptions
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.Surface.Core.Properties;
using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace Microsoft.Surface.Core
{
  internal static class SurfaceCoreExceptions
  {
    internal static System.ArgumentNullException ArgumentNullException(string argumentName)
    {
      return new System.ArgumentNullException(argumentName);
    }

    internal static System.ArgumentOutOfRangeException ArgumentOutOfRangeException(
      string argumentName)
    {
      return new System.ArgumentOutOfRangeException(argumentName);
    }

    internal static InvalidOperationException WindowsApiFailedException(
      string apiName,
      int errorCode)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat(Resources.WindowsApiFailureException, (object) apiName, (object) errorCode);
      return new InvalidOperationException(stringBuilder.ToString());
    }

    internal static Exception FiniteExpected(string property, string paramName, float value)
    {
      return (Exception) new System.ArgumentOutOfRangeException(paramName, (object) value, string.Format((IFormatProvider) CultureInfo.InvariantCulture, Resources.FiniteExpected, new object[1]
      {
        (object) property
      }));
    }

    internal static Exception FinitePositiveExpected(
      string property,
      string paramName,
      float value)
    {
      return (Exception) new System.ArgumentOutOfRangeException(paramName, (object) value, string.Format((IFormatProvider) CultureInfo.InvariantCulture, Resources.FinitePositiveExpected, new object[1]
      {
        (object) property
      }));
    }

    internal static Exception FiniteNonNegativeOrNaNExpected(
      string property,
      string paramName,
      float value)
    {
      return (Exception) new System.ArgumentOutOfRangeException(paramName, (object) value, string.Format((IFormatProvider) CultureInfo.InvariantCulture, Resources.FiniteNonNegativeOrNaNExpected, new object[1]
      {
        (object) property
      }));
    }

    internal static Exception FiniteNonNegativeExpected(
      string property,
      string paramName,
      float value)
    {
      return (Exception) new System.ArgumentOutOfRangeException(paramName, (object) value, string.Format((IFormatProvider) CultureInfo.InvariantCulture, Resources.FiniteNonNegativeExpected, new object[1]
      {
        (object) property
      }));
    }

    internal static Exception FiniteOrNaNExpected(string property, string paramName, float value)
    {
      return (Exception) new System.ArgumentOutOfRangeException(paramName, (object) value, string.Format((IFormatProvider) CultureInfo.InvariantCulture, Resources.FiniteOrNaNExpected, new object[1]
      {
        (object) property
      }));
    }

    internal static Exception ReadOnlyDictionaryOperationIsNotSupported()
    {
      return (Exception) new NotSupportedException(Resources.ReadOnlyDictionary_OperationIsNotSupported);
    }

    internal static ArgumentException HwndNotOwnedByCurrentProcessException()
    {
      return new ArgumentException(Resources.HwndNotOwnedByCurrentProcess);
    }

    internal static InvalidOperationException UnableToInstallWndProcHookException()
    {
      return new InvalidOperationException(Resources.SetWindowsHookExFailed);
    }

    internal static InvalidOperationException FrameEventsAlreadyDisabled()
    {
      return new InvalidOperationException(Resources.FrameEventsAlreadyDisabled);
    }

    internal static ObjectDisposedException TouchTargetDisposedException()
    {
      return new ObjectDisposedException(Resources.TouchTargetDisposedException);
    }

    internal static InvalidOperationException CannotSynchronizeEventDispatch()
    {
      return new InvalidOperationException(Resources.CannotSynchronizeEventDispatchException);
    }

    internal static ArgumentException InvalidImageTypeException()
    {
      return new ArgumentException(Resources.InvalidImageTypeException);
    }

    internal static InvalidOperationException InvalidBufferLengthForUpdateRawImageException()
    {
      return new InvalidOperationException(Resources.InvalidBufferSizeForUpdateRawImageException);
    }

    internal static InvalidOperationException ImageTypeNotEnabledException()
    {
      return new InvalidOperationException(Resources.ImageTypeNotEnabledException);
    }
  }
}
