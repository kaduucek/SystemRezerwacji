using System;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SystemRezerwacji.Models;
using SystemRezerwacji.Services;

namespace SystemRezerwacji.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthService _auth = new(new PrzychodniaContext());

        [ObservableProperty]
        private string _email = "";

        public IAsyncRelayCommand<string> LoginCommand { get; }
        public IRelayCommand PrzejdzDoRejestracjiCommand { get; }

        // Zdarzenia dla okna-hosta (nawigacja)
        public event Action? ZalogowanoPomyslnie;
        public event Action? ZadanieRejestracji;

        public LoginViewModel()
        {
            LoginCommand = new AsyncRelayCommand<string>(Zaloguj);
            PrzejdzDoRejestracjiCommand = new RelayCommand(() => ZadanieRejestracji?.Invoke());
        }

        private async Task Zaloguj(string? haslo)
        {
            var pacjent = await _auth.LoginAsync(Email, haslo ?? "");
            if (pacjent == null)
            {
                MessageBox.Show("Niepoprawny e-mail lub hasło.", "Błąd logowania",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SessionService.Zaloguj(pacjent.IdPacjenta, pacjent.Email);
            ZalogowanoPomyslnie?.Invoke();
        }
    }
}
