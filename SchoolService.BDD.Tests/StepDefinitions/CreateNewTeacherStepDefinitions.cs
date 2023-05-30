using AutoFixture;
using Microsoft.AspNetCore.Http;
using SchoolService.Application.Schools.Dtos;
using SchoolService.Application.Teachers.DTOs;
using SchoolService.BDD.Tests.Collections;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TechTalk.SpecFlow;

namespace SchoolService.BDD.Tests.StepDefinitions
{
    [Binding]
    public class CreateNewTeacherStepDefinitions
    {
        private readonly HttpClient _httpClient;
        private TeacherCreateDto? _teacherCreateDto;
        private int? _teacherId;
        private readonly Fixture _fixture;
        private TeacherDto? _teacherDto;
        private HttpResponseMessage? _httpResponseMessage;
        public CreateNewTeacherStepDefinitions(SchoolsWebService schoolsWebService)
        {
            _httpClient = schoolsWebService._httpClient;
            _fixture = new Fixture();
        }

        [When(@"I Create a new teacher with valid data")]
        public async Task WhenICreateANewTeacher()
        {
            _teacherCreateDto = _fixture.Create<TeacherCreateDto>();

            var httpResponse = await _httpClient.PostAsJsonAsync<TeacherCreateDto>("api/Teachers", _teacherCreateDto);

            Assert.Equal(HttpStatusCode.Created, httpResponse.StatusCode);

            TeacherDto? teacher = JsonSerializer.Deserialize<TeacherDto>(await httpResponse.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            Assert.NotNull(teacher);

            _teacherId = teacher?.Id;
        }

        [When(@"I get the teacher sing Id")]
        public async Task WhenIGetTheTeacherSingId()
        {
            var httpResponse = await _httpClient.GetAsync("api/Teachers/" + _teacherId);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);

            _teacherDto = JsonSerializer.Deserialize<TeacherDto>(await httpResponse.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            Assert.NotNull(_teacherDto);
        }

        [Then(@"I should get the teacher")]
        public void ThenIShouldGetTheTeacher()
        {
            Assert.NotNull(_teacherDto);
            Assert.Equal(_teacherCreateDto?.Name, _teacherDto?.Name);
        }

        [When(@"I Create a new teacher using invalid data")]
        public async Task WhenICreateANewTeacherUsingInvalidData()
        {
            _teacherCreateDto = new TeacherCreateDto();

            _httpResponseMessage = await _httpClient.PostAsJsonAsync<TeacherCreateDto>("api/Teachers", _teacherCreateDto);
        }

        [Then(@"I should get data not valid response")]
        public void ThenIShouldGetDataNotValidResponse()
        {
            Assert.NotNull(_httpResponseMessage);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, _httpResponseMessage?.StatusCode);

        }
    }
}
