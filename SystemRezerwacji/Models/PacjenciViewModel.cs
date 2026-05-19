using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore; // Upewnij się, że ten using jest na samej górze pliku


namespace SystemRezerwacji.Models
{
    public partial class PacjenciViewModel : ObservableObject
    {
        private readonly PrzychodniaContext _context = new PrzychodniaContext();

        [ObservableProperty]
        private ObservableCollection<Pacjent> _listaPacjentow;

        public PacjenciViewModel()
        {
            WczytajDane();
        }


public void WczytajDane()
{
    // OPTYMALIZACJA LINQ: Dodano AsNoTracking() do odczytu danych
    var dane = _context.Pacjenci.AsNoTracking().ToList();
    
    ListaPacjentow = new ObservableCollection<Pacjent>(dane);
}
    }
}

