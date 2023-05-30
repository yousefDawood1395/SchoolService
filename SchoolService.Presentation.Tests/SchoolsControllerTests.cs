using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SchoolService.Application.Schools;
using SchoolService.Application.Schools.Dtos;
using SchoolService.Domain.Schools;
using SchoolService.Domain.Shared.Exceptions;
using SchoolService.Domain.Shared.Pagination;
using SchoolService.Presentation.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolService.Presentation.Tests
{
    public class SchoolsControllerTests
    {
        private readonly SchoolsController _sut;
        private readonly Mock<ISchoolsService> _schoolServiceMock;
        private readonly Mock<ISchoolsQueryService> _schoolsQueryServiceMock;
        private readonly Fixture _fixture;
        public SchoolsControllerTests()
        {
            _schoolServiceMock = new Mock<ISchoolsService>();
            _schoolsQueryServiceMock = new Mock<ISchoolsQueryService>();
            _sut = new SchoolsController(_schoolServiceMock.Object, _schoolsQueryServiceMock.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Create_ValidInput_success()
        {
            _schoolServiceMock.Setup(x => x.Create(It.IsAny<SchoolCreateDto>())).ReturnsAsync(SchoolDto(_fixture));
            var request = SchoolCreateDto(_fixture);

            var output = await _sut.Create(request);

            Assert.NotNull(output);

            Assert.IsType<CreatedResult>(output);

            Assert.NotEmpty(output.Location);

            Assert.NotNull(output.Value);

            Assert.IsType<SchoolDto>(output.Value);
        }
        [Fact]
        public async Task Create_InvalidInput_ThrowsNotValidException()
        {
            _schoolServiceMock.Setup(x => x.Create(It.IsAny<SchoolCreateDto>())).ThrowsAsync(new DataNotValidException());

            await Assert.ThrowsAsync<DataNotValidException>(() => _sut.Create(SchoolCreateDto(_fixture)));
        }

        [Fact]
        public async Task Update_ValidInput_Success()
        {
            _schoolServiceMock.Setup(x => x.Update(It.IsAny<SchoolUpdateDto>())).ReturnsAsync(SchoolDto(_fixture));

            var output = await _sut.Update(SchoolUpdateDto(_fixture));

            Assert.NotNull(output);

            Assert.IsType<ActionResult<SchoolDto>>(output);

            Assert.NotNull(output.Result);

            Assert.IsType<OkObjectResult>(output.Result);
        }

        [Fact]
        public async Task Update_InvalidInput_ThrowsNotFoundException()
        {
            _schoolServiceMock.Setup(x => x.Update(It.IsAny<SchoolUpdateDto>())).ThrowsAsync(new DataNotFoundException());

            await Assert.ThrowsAsync<DataNotFoundException>(() => _sut.Update(SchoolUpdateDto(_fixture)));
        }

        [Fact]
        public async Task Update_InvalidInput_ThrowsNotValidException()
        {
            _schoolServiceMock.Setup(x => x.Update(It.IsAny<SchoolUpdateDto>())).ThrowsAsync(new DataNotValidException());

            await Assert.ThrowsAsync<DataNotValidException>(() => _sut.Update(SchoolUpdateDto(_fixture)));
        }

        [Fact]
        public async Task Delete_ValidInput_Success()
        {
            _schoolServiceMock.Setup(x => x.Delete(It.IsAny<int>())).ReturnsAsync(true);

            var output = await _sut.Delete(_fixture.Create<int>());

            Assert.NotNull(output);

            Assert.IsType<ActionResult<bool>>(output);

            Assert.NotNull(output.Result);

            Assert.IsType<OkObjectResult>(output.Result);
        }

        [Fact]
        public async Task Delete_InvalidInput_ThrowsNotFoundException()
        {
            _schoolServiceMock.Setup(x => x.Delete(It.IsAny<int>())).ThrowsAsync(new DataNotFoundException());

            await Assert.ThrowsAsync<DataNotFoundException>(() => _sut.Delete(_fixture.Create<int>()));
        }

        [Fact]
        public async Task Delete_InvalidInput_ThrowsBusinessException()
        {
            _schoolServiceMock.Setup(x => x.Delete(It.IsAny<int>())).ThrowsAsync(new BusinessException());

            await Assert.ThrowsAsync<BusinessException>(() => _sut.Delete(_fixture.Create<int>()));
        }

        [Fact]
        public async Task AddClass_ValidInput_Success()
        {
            _schoolServiceMock.Setup(x => x.AddClass(It.IsAny<ClassRoomAddDto>())).ReturnsAsync(SchoolDto(_fixture));

            int schoolId = _fixture.Create<int>();
            var classroomAdd = ClassRoomAddDto(_fixture);
            classroomAdd.SchoolId = schoolId;

            var output = await _sut.AddClass(schoolId, classroomAdd);

            Assert.NotNull(output);

            Assert.IsType<ActionResult<SchoolDto>>(output);

            Assert.NotNull(output.Result);

            Assert.IsType<OkObjectResult>(output.Result);
        }

        [Fact]
        public async Task AddClass_MismatchSchoolId_ThrowsNotValidException()
        {
            int schoolId = _fixture.Create<int>();
            var classroomAdd = ClassRoomAddDto(_fixture);
            await Assert.ThrowsAsync<DataNotValidException>(() => _sut.AddClass(schoolId, classroomAdd));
        }

        [Fact]
        public async Task AddClass_InvalidInput_ThrowsDataNotFoundException()
        {
            _schoolServiceMock.Setup(x => x.AddClass(It.IsAny<ClassRoomAddDto>())).ThrowsAsync(new DataNotFoundException());

            int schoolId = _fixture.Create<int>();
            var classroomAdd = ClassRoomAddDto(_fixture);
            classroomAdd.SchoolId = schoolId;

            await Assert.ThrowsAsync<DataNotFoundException>(() => _sut.AddClass(schoolId, classroomAdd));
        }

        [Fact]
        public async Task DeleteClass_ValidInput_Success()
        {
            _schoolServiceMock.Setup(x => x.DeleteClass(It.IsAny<ClassRoomDeleteDto>())).ReturnsAsync(true);

            var output = await _sut.DeleteClass(_fixture.Create<int>(), _fixture.Create<int>());

            Assert.NotNull(output);

            Assert.IsType<ActionResult<bool>>(output);

            Assert.NotNull(output.Result);

            Assert.IsType<OkObjectResult>(output.Result);
        }

        [Fact]
        public async Task DeleteClass_InvalidInput_ThrowsDataNotFoundexception()
        {
            _schoolServiceMock.Setup(x => x.DeleteClass(It.IsAny<ClassRoomDeleteDto>())).ThrowsAsync(new DataNotFoundException());

            await Assert.ThrowsAsync<DataNotFoundException>(() => _sut.DeleteClass(_fixture.Create<int>(), _fixture.Create<int>()));
        }

        [Fact]
        public async Task Get_ValidInput_Success()
        {
            _schoolsQueryServiceMock.Setup(x => x.GetSchool(It.IsAny<int>())).ReturnsAsync(SchoolDto(_fixture));

            var output = await _sut.Get(_fixture.Create<int>());

            Assert.NotNull(output);

            Assert.IsType<ActionResult<SchoolDto>>(output);

            Assert.NotNull(output.Result);

            Assert.IsType<OkObjectResult>(output.Result);
        }

        [Fact]
        public async Task Get_ValidInput_ThrowsDataNotFoundException()
        {
            _schoolsQueryServiceMock.Setup(x => x.GetSchool(It.IsAny<int>())).ThrowsAsync(new DataNotFoundException());

            await Assert.ThrowsAsync<DataNotFoundException>(() => _sut.Get(_fixture.Create<int>()));
        }

        [Fact]
        public async Task Search_ValidInput_Success()
        {
            _schoolsQueryServiceMock.Setup(x => x.SearchSchools(It.IsAny<SchoolSearchDto>())).ReturnsAsync(PagedSchools(_fixture));

            var output = await _sut.Search(SchoolSearchDto(_fixture));

            Assert.NotNull(output);

            Assert.IsType<ActionResult<PagedResponse<SchoolDto>>>(output);

            Assert.NotNull(output.Result);

            Assert.IsType<OkObjectResult>(output.Result);
        }
        private static SchoolDto SchoolDto(Fixture fixture) =>
            fixture.Create<SchoolDto>();

        private static SchoolCreateDto SchoolCreateDto(Fixture fixture) =>
            fixture.Create<SchoolCreateDto>();

        private static SchoolUpdateDto SchoolUpdateDto(Fixture fixture) =>
            fixture.Create<SchoolUpdateDto>();

        private static ClassRoomAddDto ClassRoomAddDto(Fixture fixture) =>
            fixture.Create<ClassRoomAddDto>();

        private static PagedResponse<SchoolDto> PagedSchools(Fixture fixture) =>
            new PagedResponse<SchoolDto>
            {
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 10,
                Data = Enumerable.Range(1, 10).Select(s => SchoolDto(fixture)).ToList()
            };

        private static SchoolSearchDto SchoolSearchDto(Fixture fixture) =>
            fixture.Create<SchoolSearchDto>();
    }
}