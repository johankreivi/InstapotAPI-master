using Microsoft.EntityFrameworkCore;
using InstapotAPI.Entity;
using InstapotAPI.Infrastructure;

namespace InstapotAPI.Infrastructure.Repositories;

public class ImageRepository : IImageRepository
{
    private InstapotContext _dbContext;
    public ImageRepository(InstapotContext context)
    {
        _dbContext = context;
    }

    public async Task<Image?> CreateNewImage(Image newImage)
    {
        if (IsImageFreeFromNullValues(newImage))
        {
            _dbContext.Images.Add(newImage);
            await _dbContext.SaveChangesAsync();

            return newImage;
        }
        else return null;
    }
    #region CreateNewImage help methods
    private bool IsImageFreeFromNullValues(Image image)
    {
        if (image == null) return false;
        if (image.UserID == null) return false;
        if (image.Path == null) return false;
        if (image.Title == null) return false;
        if (image.Description == null) return false;
        if (image.CreatedDate == null) return false;
        if (image.Comments == null) return false;
        if (image.isPublished == null) return false;
        if (image.LikedBy == null) return false;
        return true;
    }
    #endregion
    public async Task<List<Image>> GetAllImages()
    {
        return await _dbContext.Images.OrderByDescending(img => img.CreatedDate).ToListAsync();
    }
    public async Task<Image?> GetImage(int id)
    {
        return await _dbContext.Images.FindAsync(id);
    }
    public async Task<List<Image>?> GetLikedImage(int id)
    {
        return await _dbContext.Images.Where(img => img.LikedBy.Contains(id)).OrderByDescending(img => img.CreatedDate).ToListAsync();
    }
    public async Task<List<Image>?> GetImageFlow(int id)
    {
        return _dbContext.Images.Where(img => img.UserID != id && img.isPublished == true).OrderByDescending(img => img.CreatedDate).ToList();
    }
    public async Task<Image?> DeleteImage(int id)
    {
        var deletedImage = await _dbContext.Images.FindAsync(id);
        if (deletedImage is null) return null;

        _dbContext.Images.Remove(deletedImage);
        _dbContext.SaveChanges();

        return deletedImage;
    }
    public async Task<Image?> ChangeTitel(int id, string newTitle)
    {
        var changedImage = await _dbContext.Images.FindAsync(id);

        if (changedImage is null) return null;

        if (newTitle is null || newTitle == string.Empty)
            return changedImage;

        changedImage.Title = newTitle;

        return changedImage;
    }
    public async Task<Image?> ChangeDescription(int id, string newDescription)
    {
        var changedImage = await _dbContext.Images.FindAsync(id);

        if (changedImage is null) return null;

        if (newDescription is null)
            return changedImage;

        changedImage.Title = newDescription;

        return changedImage;
    }
    public async Task<List<Image>> GetImagesFromUser(int userId)
    {
        return _dbContext.Images.Where(img => img.UserID == userId).ToList();
    }
    public async Task<int?> GetLikeCount(int id)
    {
        var image = await _dbContext.Images.FindAsync(id);

        return image?.LikedBy.Count();
    }
    public async Task<int?> AddLike(int id, int userId)
    {
        var image = await _dbContext.Images.FindAsync(id);

        if (image is null) return null;

        if (image.LikedBy.Contains(userId) is false)
            image.LikedBy.Add(userId);
        await _dbContext.SaveChangesAsync();
        return image.LikedBy.Count();
    }
    public async Task<int?> RemoveLike(int id, int userId)
    {
        var image = await _dbContext.Images.FindAsync(id);

        if (image is null) return null;

        image.LikedBy.Remove(userId);
        await _dbContext.SaveChangesAsync();
        return image.LikedBy.Count();
    }
    public async Task<bool?> IsPublished(int id)
    {
        return (await _dbContext.Images.FindAsync(id))?.isPublished;
    }
    public async Task<bool?> SetPublished(int id, bool published)
    {
        var image = await _dbContext.Images.FindAsync(id);

        if (image is null) return null;

        image.isPublished = published;

        return image.isPublished;
    }
}
