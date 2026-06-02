namespace SystemRezerwacji.Services
{
    public static class SessionService
    {
        public static int IdZalogowanegoPacjenta { get; private set; }
        public static string Email { get; private set; }
        public static bool CzyZalogowany => IdZalogowanegoPacjenta > 0;

        public static void Zaloguj(int idPacjenta, string email)
        {
            IdZalogowanegoPacjenta = idPacjenta;
            Email = email;
        }

        public static void Wyloguj()
        {
            IdZalogowanegoPacjenta = 0;
            Email = null;
        }
    }
}
