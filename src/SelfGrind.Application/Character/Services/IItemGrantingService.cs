using SelfGrind.Domain.Entities;

namespace SelfGrind.Application.Character.Services;

public interface IItemGrantingService
{
    IReadOnlyList<UserItem> CalculateNewlyGranted(
        string userId,
        IEnumerable<Item> catalog,
        IEnumerable<UserItem> alreadyOwned,
        int userLevel);
}
