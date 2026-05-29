using MediatR;
using SelfGrind.Application.Analytics.Dtos;

namespace SelfGrind.Application.Analytics.Queries.GetStatGrowth;

public class GetStatGrowthQuery : IRequest<StatGrowthDto>
{
    public int Weeks { get; set; } = 6;
}
