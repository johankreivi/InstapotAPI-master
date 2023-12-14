using InstapotAPI.Entity;
using Microsoft.EntityFrameworkCore;

namespace InstapotAPI.Infrastructure.Repositories;

public interface ICommentRepository
{
    public  Task<Comment?> CreateComment(Comment comment);
    public  Task<Comment?> DeleteComment(int id);
    public  Task<List<Comment>> GetCommentsOnImage(int id);
    public  Task<List<Comment>> GetCommentsFromUser(int id);
}
