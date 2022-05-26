using Ardalis.ApiEndpoints;
using OData.Core.ProjectAggregate;
using OData.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using OData.Core.ProjectAggregate.Specifications;
using Microsoft.AspNetCore.OData.Query;

namespace OData.Web.Endpoints.ProjectEndpoints;

public class ListProjectsOData : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<ProjectListResponseOData>
{
  private readonly IReadRepository<Project> _repository;

  public ListProjectsOData(IReadRepository<Project> repository)
  {
    _repository = repository;
  }

  [HttpGet("/ProjectsWithOData")]
  [EnableQuery]
  [SwaggerOperation(
      Summary = "Gets a list of all Projects",
      Description = "Gets a list of all Projects",
      OperationId = "Project.ListOdata",
      Tags = new[] { "ProjectEndpoints" })
  ]
  public override async Task<ActionResult<ProjectListResponseOData>> HandleAsync(CancellationToken cancellationToken)
  {
    var response = new ProjectListResponseOData();

    var spec = new BlankProjectSpec();

    var query = _repository.GetQueryBySpec(spec);

    return Ok(query);

    //response.Projects = (await _repository.ListAsync()) // TODO: pass cancellation token
    //    .Select(project => new ProjectRecord(project.Id, project.Name))
    //    .ToList();

    //return Ok(response);
  }
}
