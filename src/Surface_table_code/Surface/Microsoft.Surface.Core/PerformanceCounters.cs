// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.PerformanceCounters
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using System;
using System.Diagnostics;
using System.Reflection;

#nullable disable
namespace Microsoft.Surface
{
  internal abstract class PerformanceCounters
  {
    private bool isEnabled;

    public bool IsEnabled => this.isEnabled;

    public abstract string CategoryName { get; }

    protected PerformanceCounters()
    {
      if (PerformanceCounterCategory.Exists(this.CategoryName))
      {
        AppDomain.CurrentDomain.DomainUnload += new EventHandler(this.ExitEventHandler);
        AppDomain.CurrentDomain.ProcessExit += new EventHandler(this.ExitEventHandler);
        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.ExceptionEventHandler);
        this.Initialize(PerformanceCounters.GetInstanceName());
        this.isEnabled = true;
      }
      else
        this.PerformanceCountersAreNotInstalled();
    }

    public virtual void Initialize(string instanceName)
    {
    }

    protected virtual void PerformanceCountersAreNotInstalled()
    {
    }

    public virtual void CannotInstantiatePerformanceCounter(string counterName, Exception e)
    {
    }

    public virtual void SkipInitialize()
    {
    }

    public virtual void Dispose()
    {
    }

    protected static string GetAssemblyName()
    {
      string assemblyName = (string) null;
      Assembly entryAssembly = Assembly.GetEntryAssembly();
      if (entryAssembly != (Assembly) null)
      {
        AssemblyName name = entryAssembly.GetName();
        if (name != null)
          assemblyName = name.Name;
      }
      return assemblyName;
    }

    protected static string GetInstanceName()
    {
      string str = PerformanceCounters.GetAssemblyName();
      if (string.IsNullOrEmpty(str))
        str = AppDomain.CurrentDomain.FriendlyName;
      return str.Replace('(', '[').Replace(')', ']').Replace('#', '_').Replace('/', '_').Replace('\\', '_');
    }

    public static void SafeDispose(PerformanceCounters.CounterBase counter) => counter?.Dispose();

    protected void ExceptionEventHandler(object sender, UnhandledExceptionEventArgs e)
    {
      if (e == null || !e.IsTerminating)
        return;
      this.Dispose();
    }

    protected void ExitEventHandler(object sender, EventArgs e) => this.Dispose();

    internal abstract class CounterBase
    {
      protected PerformanceCounter instance;

      protected CounterBase(
        PerformanceCounters parent,
        string categoryName,
        string counterName,
        string instanceName)
      {
        try
        {
          this.instance = new PerformanceCounter()
          {
            CategoryName = categoryName,
            CounterName = counterName,
            InstanceName = instanceName,
            InstanceLifetime = PerformanceCounterInstanceLifetime.Global,
            ReadOnly = false,
            RawValue = 0L
          };
        }
        catch (InvalidOperationException ex)
        {
          parent.CannotInstantiatePerformanceCounter(counterName, (Exception) ex);
        }
      }

      protected CounterBase()
      {
      }

      public void Dispose()
      {
        PerformanceCounter instance = this.instance;
        this.instance = (PerformanceCounter) null;
        instance?.RemoveInstance();
      }
    }

    internal class Counter : PerformanceCounters.CounterBase
    {
      public static readonly PerformanceCounters.Counter None = new PerformanceCounters.Counter();

      public Counter(
        PerformanceCounters parent,
        string categoryName,
        string counterName,
        string instanceName)
        : base(parent, categoryName, counterName, instanceName)
      {
      }

      public void Decrement() => this.instance?.Decrement();

      public void Increment() => this.instance?.Increment();

      public void IncrementBy(int value) => this.instance?.IncrementBy((long) value);

      public void Set(int value)
      {
        PerformanceCounter instance = this.instance;
        if (instance == null)
          return;
        instance.RawValue = (long) value;
      }

      private Counter()
      {
      }
    }

    internal class ElapsedTimeCounter : PerformanceCounters.CounterBase
    {
      public static readonly PerformanceCounters.ElapsedTimeCounter None = new PerformanceCounters.ElapsedTimeCounter();

      public ElapsedTimeCounter(
        PerformanceCounters parent,
        string categoryName,
        string counterName,
        string instanceName)
        : base(parent, categoryName, counterName, instanceName)
      {
      }

      public void Set(long timestamp)
      {
        PerformanceCounter instance = this.instance;
        if (instance == null)
          return;
        long num = 1000L * (Stopwatch.GetTimestamp() - timestamp) / Stopwatch.Frequency;
        instance.RawValue = num;
      }

      public void Clear()
      {
        PerformanceCounter instance = this.instance;
        if (instance == null)
          return;
        instance.RawValue = 0L;
      }

      private ElapsedTimeCounter()
      {
      }
    }
  }
}
