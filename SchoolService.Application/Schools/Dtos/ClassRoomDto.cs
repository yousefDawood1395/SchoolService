using SchoolService.Domain.Schools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolService.Application.Schools.Dtos;
public class ClassRoomDto
{
    public int Id { get; set; }
    public int SchoolId { get; set; }
    public string? Name { get; set; }

    public static ClassRoomDto FromClassroom(ClassRoom input) =>
        new ClassRoomDto
        {
            Id = input.Id,
            SchoolId = input.SchoolId,
            Name = input.Name
        };
}
