// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.ApplicationServicesException
// Assembly: Microsoft.Surface, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 3075CDD2-EF12-4C05-803F-ABBE1D5E74B0
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Microsoft.Surface
{
  [Serializable]
  public sealed class ApplicationServicesException : Exception
  {
    internal ApplicationServicesException()
    {
    }

    internal ApplicationServicesException(string message)
      : base(message)
    {
    }

    internal ApplicationServicesException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    private ApplicationServicesException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
