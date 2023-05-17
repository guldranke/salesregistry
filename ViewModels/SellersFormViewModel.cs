using Sales.Models;

namespace Sales.ViewModels;

/// <summary>
/// Class SellersFormViewModel models the form for <see cref="Views.Sellers.Sellers"/>
/// </summary>
public class SellersFormViewModel : BaseViewModel {
    private bool isTemporary;
    public bool IsTemporary { 
        get => isTemporary; set {
            isTemporary = value;
            OnPropertyChanged();
        } 
    }


    private int salesManId;
    public int SalesManId {
        get => salesManId; set {
            salesManId = value;
            OnPropertyChanged();
        }
    }

    private string firstname = string.Empty;
    public string Firstname {
        get => firstname; set {
            firstname = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanSubmit));
        }
    }


    private string lastname = string.Empty;
    public string Lastname {
        get => lastname; set {
            lastname = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanSubmit));
        }
    }

    public bool CanSubmit => !string.IsNullOrEmpty(firstname) && !string.IsNullOrEmpty(lastname);

    public SellersFormViewModel(SalesMan salesMan) {
        SalesManId = salesMan.SalesManId;
        Firstname = salesMan.Firstname;
        Lastname = salesMan.Lastname;
        IsTemporary =  salesMan.IsTemporary;
    }
}
