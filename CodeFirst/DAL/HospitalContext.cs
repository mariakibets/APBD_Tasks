using CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.DAL;

public class HospitalContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Prescription_Medicament> PrescriptionMedicaments { get; set; }

    public HospitalContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Prescription_Medicament>()
            .HasKey(pm => new { pm.IdPrescription, pm.IdMedicament });

        modelBuilder.Entity<Prescription_Medicament>()
            .HasOne(pm => pm.Medicament)
            .WithMany(m => m.Perscription_Medicaments)
            .HasForeignKey(pm => pm.IdMedicament);

        modelBuilder.Entity<Prescription_Medicament>()
            .HasOne(pm => pm.Prescription)
            .WithMany(p => p.Perscription_Medicaments)
            .HasForeignKey(pm => pm.IdPrescription);
        
        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Patient)
            .WithMany(pa => pa.Prescriptions)
            .HasForeignKey(p => p.IdPatient);

        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Doctor)
            .WithMany(d => d.Prescriptions)
            .HasForeignKey(p => p.IdDoctor);
    }
}