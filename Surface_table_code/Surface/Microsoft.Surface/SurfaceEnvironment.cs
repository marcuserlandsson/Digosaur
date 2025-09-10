// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.SurfaceEnvironment
// Assembly: Microsoft.Surface, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 3075CDD2-EF12-4C05-803F-ABBE1D5E74B0
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.dll

using Microsoft.Win32;
using System;
using System.Threading;

#nullable disable
namespace Microsoft.Surface
{
  public static class SurfaceEnvironment
  {
    private const string SurfaceSDKKeyPath = "SOFTWARE\\Microsoft\\Surface\\v2.0\\SDK";
    private const string ForceSurfaceEnvironmentValueNameKey = "ForceSurfaceEnvironment";
    private const string ShellMutexName = "Microsoft.Surface.Shell.UI.MainApplication";
    private const string LoaderMutexName = "Microsoft.Surface.ShellInit.Loader";
    private static bool? isSurfaceInputAvailable;
    private static bool? isSurfaceEnvironmentAvailable;
    private static Type interactiveSurfaceType;
    private static bool istInitialized;

    public static bool IsSurfaceInputAvailable
    {
      get
      {
        if (!SurfaceEnvironment.isSurfaceInputAvailable.HasValue)
          SurfaceEnvironment.isSurfaceInputAvailable = new bool?(SurfaceEnvironment.InteractiveSurfaceType != (Type) null);
        return SurfaceEnvironment.isSurfaceInputAvailable.Value;
      }
    }

    public static bool IsSurfaceEnvironmentAvailable
    {
      get
      {
        if (!SurfaceEnvironment.isSurfaceEnvironmentAvailable.HasValue)
          SurfaceEnvironment.isSurfaceEnvironmentAvailable = new bool?(SurfaceEnvironment.ShouldForceSurfaceEnvironment || SurfaceEnvironment.DoesMutexExist("Microsoft.Surface.ShellInit.Loader") || SurfaceEnvironment.DoesMutexExist("Microsoft.Surface.Shell.UI.MainApplication"));
        return SurfaceEnvironment.isSurfaceEnvironmentAvailable.Value;
      }
    }

    private static bool ShouldForceSurfaceEnvironment
    {
      get
      {
        object obj = (object) null;
        using (RegistryKey registryKey1 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
        {
          if (registryKey1 != null)
          {
            using (RegistryKey registryKey2 = registryKey1.OpenSubKey("SOFTWARE\\Microsoft\\Surface\\v2.0\\SDK"))
            {
              if (registryKey2 != null)
                obj = registryKey2.GetValue("ForceSurfaceEnvironment");
            }
          }
        }
        return obj != null && obj is int num && num != 0;
      }
    }

    private static bool DoesMutexExist(string mutexName)
    {
      Mutex mutex = (Mutex) null;
      try
      {
        mutex = Mutex.OpenExisting(mutexName);
        return true;
      }
      catch (WaitHandleCannotBeOpenedException ex)
      {
        return false;
      }
      catch (UnauthorizedAccessException ex)
      {
        return true;
      }
      finally
      {
        mutex?.Dispose();
      }
    }

    private static string InteractiveSurfaceName
    {
      get
      {
        return ReflectionUtilities.ConstructAssemblyQualifiedName("Microsoft.Surface.Core", "Microsoft.Surface.Core.InteractiveSurface");
      }
    }

    private static Type InteractiveSurfaceType
    {
      get
      {
        if (!SurfaceEnvironment.istInitialized)
        {
          SurfaceEnvironment.istInitialized = true;
          SurfaceEnvironment.interactiveSurfaceType = Type.GetType(SurfaceEnvironment.InteractiveSurfaceName, false, false);
        }
        return SurfaceEnvironment.interactiveSurfaceType;
      }
    }
  }
}
