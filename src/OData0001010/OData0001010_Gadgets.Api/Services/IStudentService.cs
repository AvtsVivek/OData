using System.Linq;
using OData0001010_Gadgets.Api.Models;

namespace OData0001010_Gadgets.Api.Services
{
    public interface IGadgetsService
    {
        IQueryable<Product> RetrieveAllGadgets();
    }
}
