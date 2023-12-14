using Bogus.DataSets;
using InstapotAPI.Entity;

using Microsoft.EntityFrameworkCore;

namespace InstapotAPI.Infrastructure.Repositories;

public interface IImageRepository
{
    public Task<Image?> CreateNewImage(Image newImage);
    public Task<List<Image>> GetAllImages();
    public Task<Image?> GetImage(int id);
    public Task<List<Image>?> GetImageFlow(int id);
    public Task<List<Image>?> GetLikedImage(int id);
    public Task<Image?> DeleteImage(int id);
    public Task<Image?> ChangeTitel(int id, string newTitle);
    public Task<Image?> ChangeDescription(int id, string newDescription);
    public Task<List<Image>> GetImagesFromUser(int userId);
    public Task<int?> GetLikeCount(int id);
    public Task<int?> AddLike(int id, int userId);
    public Task<int?> RemoveLike(int id, int userId);
    public Task<bool?> IsPublished(int id);
    public Task<bool?> SetPublished(int id, bool published);
}
