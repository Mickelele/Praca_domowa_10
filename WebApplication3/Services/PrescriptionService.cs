using WebApplication3.Context;
using WebApplication3.DTO_s;
using WebApplication3.Models;

namespace WebApplication3.Services;

public class PrescriptionService
{

    private readonly ApbdContext _context;

    public PrescriptionService(ApbdContext context)
    {
        _context = context;
    }
    
    public async Task InsertPrescription(PrescriptionInsertSchemaDTO prescriptionInsertSchemaDto, int id)
    {
        int indeks = await CzyPacjentIstnieje(prescriptionInsertSchemaDto);
        czyLekiIstnieja(prescriptionInsertSchemaDto);
        czyData(prescriptionInsertSchemaDto);

        var newPrescription = new Prescription()
        {
            Date = prescriptionInsertSchemaDto.Date,
            DueDate = prescriptionInsertSchemaDto.DueDate,
            IdPatient = indeks,
            IdDoctor = id
        };

        await _context.prescriptions.AddAsync(newPrescription);
        await _context.SaveChangesAsync();

        foreach (var m in prescriptionInsertSchemaDto.Medicaments)
        {
            var newPreMed = new Prescription_Medicament()
            {
                IdPrescription = newPrescription.IdPrescription,
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Details = m.Description
            };
           await _context.prescriptionMedicaments.AddAsync(newPreMed);
        }
        await _context.SaveChangesAsync();
        

    }

    private async Task<int> CzyPacjentIstnieje(PrescriptionInsertSchemaDTO prescriptionInsertSchemaDto)
    {
        var patient = prescriptionInsertSchemaDto.Patient;
        int id;
        var czyIstnieje = _context.Patients.FirstOrDefault(e => e.IdPatient == patient.IdPatient);
        if (czyIstnieje == null)
        {
            var newPatient = new Patient()
            {
                FirstName = prescriptionInsertSchemaDto.Patient.FirstName,
                LastName = prescriptionInsertSchemaDto.Patient.LastName,
                Birthdate = prescriptionInsertSchemaDto.Patient.Birthdate
            };
            await _context.Patients.AddAsync(newPatient);
            await _context.SaveChangesAsync();
            id = newPatient.IdPatient;
        }
        else
        {
            id = prescriptionInsertSchemaDto.Patient.IdPatient;
        }

        return id;
    }

    public void czyLekiIstnieja(PrescriptionInsertSchemaDTO prescriptionInsertSchemaDto)
    {
        if (prescriptionInsertSchemaDto.Medicaments.Count > 10)
        {
            throw new Exception($"Zbyt duza ilosc lekow na recepcie");
        }

        foreach (var m in prescriptionInsertSchemaDto.Medicaments)
        {
            if (!_context.Medicaments.Any(e => e.IdMedicament == m.IdMedicament))
            {
                throw new Exception($"Lek o id: {m.IdMedicament} nie istnieje.");
            }
        }
    }


    public void czyData(PrescriptionInsertSchemaDTO prescriptionInsertSchemaDto)
    {
        if (!(prescriptionInsertSchemaDto.DueDate >= prescriptionInsertSchemaDto.Date))
        {
            throw new Exception($"Blad w dacie");
        }
    }
    
}