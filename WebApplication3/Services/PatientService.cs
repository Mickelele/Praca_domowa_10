using Microsoft.EntityFrameworkCore;
using WebApplication3.DTO_s;
using WebApplication3.Models;

namespace WebApplication3.Services;

public class PatientService
{

    private readonly Context.ApbdContext _apbdContext;

    public PatientService(Context.ApbdContext apbdContext)
    {
        _apbdContext = apbdContext;
    }


    public async Task<PatientInsertDTO> getPatientInfo(int id)
    {
        var patient = await _apbdContext.Patients
            .Include(e => e.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionsMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .FirstOrDefaultAsync(p => p.IdPatient == id);

        var result = new PatientInsertDTO()
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate,
            Prescriptions = patient.Prescriptions.Select(pr => new PrescriptionDTO()
            {
                IdPrescription = pr.IdPrescription,
                Date = pr.Date,
                DueDate = pr.Date,
                Medicaments = pr.PrescriptionsMedicaments.Select(pm => new MedicamentDTO()
                {
                    IdMedicament = pm.Medicament.IdMedicament,
                    Name = pm.Medicament.Name,
                    Dose = pm.Dose,
                    Description = pm.Medicament.Description
                }).ToList(),
                Doctor = new DoctorDTO()
                {
                    IdDoctor = pr.Doctor.IdDoctor,
                    FirstName = pr.Doctor.FirstName
                }
            }).ToList()
        };

        return result;
    }
    
    
    
    
}
    