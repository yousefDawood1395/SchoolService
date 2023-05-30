using AutoFixture;
using SchoolService.Application.Schools.Dtos;
using SchoolService.BDD.Tests.Collections;
using SchoolService.Domain.Shared.Pagination;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace SchoolService.BDD.Tests.StepDefinitions;

[Binding]
[Collection(nameof(SchoolsWebServiceCollectionDefinition))]
public class SearchSchoolsFeatureStepDefinitions
{
    private readonly HttpClient _httpClient;
    private readonly List<SchoolCreateDto> _createdSchools;
    private readonly Fixture _fixture;
    private PagedResponse<SchoolDto>? _searchResults;

    public SearchSchoolsFeatureStepDefinitions(SchoolsWebService schoolsWebService)
    {
        _httpClient = schoolsWebService._httpClient;
        _createdSchools = new List<SchoolCreateDto>();
        _fixture = new Fixture();
    }

    [Given(@"I have a list of schools in my database")]
    public async Task GivenIHaveAListOfSchoolsInMyDatabase()
    {
        _createdSchools.AddRange(Enumerable.Range(1, 10).Select(s => _fixture.Create<SchoolCreateDto>()).ToList());

        foreach (var item in _createdSchools)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync<SchoolCreateDto>("api/Schools", item);
            Assert.Equal(HttpStatusCode.Created, httpResponse.StatusCode);
        }
    }

    [When(@"I search for schools using school name")]
    public async Task WhenISearchForSchoolsUsingSchoolName()
    {
        var httpResponse = await _httpClient.GetAsync($"api/Schools/Search?Name={_createdSchools.First().Name}&PageNo={1}&PageSize={10}");

        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);

        _searchResults = JsonSerializer.Deserialize<PagedResponse<SchoolDto>>(await httpResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    [Then(@"I should get results satisfying the search terms")]
    public void ThenIShouldGetResultsSatisfyingTheSearchTerms()
    {
        Assert.NotNull(_searchResults);

        Assert.True(_searchResults?.TotalCount > 0);
    }

    [When(@"I search for school using invalid name")]
    public async Task WhenISearchForSchoolUsingInvalidName()
    {
        var httpResponse = await _httpClient.GetAsync($"api/Schools/Search?Name=non existing name{_fixture.Create<string>()}&PageNo={1}&PageSize={10}");

        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);

        _searchResults = JsonSerializer.Deserialize<PagedResponse<SchoolDto>>(await httpResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    [Then(@"I should get and empty school list result")]
    public void ThenIShouldGetAndEmptySchoolListResult()
    {
        Assert.NotNull(_searchResults);

        Assert.True(_searchResults?.TotalCount == 0);
    }
}
