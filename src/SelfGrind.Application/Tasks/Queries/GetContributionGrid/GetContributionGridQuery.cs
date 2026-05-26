using MediatR;
using SelfGrind.Application.Tasks.Dtos;

namespace SelfGrind.Application.Tasks.Queries.GetContributionGrid;

public class GetContributionGridQuery : IRequest<ContributionGridDto>
{
    public int Year { get; set; }
}
