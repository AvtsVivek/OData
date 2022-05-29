using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Boxed.AspNetCore;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Text;
using System.Text.Json;
using System;
using System.Linq;

namespace AirVinyl.Tests
{

    public class QueryTests : CustomWebApplicationFactory<TestStartup>
    {
        private readonly HttpClient _httpClient;
        private readonly MediaTypeFormatterCollection formatters;

        public QueryTests()
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
        public async Task SelectQueryForEmailOnPersonEntity_RetunsOk()
        {
            var response = await _httpClient.GetAsync("/odata/people?$select=Email");
            var responseStream = await _httpClient.GetStreamAsync("/odata/people?$select=Email");
            var people = await JsonSerializer.DeserializeAsync<ExpectedPeopleModel>(responseStream);


            var stringResponse = await response.Content.ReadAsStringAsync();
            people.Should().NotBeNull();
            people!.value.Should().NotBeNull();
            people!.value.Count.Should().BePositive();
            stringResponse.Should().NotBe(String.Empty);
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            people!.value.ToArray()[0].Email.Should().NotBeNullOrEmpty();
            people!.value.ToArray()[1].Email.Should().NotBeNullOrEmpty();
            // We expect other values to be null
            people!.value.ToArray()[0].FirstName.Should().BeNull();
            people!.value.ToArray()[0].LastName.Should().BeNull();
            people!.value.ToArray()[1].FirstName.Should().BeNull();
            people!.value.ToArray()[1].LastName.Should().BeNull();
        }
    }

    public class CreateUpdateDeleteTests : CustomWebApplicationFactory<TestStartup>
    {
        private readonly HttpClient _httpClient;
        private readonly MediaTypeFormatterCollection formatters;

        public CreateUpdateDeleteTests()
        {
            _httpClient = CreateClient();
            formatters = new MediaTypeFormatterCollection();
            formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.RestfulJson));
            formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.ProblemJson));
        }

        protected override void ConfigureInMemoryDatabase(IServiceCollection services, bool bConfigureDb = true)
        {
            base.ConfigureInMemoryDatabase(services, false);
        }

        [Fact]
        public async Task CreatePersonEntity_RetunsOk()
        {
            var jsonString = $"{{ " +
                $"\"FirstName\":\"John\", " +
                $"\"LastName\":\"Smith\", " +
                $"\"Email\": \"john.smith@someprovider.com\", " +
                $"\"Gender\":\"Male\", " +
                $"\"DateOfBirth\": \"1980-01-30\" " +
                $"}}";

            var data = new StringContent(jsonString, Encoding.UTF8, "application/json");
            //We turn an object into a JSON data with the help of the Newtonsoft.Json package.

            var response = await _httpClient.PostAsync("/odata/people", data);
            //We send an asynchronous POST request with the PostAsync method.

            var result = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(result);

            // I think both are not needed. One of the following should be suffecient.
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task CreatePersonWithoutFirstName_RetunsBadRequest()
        {
            var jsonString = $"{{ " +
                $"\"LastName\":\"Smith\", " +
                $"\"Email\": \"john.smith@someprovider.com\", " +
                $"\"Gender\":\"Male\", " +
                $"\"DateOfBirth\": \"1980-01-30\" " +
                $"}}";

            var data = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/odata/people", data);

            // I think both are not needed. One of the following should be suffecient.
            // response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdatePersonWithPutWithoutEmail_RetunsBadRequest()
        {
            var jsonString = 
                $"{{ " +
                $"\"FirstName\": \"Nick\", " +
                $"\"LastName\":\"Missorten\", " +
                $"\"DateOfBirth\": \"1983-05-18T00:00:00+02:00\", " +
                $"\"Gender\":\"Male\", " +
                $"\"NumberOfRecordsOnWishList\": 23, " +
                $"\"AmountOfCashToSpend\": 2500 " +
                $"}}";

            var data = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("/odata/people(3)", data);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Now get the same entity and check. The email should be empty. 

            var getResponseStream = await _httpClient.GetStreamAsync("/odata/people(3)");
            var person = await JsonSerializer.DeserializeAsync<ExpectedPersonModel>(getResponseStream);
            person.Should().NotBeNull();
            person!.Email.Should().BeNull();
            person!.FirstName.Should().Be("Nick");
            person!.LastName.Should().Be("Missorten");
        }

        [Fact]
        public async Task UpdatePersonWithPutWithEmail_RetunsBadRequest()
        {
            var jsonString =
                $"{{ " +
                $"\"FirstName\": \"Nick\", " +
                $"\"LastName\":\"Missorten\", " +
                $"\"DateOfBirth\": \"1983-05-18\", " +
                $"\"Gender\":\"Male\", " +
                $"\"NumberOfRecordsOnWishList\": 23, " +
                $"\"Email\": \"asdfasdf@hasdf.commm\", " +
                $"\"AmountOfCashToSpend\": 2500 " +
                $"}}";

            var data = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("/odata/people(3)", data);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Now get the same entity and check. The email should be empty. 

            var getResponseStream = await _httpClient.GetStreamAsync("/odata/people(3)");
            var person = await JsonSerializer.DeserializeAsync<ExpectedPersonModel>(getResponseStream);
            person.Should().NotBeNull();
            //person!.Email.Should().BeNull();
            person!.FirstName.Should().Be("Nick");
            person!.LastName.Should().Be("Missorten");
            person!.Email.Should().Be("asdfasdf@hasdf.commm");
        }

        [Fact]
        public async Task DeletePerson_RetunsNoContent()
        {
            var getResponseBefore = await _httpClient.GetAsync("/odata/people(3)");
            getResponseBefore.StatusCode.Should().Be(HttpStatusCode.OK);

            var response = await _httpClient.DeleteAsync("/odata/people(3)");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponseAfter = await _httpClient.GetAsync("/odata/people(3)");
            getResponseAfter.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}
