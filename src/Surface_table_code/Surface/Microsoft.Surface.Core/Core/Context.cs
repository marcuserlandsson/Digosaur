// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.Context
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.Surface.Core.RawInput;
using System;
using System.Collections.ObjectModel;

#nullable disable
namespace Microsoft.Surface.Core
{
  internal sealed class Context : KeyedCollection<object, TouchTarget>, IDisposable
  {
    private readonly IntPtr hwnd;
    private readonly int threadId;
    private bool disposed;

    internal Context(TouchTarget target)
    {
      if (target.Hwnd != IntPtr.Zero)
        this.threadId = NativeMethods.GetWindowThreadProcessId(target.Hwnd, out int _);
      this.hwnd = target.Hwnd;
      this.Add(target);
    }

    public void Dispose()
    {
      if (this.disposed)
        return;
      this.disposed = true;
      TouchTarget[] array = new TouchTarget[this.Count];
      this.CopyTo(array, 0);
      this.Clear();
      foreach (TouchTarget touchTarget in array)
        touchTarget.Dispose();
    }

    public IntPtr Hwnd => this.hwnd;

    public int ThreadId => this.threadId;

    public bool IsFullScreen => this.hwnd == IntPtr.Zero;

    protected override object GetKeyForItem(TouchTarget item) => (object) item;

    public void HandleInputEvent(RawInputEvent inputEvent)
    {
      switch (inputEvent.EventId)
      {
        case SurfaceEventId.TouchDown:
          this.HandleTouchDown(inputEvent);
          break;
        case SurfaceEventId.TouchUp:
          this.HandleTouchUp(inputEvent);
          break;
        case SurfaceEventId.TouchMove:
          this.HandleTouchMove(inputEvent);
          break;
        case SurfaceEventId.FrameReceived:
          this.HandleFrameReceived(inputEvent);
          break;
        case SurfaceEventId.SystemGestureCompleted:
          this.HandleSystemGestureCompleted(inputEvent);
          break;
        case SurfaceEventId.SystemStateChanged:
          this.HandleSystemStateChanged(inputEvent);
          break;
      }
    }

    private TouchPoint ConstructTouchPoint(TouchProperties properties)
    {
      if (this.IsFullScreen && properties.HwndTarget != IntPtr.Zero)
      {
        properties = properties.Clone();
        properties.Bounds = new TouchBounds(Context.ClientToScreenCoordinates(new PointF(properties.Bounds.Left, properties.Bounds.Top), properties.HwndTarget), new SizeF(properties.Bounds.Width, properties.Bounds.Height));
        properties.CenterPosition = Context.ClientToScreenCoordinates(properties.CenterPosition, properties.HwndTarget);
        properties.Position = Context.ClientToScreenCoordinates(properties.Position, properties.HwndTarget);
        properties.HwndTarget = IntPtr.Zero;
      }
      return new TouchPoint(properties);
    }

    private static PointF ClientToScreenCoordinates(PointF clientCoordinates, IntPtr hwnd)
    {
      float x = clientCoordinates.X;
      float y = clientCoordinates.Y;
      try
      {
        NativeMethods.ClientToScreen(hwnd, ref x, ref y);
      }
      catch (InvalidOperationException ex)
      {
        return clientCoordinates;
      }
      return new PointF(x, y);
    }

    private void HandleTouchDown(RawInputEvent inputEvent)
    {
      this.IterateTouchTargets<TouchPoint>(new Context.EventDispatchCallback<TouchPoint>(this.DispatchTouchDown), this.ConstructTouchPoint((TouchProperties) inputEvent.Properties));
    }

    private void HandleTouchMove(RawInputEvent inputEvent)
    {
      this.IterateTouchTargets<TouchPoint>(new Context.EventDispatchCallback<TouchPoint>(this.DispatchTouchMove), this.ConstructTouchPoint((TouchProperties) inputEvent.Properties));
    }

    private void HandleTouchUp(RawInputEvent inputEvent)
    {
      this.IterateTouchTargets<TouchPoint>(new Context.EventDispatchCallback<TouchPoint>(this.DispatchTouchUp), this.ConstructTouchPoint((TouchProperties) inputEvent.Properties));
    }

    private void HandleFrameReceived(RawInputEvent inputEvent)
    {
      this.IterateTouchTargets<FrameProperties>(new Context.EventDispatchCallback<FrameProperties>(this.DispatchFrameReceived), (FrameProperties) inputEvent.Properties);
    }

    private void HandleSystemGestureCompleted(RawInputEvent inputEvent)
    {
      this.IterateTouchTargets<SystemGestureProperties>(new Context.EventDispatchCallback<SystemGestureProperties>(this.DispatchSystemGestureCompleted), (SystemGestureProperties) inputEvent.Properties);
    }

    private void HandleSystemStateChanged(RawInputEvent inputEvent)
    {
      this.IterateTouchTargets<SystemStateProperties>(new Context.EventDispatchCallback<SystemStateProperties>(this.DispatchSystemStateChanged), (SystemStateProperties) inputEvent.Properties);
    }

    private void IterateTouchTargets<T>(Context.EventDispatchCallback<T> dispatchProcedure, T data)
    {
      foreach (TouchTarget target in (Collection<TouchTarget>) this)
        dispatchProcedure(target, data);
    }

    private void DispatchTouchDown(TouchTarget target, TouchPoint touchPoint)
    {
      target.OnTouchDown(touchPoint);
    }

    private void DispatchTouchMove(TouchTarget target, TouchPoint touchPoint)
    {
      target.OnTouchMove(touchPoint);
    }

    private void DispatchTouchUp(TouchTarget target, TouchPoint touchPoint)
    {
      target.OnTouchUp(touchPoint);
    }

    private void DispatchFrameReceived(TouchTarget target, FrameProperties frameProperties)
    {
      target.OnFrameReceived(frameProperties);
    }

    private void DispatchSystemGestureCompleted(
      TouchTarget target,
      SystemGestureProperties systemGestureProperties)
    {
      target.OnSystemGestureCompleted(systemGestureProperties);
    }

    private void DispatchSystemStateChanged(
      TouchTarget target,
      SystemStateProperties systemStateProperties)
    {
      target.OnSystemStateChanged(systemStateProperties);
    }

    private delegate void EventDispatchCallback<T>(TouchTarget target, T data);
  }
}
