using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SystemRezerwacji.Models;
using SystemRezerwacji.Services;
using SystemRezerwacji.Views;

namespace SystemRezerwacji.ViewModels
{
    public partial class WizytyViewModel : ObservableObject
    {
        private readonly AppointmentService _service = new();

        [ObservableProperty]
        private ObservableCollection<Wizyta> _wizyty = new();

        public IAsyncRelayCommand OdswiezCommand { get; }
        public IAsyncRelayCommand<int> EdytujWizyteCommand { get; }
        public IAsyncRelayCommand<int> AnulujWizyteCommand { get; }

        public WizytyViewModel()
        {
            OdswiezCommand = new AsyncRelayCommand(LoadWizyty);
            EdytujWizyteCommand = new AsyncRelayCommand<int>(EdytujWizyte);
            AnulujWizyteCommand = new AsyncRelayCommand<int>(AnulujWizyte);
            _ = LoadWizyty();
        }

        private async Task LoadWizyty()
        {
            var dane = await _service.PobierzWizytyPacjentaAsync(SessionService.IdZalogowanegoPacjenta);
            Wizyty = new ObservableCollection<Wizyta>(dane);
        }

        private async Task EdytujWizyte(int idWizyty)
        {
            var okno = new EdytujWizyteView
            {
                DataContext = new EdytujWizyteViewModel(idWizyty)
            };
            okno.ShowDialog();
            await LoadWizyty();
        }

        private async Task AnulujWizyte(int idWizyty)
        {
            var odp = MessageBox.Show("Czy na pewno anulować tę wizytę?", "Potwierdzenie",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (odp != MessageBoxResult.Yes)
                return;

            bool ok = await _service.AnulujAsync(idWizyty);
            if (!ok)
            {
                MessageBox.Show("Nie można anulować tej wizyty (np. mniej niż 24h do terminu lub już anulowana).",
                    "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            await LoadWizyty();
        }
    }
}
