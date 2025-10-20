// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.ContextMap
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.Surface.Core.RawInput;
using Microsoft.Surface.Core.RawInput.HidInput;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

#nullable disable
namespace Microsoft.Surface.Core
{
  internal sealed class ContextMap : IDisposable
  {
    private static readonly ContextMap instance = new ContextMap();
    private object mapSync = new object();
    private int frameEventEnabledCount;
    private object frameEventEnabledLock = new object();
    private Dictionary<IntPtr, Context> contextMap = new Dictionary<IntPtr, Context>();
    private Dictionary<int, ContextMap.HookRecord> threadMap = new Dictionary<int, ContextMap.HookRecord>();
    private Microsoft.Surface.NativeMethods.HookProc wndProcCallback;
    private Microsoft.Surface.NativeMethods.HookProc mouseProcCallback;
    private Thread eventThread;
    private RawInputProvider inputProvider;
    private bool inputProviderInitialized;
    private readonly EventWaitHandle surfaceApplicationEvent;

    private ContextMap()
    {
      this.wndProcCallback = new Microsoft.Surface.NativeMethods.HookProc(this.WindowHookProc);
      this.mouseProcCallback = new Microsoft.Surface.NativeMethods.HookProc(this.MouseHookProc);
      this.surfaceApplicationEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "Global\\Microsoft.Surface.Core.IsSurfaceApp:" + (object) Process.GetCurrentProcess().Id);
    }

    public static ContextMap Instance => ContextMap.instance;

    internal RawInputProvider InputProvider
    {
      get => this.inputProvider;
      set => this.inputProvider = value;
    }

    public void Dispose()
    {
      if (this.inputProvider != null)
        this.inputProvider.Dispose();
      if (this.surfaceApplicationEvent == null)
        return;
      this.surfaceApplicationEvent.Dispose();
    }

    internal void EnsureInputProvider()
    {
      if (this.inputProvider != null && this.inputProviderInitialized)
        return;
      lock (this.mapSync)
      {
        if (this.inputProvider == null)
          this.inputProvider = (RawInputProvider) new HidInputProvider();
        if (this.inputProviderInitialized)
          return;
        this.inputProvider.Initialize();
        this.inputProviderInitialized = true;
        if (this.eventThread != null)
          return;
        this.eventThread = new Thread(new ThreadStart(this.DispatchEvents));
        this.eventThread.IsBackground = true;
        this.eventThread.Start();
      }
    }

    public int Count
    {
      get
      {
        lock (this.mapSync)
          return this.contextMap.Count;
      }
    }

    public int HookedThreadCount
    {
      get
      {
        lock (this.mapSync)
          return this.threadMap.Count;
      }
    }

    public bool AreDeviceCapabilitiesAvailable()
    {
      this.EnsureInputProvider();
      return this.inputProvider.AreDeviceCapabilitiesAvailable();
    }

    public void EnableFrameEvents()
    {
      lock (this.frameEventEnabledLock)
      {
        ++this.frameEventEnabledCount;
        if (this.inputProvider == null)
          return;
        this.inputProvider.FrameEventEnabled = true;
      }
    }

    public void DisableFrameEvents()
    {
      if (this.frameEventEnabledCount == 0)
        throw SurfaceCoreExceptions.FrameEventsAlreadyDisabled();
      lock (this.frameEventEnabledLock)
      {
        --this.frameEventEnabledCount;
        if (this.frameEventEnabledCount != 0 || this.inputProvider == null)
          return;
        this.inputProvider.FrameEventEnabled = false;
      }
    }

    public bool SetImageTypeEnabled(ImageType type, bool enabled)
    {
      this.EnsureInputProvider();
      return this.inputProvider.TrySetImageEnabled(type, enabled);
    }

    public void AddTouchTarget(TouchTarget target)
    {
      lock (this.mapSync)
      {
        this.EnsureInputProvider();
        Context context;
        if (this.contextMap.TryGetValue(target.Hwnd, out context))
        {
          context.Add(target);
        }
        else
        {
          context = new Context(target);
          this.Hook(context);
          this.contextMap.Add(target.Hwnd, context);
          if (this.InputProvider == null || !context.IsFullScreen)
            return;
          this.InputProvider.FullScreenEventsEnabled = true;
        }
      }
    }

    public bool RemoveTouchTarget(TouchTarget target)
    {
      lock (this.mapSync)
      {
        Context context;
        if (this.contextMap.TryGetValue(target.HwndCore, out context))
        {
          context.Remove((object) target);
          if (context.Count == 0)
          {
            if (this.InputProvider != null && context.IsFullScreen)
              this.InputProvider.FullScreenEventsEnabled = false;
            this.contextMap.Remove(context.Hwnd);
            this.Unhook(context);
          }
          return true;
        }
      }
      return false;
    }

    private void ReleaseContext(Context context)
    {
      lock (this.mapSync)
        context.Dispose();
    }

    private void Hook(Context context)
    {
      if (context.IsFullScreen)
        return;
      int threadId = context.ThreadId;
      ContextMap.HookRecord hookRecord;
      if (!this.threadMap.TryGetValue(threadId, out hookRecord))
      {
        IntPtr num1 = Microsoft.Surface.NativeMethods.SetWindowsHookEx(Microsoft.Surface.NativeMethods.HookProcedure.CallWindowProcedure, this.wndProcCallback, IntPtr.Zero, threadId);
        IntPtr num2 = Microsoft.Surface.NativeMethods.SetWindowsHookEx(Microsoft.Surface.NativeMethods.HookProcedure.MouseProcedure, this.mouseProcCallback, IntPtr.Zero, threadId);
        if (num1 == IntPtr.Zero || num2 == IntPtr.Zero)
          throw SurfaceCoreExceptions.UnableToInstallWndProcHookException();
        hookRecord = new ContextMap.HookRecord();
        hookRecord.ProcHook = num1;
        hookRecord.MouseHook = num2;
        hookRecord.RefCount = 1;
        this.threadMap.Add(threadId, hookRecord);
      }
      else
        ++hookRecord.RefCount;
    }

    private void Unhook(Context context)
    {
      if (context.IsFullScreen)
        return;
      ContextMap.HookRecord thread = this.threadMap[context.ThreadId];
      --thread.RefCount;
      if (thread.RefCount != 0)
        return;
      Microsoft.Surface.NativeMethods.UnhookWindowsHookEx(thread.ProcHook);
      Microsoft.Surface.NativeMethods.UnhookWindowsHookEx(thread.MouseHook);
      this.threadMap.Remove(context.ThreadId);
    }

    private IntPtr WindowHookProc(int nCode, IntPtr wParam, IntPtr lParam)
    {
      if (nCode >= 0)
      {
        Microsoft.Surface.NativeMethods.CallWndProcStruct structure = (Microsoft.Surface.NativeMethods.CallWndProcStruct) Marshal.PtrToStructure(lParam, typeof (Microsoft.Surface.NativeMethods.CallWndProcStruct));
        IntPtr hwnd = structure.hwnd;
        int lpdwProcessId;
        Microsoft.Surface.NativeMethods.GetWindowThreadProcessId(hwnd, out lpdwProcessId);
        Context context;
        if (Process.GetCurrentProcess().Id == lpdwProcessId && structure.message == Microsoft.Surface.NativeMethods.WindowMessage.Destroy && this.contextMap.TryGetValue(hwnd, out context))
          this.ReleaseContext(context);
      }
      return Microsoft.Surface.NativeMethods.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
    }

    private IntPtr MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
    {
      if (nCode >= 0)
      {
        Microsoft.Surface.NativeMethods.MOUSEHOOKSTRUCT structure = (Microsoft.Surface.NativeMethods.MOUSEHOOKSTRUCT) Marshal.PtrToStructure(lParam, typeof (Microsoft.Surface.NativeMethods.MOUSEHOOKSTRUCT));
        foreach (Context context in this.contextMap.Values)
        {
          if (context.Hwnd == structure.Hwnd)
          {
            Microsoft.Surface.NativeMethods.RECT rect;
            Microsoft.Surface.NativeMethods.GetClientRect(structure.Hwnd, out rect);
            float x = (float) structure.Pt.X;
            float y = (float) structure.Pt.Y;
            Microsoft.Surface.NativeMethods.ScreenToClient(structure.Hwnd, ref x, ref y);
            if ((double) y >= (double) rect.top && (double) y <= (double) rect.bottom && (double) x >= (double) rect.left && (double) x <= (double) rect.right)
            {
              bool flag1 = ((int) (uint) structure.ExtraInfo.ToInt64() & -256) == -11446528;
              bool flag2 = ((uint) structure.ExtraInfo.ToInt64() & 128U) > 0U;
              if (flag1 && flag2)
              {
                if ((int) wParam == 513 && Microsoft.Surface.NativeMethods.GetForegroundWindow() != structure.Hwnd && (Microsoft.Surface.NativeMethods.GetWindowLong(structure.Hwnd, -20) & 134217728) == 0)
                  Microsoft.Surface.NativeMethods.SetForegroundWindow(structure.Hwnd);
                return new IntPtr(1);
              }
            }
          }
        }
      }
      return Microsoft.Surface.NativeMethods.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
    }

    private void DispatchEvents()
    {
      EventsCore.TraceInputLoopStart();
label_1:
      try
      {
        this.inputProvider.DataReady.WaitOne();
        while (true)
        {
          RawInputEvent inputEvent = (RawInputEvent) null;
          lock (this.inputProvider.SyncRoot)
          {
            if (this.inputProvider.EventQueue.Count != 0)
              inputEvent = this.inputProvider.EventQueue.Dequeue();
            else
              goto label_1;
          }
          this.DeliverEvent(inputEvent);
        }
      }
      catch (ThreadAbortException ex)
      {
        EventsCore.TraceInputLoopFinish();
        throw;
      }
      catch (Exception ex)
      {
        EventsCore.LogUnexpectedExceptionDuringEventDispatching(ex.ToString());
        throw;
      }
    }

    private void DeliverEvent(RawInputEvent inputEvent)
    {
      bool okToSend;
      if (this.IsBroadcastEvent(inputEvent, out okToSend))
      {
        if (!okToSend)
          return;
        this.BroadcastEvent(inputEvent);
      }
      else
      {
        Context context1;
        if (this.TryGetContextForEvent(inputEvent, out context1))
        {
          SurfaceEventId surfaceEventId = SurfaceEventId.UnknownEvent;
          if (inputEvent.EventId == SurfaceEventId.TouchRoutedFrom)
          {
            surfaceEventId = inputEvent.EventId;
            inputEvent.EventId = SurfaceEventId.TouchUp;
          }
          else if (inputEvent.EventId == SurfaceEventId.TouchRoutedTo)
          {
            surfaceEventId = inputEvent.EventId;
            inputEvent.EventId = SurfaceEventId.TouchDown;
          }
          lock (this.mapSync)
            context1.HandleInputEvent(inputEvent);
          if (surfaceEventId != SurfaceEventId.UnknownEvent)
            inputEvent.EventId = surfaceEventId;
        }
        Context context2;
        if (!this.TryGetFullScreenContext(out context2) || context2 == context1 || inputEvent.EventId == SurfaceEventId.TouchRoutedFrom)
          return;
        if (inputEvent.EventId == SurfaceEventId.TouchRoutedTo)
          inputEvent.EventId = SurfaceEventId.TouchMove;
        lock (this.mapSync)
          context2.HandleInputEvent(inputEvent);
      }
    }

    private bool IsBroadcastEvent(RawInputEvent inputEvent, out bool okToSend)
    {
      okToSend = false;
      switch (inputEvent.EventId)
      {
        case SurfaceEventId.FrameReceived:
          if (this.frameEventEnabledCount > 0)
            okToSend = true;
          return true;
        case SurfaceEventId.SystemStateChanged:
          okToSend = true;
          return true;
        default:
          return false;
      }
    }

    private void BroadcastEvent(RawInputEvent inputEvent)
    {
      lock (this.mapSync)
      {
        foreach (Context context in this.contextMap.Values)
          context.HandleInputEvent(inputEvent);
      }
    }

    private bool TryGetContextForEvent(RawInputEvent inputEvent, out Context context)
    {
      return this.TryGetContextForHwnd(inputEvent.TargetHwnd, out context);
    }

    private bool TryGetFullScreenContext(out Context context)
    {
      return this.TryGetContextForHwnd(IntPtr.Zero, out context);
    }

    private bool TryGetContextForHwnd(IntPtr hwnd, out Context context)
    {
      lock (this.mapSync)
        return this.contextMap.TryGetValue(hwnd, out context);
    }

    private class HookRecord
    {
      public IntPtr ProcHook;
      public IntPtr MouseHook;
      public int RefCount;
    }
  }
}
