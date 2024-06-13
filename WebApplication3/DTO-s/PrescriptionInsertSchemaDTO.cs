using WebApplication3.Models;

namespace WebApplication3.DTO_s;

public class PrescriptionInsertSchemaDTO
{
    public PatientInsertDTO2 Patient { get; set; }
    public ICollection<MedicamentDTOInsert> Medicaments { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
}