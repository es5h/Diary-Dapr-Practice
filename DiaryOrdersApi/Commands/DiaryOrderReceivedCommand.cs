namespace MvcFront.Commands;

public record DiaryOrderReceivedCommand(
    Guid DiaryId,
    string? Title,
    string? UserEmail,
    IEnumerable<string?>? ContentItem,
    int FeelingScore);