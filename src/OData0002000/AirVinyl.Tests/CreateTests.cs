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

namespace AirVinyl.Tests
{
    public class CreateTests : CustomWebApplicationFactory<TestStartup>
    {
        private readonly HttpClient _httpClient;
        private readonly MediaTypeFormatterCollection formatters;

        public CreateTests()
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
        public async Task OData_RetunsOk()
        {

            /*
### odata
POST https://localhost:5001/odata/People HTTP/1.1
Accept: application/json
Content-Type: application/json

{    
"FirstName":"John",
"LastName":"Smith",
"Email": "john.smith@someprovider.com",    
"Gender":"Male",
"DateOfBirth": "1980-01-30"
}         
 */


            //var response = await _httpClient.GetAsync("/odata/people");

            //var person = new Person("John Doe", "gardener");
            //var json = JsonConvert.SerializeObject(person);

            var jsonString = $"{{ " +
                $"\"FirstName\":\"John\", \"LastName\":\"Smith\", \"Email\": \"john.smith@someprovider.com\", " +
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

    }
}
