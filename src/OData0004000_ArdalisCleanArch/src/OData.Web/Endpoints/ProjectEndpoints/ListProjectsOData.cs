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
    .WithActionResult<IQueryable<Project>>
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


  //public override async Task<ActionResult<IQueryable<Project>>> HandleAsync(CancellationToken cancellationToken)
  //{
  //  var spec = new BlankProjectSpec();
  //  IQueryable<Project> query = _repository.GetQueryBySpec(spec);
  //  return Ok(query);
  //}

  public override Task<ActionResult<IQueryable<Project>>> HandleAsync(CancellationToken cancellationToken)
  {
    var spec = new BlankProjectSpec();
    IQueryable<Project> query = _repository.GetQueryBySpec(spec);
    return Task.FromResult<ActionResult<IQueryable<Project>>>(Ok(query));
  }

  //public override Task<ActionResult<IQueryable<Project>>> HandleAsync(CancellationToken cancellationToken)
  //{

  //  var spec = new BlankProjectSpec();

  //  IQueryable<Project> query = _repository.GetQueryBySpec(spec);
  //  return Task.FromResult<ActionResult<IQueryable<Project>>>(Ok(query));
  //  //return Ok(query);

  //  //response.Projects = (await _repository.ListAsync()) // TODO: pass cancellation token
  //  //    .Select(project => new ProjectRecord(project.Id, project.Name))
  //  //    .ToList();

  //  //return Ok(response);
  //}
}
