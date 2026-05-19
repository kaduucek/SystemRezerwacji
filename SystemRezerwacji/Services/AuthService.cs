using System;
using System.Threading.Tasks;

namespace SystemRezerwacji.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterPatientAsync(string email, string password, string firstName, string lastName);
        Task<bool> LoginAsync(string email, string password);
    }

    public class AuthService : IAuthService
    {
        public async Task<bool> RegisterPatientAsync(string email, string password, string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return false;

            // Tutaj w przyszłości pojawi się hashowanie z zadania Krystiana (T-05)
            // oraz zapis do bazy danych przy użyciu Twoich modeli z folderu Models/
            return true; 
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return false;

            return true;
        }
    }
}
