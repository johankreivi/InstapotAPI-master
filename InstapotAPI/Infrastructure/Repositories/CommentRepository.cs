using InstapotAPI.Entity;

namespace InstapotAPI.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private InstapotContext _dbContext;

    public CommentRepository(InstapotContext context)
    {
        _dbContext = context;
    }
    public async Task<Comment?> CreateComment(Comment comment)
    {
        if (comment?.UserID == null) return null;
        if (comment?.ImageID == null) return null;
        if (comment?.CreatedDate == null) return null;
        if (comment?.Text == null) return null;

        _dbContext.Comments.Add(comment);
        _dbContext.SaveChanges();

        return comment;
    }
    public async Task<Comment?> DeleteComment(int id)
    {
        var deletedComment = await _dbContext.Comments.FindAsync(id);
        if (deletedComment == null) return null;

        _dbContext.Comments.Remove(deletedComment);
        _dbContext.SaveChanges();

        return deletedComment;
    }
    public async Task<List<Comment>> GetCommentsOnImage(int id)
    {
        var comments = _dbContext.Comments.Where(comment => comment.ImageID == id).ToList();

        return comments;
    }
    public async Task<List<Comment>> GetCommentsFromUser(int id)
    {
        var comments = _dbContext.Comments.Where(comment => comment.UserID == id).ToList();

        return comments;
    }
}
