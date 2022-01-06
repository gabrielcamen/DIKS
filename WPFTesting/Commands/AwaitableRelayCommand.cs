using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFTesting.Commands
{
    public class AwaitableRelayCommand : AwaitableRelayCommand<object>, IAsyncCommand
    {
        #region Constructors
        public AwaitableRelayCommand(Func<Task> executeMethod, bool disableCanExecuteWhileRunning = false)
            : base(o => executeMethod(), disableCanExecuteWhileRunning)
        {
        }
        
        
        public AwaitableRelayCommand(Func<Task> executeMethod, Func<bool> canExecuteMethod, bool disableCanExecuteWhileRunning = false)
            : base(o => executeMethod(), o => canExecuteMethod(), disableCanExecuteWhileRunning)
        {
        }

        /// <inheritdoc />
        public AwaitableRelayCommand(Func<object, Task> executeMethod, Func<bool> canExecuteMethod, bool disableCanExecuteWhileRunning = false)
            : base(executeMethod, o => canExecuteMethod(), disableCanExecuteWhileRunning)
        {
        }

        /// <inheritdoc />
        public AwaitableRelayCommand(Func<object, Task> executeMethod, Func<object, bool> canExecuteMethod,
            bool disableCanExecuteWhileRunning = false)
            : base(executeMethod, canExecuteMethod, disableCanExecuteWhileRunning)
        {
        }
        #endregion

        #region Interface Implementations
        public bool CanExecute()
        {
            return base.CanExecute();
        }

        public Task ExecuteAsync()
        {
            return base.ExecuteAsync(null);
        }
        #endregion
    }
    
    public class AwaitableRelayCommand<T> : IAsyncCommand<T>, ICommand
    {
        #region Static Fields and Constants
        #endregion

        #region Fields
        private readonly bool _enableCanExecuteWhileRunning;
        private readonly bool _enableLogging;
        private readonly Func<T, Task> _executeMethod;
        private readonly RelayCommand<T> _underlyingCommand;
        private bool _isExecuting;
        #endregion

        #region Properties and Indexers
        public ICommand Command => this;
        #endregion

        #region Events
        public event EventHandler CanExecuteChanged
        {
            add => _underlyingCommand.CanExecuteChanged += value;
            remove => _underlyingCommand.CanExecuteChanged -= value;
        }
        #endregion

        #region Constructors
        /// <inheritdoc />
        public AwaitableRelayCommand(Func<T, Task> executeMethod, bool disableCanExecuteWhileRunning = false, bool enableLogging = true)
            : this(executeMethod, _ => true, disableCanExecuteWhileRunning, enableLogging)
        {
        }

        /// <summary>
        ///     Creates a new <see cref="T:General.Core.Wpf.Commands.AwaitableRelayCommand`1" />
        /// </summary>
        /// <param name="executeMethod">Defines the method to be called when the command is invoked.</param>
        /// <param name="canExecuteMethod">Defines the method that determines whether the command can execute in its current state.</param>
        /// <param name="disableCanExecuteWhileRunning">
        ///     Whether or not to keep the command's CanExecute enabled while executing.
        ///     Defaults to false.
        /// </param>
        public AwaitableRelayCommand(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod, bool disableCanExecuteWhileRunning = false,
            bool enableLogging = true)
        {
            _executeMethod = executeMethod;
            _underlyingCommand = new RelayCommand<T>(x => { }, canExecuteMethod);
            _enableCanExecuteWhileRunning = disableCanExecuteWhileRunning;
            _enableLogging = enableLogging;
        }

        /// <inheritdoc />
        public AwaitableRelayCommand(Func<T, Task> executeMethod, Func<bool> canExecuteMethod, bool disableCanExecuteWhileRunning = false,
            bool enableLogging = true)
            : this(executeMethod, o => canExecuteMethod.Invoke(), disableCanExecuteWhileRunning, enableLogging)
        {
        }
        #endregion

        #region Interface Implementations
        /// <inheritdoc cref="ICommand.CanExecute" />
        public bool CanExecute(object parameter = null)
        {
            return !_isExecuting && _underlyingCommand.CanExecute((T)parameter);
        }

        /// <inheritdoc cref="ICommand.Execute" />
        public async void Execute(object parameter)
        {
            await ExecuteAsync((T)parameter);
        }

        public async Task ExecuteAsync(T obj)
        {
            try
            {
                if (_enableCanExecuteWhileRunning)
                {
                    _isExecuting = true;
                    RaiseCanExecuteChanged();
                }
                var methodNames = "";
                var isInfoEnabled = _enableLogging;
                if (isInfoEnabled)
                {
                    methodNames = _executeMethod.GetReflectedMethodNames();
                }
                await _executeMethod(obj);
            }
            finally
            {
                if (_enableCanExecuteWhileRunning)
                {
                    _isExecuting = false;
                    RaiseCanExecuteChanged();
                }
            }
        }

        public void RaiseCanExecuteChanged()
        {
            _underlyingCommand.RaiseCanExecuteChanged();
        }
        #endregion
    }
    
    public static class FuncExtensions
    {
        #region Public members
        public static string GetReflectedMethodNames<T>(this Func<T, Task> func)
        {
            return func?.Method.ReflectedType?.FullName == null
                ? ""
                : func.Method.GetReflectedMethodNames();
        }
        #endregion
    }
    
    public static class MethodInfoExtensions
    {
        #region Public members
        public static string GetReflectedMethodNames(this MethodInfo methodInfo)
        {
            if (methodInfo?.ReflectedType?.FullName == null) return "";
            var reflectedActionName = methodInfo.ReflectedType.FullName.Split('+')[0];

            reflectedActionName += ".";

            var t = methodInfo.Name.Split('<', '>');
            if (t.Length == 1)
            {
                reflectedActionName += t[0];
            }
            else
            {
                reflectedActionName += t[1];
            }
            return reflectedActionName;
        }
        #endregion
    }
    
    //source: http://jake.ginnivan.net/awaitable-delegatecommand/
    public interface IAsyncCommand : IAsyncCommand<object>
    {
        #region Public members
        bool CanExecute();
        Task ExecuteAsync();
        #endregion
    }

    public interface IAsyncCommand<in T> : IRaiseCanExecuteChanged
    {
        #region Properties and Indexers
        ICommand Command { get; }
        #endregion

        #region Public members
        bool CanExecute(object obj);
        Task ExecuteAsync(T obj);
        #endregion
    }
    
    public interface IRaiseCanExecuteChanged
    {
        #region Public members
        void RaiseCanExecuteChanged();
        #endregion
    }
}
