using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelfGrind.Application.Character.Dtos;
using SelfGrind.Application.Character.Commands.EquipItem;
using SelfGrind.Application.Character.Commands.UnequipItem;
using SelfGrind.Application.Character.Queries.GetCharacterHero;
using SelfGrind.Application.Character.Queries.GetEquippedItems;
using SelfGrind.Application.Character.Queries.GetEvolutionTiers;
using SelfGrind.Application.Character.Queries.GetInventory;
using SelfGrind.Application.Character.Queries.GetSkillTrees;

namespace SelfGrind.Controllers;

[ApiController]
[Route("api/character")]
[Authorize]
public class CharacterController(IMediator mediator) : ControllerBase
{
    [HttpGet("hero")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CharacterHeroDto>> GetHero()
    {
        var hero = await mediator.Send(new GetCharacterHeroQuery());
        return Ok(hero);
    }

    [HttpGet("evolution-tiers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EvolutionTierDto[]>> GetEvolutionTiers()
    {
        var tiers = await mediator.Send(new GetEvolutionTiersQuery());
        return Ok(tiers);
    }

    [HttpGet("skill-trees")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SkillCategoryDto[]>> GetSkillTrees()
    {
        var categories = await mediator.Send(new GetSkillTreesQuery());
        return Ok(categories);
    }

    [HttpGet("inventory")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<InventoryItemDto[]>> GetInventory()
    {
        var items = await mediator.Send(new GetInventoryQuery());
        return Ok(items);
    }

    [HttpGet("equipped-items")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<EquippedItemDto[]>> GetEquippedItems()
    {
        var items = await mediator.Send(new GetEquippedItemsQuery());
        return Ok(items);
    }

    [HttpPost("items/{userItemId:guid}/equip")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> EquipItem([FromRoute] Guid userItemId)
    {
        await mediator.Send(new EquipItemCommand { UserItemId = userItemId });
        return NoContent();
    }

    [HttpPost("items/{userItemId:guid}/unequip")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UnequipItem([FromRoute] Guid userItemId)
    {
        await mediator.Send(new UnequipItemCommand { UserItemId = userItemId });
        return NoContent();
    }
}
