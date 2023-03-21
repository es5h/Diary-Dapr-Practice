namespace DiaryApi.Commands;

public record ProcessOrderCommand(
    Guid DiaryId,
    string? Title,
    string? UserEmail,
    IEnumerable<string?>? ContentItem,
    int FeelingScore);