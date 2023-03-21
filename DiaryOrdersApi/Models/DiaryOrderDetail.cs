using Microsoft.EntityFrameworkCore;

namespace OrdersApi.Models;

[PrimaryKey(nameof(DiaryId), nameof(GeneratedDiaryId))]
public class DiaryOrderDetail
{
    public Guid DiaryId { get; init; }
    public int GeneratedDiaryId { get; init; }
    public string? Title { get; init; }
    public string? GeneratedContent { get; init; }
    public DateTime CreatedAt { get; init; }
}