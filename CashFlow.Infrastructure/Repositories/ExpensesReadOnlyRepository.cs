using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.Repositories;

internal class ExpensesRepository : IExpensesReadOnlyRepository, IExpensesWriteOnlyRepository, IExpensesUpdateOnlyRepository
{
    private readonly CashFlowDbContext _context;

    public ExpensesRepository(CashFlowDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Expense expense)
    {
        await _context.Expenses.AddAsync(expense);
    }

    public async Task<bool> DeleteAsync(long id)
    {
       var result =  await _context.Expenses.FirstOrDefaultAsync(a=>a.Id == id);
       if (result is null) return false;
       _context.Expenses.Remove(result);
       return true;
    }

    public async Task<List<Expense>> GetAllAsync()
    {
        return await _context.Expenses.AsNoTracking().ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetByIdAsync(long id)
    {
        return await _context.Expenses.AsNoTracking().FirstOrDefaultAsync(a=>a.Id == id);
    }

    public async Task<List<Expense>> FilterByMonthAsync(DateOnly month)
    {
        var startDate = new DateTime(year: month.Year, month.Month, 1).Date;
        
        var daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
        var endDate = new DateTime(year: month.Year, month: month.Month, day: daysInMonth, hour:23, minute:59,second:59);
        
        return await _context.Expenses
            .AsNoTracking()
            .Where(w => w.Date >= startDate && w.Date <= endDate)
            .OrderBy(o=>o.Date)
            .ThenBy(t=>t.Title)
            .ToListAsync();
    }

    async Task<Expense?> IExpensesUpdateOnlyRepository.GetByIdAsync(long id)
    {
        return await _context.Expenses.FirstOrDefaultAsync(a=>a.Id == id);
    }

    public void Updateasync(Expense expense)
    {
        _context.Expenses.Update(expense);
    }
}