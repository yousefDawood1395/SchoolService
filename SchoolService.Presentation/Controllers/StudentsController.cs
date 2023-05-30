using Microsoft.AspNetCore.Mvc;
using SchoolService.Application.Students;
using SchoolService.Application.Students.DTOs;
using SchoolService.Domain.Shared.Exceptions;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Presentation.ExceptionHandlers;

namespace SchoolService.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly IStudentsService _studentsService;
    private readonly IStudentsQueryService _studentsQueryService;

    public StudentsController(IStudentsService studentsService, IStudentsQueryService studentsQueryService)
    {
        _studentsService = studentsService;
        _studentsQueryService = studentsQueryService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(StudentDto), 201)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
    public async Task<CreatedResult> Create([FromBody] StudentCreateDto request)
    {
        var output = await _studentsService.Create(request);
        return Created("api/students/" + output.Id, output);
    }

    [HttpPut]
    [ProducesResponseType(typeof(StudentDto), 200)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
    public async Task<ActionResult<StudentDto>> Update([FromBody] StudentUpdateDto request)
    {
        return Ok(await _studentsService.Update(request));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
    public async Task<ActionResult<bool>> Delete([FromRoute] int id)
    {
        return Ok(await _studentsService.Delete(id));
    }

    [HttpPut("{id:int}/class")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
    public async Task<ActionResult<bool>> SetClass([FromRoute] int id, [FromBody] StudentSetClassroomDto request)
    {
        if (id != request.StudentId)
        {
            throw new DataNotValidException("Student id mismatch");
        }

        return Ok(await _studentsService.SetClass(request));

    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(StudentDto), 200)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
    public async Task<ActionResult<StudentDto>> Get([FromRoute] int id)
    {
        return Ok(await _studentsQueryService.Get(id));
    }

    [HttpGet("[Action]")]
    [ProducesResponseType(typeof(PagedResponse<StudentDto>), 200)]
    public async Task<ActionResult<PagedResponse<StudentDto>>> Search([FromQuery] StudentSearchDto request)
    {
        return Ok(await _studentsQueryService.Search(request));
    }
}