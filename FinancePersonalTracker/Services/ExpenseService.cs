using FinancePersonalTracker.Data;
using FinancePersonalTracker.Interface;
using FinancePersonalTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancePersonalTracker.Services
{
    public class ExpenseService(ApplicationDbContext context) : IExpenseService
    {
        public async Task AddExpenseAsync(Expense expense)
        {
            expense.Id = Guid.NewGuid();
            expense.Date = expense.Date;

            context.Expenses.Add(expense);
            await context.SaveChangesAsync();
        }

        public async Task DeleteExpenseAsync(Guid expenseId)
        {
            var expense = await context.Expenses.FindAsync(expenseId);
            if (expense != null)
            {
                context.Expenses.Remove(expense);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateExpenseAsync(Guid expenseId, decimal amount)
        {
            var expense = await context.Expenses.FindAsync(expenseId);
            if (expense != null)
            {
                expense.Amount = amount;
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<UserProfile>> GetAllUserProfilesWithExpensesAsync(string userProfileId)
        {
            return await context.UserProfiles
            .Include(u => u.Expenses)
            .Where(u => u.IdentityUserId == userProfileId)
            .ToListAsync();
        }

        public async Task<UserProfile> GetUserProfilesWithExpensesAsync(string userProfileId)
        {
            return await context.UserProfiles
            .Include(u => u.Expenses)
            .Where(u => u.IdentityUserId == userProfileId).FirstOrDefaultAsync();
        }

        public async Task<decimal> GetTotalForUserAsync(Guid userProfileId)
        {
            return await context.Expenses
            .Where(e => e.UserProfileId == userProfileId)
            .SumAsync(e => e.Amount);
        }

        public async Task<(double[] Expenses, double[] Salaries)> GetMonthlyTotalsAsync(string userProfileId, int year)
        {
            var user = await context.UserProfiles
                .Include(u => u.Expenses)
                .FirstOrDefaultAsync(u => u.IdentityUserId == userProfileId);

            if (user == null)
                return (new double[12], new double[12]);

            var expenses = new double[12];
            var salaries = new double[12];

            foreach (var e in user.Expenses.Where(e => e.Date.Year == year))
            {
                var monthIndex = e.Date.Month - 1; // 0-based index for chart
                if (e.ExpenseType == ExpenseType.Expense)
                    expenses[monthIndex] += (double)e.Amount;
                else if (e.ExpenseType == ExpenseType.Salary)
                    salaries[monthIndex] += (double)e.Amount;
            }

            return (expenses, salaries);
        }

        public async Task<MonthlyTotalsForFamilyDto> GetMonthlyTotalsForFamilyAsync(string identityUserId, int year)
        {
            var user = await context.UserProfiles
                .Include(u => u.Expenses)
                .Include(u => u.FamilyGroup)
                    .ThenInclude(f => f.Members)
                        .ThenInclude(m => m.Expenses)
                .FirstOrDefaultAsync(u => u.IdentityUserId == identityUserId);

            if (user == null)
                return new MonthlyTotalsForFamilyDto();

            var result = new MonthlyTotalsForFamilyDto();

            // If user is part of a family group, show family data
            if (user.FamilyGroup != null)
            {
                foreach (var member in user.FamilyGroup.Members)
                {
                    var expenses = new double[12];
                    var salaries = new double[12];

                    foreach (var e in member.Expenses.Where(e => e.Date.Year == year))
                    {
                        var monthIndex = e.Date.Month - 1;
                        if (e.ExpenseType == ExpenseType.Expense)
                            expenses[monthIndex] += (double)e.Amount;
                        else if (e.ExpenseType == ExpenseType.Salary)
                            salaries[monthIndex] += (double)e.Amount;

                        // Add to family total
                        if (e.ExpenseType == ExpenseType.Expense)
                            result.TotalExpenses[monthIndex] += (double)e.Amount;
                        else if (e.ExpenseType == ExpenseType.Salary)
                            result.TotalSalaries[monthIndex] += (double)e.Amount;
                    }

                    var userLabel = member.DisplayName ?? member.IdentityUserId;
                    result.ExpensesByUser[userLabel] = expenses;
                    result.SalariesByUser[userLabel] = salaries;
                }
            }
            else
            {
                // If user is not part of a family group, show individual data
                var expenses = new double[12];
                var salaries = new double[12];

                foreach (var e in user.Expenses.Where(e => e.Date.Year == year))
                {
                    var monthIndex = e.Date.Month - 1;
                    if (e.ExpenseType == ExpenseType.Expense)
                        expenses[monthIndex] += (double)e.Amount;
                    else if (e.ExpenseType == ExpenseType.Salary)
                        salaries[monthIndex] += (double)e.Amount;

                    // Add to total
                    if (e.ExpenseType == ExpenseType.Expense)
                        result.TotalExpenses[monthIndex] += (double)e.Amount;
                    else if (e.ExpenseType == ExpenseType.Salary)
                        result.TotalSalaries[monthIndex] += (double)e.Amount;
                }

                var userLabel = user.DisplayName ?? user.IdentityUserId;
                result.ExpensesByUser[userLabel] = expenses;
                result.SalariesByUser[userLabel] = salaries;
            }

            return result;
        }
    }
}
