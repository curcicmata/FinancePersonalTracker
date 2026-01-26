using FinancePersonalTracker.Models;

namespace FinancePersonalTracker.Interface
{
    public interface IExpenseService
    {
        Task<List<UserProfile>> GetAllUserProfilesWithExpensesAsync(string userProfileId);
        Task AddExpenseAsync(Expense expense);
        Task UpdateExpenseAsync(Guid expenseId, decimal amount);
        Task DeleteExpenseAsync(Guid expenseId);
        Task<decimal> GetTotalForUserAsync(Guid userProfileId);
        Task<(double[] Expenses, double[] Salaries)> GetMonthlyTotalsAsync(string userProfileId, int year);
        Task<UserProfile> GetUserProfilesWithExpensesAsync(string userProfileId);
        Task<MonthlyTotalsForFamilyDto> GetMonthlyTotalsForFamilyAsync(string identityUserId, int year);
    }
}
