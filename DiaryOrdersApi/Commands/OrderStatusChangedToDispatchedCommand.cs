namespace OrdersApi.Commands;

public record OrderStatusChangedToDispatchedCommand(Guid DiaryId, DateTime DispatchedDateTime);