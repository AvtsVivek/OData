using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Linq;

namespace ODataCoreFaq.Service.Controllers
{
    public class CustomersController : ODataController
    {
        private readonly OrderManagementContext db;

        public CustomersController(OrderManagementContext context)
        {
            db = context;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(db.Customers);

            // This is how you would return OData count property:
            // return Ok(new PageResult<Customer>(db.Customers, null, count: db.Customers.Count()));
        }


        [EnableQuery]
        [HttpGet]
        public IActionResult OrderedBike()
        {
            // We can specify in our service, very complex queries, behind an easy to consume name.
            // The following returns a list of customers, who have atleast one order, who have atleast one order detail with Bike
            return Ok(from c in db.Customers
                      where c.Orders.Any(o => o!.OrderDetails!.Any(od => od!.Product!.CategoryCode == "BIKE"))
                      select c);
        }
    }
}
