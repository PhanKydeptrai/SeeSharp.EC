
using API.Extentions;
using API.Infrastructure;
using Application.Features.EmployeeFeature.Commands.CreateNewEmployee;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/employees")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly ISender _sender;

    public EmployeesController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Tạo nhân viên mới
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="email"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="dateOfBirth"></param>
    /// <param name="imageFile"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiKey]
    [Authorize]
    public async Task<IResult> CreateEmployee(
        [FromForm] string userName,
        [FromForm] string email,
        [FromForm] string phoneNumber,
        [FromForm] DateTime? dateOfBirth,
        IFormFile? imageFile)
    {
        var result = await _sender.Send(new CreateNewEmployeeCommand(userName, email, phoneNumber, dateOfBirth, imageFile));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }
    

    
}
