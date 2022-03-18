using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Boxed.AspNetCore;
using FluentAssertions;
using Xunit;
using System.Text.Json;
using System.Collections.Generic;

namespace OdataExpandDemo.Tests
{
    public class PeopleWithAccountsTests : CustomWebApplicationFactory<TestStartup> 
    {
        private readonly HttpClient _httpClient;
        private readonly MediaTypeFormatterCollection formatters;
        private readonly string _baseUrl = "/api/people/";
        public PeopleWithAccountsTests()
        {
            _httpClient = CreateClient();
            formatters = new MediaTypeFormatterCollection();
            formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.RestfulJson));
            formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.ProblemJson));
        }
        [Fact]
        public async Task PeopleWithBankAccounts_RetunsOk()
        {
            // GET {{baseUrlWithPeople}}/person?$expand=BankAccounts($select=Id, BankName)
            var url = $"{_baseUrl}person?$expand=BankAccounts($select=Id, BankName)";
            var response = await _httpClient.GetAsync(url);
            var stringResponse = await response.Content.ReadAsStringAsync();

            // I think both are not needed. One of the following should be suffecient.
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseStream = await _httpClient.GetStreamAsync(url);
            var people = await JsonSerializer.DeserializeAsync<List<PersonModel>>(responseStream);
            people!.Should().NotBeNull();
            people!.Count.Should().BePositive();
            people![0].Name.Should().NotBeEmpty();
            people![0].BankAccounts.Count.Should().BePositive();
            stringResponse.Should().NotBe(string.Empty);
        }
    }
}