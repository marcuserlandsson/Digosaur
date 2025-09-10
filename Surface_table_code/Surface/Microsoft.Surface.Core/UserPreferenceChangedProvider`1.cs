// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.UserPreferenceChangedProvider`1
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.Win32;
using System;

#nullable disable
namespace Microsoft.Surface
{
  internal class UserPreferenceChangedProvider<TResult>
  {
    private Action<UserPreferenceChangedEventHandler> attach;
    private Action<UserPreferenceChangedEventHandler> detach;
    private Func<TResult> accessor;

    public UserPreferenceChangedProvider(
      Action<UserPreferenceChangedEventHandler> attach,
      Action<UserPreferenceChangedEventHandler> detach,
      Func<TResult> accessor)
    {
      this.attach = attach;
      this.detach = detach;
      this.accessor = accessor;
    }

    public event UserPreferenceChangedEventHandler Changed
    {
      add => this.attach(value);
      remove => this.detach(value);
    }

    public TResult Value => this.accessor();
  }
}
