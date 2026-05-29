using MediatR;
using SelfGrind.Application.Analytics.Dtos;

namespace SelfGrind.Application.Analytics.Queries.GetLifeBalance;

public class GetLifeBalanceQuery : IRequest<LifeBalanceDto>
{
}
