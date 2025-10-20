// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.RawInput.RawInputProvider
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace Microsoft.Surface.Core.RawInput
{
  internal abstract class RawInputProvider : IDisposable
  {
    protected EventWaitHandle dataReadyEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
    protected Queue<RawInputEvent> eventQueue = new Queue<RawInputEvent>();
    protected object syncRoot = new object();
    private bool frameEventEnabled;
    private bool fullScreenEventsEnabled;
    private bool disposed;

    public abstract DeviceCapabilities PrimaryDeviceCapabilities { get; }

    public abstract bool AreDeviceCapabilitiesAvailable();

    public abstract void Initialize();

    public virtual bool FrameEventEnabled
    {
      get => this.frameEventEnabled;
      set => this.frameEventEnabled = value;
    }

    public virtual bool FullScreenEventsEnabled
    {
      get => this.fullScreenEventsEnabled;
      set => this.fullScreenEventsEnabled = value;
    }

    public abstract bool TrySetImageEnabled(ImageType type, bool enabled);

    public EventWaitHandle DataReady => this.dataReadyEvent;

    public Queue<RawInputEvent> EventQueue => this.eventQueue;

    public object SyncRoot => this.syncRoot;

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing && this.dataReadyEvent != null)
        this.dataReadyEvent.Close();
      this.disposed = true;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    ~RawInputProvider() => this.Dispose(false);
  }
}
