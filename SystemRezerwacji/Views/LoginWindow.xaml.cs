using System.Windows;
using SystemRezerwacji.ViewModels;

namespace SystemRezerwacji.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            PokazLogowanie();
        }

        private void PokazLogowanie()
        {
            var vm = new LoginViewModel();
            vm.ZalogowanoPomyslnie += () =>
            {
                new MainWindow().Show();
                Close();
            };
            vm.ZadanieRejestracji += PokazRejestracje;

            Host.Content = new LoginView { DataContext = vm };
        }

        private void PokazRejestracje()
        {
            var vm = new RegisterViewModel();
            vm.Zarejestrowano += PokazLogowanie;
            vm.Powrot += PokazLogowanie;

            Host.Content = new RegisterView { DataContext = vm };
        }
    }
}
