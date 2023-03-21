namespace OrdersApi.Commands;

public record OrderStatusChangedToProcessedCommand(Guid DiaryId, string? Title, string? UserEmail, string? GeneratedContent);