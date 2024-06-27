using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.DTO_s;
using WebApplication3.Services;

namespace WebApplication3.Controllers;

[Authorize(Roles = "user")]
[ApiController]
[Route("api/prescriptions")]
public class PrescriptionController : ControllerBase
{

    private readonly PrescriptionService _prescriptionService;

    public PrescriptionController(PrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }


    [HttpPost("{id:int}/doctor")]

    public async Task<IActionResult> InsertPrescription([FromBody] PrescriptionInsertSchemaDTO prescriptionInsertSchemaDto, int id)
    {
        await _prescriptionService.InsertPrescription(prescriptionInsertSchemaDto, id);
        return Ok();
    }



}