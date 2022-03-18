using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Boxed.AspNetCore;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Text.Json;
using System.Collections.Generic;

namespace OdataExpandDemo.Tests
{

    public class PeopleTests : CustomWebApplicationFactory<TestStartup>
    {
        private readonly HttpClient _httpClient;
        private readonly MediaTypeFormatterCollection formatters;
        private readonly string _baseUrl = "/api/people/";
        private readonly string _odataUrl = "/OData/";

        public PeopleTests()
        {
            _httpClient = CreateClient();
            formatters = new MediaTypeFormatterCollection();
            formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.RestfulJson));
            formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.ProblemJson));
        }

        [Fact]
        public async Task StudentsApiCall_RetunsOk()
        {
            var response = await _httpClient.GetAsync(_baseUrl);

            // I think both are not needed. One of the following should be suffecient.
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task StudentsApiSelectName_RetunsOk()
        {
            var url = $"{_baseUrl}?$select=name, Id";
            var response = await _httpClient.GetAsync(url);
            var stringResponse = await response.Content.ReadAsStringAsync();

            // I think both are not needed. One of the following should be suffecient.
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseStream = await _httpClient.GetStreamAsync(url);
            var students = await JsonSerializer.DeserializeAsync<List<PersonModel>>(responseStream);
            students!.Should().NotBeNull();
            students!.Count.Should().BePositive();
            students![0].Name.Should().NotBeEmpty();
            stringResponse.Should().NotBe(string.Empty);
        }

        [Fact]
        public async Task Filter_RetunsOk()
        {
            var url = $"{_baseUrl}?$filter=name eq 'Vishu Goli'";
            var response = await _httpClient.GetAsync(url);
            var stringResponse = await response.Content.ReadAsStringAsync();

            // I think both are not needed. One of the following should be suffecient.
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseStream = await _httpClient.GetStreamAsync(url);
            var students = await JsonSerializer.DeserializeAsync<List<PersonModel>>(responseStream);
            students.Should().NotBeNull();
            students!.Should().NotBeNull();
            students!.Count.Should().BePositive();
            stringResponse.Should().NotBe(string.Empty);
        }

        [Fact]
        public async Task OrderById_RetunsOk()
        {
            var url = $"{_baseUrl}?$orderby=Id desc";
            var response = await _httpClient.GetAsync(url);
            var stringResponse = await response.Content.ReadAsStringAsync();

            // I think both are not needed. One of the following should be suffecient.
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseStream = await _httpClient.GetStreamAsync(url);
            var students = await JsonSerializer.DeserializeAsync<List<PersonModel>>(responseStream);
            students.Should().NotBeNull();
            students!.Should().NotBeNull();
            students!.Count.Should().BePositive();
            stringResponse.Should().NotBe(string.Empty);
        }


        [Fact]
        public async Task TopFour_RetunsOk()
        {
            var url = $"{_baseUrl}?$top=4";
            var response = await _httpClient.GetAsync(url);
            var stringResponse = await response.Content.ReadAsStringAsync();

            // I think both are not needed. One of the following should be suffecient.
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseStream = await _httpClient.GetStreamAsync(url);
            var students = await JsonSerializer.DeserializeAsync<List<PersonModel>>(responseStream);
            students.Should().NotBeNull();
            students!.Should().NotBeNull();
            students!.Count.Should().Be(4);
            stringResponse.Should().NotBe(string.Empty);
        }

        [Fact]
        public async Task TopSix_RetunsBadRequest()
        {
            var url = $"{_baseUrl}?$top=6";
            var response = await _httpClient.GetAsync(url);
            var stringResponse = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task FilterLessThan_RetunsBadRequest()
        {
            var url = $"{_baseUrl}?$filter=score lt 165";
            var response = await _httpClient.GetAsync(url);
            var stringResponse = await response.Content.ReadAsStringAsync();

            // I think both are not needed. One of the following should be suffecient.
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseStream = await _httpClient.GetStreamAsync(url);
            var students = await JsonSerializer.DeserializeAsync<List<PersonModel>>(responseStream);
            students.Should().NotBeNull();
            students!.Should().NotBeNull();
            students!.Count.Should().Be(3);
            stringResponse.Should().NotBe(string.Empty);
        }

        [Fact]
        public async Task Orderby_RetunsBadRequest()
        {
            var url = $"{_baseUrl}?$orderby=Name desc";
            var response = await _httpClient.GetAsync(url);
            var stringResponse = await response.Content.ReadAsStringAsync();

            // I think both are not needed. One of the following should be suffecient.
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseStream = await _httpClient.GetStreamAsync(url);
            var students = await JsonSerializer.DeserializeAsync<List<PersonModel>>(responseStream);
            students.Should().NotBeNull();
            students!.Should().NotBeNull();
            students!.Count.Should().Be(6);
            stringResponse.Should().NotBe(string.Empty);
        }

        [Fact]
        public async Task Odata_ReturnsOk()
        {
            var url = $"{_odataUrl}";
            var response = await _httpClient.GetAsync(url);
            var stringResponse = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}