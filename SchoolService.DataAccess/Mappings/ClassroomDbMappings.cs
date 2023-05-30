using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using SchoolService.Domain.Schools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolService.DataAccess.Mappings;
public class ClassroomDbMappings : IEntityTypeConfiguration<ClassRoom>
{
    public void Configure(EntityTypeBuilder<ClassRoom> builder)
    {
        builder.ToTable("ClassRooms").HasKey(x => x.Id);
        builder.HasOne(x => x.School).WithMany(x => x.Classes).HasForeignKey(x => x.SchoolId);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Ignore(x => x.Validator);
    }
}
