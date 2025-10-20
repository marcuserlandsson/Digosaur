// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.NativeMethods
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.Surface.Core;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.Surface
{
  internal static class NativeMethods
  {
    public const int WM_DESTROY = 2;
    public const int GWL_STYLE = -16;
    public const int GWL_EXSTYLE = -20;
    public const int WS_CHILD = 1073741824;
    public const int WS_EX_NOACTIVATE = 134217728;
    public const int HWND_NOTOPMOST = -2;
    public const int SWP_NOSIZE = 1;
    public const int SWP_NOMOVE = 2;
    public const int SWP_NOACTIVATE = 16;
    public const int WM_DISPLAYCHANGE = 126;
    internal const int LOGPIXELSX = 88;
    internal const int LOGPIXELSY = 90;
    public const uint WISPTIS_MASK = 4294967040;
    public const uint WISPTIS_DEVICE = 4283520768;
    public const uint WISPTIS_TYPE = 128;
    private const int TABLET_DISABLE_PRESSANDHOLD = 1;
    private const int TABLET_DISABLE_PENTAPFEEDBACK = 8;
    private const int TABLET_DISABLE_PENBARRELFEEDBACK = 16;
    private const int TABLET_DISABLE_FLICKS = 65536;
    private const int TABLET_ENABLE_MULTITOUCHDATA = 16777216;
    public static readonly IntPtr InvalidHandle = (IntPtr) -1;

    [DllImport("user32.dll")]
    public static extern int SetWindowPos(
      IntPtr hwnd,
      int hwndInsertAfter,
      int x,
      int y,
      int cx,
      int cy,
      int wFlags);

    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetForegroundWindow(IntPtr hwnd);

    [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
    public static extern int GetDeviceCaps(IntPtr hDC, int nIndex);

    [DllImport("gdi32.dll", EntryPoint = "CreateDC", CharSet = CharSet.Unicode)]
    private static extern IntPtr CreateDCUnsafe(
      string lpszDriver,
      string lpszDeviceName,
      string lpszOutput,
      IntPtr devMode);

    public static IntPtr CreateDC(string lpszDriver)
    {
      return NativeMethods.CreateDCUnsafe(lpszDriver, (string) null, (string) null, IntPtr.Zero);
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeleteDC(IntPtr hDC);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr WindowFromPoint(NativeMethods.POINT point);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetMessageExtraInfo();

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ClientToScreen(IntPtr hwnd, ref NativeMethods.POINT pt);

    public static void ClientToScreen(IntPtr client, ref float x, ref float y)
    {
      NativeMethods.POINT pt = new NativeMethods.POINT((int) Math.Round((double) x), (int) Math.Round((double) y));
      float num1 = x - (float) pt.X;
      float num2 = y - (float) pt.Y;
      if (!NativeMethods.ClientToScreen(client, ref pt))
        throw SurfaceCoreExceptions.WindowsApiFailedException(nameof (ClientToScreen), Marshal.GetLastWin32Error());
      x = (float) pt.X + num1;
      y = (float) pt.Y + num2;
    }

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ScreenToClient(IntPtr hwnd, ref NativeMethods.POINT pt);

    public static void ScreenToClient(IntPtr client, ref float x, ref float y)
    {
      NativeMethods.POINT pt = new NativeMethods.POINT((int) Math.Round((double) x), (int) Math.Round((double) y));
      float num1 = x - (float) pt.X;
      float num2 = y - (float) pt.Y;
      if (!NativeMethods.ScreenToClient(client, ref pt))
        throw SurfaceCoreExceptions.WindowsApiFailedException(nameof (ScreenToClient), Marshal.GetLastWin32Error());
      x = (float) pt.X + num1;
      y = (float) pt.Y + num2;
    }

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetClientRect(IntPtr hwnd, out NativeMethods.RECT rect);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int MapWindowPoints(
      IntPtr hwndFrom,
      IntPtr hwndTo,
      IntPtr lpPoints,
      int cPoints);

    public static unsafe void MapWindowPoints(IntPtr hwndFrom, IntPtr hwndTo, PointF[] pointArray)
    {
      NativeMethods.POINT[] pointArray1 = new NativeMethods.POINT[pointArray.Length];
      for (int index = 0; index < pointArray.Length; ++index)
      {
        PointF point = pointArray[index];
        pointArray1[index] = new NativeMethods.POINT((int) point.X, (int) point.Y);
      }
      fixed (NativeMethods.POINT* pointPtr = pointArray1)
        NativeMethods.MapWindowPoints(hwndFrom, hwndTo, new IntPtr((void*) pointPtr), pointArray.Length);
      for (int index = 0; index < pointArray1.Length; ++index)
      {
        NativeMethods.POINT point = pointArray1[index];
        pointArray[index].X = pointArray[index].X - (float) (int) pointArray[index].X + (float) point.X;
        pointArray[index].Y = pointArray[index].Y - (float) (int) pointArray[index].Y + (float) point.Y;
      }
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    public static extern IntPtr SetWindowsHookEx(
      [MarshalAs(UnmanagedType.I4)] NativeMethods.HookProcedure idHook,
      NativeMethods.HookProc lpfn,
      IntPtr hInstance,
      int dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    public static extern IntPtr CallNextHookEx(
      IntPtr hhook,
      int nCode,
      IntPtr wParam,
      IntPtr lParam);

    [DllImport("ole32.dll", PreserveSig = false)]
    [return: MarshalAs(UnmanagedType.IUnknown)]
    public static extern object CoCreateInstance(
      [MarshalAs(UnmanagedType.LPStruct), In] Guid rclsid,
      [MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter,
      NativeMethods.CLSCTX dwClsContext,
      [MarshalAs(UnmanagedType.LPStruct), In] Guid riid);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern short GlobalAddAtom(string atom);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int SetProp(IntPtr hWnd, string atom, IntPtr handle);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr RemoveProp(IntPtr hWnd, string atom);

    public static bool EnableTabletGestures(IntPtr hWnd, bool enable)
    {
      string atom = "MicrosoftTabletPenServiceProperty";
      if (NativeMethods.GlobalAddAtom(atom) == (short) 0)
        return false;
      if (enable)
        return NativeMethods.RemoveProp(hWnd, atom).ToInt64() == 1L;
      int num = 16842777;
      return NativeMethods.SetProp(hWnd, atom, new IntPtr(num)) == 1;
    }

    internal static class ExternDll
    {
      public const string Kernel32 = "kernel32.dll";
      public const string User32 = "user32.dll";
      public const string Gdi32 = "gdi32.dll";
    }

    public struct POINT
    {
      public int X;
      public int Y;

      public POINT(int x, int y)
      {
        this.X = x;
        this.Y = y;
      }
    }

    public struct RECT
    {
      public int left;
      public int top;
      public int right;
      public int bottom;
    }

    public struct MOUSEHOOKSTRUCT
    {
      public NativeMethods.POINT Pt;
      public IntPtr Hwnd;
      public uint HitTestCode;
      public IntPtr ExtraInfo;
    }

    public enum WindowMessage
    {
      Null = 0,
      Create = 1,
      Destroy = 2,
      Move = 3,
      Size = 5,
      Activate = 6,
      MouseMove = 512, // 0x00000200
      MouseDown = 513, // 0x00000201
      MouseUp = 514, // 0x00000202
    }

    public enum HookProcedure
    {
      CallWindowProcedure = 4,
      MouseProcedure = 7,
    }

    public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class CallWndProcStruct
    {
      public int lParam;
      public int wParam;
      [MarshalAs(UnmanagedType.I4)]
      public NativeMethods.WindowMessage message;
      public IntPtr hwnd;
    }

    public enum CLSCTX
    {
      CLSCTX_INPROC_SERVER = 1,
      CLSCTX_LOCAL_SERVER = 4,
    }
  }
}
