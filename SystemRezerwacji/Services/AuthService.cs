using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SystemRezerwacji.Models; // Dodane, ponieważ PrzychodniaContext i Pacjent są w folderze Models

namespace SystemRezerwacji.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterPatientAsync(string email, string password, string firstName, string lastName);
        Task<bool> LoginAsync(string email, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly PrzychodniaContext _context;

        // Wstrzykujemy Twój właściwy kontekst bazy danych przez konstruktor
        public AuthService(PrzychodniaContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterPatientAsync(string email, string password, string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return false;

            // Sprawdzamy w bazie Przychodnia, czy pacjent o tym emailu już istnieje
            bool emailExist = await _context.Pacjenci.AnyAsync(p => p.Email == email);
            if (emailExist)
                return false; 

            // ZADANIE T-05: Hashowanie hasła za pomocą Twojej klasy statycznej
            string zahasowaneHaslo = HaszowanieHasel.GenerujHasz(password);

            // Tworzymy nowego pacjenta i przypisujemy mu bezpieczny hash zamiast czystego tekstu
            var nowyPacjent = new Pacjent
            {
                Email = email,
                Imię = firstName,        // Jeżeli w Twoim modelu jest "Imie" bez kreski, usuń kreskę
                Nazwisko = lastName,
                HaszHasła = zahasowaneHaslo, // Zapisujemy wygenerowany HASH
                DataRejestracji = DateTime.Now
            };

            // Zapis do bazy danych
            _context.Pacjenci.Add(nowyPacjent);
            await _context.SaveChangesAsync();

            return true; 
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return false;

            // Pobranie pacjenta z bazy Przychodnia na podstawie wpisanego emailu
            var pacjent = await _context.Pacjenci.SingleOrDefaultAsync(p => p.Email == email);
            
            if (pacjent == null)
                return false; // Pacjent o takim emailu nie istnieje

            // ZADANIE T-05: Weryfikacja czy wpisane hasło pasuje do hasha z bazy danych
            bool czyHasloPoprawne = HaszowanieHasel.WeryfikujHaslo(password, pacjent.HaszHasła);

            return czyHasloPoprawne;
        }
    }
}
