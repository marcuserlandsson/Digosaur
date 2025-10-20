// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.UserNotifications
// Assembly: Microsoft.Surface, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 3075CDD2-EF12-4C05-803F-ABBE1D5E74B0
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.dll

using System;

#nullable disable
namespace Microsoft.Surface
{
  public static class UserNotifications
  {
    private static Type internalUserNotificationsType;
    private static bool initialized;

    public static event EventHandler NotificationDisplayed
    {
      add
      {
        ReflectionUtilities.AddEventHandler(UserNotifications.InternalUserNotificationsType, "NotificationDisplayedEvent", (Delegate) value);
      }
      remove
      {
        ReflectionUtilities.RemoveEventHandler(UserNotifications.InternalUserNotificationsType, "NotificationDisplayedEvent", (Delegate) value);
      }
    }

    public static event EventHandler NotificationDismissed
    {
      add
      {
        ReflectionUtilities.AddEventHandler(UserNotifications.InternalUserNotificationsType, "NotificationDismissedEvent", (Delegate) value);
      }
      remove
      {
        ReflectionUtilities.RemoveEventHandler(UserNotifications.InternalUserNotificationsType, "NotificationDismissedEvent", (Delegate) value);
      }
    }

    public static void RequestNotification(string messageTitle, string messageText)
    {
      ReflectionUtilities.InvokeMethod(UserNotifications.InternalUserNotificationsType, nameof (RequestNotification), new Type[2]
      {
        typeof (string),
        typeof (string)
      }, new object[2]
      {
        (object) messageTitle,
        (object) messageText
      });
    }

    public static void RequestNotification(
      string messageTitle,
      string messageText,
      TimeSpan duration)
    {
      ReflectionUtilities.InvokeMethod(UserNotifications.InternalUserNotificationsType, nameof (RequestNotification), new Type[3]
      {
        typeof (string),
        typeof (string),
        typeof (TimeSpan)
      }, new object[3]
      {
        (object) messageTitle,
        (object) messageText,
        (object) duration
      });
    }

    public static void RequestNotification(
      string messageTitle,
      string messageText,
      string imagePath)
    {
      ReflectionUtilities.InvokeMethod(UserNotifications.InternalUserNotificationsType, nameof (RequestNotification), new Type[3]
      {
        typeof (string),
        typeof (string),
        typeof (string)
      }, new object[3]
      {
        (object) messageTitle,
        (object) messageText,
        (object) imagePath
      });
    }

    public static void RequestNotification(
      string messageTitle,
      string messageText,
      string imagePath,
      string applicationName)
    {
      ReflectionUtilities.InvokeMethod(UserNotifications.InternalUserNotificationsType, nameof (RequestNotification), new Type[4]
      {
        typeof (string),
        typeof (string),
        typeof (string),
        typeof (string)
      }, new object[4]
      {
        (object) messageTitle,
        (object) messageText,
        (object) imagePath,
        (object) applicationName
      });
    }

    public static void RequestNotification(
      string messageTitle,
      string messageText,
      string imagePath,
      TimeSpan duration)
    {
      ReflectionUtilities.InvokeMethod(UserNotifications.InternalUserNotificationsType, nameof (RequestNotification), new Type[4]
      {
        typeof (string),
        typeof (string),
        typeof (string),
        typeof (TimeSpan)
      }, new object[4]
      {
        (object) messageTitle,
        (object) messageText,
        (object) imagePath,
        (object) duration
      });
    }

    public static void RequestNotification(
      string messageTitle,
      string messageText,
      string imagePath,
      TimeSpan duration,
      string applicationName)
    {
      ReflectionUtilities.InvokeMethod(UserNotifications.InternalUserNotificationsType, nameof (RequestNotification), new Type[5]
      {
        typeof (string),
        typeof (string),
        typeof (string),
        typeof (TimeSpan),
        typeof (string)
      }, new object[5]
      {
        (object) messageTitle,
        (object) messageText,
        (object) imagePath,
        (object) duration,
        (object) applicationName
      });
    }

    private static string UserNotificationsName
    {
      get
      {
        return ReflectionUtilities.ConstructAssemblyQualifiedName("Microsoft.Surface.Shell.ShellApi", "Microsoft.Surface.Shell.InternalUserNotifications");
      }
    }

    private static Type InternalUserNotificationsType
    {
      get
      {
        if (!UserNotifications.initialized)
        {
          UserNotifications.initialized = true;
          UserNotifications.internalUserNotificationsType = Type.GetType(UserNotifications.UserNotificationsName, false, false);
        }
        return UserNotifications.internalUserNotificationsType;
      }
    }
  }
}
