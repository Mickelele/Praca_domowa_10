namespace WebApplication3.DTO_s;

public class PrescriptionDTO
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public ICollection<MedicamentDTO> Medicaments { get; set; }
    public DoctorDTO Doctor { get; set; }
}