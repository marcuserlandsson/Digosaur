// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.RawInput.HidInput.RawImageDevice
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

#nullable disable
namespace Microsoft.Surface.Core.RawInput.HidInput
{
  internal class RawImageDevice : IDisposable
  {
    private const string RawImageDeviceInterfaceGuid = "B79BEC6B-837E-4E7C-AFEB-02157C136947";
    private const uint MaxPacketSize = 512;
    private const int DeviceWaitTimeout = 1000;
    private static RawImageDevice singleton;
    private static readonly object staticSyncRoot = new object();
    private bool disposed;
    private readonly uint IOCTL_VIP_RAWIMAGE_GET_IMAGE;
    private readonly uint IOCTL_SURFACE_GET_IMAGE_FORMAT;
    private uint imageHeaderSize;
    private uint imageWidth;
    private uint imageHeight;
    private uint imageSize;
    private uint imageBufferSize;
    private uint imageBufferDataOffset;
    private uint bitsPerPixel;
    private IntPtr imageBuffer;
    private SafeFileHandle deviceHandle;
    private readonly string devicePath;
    private readonly AutoResetEvent deviceIoWaitHandle;
    private readonly IArrayFactory<byte> imageDataFactory = (IArrayFactory<byte>) new ArrayFactory<byte>();

    private RawImageDevice()
    {
      this.IOCTL_VIP_RAWIMAGE_GET_IMAGE = RawImageDevice.ConstructIOCTL(11U, 2048U, 2U, 1U);
      this.IOCTL_SURFACE_GET_IMAGE_FORMAT = RawImageDevice.ConstructIOCTL(11U, 2049U, 2U, 1U);
      this.devicePath = RawImageDevice.FindFirstInterface(new Guid("B79BEC6B-837E-4E7C-AFEB-02157C136947"));
      this.deviceIoWaitHandle = new AutoResetEvent(false);
    }

    public unsafe bool Open(bool openExclusive)
    {
      if (!this.IsOpen())
      {
        this.deviceHandle = Microsoft.Surface.NativeWrappers.NativeMethods.CreateFile(this.devicePath, FileAccess.Read, openExclusive ? FileShare.None : FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, (FileAttributes) 1073741824, IntPtr.Zero);
        if (this.deviceHandle.IsInvalid)
          return false;
        NativeOverlapped nativeOverlapped = new NativeOverlapped();
        nativeOverlapped.EventHandle = this.deviceIoWaitHandle.SafeWaitHandle.DangerousGetHandle();
        RawImageDevice.ImageFormat structure = new RawImageDevice.ImageFormat();
        uint num = 0;
        Microsoft.Surface.NativeWrappers.NativeMethods.DeviceIoControl(this.deviceHandle.DangerousGetHandle(), this.IOCTL_SURFACE_GET_IMAGE_FORMAT, IntPtr.Zero, 0U, new IntPtr((void*) &structure), (uint) Marshal.SizeOf((object) structure), ref num, ref nativeOverlapped);
        if (!this.deviceIoWaitHandle.WaitOne(1000))
          return false;
        this.imageWidth = (uint) structure.ImageWidth;
        this.imageHeight = (uint) structure.ImageHeight;
        this.imageSize = this.imageWidth * this.imageHeight;
        this.imageHeaderSize = (uint) Marshal.SizeOf(typeof (RawImageDevice.ImageHeader));
        this.imageBufferDataOffset = this.imageHeaderSize + (512U - this.imageHeaderSize);
        this.imageBufferSize = this.imageBufferDataOffset + this.imageSize + this.imageSize % 512U;
        this.imageBuffer = Marshal.AllocHGlobal((int) this.imageBufferSize);
        this.bitsPerPixel = (uint) structure.BitDepth;
      }
      return true;
    }

    public void Close()
    {
      if (!this.IsOpen())
        return;
      this.deviceHandle.Dispose();
      this.deviceHandle = (SafeFileHandle) null;
      if (!(this.imageBuffer != IntPtr.Zero))
        return;
      Marshal.FreeHGlobal(this.imageBuffer);
      this.imageBuffer = IntPtr.Zero;
    }

    public bool IsOpen()
    {
      return this.deviceHandle != null && !this.deviceHandle.IsClosed && !this.deviceHandle.IsInvalid;
    }

    public unsafe RawImage GetNextImage()
    {
      NativeOverlapped nativeOverlapped = new NativeOverlapped();
      nativeOverlapped.EventHandle = this.deviceIoWaitHandle.SafeWaitHandle.DangerousGetHandle();
      uint num = 0;
      Microsoft.Surface.NativeWrappers.NativeMethods.DeviceIoControl(this.deviceHandle.DangerousGetHandle(), this.IOCTL_VIP_RAWIMAGE_GET_IMAGE, IntPtr.Zero, 0U, this.imageBuffer, this.imageBufferSize, ref num, ref nativeOverlapped);
      return !this.deviceIoWaitHandle.WaitOne(1000) ? (RawImage) null : new RawImage(ImageType.Normalized, (int) this.imageWidth, (int) this.imageHeight, (int) this.bitsPerPixel, (byte*) (this.imageBuffer + (int) this.imageBufferDataOffset).ToPointer(), this.imageDataFactory);
    }

    public static RawImageDevice Instance
    {
      get
      {
        lock (RawImageDevice.staticSyncRoot)
        {
          if (RawImageDevice.singleton == null)
            RawImageDevice.singleton = new RawImageDevice();
          return RawImageDevice.singleton;
        }
      }
    }

    public int ImageWidth => (int) this.imageWidth;

    public int ImageHeight => (int) this.imageHeight;

    public int BitsPerPixel => (int) this.bitsPerPixel;

    private static uint ConstructIOCTL(uint deviceType, uint function, uint method, uint access)
    {
      return (uint) ((int) deviceType << 16 | (int) access << 14 | (int) function << 2) | method;
    }

    private static string FindFirstInterface(Guid interfaceGuid)
    {
      bool flag = false;
      string firstInterface = "";
      IntPtr classDevs = Microsoft.Surface.NativeWrappers.NativeMethods.SetupDiGetClassDevs(ref interfaceGuid, IntPtr.Zero, IntPtr.Zero, 18U);
      if (classDevs.ToInt32() == -1)
        return string.Empty;
      Microsoft.Surface.NativeWrappers.NativeMethods.SP_DEVICE_INTERFACE_DATA structure1 = new Microsoft.Surface.NativeWrappers.NativeMethods.SP_DEVICE_INTERFACE_DATA();
      structure1.Size = (uint) Marshal.SizeOf((object) structure1);
      int num1 = 0;
      while (Microsoft.Surface.NativeWrappers.NativeMethods.SetupDiEnumDeviceInterfaces(classDevs, IntPtr.Zero, ref interfaceGuid, num1, ref structure1) && !flag)
      {
        ++num1;
        Microsoft.Surface.NativeWrappers.NativeMethods.SP_DEVINFO_DATA structure2 = new Microsoft.Surface.NativeWrappers.NativeMethods.SP_DEVINFO_DATA();
        structure2.Size = (uint) Marshal.SizeOf((object) structure2);
        uint cb;
        Microsoft.Surface.NativeWrappers.NativeMethods.SetupDiGetDeviceInterfaceDetailW(classDevs, ref structure1, IntPtr.Zero, 0U, ref cb, ref structure2);
        if (Marshal.GetLastWin32Error() == 122)
        {
          Microsoft.Surface.NativeWrappers.NativeMethods.SP_DEVICE_INTERFACE_DETAIL_DATA structure3 = new Microsoft.Surface.NativeWrappers.NativeMethods.SP_DEVICE_INTERFACE_DETAIL_DATA();
          structure3.Size = IntPtr.Size == 8 ? 8U : (uint) (4 + Marshal.SystemDefaultCharSize);
          IntPtr num2 = Marshal.AllocHGlobal((int) cb);
          Marshal.StructureToPtr((object) structure3, num2, false);
          if (Microsoft.Surface.NativeWrappers.NativeMethods.SetupDiGetDeviceInterfaceDetailW(classDevs, ref structure1, num2, cb, ref cb, ref structure2))
          {
            firstInterface = Marshal.PtrToStringUni(num2 + Marshal.SizeOf((object) structure3.Size));
            flag = true;
          }
          Marshal.FreeHGlobal(num2);
        }
      }
      Microsoft.Surface.NativeWrappers.NativeMethods.SetupDiDestroyDeviceInfoList(classDevs);
      return firstInterface;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!this.disposed)
      {
        if (disposing)
        {
          if (this.deviceHandle != null && !this.deviceHandle.IsInvalid && !this.deviceHandle.IsClosed)
            this.deviceHandle.Close();
          this.deviceIoWaitHandle.Dispose();
        }
        if (this.imageBuffer != IntPtr.Zero)
        {
          Marshal.FreeHGlobal(this.imageBuffer);
          this.imageBuffer = IntPtr.Zero;
        }
      }
      this.disposed = true;
    }

    ~RawImageDevice() => this.Dispose(false);

    private struct ImageHeader
    {
      public uint Signature;
      public uint FrameNumber;
      public uint ImageSize;
      public uint TimeStampLow;
      public uint TimeStampHigh;
    }

    private struct ImageFormat
    {
      public byte BitDepth;
      public byte Reserved;
      public ushort ImageWidth;
      public ushort ImageHeight;
    }
  }
}
