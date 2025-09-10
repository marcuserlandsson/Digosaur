// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.TouchTarget
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.Surface.Core.Properties;
using Microsoft.Surface.Core.RawInput;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;

#nullable disable
namespace Microsoft.Surface.Core
{
  public class TouchTarget : IDisposable
  {
    private const int VipRawImageWidth = 960;
    private const int VipRawImageHeight = 540;
    private IntPtr hwnd;
    private bool inputEnabled;
    private bool disposed;
    private SynchronizationContext dispatchContext;
    private TouchPointCollection currentTouchPoints = new TouchPointCollection();
    internal EventHandler<FrameReceivedEventArgs> frameReceived;
    private readonly object frameEventLock = new object();
    private bool[] imageTypeEnabled = new bool[Enum.GetValues(typeof (ImageType)).Length];
    private object imageTypeLock = new object();

    public TouchTarget(IntPtr hwnd)
      : this(hwnd, EventThreadChoice.OnCurrentThread)
    {
    }

    public TouchTarget(IntPtr hwnd, bool immediatelyEnableInput)
      : this(hwnd, EventThreadChoice.OnCurrentThread, immediatelyEnableInput)
    {
    }

    public TouchTarget(IntPtr hwnd, EventThreadChoice threadSelection)
      : this(hwnd, threadSelection, false)
    {
    }

    public TouchTarget(IntPtr hwnd, EventThreadChoice threadSelection, bool immediatelyEnableInput)
    {
      this.Initialize(hwnd, threadSelection);
      if (!immediatelyEnableInput)
        return;
      this.EnableInput();
    }

    private void Initialize(IntPtr hwndTarget, EventThreadChoice threadSelection)
    {
      EventsCore.PerfTouchTargetInitializeStart();
      EventsCore.TraceTouchTargetCreated(RuntimeHelpers.GetHashCode((object) this), hwndTarget.ToInt32());
      if (hwndTarget != IntPtr.Zero)
      {
        int lpdwProcessId;
        NativeMethods.GetWindowThreadProcessId(hwndTarget, out lpdwProcessId);
        if (Process.GetCurrentProcess().Id != lpdwProcessId)
          throw SurfaceCoreExceptions.HwndNotOwnedByCurrentProcessException();
      }
      this.hwnd = hwndTarget;
      if (threadSelection == EventThreadChoice.OnCurrentThread)
      {
        this.dispatchContext = SynchronizationContext.Current;
        if (this.dispatchContext == null)
          throw SurfaceCoreExceptions.CannotSynchronizeEventDispatch();
      }
      NativeMethods.EnableTabletGestures(hwndTarget, false);
      ContextMap.Instance.AddTouchTarget(this);
      if (CorePerformanceCounters.Instance.IsEnabled)
        TouchTarget.UpdatePerformanceCounters(0, 0L);
      EventsCore.PerfTouchTargetInitializeFinish();
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      EventsCore.TraceTouchTargetDisposed(this.GetHashCode(), this.hwnd.ToInt32());
      if (disposing)
      {
        this.DisableAllImages();
        lock (this.frameEventLock)
        {
          if (this.frameReceived != null)
            TouchTarget.DisableFrameEvents();
        }
        ContextMap.Instance.RemoveTouchTarget(this);
        this.hwnd = IntPtr.Zero;
      }
      this.disposed = true;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public void EnableInput()
    {
      this.EnsureNotDisposed();
      this.inputEnabled = true;
    }

    public bool EnableImage(ImageType type)
    {
      bool flag = false;
      this.EnsureNotDisposed();
      ImageTypeValidation.ValidateImageType(type);
      lock (this.imageTypeLock)
      {
        if (this.imageTypeEnabled[(int) type])
          return true;
        flag = ContextMap.Instance.SetImageTypeEnabled(type, true);
        this.imageTypeEnabled[(int) type] = true;
      }
      return flag;
    }

    public void DisableImage(ImageType type)
    {
      this.EnsureNotDisposed();
      ImageTypeValidation.ValidateImageType(type);
      lock (this.imageTypeLock)
      {
        if (!this.imageTypeEnabled[(int) type])
          return;
        ContextMap.Instance.SetImageTypeEnabled(type, false);
        this.imageTypeEnabled[(int) type] = false;
      }
    }

    internal ImageMetrics GetRawImageMetrics(ImageType type)
    {
      this.EnsureNotDisposed();
      ImageTypeValidation.ValidateImageType(type);
      int num = 960;
      int height = 540;
      if (type == ImageType.Normalized)
        return new ImageMetrics(8, 32.0, 32.0, num, num, height);
      throw SurfaceCoreExceptions.InvalidImageTypeException();
    }

    private void DisableAllImages()
    {
      lock (this.imageTypeLock)
      {
        for (int type = 0; type < this.imageTypeEnabled.Length; ++type)
        {
          if (this.imageTypeEnabled[type])
            this.DisableImage((ImageType) type);
        }
      }
    }

    internal bool IsImageTypeEnabled(ImageType type) => this.imageTypeEnabled[(int) type];

    public IntPtr Hwnd
    {
      get
      {
        this.EnsureNotDisposed();
        return this.HwndCore;
      }
    }

    internal IntPtr HwndCore => this.hwnd;

    internal bool Enabled => this.inputEnabled;

    public ReadOnlyTouchPointCollection GetState()
    {
      lock (this.currentTouchPoints)
        return new ReadOnlyTouchPointCollection(this.currentTouchPoints.Clone());
    }

    public ReadOnlyTouchPointCollection GetState(IEnumerable<int> filter)
    {
      TouchPointCollection touchPointCollection = new TouchPointCollection();
      if (filter != null)
      {
        lock (this.currentTouchPoints)
        {
          foreach (int key in filter)
          {
            if (this.currentTouchPoints.Contains(key))
              touchPointCollection.Add(this.currentTouchPoints[key].Clone());
          }
        }
      }
      return new ReadOnlyTouchPointCollection(touchPointCollection);
    }

    private int AddTouchPointToCollection(TouchPoint touchPoint)
    {
      lock (this.currentTouchPoints)
      {
        this.currentTouchPoints.Add(touchPoint);
        return this.currentTouchPoints.Count;
      }
    }

    private int UpdateTouchPointInCollection(TouchPoint touchPoint)
    {
      lock (this.currentTouchPoints)
      {
        if (!this.currentTouchPoints.Contains(touchPoint.Id))
          return this.AddTouchPointToCollection(touchPoint);
        this.currentTouchPoints[touchPoint.Id].SetPropertiesFromTouchPoint(touchPoint);
        return this.currentTouchPoints.Count;
      }
    }

    private int RemoveTouchPointFromCollection(TouchPoint touchPoint)
    {
      lock (this.currentTouchPoints)
      {
        this.currentTouchPoints.Remove(touchPoint.Id);
        return this.currentTouchPoints.Count;
      }
    }

    public event EventHandler<TouchEventArgs> TouchDown;

    public event EventHandler<TouchEventArgs> TouchUp;

    public event EventHandler<TouchEventArgs> TouchMove;

    public event EventHandler<TouchEventArgs> TouchTapGesture;

    public event EventHandler<TouchEventArgs> TouchHoldGesture;

    public event EventHandler<FrameReceivedEventArgs> FrameReceived
    {
      add
      {
        if (value == null)
          return;
        lock (this.frameEventLock)
        {
          bool flag = this.frameReceived == null;
          this.frameReceived += value;
          if (!flag)
            return;
          TouchTarget.EnableFrameEvents();
        }
      }
      remove
      {
        if (value == null)
          return;
        lock (this.frameEventLock)
        {
          if (this.frameReceived == null)
            return;
          this.frameReceived -= value;
          if (this.frameReceived != null || this.disposed)
            return;
          TouchTarget.DisableFrameEvents();
        }
      }
    }

    private static void EnableFrameEvents() => ContextMap.Instance.EnableFrameEvents();

    private static void DisableFrameEvents()
    {
      try
      {
        ContextMap.Instance.DisableFrameEvents();
      }
      catch (InvalidOperationException ex)
      {
      }
    }

    internal void OnTouchDown(TouchPoint touchPoint)
    {
      this.DispatchInputEvent(new SendOrPostCallback(this.OnTouchDownCore), (object) touchPoint);
    }

    internal void OnTouchMove(TouchPoint touchPoint)
    {
      this.DispatchInputEvent(new SendOrPostCallback(this.OnTouchMoveCore), (object) touchPoint);
    }

    internal void OnTouchUp(TouchPoint touchPoint)
    {
      this.DispatchInputEvent(new SendOrPostCallback(this.OnTouchUpCore), (object) touchPoint);
    }

    internal void OnFrameReceived(FrameProperties p)
    {
      this.DispatchInputEvent(new SendOrPostCallback(this.OnFrameReceivedCore), (object) p);
    }

    internal void OnSystemGestureCompleted(SystemGestureProperties systemGestureProperties)
    {
      this.DispatchInputEvent(new SendOrPostCallback(this.OnSystemGestureCompletedCore), (object) systemGestureProperties);
    }

    internal void OnSystemStateChanged(SystemStateProperties systemStateProperties)
    {
      if (systemStateProperties.IsConnected)
        return;
      this.RemoveAllTouchPoints();
    }

    private void RemoveAllTouchPoints()
    {
      foreach (TouchPoint touchPoint in (Collection<TouchPoint>) this.currentTouchPoints.Clone())
        this.OnTouchUp(touchPoint);
    }

    private void DispatchInputEvent(SendOrPostCallback callback, object state)
    {
      if (!this.Enabled)
        return;
      if (this.dispatchContext == null)
        callback(state);
      else
        this.dispatchContext.Post(callback, state);
    }

    private void OnTouchDownCore(object state)
    {
      TouchPoint touchPoint = (TouchPoint) state;
      int collection = this.AddTouchPointToCollection(touchPoint);
      if (CorePerformanceCounters.Instance.IsEnabled)
        TouchTarget.UpdatePerformanceCounters(collection, touchPoint.FrameTimestamp);
      EventsCore.TraceTouchDownDebug(this.GetHashCode(), touchPoint.Id, (double) touchPoint.X, (double) touchPoint.Y);
      EventsCore.PerfRaiseTouchDown(touchPoint.Id);
      EventHandler<TouchEventArgs> touchDown = this.TouchDown;
      if (touchDown == null)
        return;
      TouchEventArgs e = TouchEventArgs.Create(touchPoint);
      touchDown((object) this, e);
    }

    private void OnTouchMoveCore(object state)
    {
      TouchPoint touchPoint = (TouchPoint) state;
      int touchCount = this.UpdateTouchPointInCollection(touchPoint);
      if (CorePerformanceCounters.Instance.IsEnabled)
        TouchTarget.UpdatePerformanceCounters(touchCount, touchPoint.FrameTimestamp);
      EventsCore.TraceTouchMoveDebug(this.GetHashCode(), touchPoint.Id, (double) touchPoint.X, (double) touchPoint.Y);
      EventsCore.PerfRaiseTouchMove(touchPoint.Id);
      EventHandler<TouchEventArgs> touchMove = this.TouchMove;
      if (touchMove == null)
        return;
      TouchEventArgs e = TouchEventArgs.Create(touchPoint);
      touchMove((object) this, e);
    }

    private void OnTouchUpCore(object state)
    {
      TouchPoint touchPoint = (TouchPoint) state;
      int touchCount = this.RemoveTouchPointFromCollection(touchPoint);
      if (CorePerformanceCounters.Instance.IsEnabled)
        TouchTarget.UpdatePerformanceCounters(touchCount, touchPoint.FrameTimestamp);
      EventsCore.TraceTouchUpDebug(this.GetHashCode(), touchPoint.Id, (double) touchPoint.X, (double) touchPoint.Y);
      EventsCore.PerfRaiseTouchUp(touchPoint.Id);
      EventHandler<TouchEventArgs> touchUp = this.TouchUp;
      if (touchUp == null)
        return;
      TouchEventArgs e = TouchEventArgs.Create(touchPoint);
      touchUp((object) this, e);
    }

    private static void UpdatePerformanceCounters(int touchCount, long timestamp)
    {
      if (touchCount == 0)
        CorePerformanceCounters.Instance.CoreTouchProcessingTime.Clear();
      else
        CorePerformanceCounters.Instance.CoreTouchProcessingTime.Set(timestamp);
    }

    private void OnFrameReceivedCore(object state)
    {
      FrameProperties frameProperties = (FrameProperties) state;
      if (CorePerformanceCounters.Instance.IsEnabled)
        CorePerformanceCounters.Instance.CoreFrameEventRate.Increment();
      EventHandler<FrameReceivedEventArgs> frameReceived = this.frameReceived;
      if (frameReceived == null)
        return;
      FrameReceivedEventArgs e = new FrameReceivedEventArgs(this, frameProperties);
      frameReceived((object) this, e);
    }

    private void EnsureNotDisposed()
    {
      if (this.disposed)
        throw SurfaceCoreExceptions.TouchTargetDisposedException();
    }

    [Conditional("DEBUG")]
    private static void ValidateAssumptions()
    {
      foreach (ImageType imageType in Enum.GetValues(typeof (ImageType)))
        ;
    }

    private void OnSystemGestureCompletedCore(object state)
    {
      SystemGestureProperties gestureProperties = (SystemGestureProperties) state;
      TouchPoint touchPoint = (TouchPoint) null;
      lock (this.currentTouchPoints)
      {
        if (!this.currentTouchPoints.Contains(gestureProperties.Id))
          return;
        touchPoint = this.currentTouchPoints[gestureProperties.Id];
      }
      EventHandler<TouchEventArgs> eventHandler = (EventHandler<TouchEventArgs>) null;
      switch (gestureProperties.GestureType)
      {
        case SystemGestureType.Tap:
          eventHandler = this.TouchTapGesture;
          break;
        case SystemGestureType.PressAndHold:
          eventHandler = this.TouchHoldGesture;
          break;
      }
      if (eventHandler == null)
        return;
      TouchEventArgs e = TouchEventArgs.Create(touchPoint);
      eventHandler((object) this, e);
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.TouchTargetToStringFormat, new object[2]
      {
        (object) this.Hwnd,
        (object) this.currentTouchPoints.Count
      });
    }
  }
}
