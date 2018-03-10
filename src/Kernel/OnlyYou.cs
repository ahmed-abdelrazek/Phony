using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Phony.Kernel
{
    public static class OnlyYou
    {
        /// <summary>
        /// Processing single instance in <see cref="SingleInstanceModes"/> <see cref="SingleInstanceModes.ForEveryUser"/> mode.
        /// </summary>
        internal static void Make()
        {
            Make(SingleInstanceModes.ForEveryUser);
        }

        /// <summary>
        /// Processing single instance.
        /// </summary>
        /// <param name="singleInstanceModes"></param>
        internal static void Make(SingleInstanceModes singleInstanceModes)
        {
            var appName = Application.Current.GetType().Assembly.ManifestModule.ScopeName;

            var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var keyUserName = windowsIdentity != null ? windowsIdentity.User.ToString() : String.Empty;

            // Be careful! Max 260 chars!
            var eventWaitHandleName = string.Format("{0}{1}", appName, singleInstanceModes == SingleInstanceModes.ForEveryUser ? keyUserName : String.Empty);

            try
            {
                using (var eventWaitHandle = EventWaitHandle.OpenExisting(eventWaitHandleName))
                {
                    // It informs first instance about other startup attempting.
                    eventWaitHandle.Set();
                }
                // Let's terminate this posterior startup.
                // For that exit no interceptions.
                MessageBox.Show("البرنامج يعمل بالفعل");
                Environment.Exit(0);
            }
            catch
            {
                // It's first instance.

                // Register EventWaitHandle.
                using (var eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, eventWaitHandleName))
                {
                    ThreadPool.RegisterWaitForSingleObject(eventWaitHandle, OtherInstanceAttemptedToStart, null, Timeout.Infinite, false);
                }
                RemoveApplicationsStartupDeadlockForStartupCrushedWindows();
            }
        }

        private static void OtherInstanceAttemptedToStart(Object state, Boolean timedOut)
        {
            RemoveApplicationsStartupDeadlockForStartupCrushedWindows();
            Application.Current.Dispatcher.BeginInvoke(new Action(() => { try { Application.Current.MainWindow.Activate(); } catch { } }));
        }

        internal static DispatcherTimer AutoExitAplicationIfStartupDeadlock;

        public static void RemoveApplicationsStartupDeadlockForStartupCrushedWindows()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                AutoExitAplicationIfStartupDeadlock =
                    new DispatcherTimer(TimeSpan.FromSeconds(6), DispatcherPriority.ApplicationIdle, (o, args) =>
                    {
                        if (Application.Current.Windows.Cast<Window>().Count(window => !Double.IsNaN(window.Left)) == 0)
                        {
                                // For that exit no interceptions.
                                Environment.Exit(0);
                        }
                    }, Application.Current.Dispatcher
                    );
            }), DispatcherPriority.ApplicationIdle);
        }
    }

    public enum SingleInstanceModes
    {
        /// <summary>
        /// Do nothing.
        /// </summary>
        NotInited = 0,

        /// <summary>
        /// Every user can have own single instance.
        /// </summary>
        ForEveryUser,
    }
}