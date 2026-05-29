namespace SelfGrind.Application.Analytics.Dtos;

public class AchievementDto
{
    public string Key { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Emoji { get; set; } = string.Empty;
    public string Variant { get; set; } = string.Empty;
    public bool Locked { get; set; }
}
