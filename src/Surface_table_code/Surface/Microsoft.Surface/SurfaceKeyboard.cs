// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.SurfaceKeyboard
// Assembly: Microsoft.Surface, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 3075CDD2-EF12-4C05-803F-ABBE1D5E74B0
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.dll

using System;

#nullable disable
namespace Microsoft.Surface
{
  public static class SurfaceKeyboard
  {
    private static Type internalSurfaceKeyboardType;
    private static bool initialized;

    public static event EventHandler Shown
    {
      add
      {
        ReflectionUtilities.AddEventHandler(SurfaceKeyboard.InternalSurfaceKeyboardType, "ShownEvent", (Delegate) value);
      }
      remove
      {
        ReflectionUtilities.RemoveEventHandler(SurfaceKeyboard.InternalSurfaceKeyboardType, "ShownEvent", (Delegate) value);
      }
    }

    public static event EventHandler Hidden
    {
      add
      {
        ReflectionUtilities.AddEventHandler(SurfaceKeyboard.InternalSurfaceKeyboardType, "HiddenEvent", (Delegate) value);
      }
      remove
      {
        ReflectionUtilities.RemoveEventHandler(SurfaceKeyboard.InternalSurfaceKeyboardType, "HiddenEvent", (Delegate) value);
      }
    }

    public static bool IsVisible
    {
      get
      {
        object obj;
        return ReflectionUtilities.TryGetProperty(SurfaceKeyboard.InternalSurfaceKeyboardType, nameof (IsVisible), out obj) && (bool) obj;
      }
      set
      {
        ReflectionUtilities.TrySetProperty(SurfaceKeyboard.InternalSurfaceKeyboardType, nameof (IsVisible), (object) value);
      }
    }

    public static float Rotation
    {
      get
      {
        object obj;
        return ReflectionUtilities.TryGetProperty(SurfaceKeyboard.InternalSurfaceKeyboardType, nameof (Rotation), out obj) ? (float) obj : 0.0f;
      }
      set
      {
        ReflectionUtilities.TrySetProperty(SurfaceKeyboard.InternalSurfaceKeyboardType, nameof (Rotation), (object) value);
      }
    }

    public static float CenterX
    {
      get
      {
        object obj;
        return ReflectionUtilities.TryGetProperty(SurfaceKeyboard.InternalSurfaceKeyboardType, nameof (CenterX), out obj) ? (float) obj : 0.0f;
      }
      set
      {
        ReflectionUtilities.TrySetProperty(SurfaceKeyboard.InternalSurfaceKeyboardType, nameof (CenterX), (object) value);
      }
    }

    public static float CenterY
    {
      get
      {
        object obj;
        return ReflectionUtilities.TryGetProperty(SurfaceKeyboard.InternalSurfaceKeyboardType, nameof (CenterY), out obj) ? (float) obj : 0.0f;
      }
      set
      {
        ReflectionUtilities.TrySetProperty(SurfaceKeyboard.InternalSurfaceKeyboardType, nameof (CenterY), (object) value);
      }
    }

    public static float Height
    {
      get
      {
        object obj;
        return ReflectionUtilities.TryGetProperty(SurfaceKeyboard.InternalSurfaceKeyboardType, nameof (Height), out obj) ? (float) obj : 0.0f;
      }
    }

    public static float Width
    {
      get
      {
        object obj;
        return ReflectionUtilities.TryGetProperty(SurfaceKeyboard.InternalSurfaceKeyboardType, nameof (Width), out obj) ? (float) obj : 0.0f;
      }
    }

    public static KeyboardLayout Layout
    {
      get
      {
        object obj;
        return ReflectionUtilities.TryGetProperty(SurfaceKeyboard.InternalSurfaceKeyboardType, nameof (Layout), out obj) ? (KeyboardLayout) obj : KeyboardLayout.Alphanumeric;
      }
      set
      {
        ReflectionUtilities.TrySetProperty(SurfaceKeyboard.InternalSurfaceKeyboardType, nameof (Layout), (object) value);
      }
    }

    public static bool ShowsFeedback
    {
      get
      {
        object obj;
        return !ReflectionUtilities.TryGetProperty(SurfaceKeyboard.InternalSurfaceKeyboardType, nameof (ShowsFeedback), out obj) || (bool) obj;
      }
      set
      {
        ReflectionUtilities.TrySetProperty(SurfaceKeyboard.InternalSurfaceKeyboardType, nameof (ShowsFeedback), (object) value);
      }
    }

    public static bool IsNativeInputAllowed
    {
      get
      {
        object obj;
        return ReflectionUtilities.TryGetProperty(SurfaceKeyboard.InternalSurfaceKeyboardType, nameof (IsNativeInputAllowed), out obj) && (bool) obj;
      }
      set
      {
        ReflectionUtilities.TrySetProperty(SurfaceKeyboard.InternalSurfaceKeyboardType, nameof (IsNativeInputAllowed), (object) value);
      }
    }

    public static void SuppressTextInputPanel(IntPtr hwnd)
    {
      ReflectionUtilities.InvokeMethod(SurfaceKeyboard.InternalSurfaceKeyboardType, nameof (SuppressTextInputPanel), new Type[1]
      {
        typeof (IntPtr)
      }, new object[1]{ (object) hwnd });
    }

    private static string SurfaceKeyboardName
    {
      get
      {
        return ReflectionUtilities.ConstructAssemblyQualifiedName("Microsoft.Surface.Shell.ShellApi", "Microsoft.Surface.Shell.InternalSurfaceKeyboard");
      }
    }

    private static Type InternalSurfaceKeyboardType
    {
      get
      {
        if (!SurfaceKeyboard.initialized)
        {
          SurfaceKeyboard.initialized = true;
          SurfaceKeyboard.internalSurfaceKeyboardType = Type.GetType(SurfaceKeyboard.SurfaceKeyboardName, false, false);
        }
        return SurfaceKeyboard.internalSurfaceKeyboardType;
      }
    }
  }
}
