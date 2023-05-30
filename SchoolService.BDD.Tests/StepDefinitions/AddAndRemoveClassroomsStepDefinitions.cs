using AutoFixture;
using SchoolService.Application.Schools.Dtos;
using SchoolService.BDD.Tests.Collections;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace SchoolService.BDD.Tests.StepDefinitions;
[Binding]
[Collection(nameof(SchoolsWebServiceCollectionDefinition))]

public class AddAndRemoveClassroomsStepDefinitions
{
    private readonly HttpClient _httpClient;
    private SchoolCreateDto? _schoolCreateDto;
    private ClassRoomAddDto? _classroomAddDto;
    private int _schoolId;
    private int? _classroomId;
    private SchoolDto? _schoolDto;
    private readonly Fixture _fixture;

    public AddAndRemoveClassroomsStepDefinitions(SchoolsWebService schoolsWebService)
    {
        _httpClient = schoolsWebService._httpClient;
        _fixture = new Fixture();
    }

    [Given(@"I created a school")]
    public async Task GivenICreatedASchool()
    {
        _schoolCreateDto = _fixture.Create<SchoolCreateDto>();
        var httpresponse = await _httpClient.PostAsJsonAsync<SchoolCreateDto>("api/Schools", _schoolCreateDto);

        Assert.Equal(HttpStatusCode.Created, httpresponse.StatusCode);

        _schoolDto = JsonSerializer.Deserialize<SchoolDto>(await httpresponse.Content.ReadAsStringAsync(), new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        Assert.NotNull(_schoolDto);

        _schoolId = _schoolDto!.Id!.Value;

        Assert.True(_schoolId > 0);
    }

    [When(@"I Add classes to school")]
    public async Task WhenIAddClassesToSchool()
    {
        Assert.True(_schoolId > 0);

        _classroomAddDto = new ClassRoomAddDto
        {
            SchoolId = _schoolId,
            Name = _fixture.Create<string>()
        };

        var httpResponse = await _httpClient.PostAsJsonAsync<ClassRoomAddDto>($"api/Schools/{_schoolId}/classes", _classroomAddDto);

        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
    }

    [When(@"I Get school using id")]
    public async Task WhenIGetSchoolUsingId()
    {
        Assert.True(_schoolId > 0);

        var httpResponse = await _httpClient.GetAsync($"api/Schools/{_schoolId}");

        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);

        _schoolDto = JsonSerializer.Deserialize<SchoolDto>(await httpResponse.Content.ReadAsStringAsync(),
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

        Assert.NotNull(_schoolDto);

        if (_schoolDto!.Classes!.Any())
        {
            _classroomId = _schoolDto.Classes!.First(w => w.Name == _classroomAddDto!.Name).Id;
        }

    }

    [Then(@"I should get school containing the class I added")]
    public void ThenIShouldGetSchoolContainingTheClassIAdded()
    {
        Assert.NotNull(_schoolDto);

        Assert.True(_schoolDto!.Classes!.Where(a => a.Name == _classroomAddDto!.Name).Count() > 0);
    }

    [When(@"I remove the class from school")]
    public async Task WhenIRemoveTheClassFromSchool()
    {
        Assert.NotNull(_classroomId);

        var httpResponse = await _httpClient.DeleteAsync($"api/Schools/{_schoolId}/classes/{_classroomId}");

        Assert.True(httpResponse.StatusCode == HttpStatusCode.OK);
    }

    [Then(@"I should get school with the class removed")]
    public void ThenIShouldGetSchoolWithTheClassRemoved()
    {
        Assert.NotNull(_schoolDto);

        Assert.False(_schoolDto!.Classes!.Where(a => a.Name == _classroomAddDto!.Name).Count() > 0);
    }
}
