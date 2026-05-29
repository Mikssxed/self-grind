using MediatR;
using SelfGrind.Application.Analytics.Dtos;

namespace SelfGrind.Application.Analytics.Queries.GetAnalyticsOverview;

public class GetAnalyticsOverviewQuery : IRequest<AnalyticsOverviewDto>
{
}
