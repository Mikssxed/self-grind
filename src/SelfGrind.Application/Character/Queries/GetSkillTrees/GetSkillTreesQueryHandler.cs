using MediatR;
using Microsoft.Extensions.Logging;
using SelfGrind.Application.Character.Constants;
using SelfGrind.Application.Character.Dtos;
using SelfGrind.Application.Character.Services;
using SelfGrind.Application.Common;
using SelfGrind.Application.User;
using SelfGrind.Domain.Constants;
using SelfGrind.Domain.Entities;
using SelfGrind.Domain.Repositories;

namespace SelfGrind.Application.Character.Queries.GetSkillTrees;

public class GetSkillTreesQueryHandler(
    ILogger<GetSkillTreesQueryHandler> logger,
    ISkillsRepository skillsRepository,
    IUsersRepository usersRepository,
    IUserContext userContext) : IRequestHandler<GetSkillTreesQuery, SkillCategoryDto[]>
{
    public async Task<SkillCategoryDto[]> Handle(GetSkillTreesQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Handling GetSkillTreesQuery for user {userId}", currentUser.Id);

        var user = await usersRepository.GetWithStatsReadOnlyOrThrowAsync(currentUser.Id, cancellationToken);

        var skills = await skillsRepository.GetAllOrderedAsync(cancellationToken);
        var statsByAttribute = user.Stats.ToDictionary(s => s.Attribute, s => s.Level);

        return skills
            .GroupBy(skill => skill.Attribute)
            .OrderBy(group => (int)group.Key)
            .Select(group => BuildCategory(group.Key, group.ToArray(), statsByAttribute))
            .ToArray();
    }

    private static SkillCategoryDto BuildCategory(
        BaseAttribute attribute,
        IReadOnlyList<Skill> skills,
        IReadOnlyDictionary<BaseAttribute, int> statsByAttribute)
    {
        var (emoji, variant) = SkillCategoryMetadata.For(attribute);
        var userLevel = statsByAttribute.TryGetValue(attribute, out var level) ? level : 1;

        return new SkillCategoryDto
        {
            Attribute = attribute,
            Name = attribute.ToString(),
            Emoji = emoji,
            Variant = variant,
            Skills = skills
                .OrderBy(s => s.Order)
                .Select(skill => new SkillDto
                {
                    Name = skill.Name,
                    Emoji = skill.Emoji,
                    Description = skill.Description,
                    Status = userLevel >= skill.RequiredAttributeLevel ? SkillStatus.Unlocked : SkillStatus.Locked,
                })
                .ToArray()
        };
    }
}
