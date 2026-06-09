using System;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SystemRezerwacji.Models;
using SystemRezerwacji.Services;

namespace SystemRezerwacji.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly AuthService _auth = new(new PrzychodniaContext());

        [ObservableProperty] private string _imie = "";
        [ObservableProperty] private string _nazwisko = "";
        [ObservableProperty] private string _email = "";

        public IAsyncRelayCommand<string> ZarejestrujCommand { get; }
        public IRelayCommand PowrotCommand { get; }

        public event Action? Zarejestrowano;
        public event Action? Powrot;

        public RegisterViewModel()
        {
            ZarejestrujCommand = new AsyncRelayCommand<string>(Zarejestruj);
            PowrotCommand = new RelayCommand(() => Powrot?.Invoke());
        }

        private async Task Zarejestruj(string? haslo)
        {
            if (string.IsNullOrWhiteSpace(Imie) || string.IsNullOrWhiteSpace(Nazwisko)
                || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(haslo))
            {
                MessageBox.Show("Wypełnij wszystkie pola.", "Uwaga",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (haslo!.Length < 6)
            {
                MessageBox.Show("Hasło musi mieć co najmniej 6 znaków.", "Uwaga",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool ok = await _auth.RegisterPatientAsync(Email, haslo, Imie, Nazwisko);
            if (!ok)
            {
                MessageBox.Show("Nie udało się zarejestrować - taki e-mail może już istnieć.", "Błąd",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show("Konto utworzone. Możesz się zalogować.", "Sukces",
                MessageBoxButton.OK, MessageBoxImage.Information);
            Zarejestrowano?.Invoke();
        }
    }
}
