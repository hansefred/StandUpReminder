using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.IO.Packaging;
using System.Media;
using System.Reflection;
using System.Windows;
using Microsoft.Extensions.Configuration;
using System.Timers;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Toolkit.Uwp.Notifications;
using StandUP;
using Timer = System.Threading.Timer; // Alias for the Timer class

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IConfigurationSection Configuration { get; set; } = new ConfigurationBuilder().AddJsonFile("App.json").Build().GetSection("TimerSetting");

        private BackgroundWorker Worker { get; set; } = new BackgroundWorker();

        private bool IsSitting { get; set; } = true;

        private string RedIcon { get; set; } = "Resources\\RedIcon.png";
        private string GreenIcon { get; set; } = "Resources\\GreenIcon.png";

        private string Beep { get; set; } = "Resources\\beep.wav";

        private TaskbarIcon? Icon { get; set; }
        public PausableTimer? Timer { get; set; }

        private App()
        {
            Worker.WorkerSupportsCancellation = true;
            Worker.DoWork += Worker_DoWork;
            Worker.RunWorkerAsync();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var resource = FindResource("Icon");
            if (resource is TaskbarIcon tb)
            {
                Icon = tb;
            }
            base.OnStartup(e);
        }


        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            var sitting = TimeSpan.Parse(Configuration["IntervalSitting"] ?? "00:25:00"); // Default to 25 minutes
            var standing = TimeSpan.Parse(Configuration["IntervalStanding"] ?? "00:05:00"); // Default to 5 minutes


            using var player = new SoundPlayer(Beep);

            var redIconFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, RedIcon);
            var greenIconFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GreenIcon);

            // Initialize the timer
            Timer = new PausableTimer(sitting.TotalMilliseconds);

            Timer.Elapsed += OnTimerElapsed;
            Timer.AutoReset = true; // Allow the timer to reset automatically
            Timer.Start();


            // Run loop until cancellation is requested
            while (!Worker.CancellationPending)
            {
                System.Threading.Thread.Sleep(100); // Avoid busy-waiting
            }

            // Cleanup
            Timer.Stop();

            // Event handler
            void OnTimerElapsed(object? timerSender, ElapsedEventArgs eventArgs)
            {
                if (IsSitting)
                {
                    player.Play();
                    ShowNotification("Zeit aufzustehen", redIconFullPath);
                    Timer.Interval = standing.TotalMilliseconds; // Switch to standing interval
                }
                else
                {
                    player.Play();
                    ShowNotification("Genug gestanden", greenIconFullPath);
                    Timer.Interval = sitting.TotalMilliseconds; // Switch to sitting interval
                }

                IsSitting = !IsSitting; // Toggle sitting/standing state
            }

            void ShowNotification(string message = "", string icon = "")
            {
                // Requires Microsoft.Toolkit.Uwp.Notifications NuGet package version 7.0 or greater
                new ToastContentBuilder()
                    .AddText(message)
                    .AddAppLogoOverride(new Uri(icon), ToastGenericAppLogoCrop.Circle)
                    .SetToastDuration(ToastDuration.Short)
                    .Show();
            }

        }




    }
}
