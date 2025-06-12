using SmartCarViewModel.interfaces;
using System.Windows.Input;

namespace SmartCarViewModel.utils
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;
        private readonly INotifyCommand? _notifyCommand;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null, INotifyCommand? command = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
            _notifyCommand = command;
        }

        public event EventHandler CanExecuteChanged
        {
            add 
            {
                _notifyCommand?.Register(value);
            }
            remove 
            { 
                _notifyCommand?.Unregister(value);
            }
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);

        public void RaiseCanExecuteChanged()
        {
            _notifyCommand?.InvalidateRequerySuggested();
        }
    }

    public class RelayCommandAsync : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Predicate<object> _canExecute;
        private readonly INotifyCommand? _notifyCommand;

        public RelayCommandAsync(Func<Task> execute, Predicate<object> canExecute = null, INotifyCommand? command = null)
        {
            ArgumentNullException.ThrowIfNull(execute);
            _execute = execute;
            _canExecute = canExecute;
            _notifyCommand = command;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                _notifyCommand?.Register(value);
            }
            remove
            {
                _notifyCommand?.Unregister(value);
            }
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object? parameter) => _execute.Invoke();

        public void RaiseCanExecuteChanged()
        {
            _notifyCommand?.InvalidateRequerySuggested();
        }
    }

    public class RelayCommandAsync<T> : ICommand
    {
        private readonly Func<T?, Task> _execute;
        private readonly Predicate<T> _canExecute;
        private readonly INotifyCommand? _notifyCommand;

        public RelayCommandAsync(Func<T?, Task> execute, Predicate<T> canExecute = null, INotifyCommand? command = null)
        {
            ArgumentNullException.ThrowIfNull(execute);
            _execute = execute;
            _canExecute = canExecute;
            _notifyCommand = command;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                _notifyCommand?.Register(value);
            }
            remove
            {
                _notifyCommand?.Unregister(value);
            }
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute((T)parameter);
        public void Execute(object? parameter) => _execute.Invoke((T?)parameter);

        public void RaiseCanExecuteChanged()
        {
            _notifyCommand?.InvalidateRequerySuggested();
        }
    }
}
