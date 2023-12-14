using InstapotAPI.Entity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace InstapotAPI.Infrastructure.Repositories
{

    public class ProfileRepository : IProfileRepository

    {
        private readonly InstapotContext _context;
        
        public ProfileRepository(InstapotContext context)
        {
            _context = context;
        }

        public async Task<Profile> Create(Profile newProfile)
        {
            newProfile.CreatedDate = DateTime.Now;
            newProfile.IsVerified = false;
            _context.Add(newProfile);
            await _context.SaveChangesAsync();

            return newProfile;
        }

        public async Task<Profile> Delete(int id)
        {
            var removedProfile = await _context.Profiles.FindAsync(id);
            
            if (removedProfile != null)
            {
                _context.Profiles.Remove(removedProfile);
                _context.SaveChangesAsync();
            }

            return removedProfile;
        }
        
        public async Task<Profile> Profile(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            return profile;
        }

        public async Task<Profile> UpdatePathToProfilePicture(Profile newPathToProfilePicture)
        {
            var updatePathToProfilePicture = await _context.Profiles.FindAsync(newPathToProfilePicture.Id);

            if (updatePathToProfilePicture != null)
            {
                updatePathToProfilePicture.ProfilePicture = newPathToProfilePicture.ProfilePicture;
                await _context.SaveChangesAsync();
            }

            return updatePathToProfilePicture;
        }

        public async Task<Profile> UpdateUsername(Profile newUsername)
        {
            var updateUsername = await _context.Profiles.FindAsync(newUsername.Id);

            if (updateUsername != null)
            {
                updateUsername.Username = newUsername.Username;
                await _context.SaveChangesAsync();
            }

            return updateUsername;
        }

        public async Task<Profile> UpdatePassword(Profile newPassword)
        {
            var updatePassword = await _context.Profiles.FindAsync(newPassword.Id);

            if (updatePassword != null)
            {
                updatePassword.Password = newPassword.Password;
                await _context.SaveChangesAsync();
            }

            return updatePassword;
        }

        public async Task<Profile> UpdateEmail(Profile newEmail)
        {
            var updateEmail = await _context.Profiles.FindAsync(newEmail.Id);

            if (updateEmail != null)
            {
                updateEmail.Email = newEmail.Email;
                await _context.SaveChangesAsync();
            }

            return updateEmail;
        }

        public async Task<Profile> Verified(Profile profile)
        {
            var confirmedProfile = await _context.Profiles.FindAsync(profile.Id);

            if (confirmedProfile != null)
            {
                confirmedProfile.IsVerified = true;
                await _context.SaveChangesAsync();
            }

            return confirmedProfile;
        }

        public async Task<string?> PathToProfilePicture(int id)
        {
            var pathToProfilePicture = await _context.Profiles.FindAsync(id);

            if (pathToProfilePicture == null || pathToProfilePicture.ProfilePicture == null)
            {
                return null;
            }

            return pathToProfilePicture.ProfilePicture;
        }

        public async Task<bool?> IsVerified(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);

            if (profile == null)
            {
                return null;
            }

            profile.IsVerified = true;
            await _context.SaveChangesAsync();

            return profile.IsVerified;

        }

        public async Task<bool?> UpdateLoginStatus(int id)
        {
            var updateLoginStatus = await _context.Profiles.FindAsync(id);

            if (updateLoginStatus == null)
            {
                return null;
            }

            updateLoginStatus.LoginStatus = !updateLoginStatus.LoginStatus;
            await _context.SaveChangesAsync();

            return updateLoginStatus.LoginStatus;
            
        }

        public async Task<bool?> SetLoginStatusToTrue(int id)
        {
            var updateLoginStatus = await _context.Profiles.FindAsync(id);

            if (updateLoginStatus == null)
            {
                return null;
            }

            updateLoginStatus.LoginStatus = true;
            await _context.SaveChangesAsync();

            return updateLoginStatus.LoginStatus;
        }

        public async Task<bool?> SetLoginStatusToFalse(int id)
        {
            var updateLoginStatus = await _context.Profiles.FindAsync(id);

            if (updateLoginStatus == null)
            {
                return null;
            }

            updateLoginStatus.LoginStatus = false;
            await _context.SaveChangesAsync();

            return updateLoginStatus.LoginStatus;
        }
    }
}
