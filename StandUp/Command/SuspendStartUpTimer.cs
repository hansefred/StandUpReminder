using System.Windows;
using System.Windows.Input;
using WpfApp1;

namespace StandUP.Command
{
    class SuspendStartUpTimerCommand : ICommand
    {
        public bool CanExecute(object? parameter)
        {

            if (Application.Current is not null)
            {
                if (Application.Current is App)
                {
                    return true;
                }
            }

            return false;
        }

        public void Execute(object? parameter)
        {
            if (Application.Current is not null)
            {
                if (Application.Current is App app)
                {
                    if (app.Timer is not null)
                    {
                        if (!app.Timer.IsPaused)
                        {
                            app.Timer.Pause();
                        }
                        else
                        {
                            app.Timer.Resume();
                        }
                    }
                }
            }
        }

        public event EventHandler? CanExecuteChanged;
    }
}
