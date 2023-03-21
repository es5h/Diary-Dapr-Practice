using System.ComponentModel.DataAnnotations;

namespace OrdersApi.Models;

public class DiaryOrder
{
    public DiaryOrder()
    {
        DiaryOrderDetails = new List<DiaryOrderDetail>();
    }

    [Key]
    public Guid DiaryId { get; init; }
    public string? Title { get; init; }
    public string? UserEmail { get; init; }
    public IEnumerable<string?>? ContentItem { get; init; }
    public int FeelingScore { get; init; }
    public DateTime CreatedAt { get; init; }
    public Status Status { get; set; }
    public List<DiaryOrderDetail> DiaryOrderDetails { get; set; }
}