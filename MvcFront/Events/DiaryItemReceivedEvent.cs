namespace MvcFront.Events;

public record DiaryItemReceivedEvent(Guid DiaryId, string? Title, string? UserEmail, IEnumerable<string?>? ContentItem,
    int FeelingScore,
    DateTime CreatedAt);