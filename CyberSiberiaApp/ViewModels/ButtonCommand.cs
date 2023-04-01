using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CyberSiberiaApp.ViewModels
{
    public class ButtonCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public Action _action;
        public Func<bool> _canExecute;

        public ButtonCommand(Action action, Func<bool> canExecute=null)
        {
            _action = action;
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
