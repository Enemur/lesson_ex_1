using System;
using System.Windows.Input;

namespace ex_1
{
    public class RelayCommand : ICommand
    {
        private Action<object> _func;
        private Func<object, bool> _canExecute;

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
                return _canExecute.Invoke(parameter);
            return true;
        }

        public void Execute(object parameter)
        {
            _func.Invoke(parameter);
        }

        public RelayCommand(Action<object> func, Func<object, bool> canExecute = null)
        {
            _func = func;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;
    }
}