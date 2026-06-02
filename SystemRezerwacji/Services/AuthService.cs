using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SystemRezerwacji.Models; 

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

        public AuthService(PrzychodniaContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterPatientAsync(string email, string password, string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return false;

            bool emailExist = await _context.Pacjenci.AnyAsync(p => p.Email == email);
            if (emailExist)
                return false; 

            string zahasowaneHaslo = HaszowanieHasel.GenerujHasz(password);

            var nowyPacjent = new Pacjent
            {
                Email = email,
                Imie = firstName,       
                Nazwisko = lastName,
                Hasz = zahasowaneHaslo, 
                DataRejestracji = DateTime.Now
            };

            _context.Pacjenci.Add(nowyPacjent);
            await _context.SaveChangesAsync();

            return true; 
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return false;

                    var pacjent = await _context.Pacjenci
                            .AsNoTracking() 
                            .SingleOrDefaultAsync(p => p.Email == email);
            
            if (pacjent == null)
                return false; 

            bool czyHasloPoprawne = HaszowanieHasel.WeryfikujHaslo(password, pacjent.Hasz);

            return czyHasloPoprawne;
        }
    }
}
