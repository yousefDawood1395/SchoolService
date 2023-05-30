using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolService.Domain.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolService.DataAccess.Mappings;
public class TeacherClassroomsDbMappings : IEntityTypeConfiguration<TeacherClassrooms>
{
    public void Configure(EntityTypeBuilder<TeacherClassrooms> builder)
    {
        builder.ToTable(nameof(TeacherClassrooms)).HasKey(x => x.Id);
        builder.HasOne(x => x.Teacher).WithMany(x => x.Classrooms).HasForeignKey(x => x.TeacherId);
    }
}
