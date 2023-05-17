using Sales.Models;

namespace Sales.ViewModels;

/// <summary>
/// Class SalesManViewModel is the ViewModel for a <see cref="SalesMan"/>
/// </summary>
public class SalesManViewModel : BaseViewModel {
    public SalesMan SalesMan { get; private set; }

    public int SalesManId => SalesMan.SalesManId;
    public string Firstname => SalesMan.Firstname;
    public string Lastname => SalesMan.Lastname;
    public string Fullname => SalesMan.Fullname;

    public SalesManViewModel(SalesMan salesMan) {
        SalesMan = salesMan;
    }

    public void Update(SalesMan salesMan) {
        SalesMan = salesMan;

        OnPropertyChanged(nameof(SalesMan));
    }
}
