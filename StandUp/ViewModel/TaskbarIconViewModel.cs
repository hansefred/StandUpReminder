using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using StandUP.Command;
using WpfApp1;

namespace StandUP.ViewModel
{
    public class TaskbarIconViewModel : INotifyPropertyChanged
    {
        private Icon _taskbarIcon;
        public ICommand? OpenSetup { get; set; } = new OpenFileCommand();

        public ICommand SuspendTimerCommand { get; set; } = new SuspendStartUpTimerCommand();

        public App? App { get; set; }

        public TaskbarIconViewModel()
        {
            if (Application.Current is not null)
            {
                if (Application.Current is App app)
                {
                    App = app;
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
