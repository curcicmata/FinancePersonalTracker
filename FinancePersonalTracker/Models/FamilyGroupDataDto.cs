namespace FinancePersonalTracker.Models
{
    public class FamilyGroupDataDto
    {
        public bool IsInFamily { get; set; }
        public string? FamilyName { get; set; }
        public List<UserProfile> FamilyMembers { get; set; } = new();
        public List<FamilyGroupInvite> PendingInvites { get; set; } = new();
    }
}
