using AutoFixture;
using SchoolService.Application.Schools.Dtos;
using SchoolService.BDD.Tests.Collections;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace SchoolService.BDD.Tests.StepDefinitions;

[Binding]
[Collection(nameof(SchoolsWebServiceCollectionDefinition))]
public sealed class GetSchoolFeatureStepDefinitions
{
    private readonly HttpClient _httpClient;
    private SchoolCreateDto? _schoolCreateDto;
    private SchoolDto? _schoolDto;
    private int schoolId;
    private readonly Fixture _fixture;

    public GetSchoolFeatureStepDefinitions(SchoolsWebService schoolsWebService)
    {
        _httpClient = schoolsWebService._httpClient;
        _fixture = new Fixture();
    }
    [When(@"I create a school")]
    public async Task WhenICreateASchool()
    {
        _schoolCreateDto = _fixture.Create<SchoolCreateDto>();
        var httpResponse = await _httpClient.PostAsJsonAsync<SchoolCreateDto>("api/Schools", _schoolCreateDto);

        Assert.Equal(HttpStatusCode.Created, httpResponse.StatusCode);
        var school = JsonSerializer.Deserialize<SchoolDto>(await httpResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        Assert.NotNull(school);

        Assert.NotNull(school!.Id);

        schoolId = school!.Id!.Value;
    }

    [When(@"I get a school with id")]
    public async Task WhenIGetASchoolWithId()
    {
        var httpResponse = await _httpClient.GetAsync("api/Schools/" + schoolId);

        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);

        var school = JsonSerializer.Deserialize<SchoolDto>(await httpResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        Assert.NotNull(school);

        _schoolDto = school;
    }

    [Then(@"I should Get the school as a response")]
    public void ThenIShouldGetTheSchoolAsAResponse()
    {
        Assert.NotNull(_schoolCreateDto);

        Assert.NotNull(_schoolDto);

        Assert.Equal(_schoolDto!.Name, _schoolCreateDto!.Name);

        Assert.Equal(_schoolDto!.OpenDate, _schoolCreateDto!.OpenDate);

        Assert.Equal(schoolId, _schoolDto!.Id);
    }

    [When(@"I get a school with invalid id")]
    public async Task WhenIGetASchoolWithInvalidId()
    {
        var httpresponse = await _httpClient.GetAsync("api/Schools/-1");

        Assert.Equal(HttpStatusCode.NotFound, httpresponse.StatusCode);

        _schoolDto = null;
    }

    [Then(@"I get response not found")]
    public void ThenIGetResponseNotFound()
    {
        Assert.Null(_schoolDto);
    }
}
