using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemRezerwacji.Models
{
    public class Lekarz
    {
        public int IdLekarza { get; set; }
        public string Imie { get; set; } = "";
        public string Nazwisko { get; set; } = "";
        public string Specjalizacja { get; set; } = "";
        public string? Email { get; set; }
        public string? NumerTelefonu { get; set; }

        // Własność nawigacyjna (1 lekarz : N wizyt)
        public ICollection<Wizyta> Wizyty { get; set; } = new List<Wizyta>();

        // Wygodne do wyświetlania w ComboBox / DataGrid (nie zapisywane w bazie)
        [NotMapped]
        public string ImieINazwisko => $"{Imie} {Nazwisko} ({Specjalizacja})";
    }
}
