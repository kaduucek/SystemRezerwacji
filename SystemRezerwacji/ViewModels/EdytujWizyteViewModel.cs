using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using SystemRezerwacji.Models;

namespace SystemRezerwacji.ViewModels
{
    public partial class EdytujWizyteViewModel : ObservableObject
    {
        private readonly int _idWizyty;
        private readonly PrzychodniaContext _context = new();

        [ObservableProperty] private List<Lekarz> _dostepniLekarze = new();
        [ObservableProperty] private Lekarz? _wybranyLekarz;
        [ObservableProperty] private DateTime _nowaData = DateTime.Today;
        [ObservableProperty] private string _nowaGodzina = "08:00";

        public IAsyncRelayCommand ZapiszCommand { get; }
        public IRelayCommand AnulujCommand { get; }

        public EdytujWizyteViewModel(int idWizyty)
        {
            _idWizyty = idWizyty;
            ZapiszCommand = new AsyncRelayCommand(Zapisz);
            AnulujCommand = new RelayCommand(ZamknijOkno);
            _ = LoadData();
        }

        private async Task LoadData()
        {
            DostepniLekarze = await _context.Lekarze.AsNoTracking().ToListAsync();

            var wizyta = await _context.Wizyty
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.IdWizyty == _idWizyty);

            if (wizyta != null)
            {
                NowaData = wizyta.DataGodzinaRozpoczecia.Date;
                NowaGodzina = wizyta.DataGodzinaRozpoczecia.ToString("HH:mm");
                WybranyLekarz = DostepniLekarze.FirstOrDefault(l => l.IdLekarza == wizyta.IdLekarza);
            }
        }

        private async Task Zapisz()
        {
            if (WybranyLekarz == null)
            {
                MessageBox.Show("Wybierz lekarza.", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TimeSpan.TryParse(NowaGodzina, out var godzina))
            {
                MessageBox.Show("Podaj godzinę w formacie GG:MM (np. 09:30).", "Uwaga",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime nowyTermin = NowaData.Date + godzina;

            if (nowyTermin < DateTime.Now.AddHours(24))
            {
                MessageBox.Show("Nie można ustawić terminu na mniej niż 24 godziny od teraz.", "Błąd",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool zajety = await _context.Wizyty.AnyAsync(w =>
                w.IdLekarza == WybranyLekarz.IdLekarza &&
                w.DataGodzinaRozpoczecia == nowyTermin &&
                w.Status == WizytaStatus.Planned &&
                w.IdWizyty != _idWizyty);

            if (zajety)
            {
                MessageBox.Show("Wybrany termin jest już zajęty.", "Błąd",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var wizyta = await _context.Wizyty.FirstOrDefaultAsync(w => w.IdWizyty == _idWizyty);
            if (wizyta == null)
                return;

            wizyta.DataGodzinaRozpoczecia = nowyTermin;
            wizyta.IdLekarza = WybranyLekarz.IdLekarza;
            await _context.SaveChangesAsync();

            MessageBox.Show("Wizyta została zmieniona.", "Sukces",
                MessageBoxButton.OK, MessageBoxImage.Information);
            ZamknijOkno();
        }

        private void ZamknijOkno()
        {
            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.DataContext == this)
                ?.Close();
        }
    }
}
