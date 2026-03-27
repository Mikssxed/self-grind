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
        
        CreateMap<TaskItem, TaskItemDto>()
            .ForMember(tid => tid.StartDate, opt => opt.MapFrom(src => src.Schedule.StartDate))
            .ForMember(tid => tid.EndDate, opt => opt.MapFrom(src => src.Schedule.EndDate))
            .ForMember(tid => tid.RepetitionType, opt => opt.MapFrom(src => src.Schedule.RepetitionType))
            .ForMember(tid => tid.RepeatInterval, opt => opt.MapFrom(src => src.Schedule.RepeatInterval))
            .ForMember(tid => tid.DaysOfWeek, opt => opt.MapFrom(src => src.Schedule.DaysOfWeek));
    }
}