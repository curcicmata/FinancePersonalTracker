namespace FinancePersonalTracker.Models
{
    public class Expense
    {
        public Guid Id { get; set; }
        public Guid UserProfileId { get; set; }
        public string? Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Notes { get; set; }
        public ExpenseType ExpenseType { get; set; }

        public UserProfile UserProfile { get; set; }
    }

    public enum ExpenseType
    {
        Expense,
        Salary
    }
}
