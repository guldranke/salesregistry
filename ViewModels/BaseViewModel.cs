using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sales.ViewModels;

/// <summary>
/// Abstract Class BaseViewModel is the base for all ViewModels
/// </summary>
public abstract class BaseViewModel : INotifyPropertyChanged {
    protected void OnPropertyChanged([CallerMemberName] string? name = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void Dispose() { }
}

