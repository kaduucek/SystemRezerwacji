using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows; 

namespace SystemRezerwacji.ViewModels
{
    public partial class EdytujWizyteViewModel : ObservableObject
    {
        private readonly int _idWizyty;
        private readonly PrzychodniaContext _context; 

        [ObservableProperty]
        private Wizyta _wizyta;

        [ObservableProperty]
        private List<Lekarz> _dostepniLekarze;

        [ObservableProperty]
        private Lekarz _wybranyLekarz;

        [ObservableProperty]
        private DateTime _nowaData;

        [ObservableProperty]
        private TimeSpan _nowaGodzina;

        public IRelayCommand ZapiszCommand { get; }
        public IRelayCommand AnulujCommand { get; }

        public EdytujWizyteViewModel(int idWizyty)
        {
            _idWizyty = idWizyty;
            _context = new PrzychodniaContext(); 

            ZapiszCommand = new RelayCommand(async () => await Zapisz(), () => CzyMoznaZapisac());
            AnulujCommand = new RelayCommand(Anuluj);

           
            Task.Run(async () => await LoadData());
        }

        private async Task LoadData()
        {
            Wizyta = await _context.Wizyty
                .Include(w => w.Lekarz)
                .FirstOrDefaultAsync(w => w.Id == _idWizyty);

            if (Wizyta != null)
            {
                NowaData = Wizyta.Data;
                NowaGodzina = Wizyta.Godzina;

                DostepniLekarze = await _context.Lekarze.ToListAsync();
                WybranyLekarz = DostepniLekarze.FirstOrDefault(l => l.Id == Wizyta.LekarzId);
            }
        }

        private async Task Zapisz()
        {
            bool zajety = await _context.Wizyty
                .AnyAsync(w => w.LekarzId == WybranyLekarz.Id &&
                               w.Data == NowaData &&
                               w.Godzina == NowaGodzina &&
                               w.Id != _idWizyty);

            if (zajety)
            {
                MessageBox.Show("Wybrany termin jest już zajęty.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (NowaData < DateTime.Now.AddHours(24))
            {
                MessageBox.Show("Nie można zmienić wizyty na mniej niż 24 godziny przed terminem.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Wizyta.Data = NowaData;
            Wizyta.Godzina = NowaGodzina;
            Wizyta.LekarzId = WybranyLekarz.Id;

            _context.Wizyty.Update(Wizyta);
            await _context.SaveChangesAsync();

            MessageBox.Show("Wizyta została zmieniona.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this);
            window?.Close();
        }

        private bool CzyMoznaZapisac()
        {
            return Wizyta != null && WybranyLekarz != null && NowaData != default && NowaGodzina != default;
        }

        private void Anuluj()
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this);
            window?.Close();
        }
    }
}