using System;
using System.Windows.Input;

namespace ZdravoCorp.Core.Commands;

public  class DelegateCommand : ICommand
{

    private Action<object> executionAction;

    private Predicate<object> canExecutePredicate;

    /// <param name="execute">The delegate to call on execution</param>
    public DelegateCommand(Action<object> execute)
        : this(execute, null)
    {
    }

    /// <param name="execute">The delegate to call on execution</param>
    /// <param name="canExecute">The predicate to determine if command is valid for execution</param>
    public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
    {
        if (execute == null)
        {
            throw new ArgumentNullException("execute");
        }

        this.executionAction = execute;
        this.canExecutePredicate = canExecute;
    }


    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    /// <param name="parameter">parameter to pass to predicate</param>
    public bool CanExecute(object parameter)
    {
        return this.canExecutePredicate == null ? true : this.canExecutePredicate(parameter);
    }


    /// <param name="parameter">parameter to pass to delegate</param>
    /// <exception cref="InvalidOperationException">Thrown if CanExecute returns false</exception>
    public void Execute(object parameter)
    {
        if (!this.CanExecute(parameter))
        {
            throw new InvalidOperationException("The command is not valid for execution, check the CanExecute method before attempting to execute.");
        }
        this.executionAction(parameter);
    }
}