using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemRezerwacji.Views;


namespace SystemRezerwacji.ViewModels
{
    public IRelayCommand EdytujWizyteCommand { get; }

    public WizytyViewModel()
    {
        EdytujWizyteCommand = new RelayCommand<int>(EdytujWizyte);
    }

    private void EdytujWizyte(int idWizyty)
    {
        var edytujViewModel = new EdytujWizyteViewModel(idWizyty);
        var okno = new EdytujWizyteView();
        okno.DataContext = edytujViewModel;
        okno.ShowDialog();

        LoadWizyty(); 
    }
}
