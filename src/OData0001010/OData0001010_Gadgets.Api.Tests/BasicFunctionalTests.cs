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

namespace OData0001010_Gadgets.Api.Tests
{
    public class BasicFunctionalTests : CustomWebApplicationFactory<TestStartup>
    {
        private readonly HttpClient _httpClient;
        private readonly MediaTypeFormatterCollection formatters;
        private readonly string _baseUrl = "/gadget/get/";

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
            var response = await _httpClient.GetAsync(_baseUrl);

            // I think both are not needed. One of the following should be suffecient.
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ODataWithMetadata_RetunsOk()
        {
            var url = $"{_baseUrl}?$select=Id,ProductName,Cost,Brand,Type";
            var response = await _httpClient.GetAsync(url);
            var stringResponse = await response.Content.ReadAsStringAsync();

            // I think both are not needed. One of the following should be suffecient.
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseStream = await _httpClient.GetStreamAsync(url);
            var gadgets = await JsonSerializer.DeserializeAsync<List<ExpectedGadgetModel>>(responseStream);
            gadgets.Should().NotBeNull();
            gadgets!.Should().NotBeNull();
            gadgets!.Count.Should().BePositive();
            stringResponse.Should().NotBe(string.Empty);
        }

        [Fact]
        public async Task Filter_RetunsOk()
        {
            var url = $"{_baseUrl}?$filter=ProductName eq 'Pen Drive'";
            var response = await _httpClient.GetAsync(url);
            var stringResponse = await response.Content.ReadAsStringAsync();

            // I think both are not needed. One of the following should be suffecient.
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseStream = await _httpClient.GetStreamAsync(url);
            var gadgets = await JsonSerializer.DeserializeAsync<List<ExpectedGadgetModel>>(responseStream);
            gadgets.Should().NotBeNull();
            gadgets!.Should().NotBeNull();
            gadgets!.Count.Should().BePositive();
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
            var gadgets = await JsonSerializer.DeserializeAsync<List<ExpectedGadgetModel>>(responseStream);
            gadgets.Should().NotBeNull();
            gadgets!.Should().NotBeNull();
            gadgets!.Count.Should().BePositive();
            stringResponse.Should().NotBe(string.Empty);
        }


        [Fact]
        public async Task Top_RetunsOk()
        {
            var url = $"{_baseUrl}?$top=2";
            var response = await _httpClient.GetAsync(url);
            var stringResponse = await response.Content.ReadAsStringAsync();

            // I think both are not needed. One of the following should be suffecient.
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseStream = await _httpClient.GetStreamAsync(url);
            var gadgets = await JsonSerializer.DeserializeAsync<List<ExpectedGadgetModel>>(responseStream);
            gadgets.Should().NotBeNull();
            gadgets!.Should().NotBeNull();
            gadgets!.Count.Should().Be(2);
            stringResponse.Should().NotBe(string.Empty);
        }
    }
}
