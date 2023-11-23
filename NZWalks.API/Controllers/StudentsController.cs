using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers;

//https://localhost:portnumber/api/students
[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    //GET:https://localhost:portnumber/api/students
    [HttpGet]
    public IActionResult GetAllStudents()
    {
       var studentNames = new[] { "John", "Jane", "Mark", "Emily", "David" };
       return Ok(studentNames);
    }
}