using FinancePersonalTracker.Data;
using FinancePersonalTracker.Interface;
using FinancePersonalTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancePersonalTracker.Services
{
    public class FamilyGroupService(ApplicationDbContext context) : IFamilyGroupService
    {
        public async Task<FamilyGroup> CreateFamilyGroupAsync(string groupName, string identityUserId)
        {
            var userProfile = await context.UserProfiles
                .FirstOrDefaultAsync(u => u.IdentityUserId == identityUserId);

            if (userProfile == null)
                throw new Exception("User profile not found.");

            var group = new FamilyGroup
            {
                Id = Guid.NewGuid(),
                Name = groupName
            };

            userProfile.FamilyGroup = group;

            context.Add(group);
            await context.SaveChangesAsync();

            return group;
        }

        public async Task InviteUserAsync(Guid familyGroupId, string inviteeEmail)
        {
            var invite = new FamilyGroupInvite
            {
                Id = Guid.NewGuid(),
                Email = inviteeEmail,
                FamilyGroupId = familyGroupId,
            };

            context.FamilyGroupInvites.Add(invite);
            await context.SaveChangesAsync();

            // OPTIONAL: Send email notification
        }

        public async Task<List<FamilyGroupInvite>> GetPendingInvitesAsync(Guid familyGroupId)
        {
            return await context.FamilyGroupInvites
                .Where(i => i.FamilyGroupId == familyGroupId && !i.Accepted)
                .ToListAsync();
        }

        public async Task<bool> AcceptInviteAsync(Guid familyGroupId, string inviteeEmail)
        {
            var userProfile = await context.UserProfiles
                .FirstOrDefaultAsync(u => u.DisplayName == inviteeEmail);

            if (userProfile == null)
                return false;

            var invite = await context.FamilyGroupInvites
                .FirstOrDefaultAsync(i => i.FamilyGroupId == familyGroupId && i.Email == inviteeEmail && !i.Accepted);

            if (invite == null)
                return false;

            userProfile.FamilyGroupId = familyGroupId;
            invite.Accepted = true;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserProfile>> GetFamilyMembersAsync(string identityUserId)
        {
            var user = await context.UserProfiles
                .Include(u => u.FamilyGroup)
                .ThenInclude(f => f.Members)
                .FirstOrDefaultAsync(u => u.IdentityUserId == identityUserId);

            return user?.FamilyGroup?.Members?.ToList() ?? new List<UserProfile>();
        }

        public async Task<FamilyGroupDataDto> GetFamilyDataAsync(string identityUserId)
        {
            var userProfile = await context.UserProfiles
                .Include(up => up.FamilyGroup)
                    .ThenInclude(fg => fg.Members)
                .FirstOrDefaultAsync(up => up.IdentityUserId == identityUserId);

            var result = new FamilyGroupDataDto();

            if (userProfile?.FamilyGroupId != null)
            {
                var familyGroup = await context.FamilyGroups
                    .Include(fg => fg.Members)
                    .FirstOrDefaultAsync(fg => fg.Id == userProfile.FamilyGroupId);

                result.IsInFamily = true;
                result.FamilyName = familyGroup?.Name;
                result.FamilyMembers = familyGroup?.Members.ToList() ?? new List<UserProfile>();
            }

            var pendingInvites = await context.FamilyGroupInvites
                .Where( inv => !inv.Accepted)
                .ToListAsync();

            result.PendingInvites = pendingInvites;

            return result;
        }
    }

}
