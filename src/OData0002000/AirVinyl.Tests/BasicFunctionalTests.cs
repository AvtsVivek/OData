using System;
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

namespace AirVinyl.Tests
{
    public class BasicFunctionalTests : CustomWebApplicationFactory<TestStartup>
    {
        private readonly HttpClient _httpClient;
        private readonly MediaTypeFormatterCollection formatters;

        public BasicFunctionalTests()
        {
            _httpClient = CreateClient();
            formatters = new MediaTypeFormatterCollection();
            formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.RestfulJson));
            formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.ProblemJson));
        }

        protected override void ConfigureInMemoryDatabase(IServiceCollection services, bool bConfigureDb = true)
        {
            base.ConfigureInMemoryDatabase(services, true);
        }

        [Fact]
        public async Task OData_RetunsOk()
        {
            var response = await _httpClient.GetAsync("/odata");

            // I think both are not needed. One of the following should be suffecient.
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ODataWithMetadata_RetunsOk()
        {
            var response = await _httpClient.GetAsync("/odata/$metadata");

            // I think both are not needed. One of the following should be suffecient.
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ODataPeople_RetunsOk()
        {
            var response = await _httpClient.GetAsync("/odata/people");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var responseStream = await _httpClient.GetStreamAsync("/odata/people");
            var people = await JsonSerializer.DeserializeAsync<ExpectedPeopleModel>(responseStream);
            people.Should().NotBeNull();
            people!.value.Should().NotBeNull();
            people!.value.Count.Should().BePositive();
            stringResponse.Should().NotBe(String.Empty);
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ODataPeopleMetadata_RetunsOk()
        {
            var response = await _httpClient.GetAsync("/odata/$metadata#People");
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task VinylRecords_RetunsOk()
        {
            var response = await _httpClient.GetAsync("/odata/vinylrecords");
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task MetadataVinylRecords_RetunsOk()
        {
            var response = await _httpClient.GetAsync("/odata/$metadata#VinylRecords");
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task SpecificPerson_RetunsOk()
        {
            var response = await _httpClient.GetAsync("/odata/people(1)");
            var responseStream = await _httpClient.GetStreamAsync("/odata/people(1)");
            var person = await JsonSerializer.DeserializeAsync<ExpectedPersonModel>(responseStream);
            person.Should().NotBeNull();
            person!.PersonId.Should().BePositive();
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task SpecificVinylRecords_RetunsOk()
        {
            var response = await _httpClient.GetAsync("/odata/VinylRecords(1)");

            var responseStream = await _httpClient.GetStreamAsync("/odata/VinylRecords(1)");
            var vinylRecord = await JsonSerializer.DeserializeAsync<ExpectedVinylRecordModel>(responseStream);
            vinylRecord.Should().NotBeNull();
            vinylRecord!.PersonId.Should().BePositive();
            vinylRecord!.VinylRecordId.Should().BePositive();
            vinylRecord!.Year.Should().BePositive();

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetProperty_ThatDoesNotExist_RetunsNotFound()
        {
            var response = await _httpClient.GetAsync("/odata/people(1)/UnexistingProperty");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetEmailProperty_RetunsOK()
        {
            var response = await _httpClient.GetAsync("/odata/people(1)/Email");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetEmailPropertyValue_RetunsOK()
        {
            var response = await _httpClient.GetAsync("/odata/people(1)/Email/$Value");
            var stringResponse = await response.Content.ReadAsStringAsync();
            stringResponse.Should().NotBe(String.Empty);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetEmail_ForPersonThatDoesNotHaveEmail_RetunsNoContent()
        {
            var response = await _httpClient.GetAsync("/odata/people(6)/Email");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GetEmailOfPersonWhoDoesNotExist_RetunsNotFound()
        {
            var response = await _httpClient.GetAsync("/odata/people(20)/Email");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        //  GET https://localhost:5001/odata/people(1)/VinylRecords
        //  Accept: application/json;odata.metadata=none

        [Fact]
        public async Task GetCollectionOfVinylProps_RetunsOK()
        {
            var response = await _httpClient.GetAsync("/odata/people(1)/VinylRecords");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var responseStream = await _httpClient.GetStreamAsync("/odata/people(1)/VinylRecords");
            var vinylRecords = await JsonSerializer.DeserializeAsync<ExpectedVinylRecordCollectionModel>(responseStream);
            stringResponse.Should().NotBe(String.Empty);
            vinylRecords.Should().NotBeNull();
            vinylRecords!.value.Should().NotBeNull();
            vinylRecords!.value.Count.Should().BePositive();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

    }
}
