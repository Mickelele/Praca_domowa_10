using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using WebApplication3.Services;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientController : ControllerBase
{
    private readonly PatientService _patientService;

    public PatientController(PatientService patientService)
    {
        _patientService = patientService;
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> getPatientInfo(int id)
    {
        var result = await _patientService.getPatientInfo(id);
        return Ok(result);
    }

}