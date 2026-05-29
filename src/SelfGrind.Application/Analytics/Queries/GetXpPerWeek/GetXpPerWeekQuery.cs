using MediatR;
using SelfGrind.Application.Analytics.Dtos;

namespace SelfGrind.Application.Analytics.Queries.GetXpPerWeek;

public class GetXpPerWeekQuery : IRequest<XpPerWeekDto>
{
    public int Weeks { get; set; } = 6;
}
