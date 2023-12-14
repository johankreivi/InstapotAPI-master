using Bogus;
using Bogus.DataSets;
using InstapotAPI.Entity;
using InstapotAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace InstapotAPI.Services;

public static class FakeData
{
    public static void InitializeData(int profilesToGenerate)
    {
        var random = new Random();

        var imagesPerUser = random.Next(0, profilesToGenerate * 2);
        var commentsPerUser = random.Next(0, profilesToGenerate * 2);

        var _profileFaker = new Faker<Profile>()
            .UseSeed(1337)
            .RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.CreatedDate, f => f.Date.Past(4))
            .RuleFor(p => p.LastLoggedIn, f => f.Date.Past(4))
            .RuleFor(p => p.Username, f => f.Internet.UserName(f.Person.FirstName, f.Person.LastName))
            .RuleFor(p => p.Email, f => f.Internet.Email())
            .RuleFor(p => p.Password, f => f.Internet.Password())
            .RuleFor(p => p.ProfilePicture, f => f.Image.LoremPixelUrl(LoremPixelCategory.People))
            .RuleFor(p => p.IsVerified, f => f.Random.Bool(.75f));

        var fakeProfile = _profileFaker.Generate(profilesToGenerate);

        var _imageFaker = new Faker<Image>()
            .UseSeed(1337)
            .RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(i => i.Path, f => f.Image.PicsumUrl())
            .RuleFor(i => i.Title, f => f.Lorem.Word())
            .RuleFor(i => i.Description, f => f.Lorem.Text())
            .RuleFor(i => i.CreatedDate, f => f.Date.Past(4))
            .RuleFor(i => i.UserID, f => f.PickRandom(fakeProfile).Id)
            .RuleFor(i => i.isPublished, f => f.Random.Bool(.80f));

        var fakeImage = _imageFaker.Generate(imagesPerUser);

        var _commentFaker = new Faker<Comment>()
            .UseSeed(1337)
            .RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(c => c.CreatedDate, f => f.Date.Past(4))
            .RuleFor(c => c.Text, f => f.Lorem.Text())
            .RuleFor(c => c.UserID, f => f.PickRandom(fakeProfile).Id)
            .RuleFor(c => c.ImageID, f => f.PickRandom(fakeImage).Id);

        var fakeComment = _commentFaker.Generate(commentsPerUser);

        foreach (var user in fakeProfile)
        {
            var images = fakeImage
                .Where(img => img.UserID == user.Id)
                .Select(img => img.Id)
                .ToList();

            if (images.Count > 0)
            {
                user.Images = images;
            }
        }

        foreach (var image in fakeImage)
        {
            var imageComment = fakeComment
                .Where(c => c.ImageID == image.Id)
                .Select(c => c.Id)
                .ToList();

            if (imageComment.Count > 0)
            {
                image.Comments = imageComment;
            }
            else
            {
                image.Comments = new List<int>();
            }

            var likedProfile = fakeProfile
                .Where(p => p.Id == image.UserID)
                .Select(p => p.Id)
                .ToList();

            if (likedProfile.Count > 0)
            {
                image.LikedBy = likedProfile;
            }
        }

        var options = new DbContextOptionsBuilder<InstapotContext>()
            .UseSqlite("Data Source=Instapot.db")
            .Options;

        using (var dbContext = new InstapotContext(options))
        {
            dbContext.Database.EnsureCreated();

            dbContext.Profiles.AddRange(fakeProfile);
            dbContext.Images.AddRange(fakeImage);
            dbContext.Comments.AddRange(fakeComment);

            dbContext.SaveChanges();
        }
    }
}
