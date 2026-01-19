namespace FinancePersonalTracker.Models
{
    public class FamilyGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<UserProfile> Members { get; set; } = new List<UserProfile>();
    }
}
