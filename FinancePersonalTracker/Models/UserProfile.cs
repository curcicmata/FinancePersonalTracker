namespace FinancePersonalTracker.Models
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public string IdentityUserId { get; set; }
        public string DisplayName { get; set; }
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

        public Guid? FamilyGroupId { get; set; }
        public FamilyGroup? FamilyGroup { get; set; }
    }
}
