using MediatR;
using SelfGrind.Application.Character.Dtos;

namespace SelfGrind.Application.Character.Queries.GetSkillTrees;

public class GetSkillTreesQuery : IRequest<SkillCategoryDto[]>;
