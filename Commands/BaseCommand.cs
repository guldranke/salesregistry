using System;
using System.Windows.Input;

namespace Sales.Commands;

/// <summary>
/// Abstract Class BaseCommand is the base for all synchronous commands
/// </summary>
public abstract class BaseCommand : ICommand {
    public event EventHandler? CanExecuteChanged;

    public virtual bool CanExecute(object? parameter) {
        return true;
    }

    public abstract void Execute(object? parameter);

    protected void OnCanExecuteChanged() {
        CanExecuteChanged?.Invoke(this, new EventArgs());
    }
}
