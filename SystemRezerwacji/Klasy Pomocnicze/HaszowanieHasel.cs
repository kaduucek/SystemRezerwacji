using BCrypt.Net;

namespace SystemRezerwacji
{
    public static class HaszowanieHasel
    {

        public static string GenerujHasz(string haslo)
        {
            return BCrypt.Net.BCrypt.HashPassword(haslo);
        }


        public static bool WeryfikujHaslo(string wpisaneHaslo, string haszZBase)
        {
            return BCrypt.Net.BCrypt.Verify(wpisaneHaslo, haszZBase);
        }
    }
}