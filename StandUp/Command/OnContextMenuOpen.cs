using System;
using System.Windows.Input;


namespace StandUP.Command
{
    public class OnContextMenuOpen : ICommand
    {
        public event EventHandler? ContextMenuOpen;
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            ContextMenuOpen?.Invoke(this, EventArgs.Empty);
        }
    }
}

