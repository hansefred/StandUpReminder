using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace StandUP.Command
{
    class OpenFileCommand : ICommand
    {
        public string? FilePath { get; set; }

        public bool CanExecute(object? parameter)
        {
            if (parameter is not string path) return false;
            if (!File.Exists(path))
            {
                return false;
            }

            FilePath = path;
            return true;

        }

        public void Execute(object? parameter)
        {
            Process.Start("explorer.exe", FilePath??"");
        }

        public event EventHandler? CanExecuteChanged;
    }
}
