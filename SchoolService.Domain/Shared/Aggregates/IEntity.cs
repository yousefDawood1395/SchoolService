using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolService.Domain.Shared.Aggregates;

public interface IEntity<TPrimaryKey>
{
    TPrimaryKey Id { get; }
}