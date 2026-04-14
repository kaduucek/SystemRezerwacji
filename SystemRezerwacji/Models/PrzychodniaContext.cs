using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using SystemRezerwacji.Models;

public class PrzychodniaContext : DbContext
{
    public DbSet<Pacjent> Pacjenci { get; set; }
    public DbSet<Lekarz> Lekarze { get; set; }
    public DbSet<Wizyta> Wizyty { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=LAPTOK;Database=PrzychodniaDB;Trusted_Connection=True;TrustServerCertificate=True;");
    }
}