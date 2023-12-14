using InstapotAPI.Entity;

namespace InstapotAPI.Infrastructure.Repositories
{
    public interface IProfileRepository
    {
        public Task<Profile> Create(Profile newProfile);

        public Task<Profile> Delete(int id);

        public Task<Profile> Profile(int id);

        public Task<Profile> UpdatePathToProfilePicture(Profile newPathToProfilePicture);

        public Task<Profile> UpdateUsername(Profile newUsername);

        public Task<Profile> UpdatePassword(Profile newPassword);

        public Task<Profile> UpdateEmail(Profile newEmail);

        public Task<Profile> Verified(Profile profile);

        public Task<string?> PathToProfilePicture(int id);

        public Task<bool?> IsVerified(int id);

        public Task<bool?> SetLoginStatusToTrue(int id);

        public Task<bool?> SetLoginStatusToFalse(int id);

    }
}
