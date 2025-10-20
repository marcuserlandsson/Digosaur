// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.RawInput.HidInput.HidInputProvider
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.Surface.HidSupport;
using Microsoft.Surface.HidSupport.NativeWrappers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Microsoft.Surface.Core.RawInput.HidInput
{
  internal class HidInputProvider : RawInputProvider
  {
    private const int OutOfRangeTimeoutPeriod = 333;
    private const int MinimumPressAndHoldInterval = 2000;
    private const int MaximumTapDelay = 1500;
    private const int MaximumGestureMovement = 10;
    private const int ContactWidthHeight = 20;
    private const bool OpenImageDeviceExclusively = false;
    private const int RawImageThreadResetTimeoutPeriod = 40;
    private readonly object frameSync = new object();
    private readonly object capsSync = new object();
    private readonly object imageSync = new object();
    private bool disposed;
    private IntPtr hwndTarget;
    private Form notificationForm;
    private Thread notificationThread;
    private AutoResetEvent handleReadyEvent;
    private IntPtr hook;
    private Microsoft.Surface.NativeWrappers.NativeMethods.HookProc hookProc;
    private readonly Dictionary<string, int> devicePathToDeviceId;
    private readonly Dictionary<int, HidInputProvider.DeviceRecord> deviceIdToDeviceRecord;
    private int currentDeviceCount;
    private int nextDeviceId = 1;
    private HidDeviceCapabilities primaryDeviceCapabilities;
    private string primaryCapabilitiesDevicePath;
    private readonly Thread rawImageReadThread;
    private readonly RawImageDevice rawImageDevice;
    private bool rawImageEnabled;
    private readonly ManualResetEvent rawImageReadThreadRunEvent = new ManualResetEvent(false);
    private readonly ManualResetEvent rawImageReadThreadPausedEvent = new ManualResetEvent(true);
    private readonly System.Threading.Timer rawImageReadThreadResetTimer;
    private float maximumBlobSize;
    private float maximumBlobWidth;
    private float maximumBlobHeight;
    private float maximumBlobAspectRatio;

    internal SurfaceDigitizer Digitizer { get; private set; }

    internal SurfaceDigitizer VirtualDigitizer { get; private set; }

    public event EventHandler NewDeviceArrivalEvent;

    public HidInputProvider()
    {
      this.SetContactSuppressionSettingsFromRegistry();
      this.rawImageDevice = RawImageDevice.Instance;
      if (this.rawImageDevice != null)
      {
        this.rawImageReadThread = new Thread(new ThreadStart(this.RawImageReadProc));
        this.rawImageReadThread.Name = "Raw Image Device Read thread";
        this.rawImageReadThread.IsBackground = true;
        this.rawImageReadThread.Start();
        this.rawImageReadThreadResetTimer = new System.Threading.Timer((TimerCallback) (state => this.rawImageReadThreadRunEvent.Set()), (object) this, -1, -1);
      }
      this.devicePathToDeviceId = new Dictionary<string, int>();
      this.deviceIdToDeviceRecord = new Dictionary<int, HidInputProvider.DeviceRecord>();
      this.AddExistingDevices();
      this.CreateNotificationWindow();
      this.SetNotificationWindowHook();
    }

    private void CreateNotificationWindow()
    {
      this.handleReadyEvent = new AutoResetEvent(false);
      this.notificationThread = new Thread((ThreadStart) (() =>
      {
        this.notificationForm = new Form();
        this.hwndTarget = this.notificationForm.Handle;
        this.handleReadyEvent.Set();
        Application.Run();
      }));
      this.notificationThread.Name = "DisplayChanged Notification Window";
      this.notificationThread.IsBackground = true;
      this.notificationThread.Start();
      this.handleReadyEvent.WaitOne();
      this.handleReadyEvent.Dispose();
    }

    private void SetNotificationWindowHook()
    {
      int windowThreadProcessId = Microsoft.Surface.NativeMethods.GetWindowThreadProcessId(this.hwndTarget, out int _);
      // ISSUE: method pointer
      this.hookProc = new Microsoft.Surface.NativeWrappers.NativeMethods.HookProc((object) this, __methodptr(HookProcedure));
      this.hook = Microsoft.Surface.NativeWrappers.NativeMethods.SetWindowsHookEx((Microsoft.Surface.NativeWrappers.NativeMethods.WindowsHookType) 4, this.hookProc, IntPtr.Zero, windowThreadProcessId);
      if (!(this.hook == IntPtr.Zero))
        return;
      Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
    }

    private IntPtr HookProcedure(int nCode, IntPtr wParam, IntPtr lParam)
    {
      if (nCode >= 0)
      {
        switch (((CWPSTRUCT) Marshal.PtrToStructure(lParam, typeof (CWPSTRUCT))).message)
        {
          case 126:
            using (Dictionary<int, HidInputProvider.DeviceRecord>.ValueCollection.Enumerator enumerator = this.deviceIdToDeviceRecord.Values.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                HidInputProvider.DeviceRecord current = enumerator.Current;
                System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle();
                if (!DigitizerDisplaySupport.TryGetMatchingScreenBounds(current.Device.HidDevice, ref rectangle))
                  rectangle = Screen.PrimaryScreen.Bounds;
                current.ScreenBounds = rectangle;
                DigitizerDisplaySupport.DisplayOrientation displayOrientation = (DigitizerDisplaySupport.DisplayOrientation) 0;
                if (!DigitizerDisplaySupport.TryGetScreenOrientation(current.Device.HidDevice, ref displayOrientation))
                  displayOrientation = (DigitizerDisplaySupport.DisplayOrientation) 0;
                current.Orientation = displayOrientation;
              }
              break;
            }
          case 144:
            if (this.hook != IntPtr.Zero)
            {
              Microsoft.Surface.NativeMethods.UnhookWindowsHookEx(this.hook);
              this.hook = IntPtr.Zero;
              break;
            }
            break;
        }
      }
      return Microsoft.Surface.NativeMethods.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
    }

    private void SetContactSuppressionSettingsFromRegistry()
    {
      using (RegistryKey registryKey1 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
      {
        if (registryKey1 == null)
          return;
        using (RegistryKey registryKey2 = registryKey1.OpenSubKey("SOFTWARE\\Microsoft\\Surface\\v2.0\\Input"))
        {
          if (registryKey2 == null)
            return;
          object obj1 = registryKey2.GetValue("MaximumBlobSize");
          if (obj1 != null)
            this.maximumBlobSize = Convert.ToSingle(obj1, (IFormatProvider) CultureInfo.InvariantCulture);
          object obj2 = registryKey2.GetValue("MaximumBlobWidth");
          if (obj2 != null)
            this.maximumBlobWidth = Convert.ToSingle(obj2, (IFormatProvider) CultureInfo.InvariantCulture);
          object obj3 = registryKey2.GetValue("MaximumBlobHeight");
          if (obj3 != null)
            this.maximumBlobHeight = Convert.ToSingle(obj3, (IFormatProvider) CultureInfo.InvariantCulture);
          object obj4 = registryKey2.GetValue("MaximumBlobAspectRatio");
          if (obj4 == null)
            return;
          this.maximumBlobAspectRatio = Convert.ToSingle(obj4, (IFormatProvider) CultureInfo.InvariantCulture);
        }
      }
    }

    public override DeviceCapabilities PrimaryDeviceCapabilities
    {
      get
      {
        lock (this.capsSync)
        {
          if (this.primaryDeviceCapabilities == null)
          {
            System.Drawing.Rectangle bounds = Screen.PrimaryScreen.Bounds;
            float width = float.NaN;
            float height = float.NaN;
            HidDevice touchDigitizerDevice = SurfaceHidDevices.GetBestTouchDigitizerDevice();
            if (touchDigitizerDevice == null)
            {
              this.primaryDeviceCapabilities = new HidDeviceCapabilities(new Microsoft.Surface.Core.Rectangle(bounds.Top, bounds.Left, bounds.Bottom, bounds.Right), new Microsoft.Surface.Core.SizeF(width, height), (HidDevice) null);
              return (DeviceCapabilities) this.primaryDeviceCapabilities;
            }
            DigitizerDisplaySupport.TryGetMatchingScreenBounds(touchDigitizerDevice, ref bounds);
            if (SurfaceHidDevices.IsSurfaceHidDevice(touchDigitizerDevice))
            {
              width = touchDigitizerDevice.DisplayWidth;
              height = touchDigitizerDevice.DisplayHeight;
            }
            this.primaryDeviceCapabilities = new HidDeviceCapabilities(new Microsoft.Surface.Core.Rectangle(bounds.Top, bounds.Left, bounds.Bottom, bounds.Right), new Microsoft.Surface.Core.SizeF(width, height), touchDigitizerDevice);
            int key;
            HidInputProvider.DeviceRecord deviceRecord;
            if (SurfaceHidDevices.IsSurfaceHidDevice(touchDigitizerDevice) && this.devicePathToDeviceId.TryGetValue(touchDigitizerDevice.DevicePath, out key) && this.deviceIdToDeviceRecord.TryGetValue(key, out deviceRecord) && deviceRecord.Device is SurfaceDigitizer device)
            {
              AccelerometerData accelerometerData = new AccelerometerData();
              if (device.TryGetAccelerometerData(ref accelerometerData))
                this.primaryDeviceCapabilities.RaiseTiltChangedEvent((object) this, new TiltChangedEventArgs(accelerometerData.OrientationY));
            }
            this.primaryCapabilitiesDevicePath = touchDigitizerDevice.DevicePath;
          }
          return (DeviceCapabilities) this.primaryDeviceCapabilities;
        }
      }
    }

    public override bool AreDeviceCapabilitiesAvailable() => true;

    public override void Initialize()
    {
      foreach (HidInputProvider.DeviceRecord deviceRecord in this.deviceIdToDeviceRecord.Values)
        deviceRecord.Device.StartRead();
      DeviceManager.Instance.NewDeviceArrivalEvent += new EventHandler<DeviceArrivalEventArgs>(this.OnNewDeviceArrival);
    }

    private void AddExistingDevices()
    {
      this.VirtualDigitizer = (SurfaceDigitizer) SurfaceVirtualDigitizer.Instance;
      if (this.VirtualDigitizer != null)
        this.AddDevice((IDigitizer) this.VirtualDigitizer);
      this.Digitizer = SurfaceDigitizer.Instance;
      if (this.Digitizer != null)
        this.AddDevice((IDigitizer) this.Digitizer);
      lock (DeviceManager.Instance.DeviceListLock)
      {
        foreach (HidDevice device in DeviceManager.Instance.DeviceList)
        {
          if (!SurfaceHidDevices.IsSurfaceHidDevice(device) && device.IsValidMultiTouchDigitizer)
            this.AddDevice((IDigitizer) new Microsoft.Surface.HidSupport.Digitizer(device, false));
        }
      }
    }

    private void AddDevice(IDigitizer device)
    {
      int num = this.nextDeviceId++;
      this.devicePathToDeviceId.Add(device.HidDevice.DevicePath, num);
      System.Drawing.Rectangle screenBounds = new System.Drawing.Rectangle();
      if (!DigitizerDisplaySupport.TryGetMatchingScreenBounds(device.HidDevice, ref screenBounds))
        screenBounds = Screen.PrimaryScreen.Bounds;
      DigitizerDisplaySupport.DisplayOrientation orientation = (DigitizerDisplaySupport.DisplayOrientation) 0;
      if (!DigitizerDisplaySupport.TryGetScreenOrientation(device.HidDevice, ref orientation))
        orientation = (DigitizerDisplaySupport.DisplayOrientation) 0;
      this.deviceIdToDeviceRecord.Add(num, new HidInputProvider.DeviceRecord(num, device, screenBounds, orientation, new TimerCallback(this.OnOutOfRangeTimerElapsed)));
      device.FrameReceivedEvent += new EventHandler<FrameReceivedEventArgs<IDigitizerContact>>(this.OnFrameReceived);
      device.DeviceRemovedEvent += new EventHandler(this.OnDeviceRemoved);
      if (device is SurfaceDigitizer surfaceDigitizer)
      {
        if (surfaceDigitizer == this.Digitizer && this.VirtualDigitizer == null || surfaceDigitizer == this.VirtualDigitizer)
          surfaceDigitizer.TiltDataReceivedEvent += new EventHandler<AccelerometerDataReceivedEventArgs>(this.OnTiltUpdateReceived);
        if (surfaceDigitizer == this.VirtualDigitizer && this.Digitizer != null)
          this.Digitizer.TiltDataReceivedEvent -= new EventHandler<AccelerometerDataReceivedEventArgs>(this.OnTiltUpdateReceived);
      }
      ++this.currentDeviceCount;
    }

    private void OnTiltUpdateReceived(object sender, AccelerometerDataReceivedEventArgs e)
    {
      lock (this.capsSync)
      {
        if (this.primaryDeviceCapabilities == null)
          return;
        this.primaryDeviceCapabilities.RaiseTiltChangedEvent((object) this, new TiltChangedEventArgs(e.Data.OrientationY));
      }
    }

    private void OnDeviceRemoved(object sender, EventArgs e)
    {
      IDigitizer idigitizer = (IDigitizer) sender;
      int key;
      if (this.devicePathToDeviceId.TryGetValue(idigitizer.HidDevice.DevicePath, out key))
      {
        HidInputProvider.DeviceRecord stateInfo = this.deviceIdToDeviceRecord[key];
        this.OnOutOfRangeTimerElapsed((object) stateInfo);
        stateInfo.Dispose();
        this.deviceIdToDeviceRecord.Remove(key);
      }
      this.devicePathToDeviceId.Remove(idigitizer.HidDevice.DevicePath);
      idigitizer.FrameReceivedEvent -= new EventHandler<FrameReceivedEventArgs<IDigitizerContact>>(this.OnFrameReceived);
      idigitizer.DeviceRemovedEvent -= new EventHandler(this.OnDeviceRemoved);
      if (idigitizer is SurfaceDigitizer surfaceDigitizer)
      {
        surfaceDigitizer.TiltDataReceivedEvent -= new EventHandler<AccelerometerDataReceivedEventArgs>(this.OnTiltUpdateReceived);
        if (surfaceDigitizer == this.VirtualDigitizer && this.Digitizer != null)
          this.Digitizer.TiltDataReceivedEvent += new EventHandler<AccelerometerDataReceivedEventArgs>(this.OnTiltUpdateReceived);
      }
      Monitor.Enter(this.capsSync);
      bool flag = idigitizer.HidDevice.DevicePath == this.primaryCapabilitiesDevicePath && this.primaryDeviceCapabilities != null;
      Monitor.Exit(this.capsSync);
      if (!flag)
        return;
      ThreadPool.QueueUserWorkItem((WaitCallback) delegate
      {
        Thread.Sleep(2000);
        lock (this.capsSync)
        {
          DeviceCapabilities deviceCapabilities = (DeviceCapabilities) this.primaryDeviceCapabilities;
          this.primaryDeviceCapabilities = (HidDeviceCapabilities) null;
          deviceCapabilities.RaiseDeviceInvalidatedEvent((object) deviceCapabilities, new EventArgs());
        }
      });
    }

    private void OnNewDeviceArrival(object sender, DeviceArrivalEventArgs e)
    {
      if (!this.devicePathToDeviceId.ContainsKey(e.Device.DevicePath) && e.Device.IsValidMultiTouchDigitizer)
      {
        IDigitizer device;
        if (SurfaceHidDevices.IsSurfaceVirtualDigitizerDevice(e.Device))
        {
          this.VirtualDigitizer = (SurfaceDigitizer) SurfaceVirtualDigitizer.Instance;
          device = (IDigitizer) this.VirtualDigitizer;
        }
        else if (SurfaceHidDevices.IsSurfaceDigitizerDevice(e.Device))
        {
          this.Digitizer = SurfaceDigitizer.Instance;
          device = (IDigitizer) this.Digitizer;
        }
        else
          device = (IDigitizer) new Microsoft.Surface.HidSupport.Digitizer(e.Device, false);
        this.AddDevice(device);
        device.StartRead();
        lock (this.capsSync)
        {
          HidDevice touchDigitizerDevice = SurfaceHidDevices.GetBestTouchDigitizerDevice();
          if (touchDigitizerDevice != null)
          {
            if (e.Device.DevicePath == touchDigitizerDevice.DevicePath)
            {
              if (this.primaryDeviceCapabilities != null)
              {
                DeviceCapabilities deviceCapabilities = (DeviceCapabilities) this.primaryDeviceCapabilities;
                this.primaryDeviceCapabilities = (HidDeviceCapabilities) null;
                deviceCapabilities.RaiseDeviceInvalidatedEvent((object) deviceCapabilities, new EventArgs());
              }
            }
          }
        }
      }
      EventHandler tmpEvent = this.NewDeviceArrivalEvent;
      if (tmpEvent == null)
        return;
      ThreadPool.QueueUserWorkItem((WaitCallback) (stateInfo => tmpEvent((object) this, EventArgs.Empty)));
    }

    private void OnFrameReceived(object sender, FrameReceivedEventArgs<IDigitizerContact> e)
    {
      EventsCore.PerfReadInputFrameStart();
      lock (this.frameSync)
      {
        DateTime now = DateTime.Now;
        int key1;
        HidInputProvider.DeviceRecord deviceRecord;
        if (!this.devicePathToDeviceId.TryGetValue(e.Device.DevicePath, out key1) || !this.deviceIdToDeviceRecord.TryGetValue(key1, out deviceRecord))
          return;
        HashSet<uint> uintSet = new HashSet<uint>();
        bool flag1 = false;
        foreach (IDigitizerContact contact in e.ContactList)
        {
          uintSet.Add(contact.ContactId);
          ContactRecord contactRecord;
          SurfaceEventId eventId = this.ProcessContact(deviceRecord, contact, now, out contactRecord);
          switch (eventId)
          {
            case SurfaceEventId.UnknownEvent:
              continue;
            case SurfaceEventId.TouchMove:
              bool flag2 = HidInputProvider.CoalesceContactEvent(deviceRecord, contactRecord, contact);
              if (HidInputProvider.ShouldContactGeneratePressAndHoldGesture(contactRecord, now))
              {
                flag1 = true;
                HidSystemGestureProperties properties = new HidSystemGestureProperties(SystemGestureType.PressAndHold, contactRecord);
                lock (this.syncRoot)
                  this.eventQueue.Enqueue(new RawInputEvent(SurfaceEventId.SystemGestureCompleted, contactRecord.CapturedHwnd, (object) properties));
              }
              if (flag2)
                continue;
              break;
          }
          flag1 = true;
          TouchProperties touchProperties = HidInputProvider.CreateTouchProperties(deviceRecord, contactRecord, contact);
          contactRecord.LastTouchProperties = touchProperties;
          if (eventId == SurfaceEventId.TouchUp && HidInputProvider.ShouldContactGenerateTapGesture(contactRecord, now))
          {
            HidSystemGestureProperties properties = new HidSystemGestureProperties(SystemGestureType.Tap, contactRecord);
            lock (this.syncRoot)
              this.eventQueue.Enqueue(new RawInputEvent(SurfaceEventId.SystemGestureCompleted, contactRecord.CapturedHwnd, (object) properties));
          }
          RawInputEvent rawInputEvent = new RawInputEvent(eventId, contactRecord.CapturedHwnd, (object) touchProperties);
          lock (this.syncRoot)
            this.eventQueue.Enqueue(rawInputEvent);
        }
        DateTime dateTime = now - TimeSpan.FromMilliseconds(333.0);
        foreach (uint key2 in deviceRecord.TrackedContacts.Keys.ToArray<uint>())
        {
          if (!uintSet.Contains(key2))
          {
            ContactRecord trackedContact = deviceRecord.TrackedContacts[key2];
            if (trackedContact.LastSeen < dateTime)
            {
              if (trackedContact.LastTouchProperties != null && !trackedContact.SuppressEvents)
              {
                flag1 = true;
                if (HidInputProvider.ShouldContactGenerateTapGesture(trackedContact, now))
                {
                  HidSystemGestureProperties properties = new HidSystemGestureProperties(SystemGestureType.Tap, trackedContact);
                  lock (this.syncRoot)
                    this.eventQueue.Enqueue(new RawInputEvent(SurfaceEventId.SystemGestureCompleted, trackedContact.CapturedHwnd, (object) properties));
                }
                RawInputEvent rawInputEvent = new RawInputEvent(SurfaceEventId.TouchUp, trackedContact.CapturedHwnd, (object) trackedContact.LastTouchProperties);
                lock (this.syncRoot)
                  this.eventQueue.Enqueue(rawInputEvent);
              }
              deviceRecord.TrackedContacts.Remove(key2);
            }
          }
        }
        if (flag1)
        {
          Dictionary<ImageType, RawImage> images = new Dictionary<ImageType, RawImage>();
          if (deviceRecord.Device == this.Digitizer && this.rawImageEnabled)
          {
            lock (this.imageSync)
            {
              if (deviceRecord.Device == this.Digitizer)
              {
                if (this.rawImageEnabled)
                {
                  this.rawImageReadThreadRunEvent.Reset();
                  this.rawImageReadThreadResetTimer.Change(40, -1);
                  RawImage nextImage = this.rawImageDevice.GetNextImage();
                  if (nextImage != null)
                    images.Add(ImageType.Normalized, nextImage);
                }
              }
            }
          }
          FrameProperties properties = new FrameProperties(images, now.Ticks);
          lock (this.syncRoot)
            this.eventQueue.Enqueue(new RawInputEvent(SurfaceEventId.FrameReceived, IntPtr.Zero, (object) properties));
          this.dataReadyEvent.Set();
        }
        deviceRecord.OutOfRangeTimer.Change(333, -1);
      }
      EventsCore.PerfReadInputFrameFinish();
    }

    private void OnOutOfRangeTimerElapsed(object stateInfo)
    {
      if (!(stateInfo is HidInputProvider.DeviceRecord deviceRecord))
        return;
      lock (this.frameSync)
      {
        DateTime now = DateTime.Now;
        bool flag = false;
        foreach (ContactRecord contactRecord in deviceRecord.TrackedContacts.Values)
        {
          if (contactRecord.LastTouchProperties != null)
          {
            flag = true;
            RawInputEvent rawInputEvent = new RawInputEvent(SurfaceEventId.TouchUp, contactRecord.CapturedHwnd, (object) contactRecord.LastTouchProperties);
            lock (this.syncRoot)
              this.eventQueue.Enqueue(rawInputEvent);
          }
        }
        deviceRecord.TrackedContacts.Clear();
        if (!flag)
          return;
        FrameProperties properties = new FrameProperties((Dictionary<ImageType, RawImage>) null, now.Ticks);
        lock (this.syncRoot)
          this.eventQueue.Enqueue(new RawInputEvent(SurfaceEventId.FrameReceived, IntPtr.Zero, (object) properties));
        this.dataReadyEvent.Set();
      }
    }

    internal void HandleNewContact(TouchProperties touchProperties)
    {
      this.QueueContactEvent(SurfaceEventId.TouchDown, touchProperties);
    }

    internal void HandleUpdateContact(TouchProperties touchProperties)
    {
      this.QueueContactEvent(SurfaceEventId.TouchMove, touchProperties);
    }

    internal void HandleRemoveContact(TouchProperties touchProperties)
    {
      this.QueueContactEvent(SurfaceEventId.TouchUp, touchProperties);
    }

    internal void HandleFrame(FrameProperties frameProperties)
    {
      lock (this.syncRoot)
        this.eventQueue.Enqueue(new RawInputEvent(SurfaceEventId.FrameReceived, IntPtr.Zero, (object) frameProperties));
      this.dataReadyEvent.Set();
    }

    private void QueueContactEvent(SurfaceEventId eventId, TouchProperties touchProperties)
    {
      RawInputEvent rawInputEvent = new RawInputEvent(eventId, touchProperties.HwndTarget, (object) touchProperties);
      lock (this.syncRoot)
        this.eventQueue.Enqueue(rawInputEvent);
    }

    public void CaptureContact(TouchProperties contactProperties, IntPtr hwnd)
    {
      lock (this.frameSync)
      {
        DateTime now = DateTime.Now;
        HidInputProvider.DeviceRecord deviceRecord;
        if (!this.deviceIdToDeviceRecord.TryGetValue(HidInputProvider.DeviceRecord.GetDeviceIdFromContactId((uint) contactProperties.Id), out deviceRecord))
          return;
        uint num = HidInputProvider.DeviceRecord.StripDeviceIdFromContactId((uint) contactProperties.Id);
        ContactRecord contactRecord;
        if (deviceRecord.TrackedContacts.TryGetValue(num, out contactRecord))
        {
          RawInputEvent rawInputEvent1 = new RawInputEvent(SurfaceEventId.TouchRoutedFrom, contactRecord.CapturedHwnd, (object) contactProperties.Clone());
          contactRecord.LastTouchProperties = contactProperties;
          contactRecord.CapturedHwnd = hwnd;
          contactRecord.SuppressEvents = false;
          contactRecord.LastTipSwitch = true;
          contactRecord.LastInRange = true;
          RawInputEvent rawInputEvent2 = new RawInputEvent(SurfaceEventId.TouchRoutedTo, hwnd, (object) contactRecord.LastTouchProperties);
          lock (this.syncRoot)
          {
            this.eventQueue.Enqueue(rawInputEvent1);
            this.eventQueue.Enqueue(rawInputEvent2);
            this.eventQueue.Enqueue(new RawInputEvent(SurfaceEventId.FrameReceived, IntPtr.Zero, (object) new FrameProperties((Dictionary<ImageType, RawImage>) null, now.Ticks)));
          }
          this.dataReadyEvent.Set();
        }
        else
        {
          contactRecord = new ContactRecord(num, hwnd, contactProperties.RecognizedTypes, deviceRecord.Device == this.Digitizer);
          deviceRecord.TrackedContacts.Add(num, contactRecord);
          contactRecord.LastTouchProperties = contactProperties;
          contactRecord.LastTouchProperties.HwndTarget = hwnd;
          contactRecord.SuppressEvents = false;
          contactRecord.LastTipSwitch = true;
          contactRecord.LastInRange = true;
          lock (this.syncRoot)
          {
            this.eventQueue.Enqueue(new RawInputEvent(SurfaceEventId.TouchRoutedFrom, contactRecord.CapturedHwnd, (object) contactProperties.Clone()));
            this.eventQueue.Enqueue(new RawInputEvent(SurfaceEventId.FrameReceived, IntPtr.Zero, (object) new FrameProperties((Dictionary<ImageType, RawImage>) null, now.Ticks)));
          }
          this.dataReadyEvent.Set();
        }
      }
    }

    private static TouchTypes GetTouchTypesForContact(IDigitizerContact contact)
    {
      if (contact is SurfaceContact surfaceContact)
      {
        switch (surfaceContact.ContactType - 1)
        {
          case 0:
            return TouchTypes.Blob;
          case 1:
            return TouchTypes.Finger;
          case 3:
            return TouchTypes.Tag;
        }
      }
      return TouchTypes.Blob;
    }

    private static ContactTypes GetContactTypesFromTouchProperties(TouchProperties touchProperties)
    {
      if (touchProperties != null)
      {
        switch (touchProperties.RecognizedTypes)
        {
          case TouchTypes.Finger:
            return (ContactTypes) 2;
          case TouchTypes.Tag:
            return (ContactTypes) 4;
        }
      }
      return (ContactTypes) 1;
    }

    private SurfaceEventId ProcessContact(
      HidInputProvider.DeviceRecord deviceRecord,
      IDigitizerContact contact,
      DateTime frameTime,
      out ContactRecord contactRecord)
    {
      Dictionary<uint, ContactRecord> trackedContacts = deviceRecord.TrackedContacts;
      bool flag1 = false;
      bool flag2 = false;
      SurfaceEventId surfaceEventId = SurfaceEventId.UnknownEvent;
      if (!trackedContacts.TryGetValue(contact.ContactId, out contactRecord))
      {
        Microsoft.Surface.Core.PointF screenCoordinates = HidInputProvider.DigitizerToScreenCoordinates(deviceRecord, contact.X, contact.Y);
        IntPtr capturedHwnd = Microsoft.Surface.NativeMethods.WindowFromPoint(new Microsoft.Surface.NativeMethods.POINT((int) screenCoordinates.X, (int) screenCoordinates.Y));
        contactRecord = new ContactRecord(contact.ContactId, capturedHwnd, HidInputProvider.GetTouchTypesForContact(contact), deviceRecord.Device == this.Digitizer);
        flag1 = true;
      }
      else if (contactRecord.SuppressEvents)
      {
        if (contactRecord.LastTipSwitch && contactRecord.LastInRange && !contact.TipSwitch)
        {
          trackedContacts.Remove(contact.ContactId);
        }
        else
        {
          contactRecord.LastTipSwitch = contact.TipSwitch;
          contactRecord.LastInRange = contact.InRange;
          contactRecord.LastSeen = frameTime;
        }
        return surfaceEventId;
      }
      contactRecord.LastSeen = frameTime;
      if (contact is SurfaceContact surfaceContact)
      {
        float num = (float) surfaceContact.Width / (float) surfaceContact.Height;
        if ((double) this.maximumBlobSize > 0.0 && (double) surfaceContact.PixelArea > (double) this.maximumBlobSize)
          flag2 = true;
        else if ((double) this.maximumBlobWidth > 0.0 && (double) surfaceContact.Width > (double) this.maximumBlobWidth)
          flag2 = true;
        else if ((double) this.maximumBlobHeight > 0.0 && (double) surfaceContact.Height > (double) this.maximumBlobHeight)
          flag2 = true;
        else if ((double) this.maximumBlobAspectRatio > 0.0 && ((double) num > (double) this.maximumBlobAspectRatio || 1.0 / (double) num > (double) this.maximumBlobAspectRatio))
          flag2 = true;
      }
      if (flag1)
      {
        if (contact.TipSwitch || contact.InRange)
        {
          if (!contact.TipSwitch && contact.InRange)
          {
            contactRecord.LastTipSwitch = false;
            contactRecord.LastInRange = true;
            trackedContacts.Add(contact.ContactId, contactRecord);
          }
          else if (contact.TipSwitch && contact.InRange)
          {
            contactRecord.LastTipSwitch = true;
            contactRecord.LastInRange = true;
            trackedContacts.Add(contact.ContactId, contactRecord);
            surfaceEventId = SurfaceEventId.TouchDown;
            contactRecord.FirstSeen = frameTime;
            contactRecord.InitialPosition = HidInputProvider.DigitizerToClientCoordinates(deviceRecord, contactRecord.CapturedHwnd, contact.X, contact.Y);
            contactRecord.AddDigitizerPositionSample(deviceRecord, contact.X, contact.Y);
            contactRecord.AddDigitizerSizeSample(deviceRecord, contact.Width, contact.Height);
            if (surfaceContact != null)
            {
              contactRecord.AddDigitizerCenterPositionSample(deviceRecord, surfaceContact.CenterOfMassX, surfaceContact.CenterOfMassY);
              contactRecord.AddDigitizerOrientationSample(deviceRecord, surfaceContact.Orientation);
              contactRecord.AddDigitizerEllipseSample(deviceRecord, surfaceContact.EllipseMajorAxis, surfaceContact.EllipseMinorAxis, surfaceContact.Orientation);
            }
            else
            {
              contactRecord.AddDigitizerCenterPositionSample(deviceRecord, contact.X, contact.Y);
              contactRecord.AddDigitizerOrientationSample(deviceRecord, 0.0f);
            }
            if (flag2)
            {
              contactRecord.SuppressEvents = true;
              surfaceEventId = SurfaceEventId.UnknownEvent;
            }
          }
        }
      }
      else if (contactRecord.LastTouchProperties != null && contactRecord.LastTouchProperties.RecognizedTypes != HidInputProvider.GetTouchTypesForContact(contact))
      {
        contactRecord.SuppressEvents = true;
        if (surfaceContact != null)
          surfaceContact.ContactType = HidInputProvider.GetContactTypesFromTouchProperties(contactRecord.LastTouchProperties);
        surfaceEventId = SurfaceEventId.TouchUp;
      }
      else if (!contact.TipSwitch && contactRecord.LastTipSwitch && contactRecord.LastInRange)
      {
        surfaceEventId = SurfaceEventId.TouchUp;
        contactRecord.AddDigitizerPositionSample(deviceRecord, contact.X, contact.Y);
        contactRecord.AddDigitizerSizeSample(deviceRecord, contact.Width, contact.Height);
        deviceRecord.TrackedContacts.Remove(contact.ContactId);
      }
      else if (contact.TipSwitch && contact.InRange && !contactRecord.LastTipSwitch)
      {
        surfaceEventId = SurfaceEventId.TouchDown;
        contactRecord.FirstSeen = frameTime;
        contactRecord.InitialPosition = HidInputProvider.DigitizerToClientCoordinates(deviceRecord, contactRecord.CapturedHwnd, contact.X, contact.Y);
        contactRecord.AddDigitizerPositionSample(deviceRecord, contact.X, contact.Y);
        contactRecord.AddDigitizerSizeSample(deviceRecord, contact.Width, contact.Height);
        contactRecord.LastTipSwitch = true;
        contactRecord.LastInRange = true;
        if (surfaceContact != null)
        {
          contactRecord.AddDigitizerCenterPositionSample(deviceRecord, surfaceContact.CenterOfMassX, surfaceContact.CenterOfMassY);
          contactRecord.AddDigitizerOrientationSample(deviceRecord, surfaceContact.Orientation);
          contactRecord.AddDigitizerEllipseSample(deviceRecord, surfaceContact.EllipseMajorAxis, surfaceContact.EllipseMinorAxis, surfaceContact.Orientation);
        }
        else
        {
          contactRecord.AddDigitizerCenterPositionSample(deviceRecord, contact.X, contact.Y);
          contactRecord.AddDigitizerOrientationSample(deviceRecord, 0.0f);
        }
        if (flag2)
        {
          contactRecord.SuppressEvents = true;
          surfaceEventId = SurfaceEventId.UnknownEvent;
        }
      }
      else if (contact.TipSwitch && contact.InRange && contactRecord.LastTipSwitch)
      {
        surfaceEventId = SurfaceEventId.TouchMove;
        if (flag2)
        {
          contactRecord.SuppressEvents = true;
          surfaceEventId = SurfaceEventId.TouchUp;
        }
      }
      if (surfaceContact != null && surfaceContact.ContactType == 4)
      {
        bool flag3 = contactRecord.SmoothedTagData == TagData.None;
        TagData tagData = surfaceContact.TagData;
        contactRecord.AddDigitizerTagDataSample(new TagData(tagData.Schema, tagData.Series, tagData.ExtValue, tagData.Value));
        if (flag3 && !contactRecord.SuppressEvents)
        {
          if (contactRecord.SmoothedTagData == TagData.None)
            surfaceEventId = SurfaceEventId.UnknownEvent;
          else if (surfaceEventId == SurfaceEventId.TouchMove)
            surfaceEventId = SurfaceEventId.TouchDown;
        }
      }
      return surfaceEventId;
    }

    private static bool CoalesceContactEvent(
      HidInputProvider.DeviceRecord deviceRecord,
      ContactRecord contactRecord,
      IDigitizerContact contact)
    {
      bool flag = true;
      Microsoft.Surface.Core.PointF smoothedPosition = contactRecord.SmoothedPosition;
      contactRecord.AddDigitizerPositionSample(deviceRecord, contact.X, contact.Y);
      if (contactRecord.SmoothedPosition != smoothedPosition)
        flag = false;
      Microsoft.Surface.Core.SizeF smoothedSize = contactRecord.SmoothedSize;
      contactRecord.AddDigitizerSizeSample(deviceRecord, contact.Width, contact.Height);
      if (contactRecord.SmoothedSize != smoothedSize)
        flag = false;
      if (contact is SurfaceContact surfaceContact)
      {
        Microsoft.Surface.Core.PointF smoothedCenter = contactRecord.SmoothedCenter;
        contactRecord.AddDigitizerCenterPositionSample(deviceRecord, surfaceContact.CenterOfMassX, surfaceContact.CenterOfMassY);
        if (contactRecord.SmoothedCenter != smoothedCenter)
          flag = false;
        float smoothedOrientation = contactRecord.SmoothedOrientation;
        contactRecord.AddDigitizerOrientationSample(deviceRecord, surfaceContact.Orientation);
        if (contactRecord.SmoothedOrientation.CompareTo(smoothedOrientation) != 0)
          flag = false;
        float ellipseMajorAxis = contactRecord.SmoothedEllipseMajorAxis;
        float ellipseMinorAxis = contactRecord.SmoothedEllipseMinorAxis;
        contactRecord.AddDigitizerEllipseSample(deviceRecord, surfaceContact.EllipseMajorAxis, surfaceContact.EllipseMinorAxis, surfaceContact.Orientation);
        if (contactRecord.SmoothedEllipseMajorAxis.CompareTo(ellipseMajorAxis) != 0 || contactRecord.SmoothedEllipseMinorAxis.CompareTo(ellipseMinorAxis) != 0)
          flag = false;
      }
      return flag;
    }

    private static bool ShouldContactGeneratePressAndHoldGesture(
      ContactRecord contactRecord,
      DateTime frameTime)
    {
      if (contactRecord.FirstSeen == DateTime.MinValue)
        return false;
      TimeSpan timeSpan = frameTime.Subtract(contactRecord.FirstSeen);
      float num1 = Math.Abs(contactRecord.InitialPosition.X - contactRecord.SmoothedPosition.X);
      float num2 = Math.Abs(contactRecord.InitialPosition.Y - contactRecord.SmoothedPosition.Y);
      if (timeSpan.TotalMilliseconds < 2000.0 || (double) num1 > 10.0 || (double) num2 > 10.0)
        return false;
      contactRecord.FirstSeen = DateTime.MinValue;
      return true;
    }

    private static bool ShouldContactGenerateTapGesture(
      ContactRecord contactRecord,
      DateTime frameTime)
    {
      TimeSpan timeSpan = frameTime.Subtract(contactRecord.FirstSeen);
      float num1 = Math.Abs(contactRecord.InitialPosition.X - contactRecord.SmoothedPosition.X);
      float num2 = Math.Abs(contactRecord.InitialPosition.Y - contactRecord.SmoothedPosition.Y);
      return timeSpan.TotalMilliseconds <= 1500.0 && (double) num1 <= 10.0 && (double) num2 <= 10.0;
    }

    private static TouchProperties CreateTouchProperties(
      HidInputProvider.DeviceRecord deviceRecord,
      ContactRecord contactRecord,
      IDigitizerContact contact)
    {
      TouchProperties touchProperties = new TouchProperties(contactRecord.CapturedHwnd)
      {
        Timestamp = contactRecord.LastSeen.Ticks,
        Id = HidInputProvider.DeviceRecord.GetDeviceContactId(deviceRecord.DeviceId, contact.ContactId),
        Position = contactRecord.SmoothedPosition
      };
      touchProperties.CenterPosition = touchProperties.Position;
      touchProperties.RecognizedTypes = HidInputProvider.GetTouchTypesForContact(contact);
      touchProperties.Tag = TagData.None;
      if (contact is SurfaceContact surfaceContact)
      {
        switch (surfaceContact.ContactType - 2)
        {
          case 0:
            touchProperties.CenterPosition = contactRecord.SmoothedCenter;
            break;
          case 2:
            touchProperties.Tag = contactRecord.SmoothedTagData;
            break;
        }
        touchProperties.MajorAxis = contactRecord.SmoothedEllipseMajorAxis;
        touchProperties.MinorAxis = contactRecord.SmoothedEllipseMinorAxis;
        Microsoft.Surface.Core.PointF clientCoordinates = HidInputProvider.DigitizerToClientCoordinates(deviceRecord, touchProperties.HwndTarget, surfaceContact.Left, surfaceContact.Top);
        Microsoft.Surface.Core.SizeF smoothedSize = contactRecord.SmoothedSize;
        touchProperties.Bounds = new TouchBounds(clientCoordinates, smoothedSize);
        touchProperties.Orientation = contactRecord.SmoothedOrientation;
        touchProperties.PhysicalArea = (float) ((double) touchProperties.MajorAxis * 0.5 * (double) touchProperties.MinorAxis * 0.5 * Math.PI);
      }
      else
      {
        if (deviceRecord.Device.HidDevice.IsContactWidthSupported && deviceRecord.Device.HidDevice.IsContactHeightSupported)
        {
          Microsoft.Surface.Core.SizeF smoothedSize = contactRecord.SmoothedSize;
          Microsoft.Surface.Core.SizeF sizeF = new Microsoft.Surface.Core.SizeF(smoothedSize.Width * 0.5f, smoothedSize.Height * 0.5f);
          Microsoft.Surface.Core.PointF topLeft = new Microsoft.Surface.Core.PointF(touchProperties.CenterPosition.X - sizeF.Width, touchProperties.CenterPosition.Y - sizeF.Height);
          touchProperties.Bounds = new TouchBounds(topLeft, smoothedSize);
        }
        else
          touchProperties.Bounds = new TouchBounds(new Microsoft.Surface.Core.PointF(touchProperties.CenterPosition.X - 10f, touchProperties.CenterPosition.Y - 10f), new Microsoft.Surface.Core.SizeF(20f, 20f));
        touchProperties.Orientation = 0.0f;
        touchProperties.MajorAxis = touchProperties.Bounds.Width;
        touchProperties.MinorAxis = touchProperties.Bounds.Height;
      }
      return touchProperties;
    }

    internal static float GetPixelAspectRatio(HidInputProvider.DeviceRecord deviceRecord)
    {
      return (deviceRecord.Orientation == 1 || deviceRecord.Orientation == 3 ? (float) deviceRecord.ScreenBounds.Height / (float) deviceRecord.ScreenBounds.Width : (float) deviceRecord.ScreenBounds.Width / (float) deviceRecord.ScreenBounds.Height) / ((float) (deviceRecord.Device.HidDevice.MaxX - deviceRecord.Device.HidDevice.MinX) / (float) (deviceRecord.Device.HidDevice.MaxY - deviceRecord.Device.HidDevice.MinY));
    }

    internal static Microsoft.Surface.Core.PointF DigitizerToClientCoordinates(
      HidInputProvider.DeviceRecord deviceRecord,
      IntPtr hwnd,
      int x,
      int y)
    {
      Microsoft.Surface.Core.PointF screenCoordinates = HidInputProvider.DigitizerToScreenCoordinates(deviceRecord, x, y);
      float x1 = screenCoordinates.X;
      float y1 = screenCoordinates.Y;
      try
      {
        Microsoft.Surface.NativeMethods.ScreenToClient(hwnd, ref x1, ref y1);
      }
      catch (InvalidOperationException ex)
      {
      }
      return new Microsoft.Surface.Core.PointF(x1, y1);
    }

    private static Microsoft.Surface.Core.PointF DigitizerToScreenCoordinates(
      HidInputProvider.DeviceRecord deviceRecord,
      int x,
      int y)
    {
      double num1 = (double) (x - deviceRecord.Device.HidDevice.MinX) / (double) (deviceRecord.Device.HidDevice.MaxX - deviceRecord.Device.HidDevice.MinX);
      double num2 = (double) (y - deviceRecord.Device.HidDevice.MinY) / (double) (deviceRecord.Device.HidDevice.MaxY - deviceRecord.Device.HidDevice.MinY);
      if (deviceRecord.Orientation == 1 || deviceRecord.Orientation == 3)
      {
        double num3 = num1;
        num1 = num2;
        num2 = num3;
      }
      float x1 = (float) num1 * (float) deviceRecord.ScreenBounds.Width;
      float y1 = (float) num2 * (float) deviceRecord.ScreenBounds.Height;
      switch ((int) deviceRecord.Orientation)
      {
        case 0:
          x1 += (float) deviceRecord.ScreenBounds.Left;
          y1 += (float) deviceRecord.ScreenBounds.Top;
          break;
        case 1:
          x1 = (float) deviceRecord.ScreenBounds.Right - x1;
          y1 += (float) deviceRecord.ScreenBounds.Top;
          break;
        case 2:
          x1 = (float) deviceRecord.ScreenBounds.Right - x1;
          y1 = (float) deviceRecord.ScreenBounds.Bottom - y1;
          break;
        case 3:
          x1 += (float) deviceRecord.ScreenBounds.Left;
          y1 = (float) deviceRecord.ScreenBounds.Bottom - y1;
          break;
      }
      return new Microsoft.Surface.Core.PointF(x1, y1);
    }

    internal static Microsoft.Surface.Core.SizeF DigitizerToScreenSpace(
      HidInputProvider.DeviceRecord deviceRecord,
      int magnitudeX,
      int magnitudeY)
    {
      return HidInputProvider.DigitizerToScreenSpace(deviceRecord, (float) magnitudeX, (float) magnitudeY);
    }

    internal static Microsoft.Surface.Core.SizeF DigitizerToScreenSpace(
      HidInputProvider.DeviceRecord deviceRecord,
      float magnitudeX,
      float magnitudeY)
    {
      double num1 = (double) magnitudeX / (double) (deviceRecord.Device.HidDevice.MaxX - deviceRecord.Device.HidDevice.MinX);
      double num2 = (double) magnitudeY / (double) (deviceRecord.Device.HidDevice.MaxY - deviceRecord.Device.HidDevice.MinY);
      if (deviceRecord.Orientation == 1 || deviceRecord.Orientation == 3)
      {
        double num3 = num1;
        num1 = num2;
        num2 = num3;
      }
      return new Microsoft.Surface.Core.SizeF((float) num1 * (float) deviceRecord.ScreenBounds.Width, (float) num2 * (float) deviceRecord.ScreenBounds.Height);
    }

    public override bool TrySetImageEnabled(ImageType type, bool enabled)
    {
      Monitor.Enter(this.imageSync);
      try
      {
        if (this.rawImageDevice == null || type != ImageType.Normalized)
          return false;
        if (enabled)
        {
          if (!this.rawImageDevice.IsOpen())
          {
            this.rawImageEnabled = this.rawImageDevice.Open(false);
            if (!this.rawImageEnabled)
              return false;
            this.rawImageReadThreadRunEvent.Set();
          }
        }
        else if (this.rawImageDevice.IsOpen())
        {
          this.rawImageReadThreadRunEvent.Reset();
          Monitor.Exit(this.imageSync);
          this.rawImageReadThreadPausedEvent.WaitOne();
          Monitor.Enter(this.imageSync);
          this.rawImageDevice.Close();
          this.rawImageEnabled = false;
        }
        return true;
      }
      finally
      {
        Monitor.Exit(this.imageSync);
      }
    }

    private void RawImageReadProc()
    {
      try
      {
        while (true)
        {
          do
          {
            this.rawImageReadThreadPausedEvent.Set();
            this.rawImageReadThreadRunEvent.WaitOne();
            this.rawImageReadThreadPausedEvent.Reset();
            if (!this.rawImageEnabled)
              goto label_12;
          }
          while (!Monitor.TryEnter(this.imageSync, 40));
          try
          {
            if (this.rawImageEnabled)
            {
              Dictionary<ImageType, RawImage> images = new Dictionary<ImageType, RawImage>();
              RawImage nextImage = this.rawImageDevice.GetNextImage();
              if (nextImage != null)
                images.Add(ImageType.Normalized, nextImage);
              FrameProperties properties = new FrameProperties(images, DateTime.Now.Ticks);
              lock (this.syncRoot)
                this.eventQueue.Enqueue(new RawInputEvent(SurfaceEventId.FrameReceived, IntPtr.Zero, (object) properties));
              this.dataReadyEvent.Set();
              continue;
            }
            continue;
          }
          finally
          {
            Monitor.Exit(this.imageSync);
          }
label_12:
          this.rawImageReadThreadRunEvent.Reset();
        }
      }
      catch (ThreadAbortException ex)
      {
        Thread.ResetAbort();
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (!this.disposed)
      {
        if (disposing)
        {
          this.notificationForm.Invoke((Delegate) (() => this.notificationForm.Close()));
          this.notificationForm.Dispose();
          this.notificationForm = (Form) null;
          this.notificationThread.Join();
          this.notificationThread = (Thread) null;
          this.hwndTarget = IntPtr.Zero;
          if (this.hook != IntPtr.Zero)
          {
            Microsoft.Surface.NativeMethods.UnhookWindowsHookEx(this.hook);
            this.hook = IntPtr.Zero;
          }
          foreach (HidInputProvider.DeviceRecord deviceRecord in this.deviceIdToDeviceRecord.Values)
          {
            IDigitizer device = deviceRecord.Device;
            device.Close();
            device.FrameReceivedEvent -= new EventHandler<FrameReceivedEventArgs<IDigitizerContact>>(this.OnFrameReceived);
            device.DeviceRemovedEvent -= new EventHandler(this.OnDeviceRemoved);
            deviceRecord.Dispose();
          }
          if (this.rawImageDevice != null)
          {
            this.rawImageReadThreadRunEvent.Reset();
            this.rawImageReadThreadPausedEvent.WaitOne();
            this.rawImageReadThread.Abort();
            this.rawImageDevice.Dispose();
          }
          this.rawImageReadThreadRunEvent.Dispose();
          this.rawImageReadThreadPausedEvent.Dispose();
          this.rawImageReadThreadResetTimer.Dispose();
        }
        this.disposed = true;
      }
      base.Dispose(disposing);
    }

    internal class DeviceRecord : IDisposable
    {
      private bool disposed;
      private TimerCallback callback;

      public int DeviceId { get; private set; }

      public IDigitizer Device { get; private set; }

      public System.Drawing.Rectangle ScreenBounds { get; internal set; }

      public DigitizerDisplaySupport.DisplayOrientation Orientation { get; internal set; }

      public Dictionary<uint, ContactRecord> TrackedContacts { get; private set; }

      public System.Threading.Timer OutOfRangeTimer { get; private set; }

      public DeviceRecord(
        int deviceId,
        IDigitizer device,
        System.Drawing.Rectangle screenBounds,
        DigitizerDisplaySupport.DisplayOrientation orientation,
        TimerCallback outOfRangeCallback)
      {
        this.DeviceId = deviceId;
        this.Device = device;
        this.ScreenBounds = screenBounds;
        this.Orientation = orientation;
        this.TrackedContacts = new Dictionary<uint, ContactRecord>();
        this.callback = outOfRangeCallback;
        if (SurfaceHidDevices.IsSurfaceVirtualDigitizerDevice(device.HidDevice))
          this.OutOfRangeTimer = new System.Threading.Timer(new TimerCallback(HidInputProvider.DeviceRecord.TimerPassThru), (object) this, -1, -1);
        else
          this.OutOfRangeTimer = new System.Threading.Timer(outOfRangeCallback, (object) this, -1, -1);
      }

      public static int GetDeviceContactId(int deviceId, uint contactId)
      {
        return deviceId << 24 | (int) contactId;
      }

      public static int GetDeviceIdFromContactId(uint contactId)
      {
        return ((int) contactId & -16777216) >> 24;
      }

      public static uint StripDeviceIdFromContactId(uint contactId) => contactId & 16777215U;

      private static void TimerPassThru(object stateInfo)
      {
        if (!(stateInfo is HidInputProvider.DeviceRecord state))
          return;
        if (Debugger.IsAttached)
        {
          state.OutOfRangeTimer.Change(333, -1);
        }
        else
        {
          if (state.callback == null)
            return;
          state.callback((object) state);
        }
      }

      private void Dispose(bool disposing)
      {
        if (this.disposed)
          return;
        if (disposing && this.OutOfRangeTimer != null)
          this.OutOfRangeTimer.Dispose();
        this.disposed = true;
      }

      public void Dispose()
      {
        this.Dispose(true);
        GC.SuppressFinalize((object) this);
      }

      ~DeviceRecord() => this.Dispose(false);
    }
  }
}
