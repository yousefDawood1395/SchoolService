using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolService.Domain.Schools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolService.DataAccess.Mappings;
public class SchoolDbMapping : IEntityTypeConfiguration<School>
{
    public void Configure(EntityTypeBuilder<School> builder)
    {
        builder.ToTable("Schools").HasKey(k => k.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.OpenDate).IsRequired();
        builder.Ignore(p => p.ClassesCount);
        builder.Ignore(p => p.Validator);
        builder.HasMany(p => p.Classes);
    }
}
