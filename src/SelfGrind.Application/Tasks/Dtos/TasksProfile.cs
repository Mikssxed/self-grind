using SelfGrind.Application.Tasks.Commands.CreateTask;
using SelfGrind.Domain.Entities;

namespace SelfGrind.Application.Tasks.Dtos;
using AutoMapper;

public class TasksProfile : Profile
{
    public TasksProfile()
    {
        CreateMap<CreateTaskCommand, TaskItem>()
            .ForMember(d => d.Schedule, opt => opt.MapFrom(src => new TaskSchedule
            {
                StartDate = src.StartDate,
                EndDate = src.EndDate,
                RepetitionType =  src.RepetitionType,
                RepeatInterval = src.RepeatInterval,
                DaysOfWeek = src.DaysOfWeek
            }));
    }
    
}