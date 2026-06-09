using System;

namespace SystemRezerwacji.Models
{
    public class Pacjent
    {
        public int IdPacjenta { get; set; }
        public string Imie { get; set; } = "";
        public string Nazwisko { get; set; } = "";
        public string PESEL { get; set; } = "";
        public string Email { get; set; } = "";
        public string Hasz { get; set; } = "";
        public string NumerTelefonu { get; set; } = "";
        public DateTime DataRejestracji { get; set; }
    }
}
