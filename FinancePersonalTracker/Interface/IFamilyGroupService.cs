using FinancePersonalTracker.Models;

namespace FinancePersonalTracker.Interface
{
    public interface IFamilyGroupService
    {
        Task<FamilyGroup> CreateFamilyGroupAsync(string groupName, string identityUserId);
        Task InviteUserAsync(Guid familyGroupId, string inviteeEmail);
        Task<List<FamilyGroupInvite>> GetPendingInvitesAsync(Guid familyGroupId);
        Task<bool> AcceptInviteAsync(Guid familyGroupId, string inviteeEmail);
        Task<List<UserProfile>> GetFamilyMembersAsync(string identityUserId);
        Task<FamilyGroupDataDto> GetFamilyDataAsync(string identityUserId);
    }
}
