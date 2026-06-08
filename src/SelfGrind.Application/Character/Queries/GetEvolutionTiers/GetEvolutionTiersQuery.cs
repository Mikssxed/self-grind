using MediatR;
using SelfGrind.Application.Character.Dtos;

namespace SelfGrind.Application.Character.Queries.GetEvolutionTiers;

public class GetEvolutionTiersQuery : IRequest<EvolutionTierDto[]>;
