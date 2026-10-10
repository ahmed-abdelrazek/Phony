using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Phony.Tests
{
    internal static class UiThread
    {
        private static readonly Dispatcher Dispatcher;

        static UiThread()
        {
            var ready = new TaskCompletionSource<Dispatcher>();
            var thread = new Thread(() =>
            {
                var application = new Application();
                ready.SetResult(application.Dispatcher);
                System.Windows.Threading.Dispatcher.Run();
            })
            { IsBackground = true };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            Dispatcher = ready.Task.GetAwaiter().GetResult();
        }

        public static void Run(Action action) => Dispatcher.Invoke(action);
        public static Task RunAsync(Func<Task> action) => Dispatcher.Invoke(action);
    }
}
