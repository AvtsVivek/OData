using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using SimpleAirVinyl.Entities;

namespace SimpleAirVinyl.EntityDataModel
{
    public class AirVinylEntityDataModel
    {
        public IEdmModel GetEntityDataModel()
        {
            var builder = new ODataConventionModelBuilder();

            //builder.Namespace = "SimpleAirVinyl.Entities";
            //builder.ContainerName = "AirVinylContainer";

            builder.EntitySet<Person>("People");
            builder.EntitySet<VinylRecord>("VinylRecords");

            return builder.GetEdmModel();
        }
    }
}
