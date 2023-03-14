using Microsoft.EntityFrameworkCore;
using OrdersApi.Models;

namespace OrdersApi.Persistence;

public interface IDiaryOrderRepository
{
    public Task<DiaryOrder?>? GetDiaryOrderAsync(Guid diaryId);
    public Task RegisterDiaryOrder(DiaryOrder diaryOrder);
    public Task UpdateDiaryOrder(DiaryOrder diaryOrder);
}

public class DiaryOrderRepository : IDiaryOrderRepository
{
    private readonly DiaryOrdersContext _context;

    public DiaryOrderRepository(DiaryOrdersContext context)
    {
        _context = context;
    }

    public Task<DiaryOrder?>? GetDiaryOrderAsync(Guid diaryId) =>
        _context?.DiaryOrders?
            .Include(diaryOrder => diaryOrder.DiaryOrderDetails)
            .FirstOrDefaultAsync(diaryOrder => diaryOrder.DiaryId == diaryId);

    public async Task RegisterDiaryOrder(DiaryOrder diaryOrder)
    {
        _context.Add(diaryOrder);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateDiaryOrder(DiaryOrder diaryOrder)
    {
        _context.Entry(diaryOrder).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}