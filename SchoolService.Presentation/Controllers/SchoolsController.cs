using Microsoft.AspNetCore.Mvc;
using SchoolService.Application.Schools;
using SchoolService.Application.Schools.Dtos;
using SchoolService.Domain.Shared.Exceptions;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Presentation.ExceptionHandlers;

namespace SchoolService.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SchoolsController : ControllerBase
{
    private readonly ISchoolsService _schoolsService;
    private readonly ISchoolsQueryService _schoolsQueryService;

    public SchoolsController(ISchoolsService schoolsService, ISchoolsQueryService schoolsQueryService)
    {
        _schoolsService = schoolsService;
        _schoolsQueryService = schoolsQueryService;
    }


    [HttpPost]
    [ProducesResponseType(typeof(SchoolDto), 201)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
    public async Task<CreatedResult> Create([FromBody] SchoolCreateDto request)
    {
        var school = await _schoolsService.Create(request);
        return Created("api/schools/" + school.Id, school);
    }

    [HttpPut]
    [ProducesResponseType(typeof(SchoolDto), 200)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
    public async Task<ActionResult<SchoolDto>> Update([FromBody] SchoolUpdateDto request)
    {
        return Ok(await _schoolsService.Update(request));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.BusinessException)]
    public async Task<ActionResult<bool>> Delete([FromRoute] int id)
    {
        return Ok(await _schoolsService.Delete(id));
    }

    [HttpPost("{id:int}/classes")]
    [ProducesResponseType(typeof(SchoolDto), 200)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataDuplicated)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotValid)]
    public async Task<ActionResult<SchoolDto>> AddClass([FromRoute] int id, [FromBody] ClassRoomAddDto request)
    {
        if (id != request.SchoolId)
        {
            throw new DataNotValidException("School id conflict");
        }
        return Ok(await _schoolsService.AddClass(request));
    }

    [HttpDelete("{id:int}/classes/{classroomId:int}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.BusinessException)]
    public async Task<ActionResult<bool>> DeleteClass([FromRoute] int id, [FromRoute] int classroomId)
    {
        return Ok(await _schoolsService.DeleteClass(new ClassRoomDeleteDto
        {
            ClassRoomId = classroomId,
            SchoolId = id
        }));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SchoolDto), 200)]
    [ProducesResponseType(typeof(HttpException), GeideaHttpStatusCodes.DataNotFound)]
    public async Task<ActionResult<SchoolDto>> Get([FromRoute] int id)
    {
        return Ok(await _schoolsQueryService.GetSchool(id));
    }

    [HttpGet("[Action]")]
    [ProducesResponseType(typeof(PagedResponse<SchoolDto>), 200)]
    public async Task<ActionResult<PagedResponse<SchoolDto>>> Search([FromQuery] SchoolSearchDto request)
    {
        return Ok(await _schoolsQueryService.SearchSchools(request));
    }
}