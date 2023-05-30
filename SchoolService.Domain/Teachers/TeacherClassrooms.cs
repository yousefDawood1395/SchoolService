using SchoolService.Domain.Shared.Aggregates;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolService.Domain.Teachers;
public class TeacherClassrooms : IEntity<int>
{
    private TeacherClassrooms()
    {

    }
    public int Id { get; private set; }
    public int TeacherId { get; private set; }
    public int ClassroomId { get; private set; }
    public int SchoolId { get; private set; }
    [ExcludeFromCodeCoverage]
    public Teacher? Teacher { get; private set; }
    public static TeacherClassrooms Create(int? id, int teacherId, int classroomId, int schoolId)
    {
        return new TeacherClassrooms
        {
            Id = id ?? 0,
            ClassroomId = classroomId,
            SchoolId = schoolId,
            TeacherId = teacherId
        };
    }
}
