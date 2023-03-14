namespace NotificationApi.Command;

public record DispatchedOrderCommand(
    Guid DiaryId,
    string? Title,
    string? UserEmail,
    string? GeneratedContent);