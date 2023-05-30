using Microsoft.AspNetCore.Mvc;
using SchoolService.Application.Teachers;
using SchoolService.Application.Teachers.DTOs;
using SchoolService.Domain.Shared.Exceptions;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Presentation.ExceptionHandlers;

namespace SchoolService.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeachersController : ControllerBase
{
    private readonly ITeachersService _teachersService;
    private readonly ITeachersQueryService _teachersQueryService;

    public TeachersController(ITeachersService teachersService, ITeachersQueryService teachersQueryService)
    {
        _teachersService = teachersService;
        _teachersQueryService = teachersQueryService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(TeacherDto), 201)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
    public async Task<CreatedResult> Create([FromBody] TeacherCreateDto request)
    {
        var output = await _teachersService.Create(request);
        return Created("api/teachers/" + output.Id, output);
    }

    [HttpPut]
    [ProducesResponseType(typeof(TeacherDto), 200)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
    public async Task<ActionResult<TeacherDto>> Update([FromBody] TeacherUpdateDto request)
    {
        return Ok(await _teachersService.Update(request));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
    public async Task<ActionResult<bool>> Delete([FromRoute] int id)
    {
        return Ok(await _teachersService.Delete(id));
    }

    [HttpPost("{id:int}/classes")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
    public async Task<ActionResult<bool>> AssignClass([FromRoute] int id, [FromBody] TeacherAssignClassroomDto request)
    {
        if (id != request.TeacherId)
        {
            throw new DataNotValidException("Teacher id mismatch");
        }
        return Ok(await _teachersService.AssignTeacherToClassroom(request));
    }

    [HttpDelete("{id:int}/classes/{classroomId:int}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
    public async Task<ActionResult<bool>> UnAssignTeacherToClass([FromRoute] int id, int classroomId)
    {
        return Ok(await _teachersService.UnAssignTeacherToClassroom(new TeacherUnAssignClassroomDto
        {
            TeacherId = id,
            ClassroomId = classroomId
        }));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TeacherDto), 200)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
    public async Task<ActionResult<TeacherDto>> Get([FromRoute] int id)
    {
        return Ok(await _teachersQueryService.Get(id));
    }

    [HttpGet("[action]")]
    [ProducesResponseType(typeof(PagedResponse<TeacherDto>), 200)]
    public async Task<ActionResult<PagedResponse<TeacherDto>>> Search([FromQuery] TeacherSearchDto request)
    {
        return Ok(await _teachersQueryService.Search(request));
    }
}