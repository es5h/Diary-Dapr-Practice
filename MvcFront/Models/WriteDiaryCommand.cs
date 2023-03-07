namespace MvcFront.Models;

public record WriteDiaryCommand(Guid DiaryId, string? Title, string? UserEmail, IEnumerable<string?>? ContentItem,
    DateTime CreatedAt);