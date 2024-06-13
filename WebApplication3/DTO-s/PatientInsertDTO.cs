using WebApplication3.Models;

namespace WebApplication3.DTO_s;

public class PatientInsertDTO
{
    public int IdPatient { get; set; }
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public DateTime Birthdate { get; set; }
    public ICollection<PrescriptionDTO> Prescriptions { get; set; }
}

public class PatientInsertDTO2
{
    public int IdPatient { get; set; }
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public DateTime Birthdate { get; set; }
}