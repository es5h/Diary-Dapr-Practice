namespace OrdersApi.Events;

public record DiaryOrderRegisteredEvent(Guid DiaryId, string? Title, string? UserEmail, IEnumerable<string?>? ContentItem, int FeelingScore);
