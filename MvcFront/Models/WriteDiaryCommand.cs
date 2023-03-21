namespace MvcFront.Models;

public record WriteDiaryCommand(Guid DiaryId, string? Title, string? UserEmail, string? Content,
    int FeelingScore,
    DateTime CreatedAt);