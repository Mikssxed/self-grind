using AutoMapper;
using SelfGrind.Application.Tasks.Commands.CreateTask;
using SelfGrind.Application.Tasks.Commands.UpdateTask;
using SelfGrind.Domain.Entities;

namespace SelfGrind.Application.Tasks.Dtos;

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

        CreateMap<UpdateTaskCommand, TaskItem>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.UserId, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.IsArchived, opt => opt.Ignore())
            .ForMember(d => d.ArchivedAt, opt => opt.Ignore())
            .ForMember(d => d.Occurrences, opt => opt.Ignore())
            .ForMember(d => d.Schedule, opt => opt.Ignore())
            .ForMember(d => d.User, opt => opt.Ignore())
            .ForMember(d => d.IsCompleted, opt => opt.Ignore());

        CreateMap<TaskOccurrence, TodayTaskItemDto>()
            .ForMember(d => d.OccurrenceId, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.OccurrenceStatus, opt => opt.MapFrom(src => src.Status))
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.TaskItem.Id))
            .ForMember(d => d.Title, opt => opt.MapFrom(src => src.TaskItem.Title))
            .ForMember(d => d.Description, opt => opt.MapFrom(src => src.TaskItem.Description))
            .ForMember(d => d.Exp, opt => opt.MapFrom(src => src.TaskItem.Exp))
            .ForMember(d => d.Attribute, opt => opt.MapFrom(src => src.TaskItem.Attribute))
            .ForMember(d => d.IsCompleted, opt => opt.MapFrom(src => src.TaskItem.IsCompleted))
            .ForMember(d => d.StartDate, opt => opt.MapFrom(src => src.TaskItem.Schedule.StartDate))
            .ForMember(d => d.EndDate, opt => opt.MapFrom(src => src.TaskItem.Schedule.EndDate))
            .ForMember(d => d.RepetitionType, opt => opt.MapFrom(src => src.TaskItem.Schedule.RepetitionType))
            .ForMember(d => d.RepeatInterval, opt => opt.MapFrom(src => src.TaskItem.Schedule.RepeatInterval))
            .ForMember(d => d.DaysOfWeek, opt => opt.MapFrom(src => src.TaskItem.Schedule.DaysOfWeek));
    }
}