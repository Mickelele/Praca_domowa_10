using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication3.Context;

public class ApbdContext : DbContext
{
    protected ApbdContext()
    {
    }

    public ApbdContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> prescriptions { get; set; }
    public DbSet<Prescription_Medicament> prescriptionMedicaments { get; set; }
    public DbSet<AppUser> Users { get; set; }
    
}