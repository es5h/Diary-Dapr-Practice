namespace NotificationApi.Events;

public record OrderDispatchedEvent(Guid DiaryId, DateTime DispatchedDateTime);