using SchoolService.Domain.Schools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolService.Application.Schools.Dtos;
public class SchoolDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public DateTime? OpenDate { get; set; }
    public List<ClassRoomDto>? Classes { get; set; }

    public static SchoolDto FromSchool(School input) =>
        new SchoolDto
        {
            Id = input.Id,
            Name = input.Name,
            OpenDate = input.OpenDate,
            Classes = input.Classes?.Select(x => ClassRoomDto.FromClassroom(x)).ToList()
        };
}