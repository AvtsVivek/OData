using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using SimpleAirVinyl.Entities;

namespace SimpleAirVinyl.EntityDataModel
{
    public class SimpleAirVinylEntityDataModel
    {
        public IEdmModel GetEntityDataModel()
        {
            var builder = new ODataConventionModelBuilder();

            builder.Namespace = "SimpleAirVinyl";
            builder.ContainerName = "AirVinylContainer";

            builder.EntitySet<Person>("People");

            return builder.GetEdmModel();
        }
    }
}
