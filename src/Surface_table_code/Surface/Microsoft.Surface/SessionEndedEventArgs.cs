// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.SessionEndedEventArgs
// Assembly: Microsoft.Surface, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 3075CDD2-EF12-4C05-803F-ABBE1D5E74B0
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.dll

using System;

#nullable disable
namespace Microsoft.Surface
{
  public sealed class SessionEndedEventArgs : EventArgs
  {
    public long SessionId { get; private set; }

    internal SessionEndedEventArgs(long sessionId) => this.SessionId = sessionId;
  }
}
