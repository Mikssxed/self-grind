namespace SelfGrind.Application.Community.Services;

public static class AvatarEmojiProvider
{
    private static readonly string[] Emojis =
    [
        "🧑", "👩", "👨", "🧙", "🦸", "🥷", "🧝", "🧚", "🦄", "🤖"
    ];

    public static string For(string identifier)
    {
        if (string.IsNullOrEmpty(identifier)) return Emojis[0];
        var hash = 0;
        foreach (var ch in identifier)
        {
            hash = unchecked(hash * 31 + ch);
        }
        return Emojis[Math.Abs(hash) % Emojis.Length];
    }
}
