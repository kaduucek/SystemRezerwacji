using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SystemRezerwacji.Models;

namespace SystemRezerwacji.Services
{
    public class AppointmentService
    {
        // UC-01: rezerwacja w transakcji - chroni przed równoczesnym zajęciem tego samego slotu
        public async Task<bool> ZarezerwujAsync(int idPacjenta, int idLekarza, DateTime termin)
        {
            using var ctx = new PrzychodniaContext();
            using var tx = await ctx.Database.BeginTransactionAsync();

            bool zajety = await ctx.Wizyty.AnyAsync(w =>
                w.IdLekarza == idLekarza &&
                w.DataGodzinaRozpoczecia == termin &&
                w.Status == WizytaStatus.Planned);

            if (zajety)
                return false;

            ctx.Wizyty.Add(new Wizyta
            {
                IdPacjenta = idPacjenta,
                IdLekarza = idLekarza,
                DataGodzinaRozpoczecia = termin,
                Status = WizytaStatus.Planned
            });

            await ctx.SaveChangesAsync();
            await tx.CommitAsync();
            return true;
        }

        // Przegląd historii wizyt pacjenta (przeszłe i przyszłe)
        public async Task<List<Wizyta>> PobierzWizytyPacjentaAsync(int idPacjenta)
        {
            using var ctx = new PrzychodniaContext();
            return await ctx.Wizyty
                .AsNoTracking()
                .Include(w => w.Lekarz)
                .Where(w => w.IdPacjenta == idPacjenta)
                .OrderByDescending(w => w.DataGodzinaRozpoczecia)
                .ToListAsync();
        }

        // UC-02: anulowanie z regułą 24h (zwalnia termin - status Cancelled)
        public async Task<bool> AnulujAsync(int idWizyty)
        {
            using var ctx = new PrzychodniaContext();
            var w = await ctx.Wizyty.FindAsync(idWizyty);

            if (w == null || w.Status != WizytaStatus.Planned)
                return false;

            if (w.DataGodzinaRozpoczecia < DateTime.Now.AddHours(24))
                return false; // reguła z README: anulowanie min. 24h przed wizytą

            w.Status = WizytaStatus.Cancelled;
            await ctx.SaveChangesAsync();
            return true;
        }
    }
}
