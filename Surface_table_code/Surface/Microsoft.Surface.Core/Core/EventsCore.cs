// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.EventsCore
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using System;
using System.Diagnostics.Eventing;

#nullable disable
namespace Microsoft.Surface.Core
{
  internal static class EventsCore
  {
    private static Guid guidProvider = new Guid("88931919-5519-47eb-8bd9-df363e0db6cb");
    private static EventProvider provider = EventsCore.OpenLog();
    private static EventDescriptor CannotConnectToVisionSystem = new EventDescriptor(1000, (byte) 0, (byte) 16, (byte) 2, (byte) 0, 0, long.MinValue);
    private static EventDescriptor UnexpectedExceptionDuringEventDispatching = new EventDescriptor(1001, (byte) 0, (byte) 16, (byte) 2, (byte) 0, 0, long.MinValue);
    private static EventDescriptor CannotInstantiatePerformanceCounter = new EventDescriptor(1002, (byte) 0, (byte) 16, (byte) 2, (byte) 0, 0, long.MinValue);
    private static EventDescriptor PerformanceCountersAreNotInstalled = new EventDescriptor(1003, (byte) 0, (byte) 16, (byte) 2, (byte) 0, 0, long.MinValue);
    private static EventDescriptor TouchDownDebug = new EventDescriptor(1100, (byte) 0, (byte) 17, (byte) 4, (byte) 0, 100, 4611686018427387906L);
    private static EventDescriptor TouchMoveDebug = new EventDescriptor(1101, (byte) 0, (byte) 17, (byte) 4, (byte) 0, 100, 4611686018427387906L);
    private static EventDescriptor TouchUpDebug = new EventDescriptor(1102, (byte) 0, (byte) 17, (byte) 4, (byte) 0, 100, 4611686018427387906L);
    private static EventDescriptor TouchTargetCreated = new EventDescriptor(1110, (byte) 0, (byte) 17, (byte) 4, (byte) 1, 101, 4611686018427387906L);
    private static EventDescriptor TouchTargetDisposed = new EventDescriptor(1111, (byte) 0, (byte) 17, (byte) 4, (byte) 2, 101, 4611686018427387906L);
    private static EventDescriptor InputLoopStart = new EventDescriptor(1120, (byte) 0, (byte) 17, (byte) 4, (byte) 1, 102, 4611686018427387906L);
    private static EventDescriptor InputLoopFinish = new EventDescriptor(1121, (byte) 0, (byte) 17, (byte) 4, (byte) 2, 102, 4611686018427387906L);
    private static EventDescriptor RaiseTouchDown = new EventDescriptor(1300, (byte) 0, (byte) 18, (byte) 4, (byte) 0, 200, 2305843009213693956L);
    private static EventDescriptor RaiseTouchMove = new EventDescriptor(1301, (byte) 0, (byte) 18, (byte) 4, (byte) 0, 200, 2305843009213693956L);
    private static EventDescriptor RaiseTouchUp = new EventDescriptor(1302, (byte) 0, (byte) 18, (byte) 4, (byte) 0, 200, 2305843009213693956L);
    private static EventDescriptor ReadTouchDown = new EventDescriptor(1310, (byte) 0, (byte) 18, (byte) 4, (byte) 0, 200, 2305843009213693956L);
    private static EventDescriptor ReadTouchMove = new EventDescriptor(1311, (byte) 0, (byte) 18, (byte) 4, (byte) 0, 200, 2305843009213693956L);
    private static EventDescriptor ReadTouchUp = new EventDescriptor(1312, (byte) 0, (byte) 18, (byte) 4, (byte) 0, 200, 2305843009213693956L);
    private static EventDescriptor ReadInputFrameStart = new EventDescriptor(1320, (byte) 0, (byte) 18, (byte) 4, (byte) 1, 201, 2305843009213693956L);
    private static EventDescriptor ReadInputFrameFinish = new EventDescriptor(1321, (byte) 0, (byte) 18, (byte) 4, (byte) 2, 201, 2305843009213693956L);
    private static EventDescriptor TouchTargetInitializeStart = new EventDescriptor(1330, (byte) 0, (byte) 18, (byte) 4, (byte) 1, 101, 2305843009213693956L);
    private static EventDescriptor TouchTargetInitializeFinish = new EventDescriptor(1331, (byte) 0, (byte) 18, (byte) 4, (byte) 2, 101, 2305843009213693956L);

    private static EventProvider OpenLog() => new EventProvider(EventsCore.guidProvider);

    public static void CloseLog() => EventsCore.provider.Close();

    public static bool IsEnabledChannelOperational
    {
      get
      {
        return EventsCore.provider.IsEnabled(EventsCore.CannotConnectToVisionSystem.Level, EventsCore.CannotConnectToVisionSystem.Keywords);
      }
    }

    public static bool IsEnabledChannelDebug
    {
      get
      {
        return EventsCore.provider.IsEnabled(EventsCore.TouchDownDebug.Level, EventsCore.TouchDownDebug.Keywords);
      }
    }

    public static bool IsEnabledChannelPerformance
    {
      get
      {
        return EventsCore.provider.IsEnabled(EventsCore.RaiseTouchDown.Level, EventsCore.RaiseTouchDown.Keywords);
      }
    }

    public static void LogCannotConnectToVisionSystem(string exception)
    {
      EventsCore.provider.WriteEvent(ref EventsCore.CannotConnectToVisionSystem, (object) exception);
    }

    public static void LogUnexpectedExceptionDuringEventDispatching(string exception)
    {
      EventsCore.provider.WriteEvent(ref EventsCore.UnexpectedExceptionDuringEventDispatching, (object) exception);
    }

    public static void LogCannotInstantiatePerformanceCounter(string name, string exception)
    {
      EventsCore.provider.WriteEvent(ref EventsCore.CannotInstantiatePerformanceCounter, (object) name, (object) exception);
    }

    public static void LogPerformanceCountersAreNotInstalled()
    {
      EventsCore.provider.WriteEvent(ref EventsCore.PerformanceCountersAreNotInstalled);
    }

    public static void TraceTouchDownDebug(int hashCode, int id, double x, double y)
    {
      EventsCore.provider.WriteEvent(ref EventsCore.TouchDownDebug, (object) hashCode, (object) id, (object) x, (object) y);
    }

    public static void TraceTouchMoveDebug(int hashCode, int id, double x, double y)
    {
      EventsCore.provider.WriteEvent(ref EventsCore.TouchMoveDebug, (object) hashCode, (object) id, (object) x, (object) y);
    }

    public static void TraceTouchUpDebug(int hashCode, int id, double x, double y)
    {
      EventsCore.provider.WriteEvent(ref EventsCore.TouchUpDebug, (object) hashCode, (object) id, (object) x, (object) y);
    }

    public static void TraceTouchTargetCreated(int hashCode, int hwnd)
    {
      EventsCore.provider.WriteEvent(ref EventsCore.TouchTargetCreated, (object) hashCode, (object) hwnd);
    }

    public static void TraceTouchTargetDisposed(int hashCode, int hwnd)
    {
      EventsCore.provider.WriteEvent(ref EventsCore.TouchTargetDisposed, (object) hashCode, (object) hwnd);
    }

    public static void TraceInputLoopStart()
    {
      EventsCore.provider.WriteEvent(ref EventsCore.InputLoopStart);
    }

    public static void TraceInputLoopFinish()
    {
      EventsCore.provider.WriteEvent(ref EventsCore.InputLoopFinish);
    }

    public static void PerfRaiseTouchDown(int id)
    {
      EventsCore.provider.WriteEvent(ref EventsCore.RaiseTouchDown, (object) id);
    }

    public static void PerfRaiseTouchMove(int id)
    {
      EventsCore.provider.WriteEvent(ref EventsCore.RaiseTouchMove, (object) id);
    }

    public static void PerfRaiseTouchUp(int id)
    {
      EventsCore.provider.WriteEvent(ref EventsCore.RaiseTouchUp, (object) id);
    }

    public static void PerfReadTouchDown(int id)
    {
      EventsCore.provider.WriteEvent(ref EventsCore.ReadTouchDown, (object) id);
    }

    public static void PerfReadTouchMove(int id)
    {
      EventsCore.provider.WriteEvent(ref EventsCore.ReadTouchMove, (object) id);
    }

    public static void PerfReadTouchUp(int id)
    {
      EventsCore.provider.WriteEvent(ref EventsCore.ReadTouchUp, (object) id);
    }

    public static void PerfReadInputFrameStart()
    {
      EventsCore.provider.WriteEvent(ref EventsCore.ReadInputFrameStart);
    }

    public static void PerfReadInputFrameFinish()
    {
      EventsCore.provider.WriteEvent(ref EventsCore.ReadInputFrameFinish);
    }

    public static void PerfTouchTargetInitializeStart()
    {
      EventsCore.provider.WriteEvent(ref EventsCore.TouchTargetInitializeStart);
    }

    public static void PerfTouchTargetInitializeFinish()
    {
      EventsCore.provider.WriteEvent(ref EventsCore.TouchTargetInitializeFinish);
    }
  }
}
