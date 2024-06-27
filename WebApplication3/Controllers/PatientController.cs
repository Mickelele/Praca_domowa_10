using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApplication3.Context;
using WebApplication3.Models;
using WebApplication3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApplication3.Helpers;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientController : ControllerBase
{
    private readonly PatientService _patientService;
    private readonly ApbdContext _context;

    public PatientController(PatientService patientService, ApbdContext context)
    {
        _patientService = patientService;
        _context = context;
    }
    
    
    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult RegisterPatient(RegisterUser model)
    {
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(model.Password);

        var user = new AppUser()
        {
            Login = model.Login,
            Password = hashedPasswordAndSalt.Item1,
            Salt = hashedPasswordAndSalt.Item2,
            RefreshToken = SecurityHelpers.GenerateRefreshToken(),
            RefreshTokenExp = DateTime.Now.AddDays(1)
        };

        _context.Uzytkownicy.Add(user);
        _context.SaveChanges();

        return Ok();
    }
    
    
    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login(LoginUser loginRequest)
    {
        AppUser user = _context.Uzytkownicy.Where(u => u.Login == loginRequest.Login).First();

        string passwordHashFromDb = user.Password;
        string curHashedPassword = SecurityHelpers.GetHashedPasswordWithSalt(loginRequest.Password, user.Salt);

        if (passwordHashFromDb != curHashedPassword)
        {
            return Unauthorized();
        }


        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Login)
        };
        claims.Add(new Claim(ClaimTypes.Role, "user"));
        

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("8hTfGvUWfZXNz7Dk5JH7fF3sDq8fJ9x2"));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "https://localhost:5001",
            audience: "https://localhost:5001",
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);
        _context.SaveChanges();

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(token),
            refreshToken = user.RefreshToken
        });
    }
    
    
    
    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public IActionResult RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var user = _context.Uzytkownicy.SingleOrDefault(u => u.RefreshToken == request.RefreshToken);

        if (user == null || user.RefreshTokenExp <= DateTime.Now)
        {
            return Unauthorized();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, "user")
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("8hTfGvUWfZXNz7Dk5JH7fF3sDq8fJ9x2"));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "https://localhost:5001",
            audience: "https://localhost:5001",
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);
        _context.SaveChanges();

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(token),
            refreshToken = user.RefreshToken
        });
    }
    
    

    [Authorize(Roles = "user")]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> getPatientInfo(int id)
    {
        var result = await _patientService.getPatientInfo(id);
        return Ok(result);
    }

}