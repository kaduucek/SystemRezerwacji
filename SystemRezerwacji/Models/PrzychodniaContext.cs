using Microsoft.EntityFrameworkCore;

namespace SystemRezerwacji.Models
{
    public class PrzychodniaContext : DbContext
    {
        public PrzychodniaContext() { }

        // Konstruktor wykorzystywany m.in. w testach (baza In-Memory)
        public PrzychodniaContext(DbContextOptions<PrzychodniaContext> options) : base(options) { }

        public DbSet<Pacjent> Pacjenci => Set<Pacjent>();
        public DbSet<Lekarz> Lekarze => Set<Lekarz>();
        public DbSet<Wizyta> Wizyty => Set<Wizyta>();
        public DbSet<HistoriaMedyczna> HistorieMedyczne => Set<HistoriaMedyczna>();
        public DbSet<Dostepnosc> Dostepnosci => Set<Dostepnosc>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Jeśli kontekst nie został skonfigurowany z zewnątrz (np. w testach),
            // używamy lokalnej bazy LocalDB - działa na każdej maszynie z Visual Studio.
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=(localdb)\MSSQLLocalDB;Database=Przychodnia;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // E-mail pacjenta unikalny - pełni rolę loginu
            modelBuilder.Entity<Pacjent>()
                .HasIndex(p => p.Email)
                .IsUnique();

            // Status zapisywany jako czytelny tekst zamiast liczby
            modelBuilder.Entity<Wizyta>()
                .Property(w => w.Status)
                .HasConversion<string>();

            // Relacje 1:N
            modelBuilder.Entity<Wizyta>()
                .HasOne(w => w.Lekarz)
                .WithMany(l => l.Wizyty)
                .HasForeignKey(w => w.IdLekarza);

            modelBuilder.Entity<Wizyta>()
                .HasOne(w => w.Pacjent)
                .WithMany()
                .HasForeignKey(w => w.IdPacjenta);

            // Dane startowe - kilku lekarzy, żeby było co rezerwować
            modelBuilder.Entity<Lekarz>().HasData(
                new Lekarz { IdLekarza = 1, Imie = "Anna", Nazwisko = "Kowalska", Specjalizacja = "Internista", Email = "a.kowalska@przychodnia.pl" },
                new Lekarz { IdLekarza = 2, Imie = "Piotr", Nazwisko = "Nowak", Specjalizacja = "Kardiolog", Email = "p.nowak@przychodnia.pl" },
                new Lekarz { IdLekarza = 3, Imie = "Maria", Nazwisko = "Wiśniewska", Specjalizacja = "Pediatra", Email = "m.wisniewska@przychodnia.pl" }
            );
        }
    }
}
