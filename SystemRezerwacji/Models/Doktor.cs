using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemRezerwacji.Models
{
    public class Lekarz
    {
        public int IdLekarza { get; set; }

        public string Imie { get; set; }
        public string Nazwisko { get; set; }

        public string Specjalizacja { get; set; }

        public string Email {  get; set; }

        public string NumerTelefonu { get; set; }

        public ICollection<Wizyta> Wityty { get; set; }
    }
}
