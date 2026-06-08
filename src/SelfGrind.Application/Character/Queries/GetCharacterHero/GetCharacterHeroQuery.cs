using MediatR;
using SelfGrind.Application.Character.Dtos;

namespace SelfGrind.Application.Character.Queries.GetCharacterHero;

public class GetCharacterHeroQuery : IRequest<CharacterHeroDto>;
