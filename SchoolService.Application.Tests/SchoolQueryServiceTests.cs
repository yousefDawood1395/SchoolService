using AutoFixture;
using Moq;
using SchoolService.Application.Schools;
using SchoolService.Application.Schools.Dtos;
using SchoolService.Domain.Schools;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Domain.Shared.Repositories;

namespace SchoolService.Application.Tests;
public class SchoolQueryServiceTests
{
    private readonly Mock<ISchoolsRepository> _schoolsRepository;
    private readonly ISchoolsQueryService _sut;
    private readonly Fixture _fixture;
    public SchoolQueryServiceTests()
    {
        _schoolsRepository = new Mock<ISchoolsRepository>();
        _fixture = new Fixture();
        _sut = new SchoolsQueryService(_schoolsRepository.Object);
    }

    [Fact]
    public async Task GetSchool_ValidInput_Success()
    {
        _schoolsRepository.Setup(x => x.GetSchool(It.IsAny<int>())).ReturnsAsync(ValidSchool(_fixture));

        var output = await _sut.GetSchool(_fixture.Create<int>());

        Assert.NotNull(output);

        Assert.IsType<SchoolDto>(output);
    }

    [Fact]
    public async Task SearchSchools_ValidInput_Success()
    {
        _schoolsRepository.Setup(x => x.SearchSchoolsByName(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(PagedSchools(_fixture));

        var output = await _sut.SearchSchools(SchoolSearchDto(_fixture));

        Assert.NotNull(output);

        Assert.IsType<PagedResponse<SchoolDto>>(output);
    }


    private static School ValidSchool(Fixture fixture) =>
        School.Create(fixture.Create<int>(), fixture.Create<string>(), fixture.Create<DateTime>());

    private static PagedResponse<School> PagedSchools(Fixture fixture) =>
        new PagedResponse<School>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 10,
            Data = Enumerable.Range(1, 10).Select(s => ValidSchool(fixture)).ToList()
        };

    private static SchoolSearchDto SchoolSearchDto(Fixture fixture) =>
        fixture.Create<SchoolSearchDto>();
}
