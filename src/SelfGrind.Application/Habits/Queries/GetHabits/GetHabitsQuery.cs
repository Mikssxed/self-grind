using MediatR;
using SelfGrind.Application.Tasks.Dtos;

namespace SelfGrind.Application.Habits.Queries.GetHabits;

public class GetHabitsQuery : IRequest<HabitDto[]>
{
    
}