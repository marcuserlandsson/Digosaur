// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.SettingsProvider`1
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using System;

#nullable disable
namespace Microsoft.Surface
{
  internal class SettingsProvider<TResult>
  {
    private Action<EventHandler> attach;
    private Action<EventHandler> detach;
    private Func<TResult> accessor;

    public SettingsProvider(
      Action<EventHandler> attach,
      Action<EventHandler> detach,
      Func<TResult> accessor)
    {
      this.attach = attach;
      this.detach = detach;
      this.accessor = accessor;
    }

    public event EventHandler Changed
    {
      add => this.attach(value);
      remove => this.detach(value);
    }

    public TResult Value => this.accessor();
  }
}
