using InstapotAPI.Infrastructure.Repositories;
using InstapotAPI.Infrastructure;
using InstapotAPI.Entity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace InstapotAPI.Tests.InfrastructureTests.RepositoriesTests;
[TestClass]
public class CommentRepositoryTests
{
    private InstapotContext _dbContext;
    private CommentRepository _sut;

    [TestInitialize]
    public void Initializer()
    {
        Comment[] testComments = [  new Comment() { CreatedDate = DateTime.Now, Text = "Text1", ImageID = 1, UserID = 1 },
                                    new Comment() { CreatedDate = DateTime.Now, Text = "Text2", ImageID = 1, UserID = 1 },
                                    new Comment() { CreatedDate = DateTime.Now, Text = "Text3", ImageID = 1, UserID = 1 },
                                    new Comment() { CreatedDate = DateTime.Now, Text = "Text4", ImageID = 2, UserID = 2 },
                                    new Comment() { CreatedDate = DateTime.Now, Text = "Text5", ImageID = 2, UserID = 2 },
                                    new Comment() { CreatedDate = DateTime.Now, Text = "Text6", ImageID = 3, UserID = 2 },
                                    new Comment() { CreatedDate = DateTime.Now, Text = "Text7", ImageID = 3, UserID = 2 },
                                    new Comment() { CreatedDate = DateTime.Now, Text = "Text8", ImageID = 3, UserID = 3 },
                                    new Comment() { CreatedDate = DateTime.Now, Text = "Text9", ImageID = 3, UserID = 3 },
                                    new Comment() { CreatedDate = DateTime.Now, Text = "Text10", ImageID = 3, UserID = 4 }];
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        _dbContext = new InstapotContext(new DbContextOptionsBuilder<InstapotContext>().UseSqlite(connection).Options);
        _dbContext.Database.EnsureCreated();

        _dbContext.Comments.AddRange(testComments);
        _dbContext.SaveChanges();

        _sut = new CommentRepository(_dbContext);
    }
    #region Creating TODO
    [TestMethod]
    public async Task When_Creating_New_Comment_Return_Newly_Created_Comment()
    {
        var expected = new Comment() { CreatedDate = new DateTime(2020, 05, 10, 8, 14, 0), Text = "Texttexttext", ImageID = 1, UserID = 1 };

        var result = await _sut.CreateComment(expected);

        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    public async Task When_Creating_New_Comment_Add_It_To_Database()
    {
        var newComment = new Comment() { CreatedDate = new DateTime(2020, 05, 10, 8, 14, 0), Text = "Texttexttext", ImageID = 1, UserID = 1 };
        var expected = _dbContext.Comments.Count() + 1;

        await _sut.CreateComment(newComment);
        var result = _dbContext.Comments.Count();

        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    public async Task When_Creating_New_Comment_With_Incomplete_Comment_Return_Null()
    {
        var incompleteComment = new Comment() { };

        var result = await _sut.CreateComment(incompleteComment);

        Assert.IsNull(result);
    }
    [TestMethod]
    public async Task When_Creating_New_Comment_With_Incomplete_Comment_Dont_Add_It_To_Database()
    {
        var newComment = new Comment() { };
        var expected = _dbContext.Comments.Count();

        var test = await _sut.CreateComment(newComment);
        var result = _dbContext.Comments.Count();

        Assert.AreEqual(expected, result);
    }
    #endregion
    #region Deleting TODO
    [TestMethod]
    public async Task When_Deleting_A_Comment_Return_Deleted_Comment()
    {
        var expected = new Comment() { CreatedDate = new DateTime(2020, 05, 10, 8, 14, 0), Text = "Texttexttext", ImageID = 1, UserID = 1 };

        var result = await _sut.CreateComment(expected);

        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    public async Task When_Deleting_A_Comment_Remove_It_From_Database()
    {
        var expected = new Comment() { CreatedDate = new DateTime(2020, 05, 10, 8, 14, 0), Text = "Texttexttext", ImageID = 1, UserID = 1 };
        _dbContext.Comments.Add(expected);
        _dbContext.SaveChanges();
        var id = _dbContext.Comments.Count();
        expected.Id = id;

        var result = await _sut.DeleteComment(id);

        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    [DataRow(-1)]
    [DataRow(0)]
    [DataRow(70)]
    public async Task When_Deleting_A_Comment_A_Non_Existing_Comment_Return_Null(int id)
    {
        var result = await _sut.DeleteComment(id);

        Assert.IsNull(result);
    }
    #endregion
    #region Get Comments On Image
    [TestMethod]
    [DataRow(1,3)]
    [DataRow(2,2)]
    [DataRow(3,5)]
    public async Task When_Getting_Comments_On_Image_Return_List_Of_Comments(int imageId, int expected)
    {
        var result = await _sut.GetCommentsOnImage(imageId);

        Assert.AreEqual(expected, result.Count());
    }
    [TestMethod]
    public async Task When_Getting_Comments_On_Image_When_No_Comments_On_Image_Return_Empty_List()
    {
        var result = await _sut.GetCommentsOnImage(60);

        Assert.AreEqual(0, result.Count());
    }
    #endregion
    #region Get Comments From User
    [TestMethod]
    [DataRow(1, 3)]
    [DataRow(2, 4)]
    [DataRow(3, 2)]
    public async Task When_Getting_Comments_From_User_Return_List_Of_Comments(int userId, int expected)
    {
        var result = await _sut.GetCommentsFromUser(userId);

        Assert.AreEqual(expected, result.Count());
    }
    [TestMethod]
    public async Task When_Getting_Comments_From_User_When_No_Comments_On_Image_Return_Empty_List()
    {
        var result = await _sut.GetCommentsFromUser(60);

        Assert.AreEqual(0, result.Count());
    }
    #endregion
}
