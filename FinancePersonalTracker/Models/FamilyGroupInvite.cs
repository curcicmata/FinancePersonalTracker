namespace FinancePersonalTracker.Models
{
    public class FamilyGroupInvite
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public Guid FamilyGroupId { get; set; }
        public FamilyGroup FamilyGroup { get; set; } = null!;
        public DateTime InvitedOn { get; set; } = DateTime.UtcNow;
        public bool Accepted { get; set; } = false;
    }
}
