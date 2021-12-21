using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;

namespace WPFTesting.Commands
{
    public class RelayCommand : RelayCommand<object>
    {
        #region Constructors
        public RelayCommand(Action<object> execute) : base(execute)
        {
        }

        public RelayCommand(Action execute) : base(o => execute())
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute) : base(o => execute(), o => canExecute())
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute) : base(execute, canExecute.Invoke)
        {
        }

        public RelayCommand(Action<object> execute, IEnumerable<Predicate<object>> canExecutes) : base(execute, canExecutes)
        {
        }

        public RelayCommand(Action<object> execute, params Predicate<object>[] canExecutes) : base(execute, canExecutes)
        {
        }

        public RelayCommand(Action execute, params Predicate<object>[] canExecutes) : base(o => execute(), canExecutes)
        {
        }

        public RelayCommand(Action execute, params Func<bool>[] canExecutes) : base(o => execute(),
            canExecutes.Select(a => new Predicate<object>(o => a())))
        {
        }

        public RelayCommand(Action<object> execute, params Func<bool>[] canExecutes) : base(execute,
            canExecutes.Select(a => new Predicate<object>(o => a())))
        {
        }
        #endregion

        #region Public members
        public void Execute()
        {
            base.Execute(null);
        }
        #endregion
    }
    
    public class RelayCommand<T> : ICommand
    {
        #region Static Fields and Constants
        #endregion

        #region Fields
        private readonly List<Predicate<T>> _canExecutes = new List<Predicate<T>>();
        private readonly Dispatcher _dispatcher;
        private readonly Action<T> _execute;
        #endregion

        #region Events
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        #endregion

        #region Constructors
        public RelayCommand(Action<T> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute) : this(execute)
        {
            if (canExecute != null) _canExecutes.Add(canExecute.Invoke);
        }

        public RelayCommand(Action<T> execute, Func<bool> canExecute) : this(execute)
        {
            if (canExecute != null) _canExecutes.Add(a => canExecute.Invoke());
        }

        public RelayCommand(Action<T> execute, IEnumerable<Predicate<T>> canExecutes) : this(execute)
        {
            foreach (var canExecute in canExecutes)
                _canExecutes.Add(canExecute);
        }
        #endregion

        #region Interface Implementations
        bool ICommand.CanExecute(object parameter)
        {
            if (typeof(T).IsEnum && parameter == null)
            {
                //apparently CommandParameters aren't always filled even if they are defined in XAML and not-null in DataBinding, this is an issue for enums.
                return false;
            }
            return CanExecute((T)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute((T)parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            _dispatcher.InvokeAsync(CommandManager.InvalidateRequerySuggested);
        }
        #endregion

        #region Public members
        public bool CanExecute(T parameter)
        {
            return _canExecutes.All(canExecute => canExecute(parameter));
        }

        public bool CanExecute()
        {
            return _canExecutes.All(canExecute => canExecute(default));
        }

        public void Execute(T parameter)
        {
            var methodNames = "";
            _execute(parameter);
        }
        #endregion
    }
}
