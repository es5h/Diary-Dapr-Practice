namespace DiaryApi.Commands;

public record DiaryOrderProcessedEvent(Guid DiaryId, string? Title, string? UserEmail, string? GeneratedContent);
