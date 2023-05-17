using System;
using System.Threading.Tasks;

namespace Sales.Commands;

/// <summary>
/// Abstract Class BaseCommand is the base for all asynchronous commands
/// </summary>
public abstract class BaseCommandAsync : BaseCommand {
    private bool _isExecuting;
    public bool IsExecuting {
        get => _isExecuting; set {
            _isExecuting = value;
            OnCanExecuteChanged();
        }
    }

    public override bool CanExecute(object? parameter) {
        return !IsExecuting && base.CanExecute(parameter);
    }

    public override async void Execute(object? parameter) {
        IsExecuting = true;

        try {
            await ExecuteAsync(parameter);
        } catch (Exception) { } finally {
            IsExecuting = false;
        }
    }

    public abstract Task ExecuteAsync(object? parameter);
}
