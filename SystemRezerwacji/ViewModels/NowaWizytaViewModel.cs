using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using SystemRezerwacji.Models;
using SystemRezerwacji.Services;

namespace SystemRezerwacji.ViewModels
{
    public partial class NowaWizytaViewModel : ObservableObject
    {
        private readonly SlotService _slotService = new();
        private readonly AppointmentService _appointmentService = new();

        [ObservableProperty] private List<Lekarz> _dostepniLekarze = new();
        [ObservableProperty] private Lekarz? _wybranyLekarz;
        [ObservableProperty] private DateTime _wybranaData = DateTime.Today;
        [ObservableProperty] private ObservableCollection<DateTime> _wolneSloty = new();
        [ObservableProperty] private DateTime? _wybranySlot;

        public IAsyncRelayCommand ZarezerwujCommand { get; }

        public NowaWizytaViewModel()
        {
            ZarezerwujCommand = new AsyncRelayCommand(Zarezerwuj);
            _ = WczytajLekarzy();
        }

        private async Task WczytajLekarzy()
        {
            using var ctx = new PrzychodniaContext();
            DostepniLekarze = await ctx.Lekarze.AsNoTracking().ToListAsync();
        }

        // Po zmianie lekarza lub daty - odśwież listę wolnych terminów
        partial void OnWybranyLekarzChanged(Lekarz? value) => _ = OdswiezSloty();
        partial void OnWybranaDataChanged(DateTime value) => _ = OdswiezSloty();

        private async Task OdswiezSloty()
        {
            if (WybranyLekarz == null)
            {
                WolneSloty = new ObservableCollection<DateTime>();
                return;
            }

            var sloty = await _slotService.GetAvailableSlotsAsync(WybranyLekarz.IdLekarza, WybranaData);
            WolneSloty = new ObservableCollection<DateTime>(sloty);
        }

        private async Task Zarezerwuj()
        {
            if (WybranyLekarz == null || WybranySlot == null)
            {
                MessageBox.Show("Wybierz lekarza i termin.", "Uwaga",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool ok = await _appointmentService.ZarezerwujAsync(
                SessionService.IdZalogowanegoPacjenta, WybranyLekarz.IdLekarza, WybranySlot.Value);

            if (ok)
            {
                MessageBox.Show("Wizyta została zarezerwowana.", "Sukces",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                WybranySlot = null;
            }
            else
            {
                MessageBox.Show("Ten termin jest już zajęty - wybierz inny.", "Błąd",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            await OdswiezSloty();
        }
    }
}
