using BCrypt.Net;

namespace SystemRezerwacji
{
    public static class HaszowanieHasel
    {
        // Wywołasz to podczas rejestracji pacjenta
        public static string GenerujHasz(string haslo)
        {
            return BCrypt.Net.BCrypt.HashPassword(haslo);
        }

        // Wywołasz to podczas logowania
        public static bool WeryfikujHaslo(string wpisaneHaslo, string haszZBase)
        {
            return BCrypt.Net.BCrypt.Verify(wpisaneHaslo, haszZBase);
        }
    }
}