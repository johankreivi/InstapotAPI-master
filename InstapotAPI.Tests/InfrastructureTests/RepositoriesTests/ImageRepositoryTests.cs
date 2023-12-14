
using InstapotAPI.Entity;
using InstapotAPI.Infrastructure;
using InstapotAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace InstapotAPI.Tests.InfrastructureTests.RepositoriesTests;
[TestClass]
public class ImageRepositoryTests
{
    private InstapotContext _dbContext;
    private ImageRepository _sut;
    private Image[] _testImages;

    [TestInitialize]
    public void Initializer()
    {
        _testImages = [
            new Image { UserID = 1, Path = "this should be a img path", Title = "Image1", Description = "Desc of Image1", CreatedDate = DateTime.Now, Comments = new List<int>(), isPublished = true, LikedBy = new List<int>() },
            new Image { UserID = 1, Path = "this should be a img path", Title = "Image2", Description = "Desc of Image2", CreatedDate = DateTime.Now, Comments = new List<int>(), isPublished = true, LikedBy = new List<int>() },
            new Image { UserID = 2, Path = "this should be a img path", Title = "Image3", Description = "Desc of Image3", CreatedDate = DateTime.Now, Comments = new List<int>(), isPublished = true, LikedBy = new List<int>() },
            new Image { UserID = 2, Path = "this should be a img path", Title = "Image4", Description = "Desc of Image4", CreatedDate = DateTime.Now, Comments = new List<int>(), isPublished = true, LikedBy = new List<int>() },
            new Image { UserID = 1, Path = "this should be a img path", Title = "Image5", Description = "Desc of Image5", CreatedDate = DateTime.Now, Comments = new List<int>(), isPublished = true, LikedBy = new List<int>() },
            new Image { UserID = 2, Path = "this should be a img path", Title = "Image6", Description = "Desc of Image6", CreatedDate = DateTime.Now, Comments = new List<int>(), isPublished = true, LikedBy = new List<int>() },
            new Image { UserID = 3, Path = "this should be a img path", Title = "Image7", Description = "Desc of Image7", CreatedDate = DateTime.Now, Comments = new List<int>(), isPublished = true, LikedBy = new List<int>() },
            new Image { UserID = 1, Path = "this should be a img path", Title = "Image8", Description = "Desc of Image8", CreatedDate = DateTime.Now, Comments = new List<int>(), isPublished = true, LikedBy = new List<int>() },
            new Image { UserID = 3, Path = "this should be a img path", Title = "Image9", Description = "Desc of Image9", CreatedDate = DateTime.Now, Comments = new List<int>(), isPublished = true, LikedBy = new List<int>() },
        ];

        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        _dbContext = new InstapotContext(new DbContextOptionsBuilder<InstapotContext>().UseSqlite(connection).Options);
        _dbContext.Database.EnsureCreated();

        _dbContext.Images.AddRange(_testImages);
        _dbContext.SaveChanges();

        _sut = new ImageRepository(_dbContext);
    }

    [TestMethod]
    public async Task When_Getting_All_Image_Return_Image_List()
    {
        var expected = _dbContext.Images.Count();

        var result = await _sut.GetAllImages();

        Assert.AreEqual(expected, result.Count());
    }

    [TestMethod]
    public async Task When_Getting_All_Liked_Image_Return_Image_List()
    {
        var id = 1;

        var expected = _dbContext.Images.Where(img => img.LikedBy.Contains(id)).Count();

        var result = await _sut.GetLikedImage(id);

        Assert.AreEqual(expected, result.Count());
    }

    #region Create New Image
    [TestMethod]
    public async Task When_Creating_New_Image_Return_Created_Image()
    {
        var expected = new Image { UserID = 3, Path = "utsaduw", Title = "newImage%¤", Description = "012euad23", CreatedDate = new DateTime(2020, 05, 10, 8, 14, 0), Comments = new List<int>(), isPublished = true, LikedBy = new List<int>() };

        var result = await _sut.CreateNewImage(expected);

        Assert.IsNotNull(result);
        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    public async Task When_Creating_New_Image_It_Is_Added_To_Database()
    {
        var newImage = new Image { UserID = 3, Path = "utsaduw", Title = "newImage%¤", Description = "012euad23", CreatedDate = new DateTime(2020, 05, 10, 8, 14, 0), Comments = new List<int>(), isPublished = true, LikedBy = new List<int>() };
        var expected = _dbContext.Images.Count() + 1;

        await _sut.CreateNewImage(newImage);
        var result = _dbContext.Images.Count();

        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    public async Task When_Creating_New_Image_With_Incomplete_Image_Return_Null()
    {
        var newImage = new Image { };

        var result = await _sut.CreateNewImage(newImage);

        Assert.IsNull(result);
    }
    [TestMethod]
    public async Task When_Creating_New_Image_With_Incomplete_Image_It_Is_Not_Added_To_Database()
    {
        var newImage = new Image { };
        var expected = _dbContext.Images.Count();

        await _sut.CreateNewImage(newImage);
        var result = _dbContext.Images.Count();

        Assert.AreEqual(expected, result);
    }
    #endregion
    #region Get Image By ID
    [TestMethod]
    public async Task When_Getting_Image_With_An_Existing_Id_Return_Image()
    {
        int id = 2;
        var expected = _dbContext.Images.FindAsync(id).Result;

        var result = await _sut.GetImage(id);

        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(20)]
    public async Task When_Getting_Image_With_A_Non_Existing_Id_Return_Null(int id)
    {
        var expected = _dbContext.Images.FindAsync(id).Result;

        var result = await _sut.GetImage(id);

        Assert.IsNull(result);
    }
    #endregion
    #region Delete Image
    [TestMethod]
    public async Task When_Successfully_Deleting_Image_Return_Deleted_Image()
    {
        var expected = new Image { UserID = 3, Path = "utsaduw", Title = "newImage%¤", Description = "012euad23", CreatedDate = new DateTime(2020, 05, 10, 8, 14, 0), Comments = new List<int>(), isPublished = true, LikedBy = new List<int>() };
        _dbContext.Images.Add(expected);
        _dbContext.SaveChanges();
        var id = _dbContext.Images.Count();

        var result = await _sut.DeleteImage(id);

        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    public async Task When_Successfully_Deleting_Image_Remove_Image_From_Database()
    {
        var expected = _dbContext.Images.Count();
        var deletedImage = new Image { UserID = 3, Path = "utsaduw", Title = "newImage%¤", Description = "012euad23", CreatedDate = new DateTime(2020, 05, 10, 8, 14, 0), Comments = new List<int>(), isPublished = true, LikedBy = new List<int>() };
        _dbContext.Images.Add(deletedImage);
        _dbContext.SaveChanges();

        await _sut.DeleteImage(expected + 1);
        var result = _dbContext.Images.Count();

        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    public async Task When_Trying_To_Delete_Non_Existing_Image_Return_Null()
    {
        var id = _dbContext.Images.Count() + 1;

        var result = await _sut.DeleteImage(id);

        Assert.IsNull(result);
    }
    #endregion
    #region Change Title
    [TestMethod]
    public async Task When_Title_Is_Changed_Return_Image_With_New_Title()
    {
        var id = 1;
        var expected = "This is the new title";

        var result = await _sut.ChangeTitel(id, expected);

        Assert.IsInstanceOfType<Image>(result);
        Assert.AreEqual(expected, result.Title);
    }
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public async Task When_Title_Is_Changed_Null_Or_To_Empty_String_Return_Image_With_Old_Title(string newTitle)
    {
        var id = 1;
        var expected = (await _dbContext.Images.FindAsync(id))?.Title;

        var result = await _sut.ChangeTitel(id, newTitle);

        Assert.AreEqual(expected, result?.Title);
    }
    [TestMethod]
    public async Task When_Title_Is_Changed_It_Is_Updated_In_Database()
    {
        var id = 1;
        var expected = "7iqfdhfsauiojr4893";

        await _sut.ChangeTitel(id, expected);
        var result = await _dbContext.Images.FindAsync(id);

        Assert.AreEqual(expected, result?.Title);
    }
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public async Task When_Title_Is_Changed_Null_Or_To_Empty_String_Database_Is_Not_Updated(string newTitle)
    {
        var id = 1;
        var expected = (await _dbContext.Images.FindAsync(id))?.Title;

        await _sut.ChangeTitel(id, newTitle);
        var result = await _dbContext.Images.FindAsync(id);

        Assert.AreEqual(expected, result?.Title);
    }
    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(100)]
    public async Task When_Title_Is_Changed_On_An_Id_That_Is_Out_Of_Bounds_Return_Null(int id)
    {
        var result = await _sut.ChangeTitel(id, "NewTitle");

        Assert.IsNull(result);
    }
    #endregion
    #region Change Description
    [TestMethod]
    [DataRow("")]
    [DataRow("This is the new description")]
    public async Task When_Description_Is_Changed_Return_Image_With_New_Description(string expected)
    {
        var id = 1;

        var result = await _sut.ChangeDescription(id, expected);

        Assert.IsInstanceOfType<Image>(result);
        Assert.AreEqual(expected, result.Title);
    }
    [TestMethod]
    public async Task When_Description_Is_Changed_Null_Return_Image_With_Old_Description()
    {
        var id = 1;
        var expected = (await _dbContext.Images.FindAsync(id))?.Title;

        var result = await _sut.ChangeDescription(id, null);

        Assert.AreEqual(expected, result?.Title);
    }
    [TestMethod]
    [DataRow("")]
    [DataRow("7iqfdhfsauiojr4893")]
    public async Task When_Description_Is_Changed_It_Is_Updated_In_Database(string expected)
    {
        var id = 1;

        await _sut.ChangeDescription(id, expected);
        var result = await _dbContext.Images.FindAsync(id);

        Assert.AreEqual(expected, result?.Title);
    }
    [TestMethod]
    public async Task When_Description_Is_Changed_Null_Database_Is_Not_Updated()
    {
        var id = 1;
        var expected = (await _dbContext.Images.FindAsync(id))?.Title;

        await _sut.ChangeDescription(id, null);
        var result = await _dbContext.Images.FindAsync(id);

        Assert.AreEqual(expected, result?.Title);
    }
    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(100)]
    public async Task When_Description_Is_Changed_On_An_Id_That_Is_Out_Of_Bounds_Return_Null(int id)
    {
        var result = await _sut.ChangeDescription(id, "NewTitle");

        Assert.IsNull(result);
    }
    #endregion
    #region Get Images from User
    [TestMethod]
    public async Task When_Getting_Images_With_Existing_User_Id_Return_All_Images_Posted_By_User()
    {
        var id = 1;
        var expected = _dbContext.Images.Where(Img => Img.UserID == id).ToList();

        var result = (await _sut.GetImagesFromUser(id));

        Assert.AreEqual(expected.Count, result.Count);
        Assert.IsTrue(result.Contains(expected[0]));
    }
    [TestMethod]
    [DataRow(-1)]
    [DataRow(0)]
    [DataRow(100)]
    public async Task When_Getting_Images_With_Non_Existing_User_Id_Return_Empty_List(int id)
    {
        var result = await _sut.GetImagesFromUser(id);

        Assert.AreEqual(0, result.Count);
    }
    #endregion
    #region Get Like Count
    [TestMethod]
    [DataRow(new int[] { })]
    [DataRow(new int[] { 1 })]
    [DataRow(new int[] { 1, 2, 3, 4, 5, 6, 7, 8 })]
    public async Task When_Getting_Like_Count_From_Image_Return_Amount_Of_Likes(int[] likes)
    {
        var image = new Image { UserID = 3, Path = "utsaduw", Title = "newImage%¤", Description = "012euad23", CreatedDate = new DateTime(2020, 05, 10, 8, 14, 0), Comments = new List<int>(), isPublished = true, LikedBy = likes.ToList() };
        _dbContext.Add(image);
        _dbContext.SaveChanges();
        var expected = likes.Count();

        var result = await _sut.GetLikeCount(_dbContext.Images.Count());

        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    [DataRow(-1)]
    [DataRow(0)]
    [DataRow(100)]
    public async Task When_Getting_Like_Count_From_Non_Existing_Image_Return_Null(int id)
    {
        var result = await _sut.GetLikeCount(id);

        Assert.IsNull(result);
    }
    #endregion
    #region Add Like
    [TestMethod]
    [DataRow(new int[] { })]
    [DataRow(new int[] { 1, 2, 3, 4, 5 })]
    public async Task When_Successfully_Adding_A_Like_Return_New_Like_Count(int[] likes)
    {
        var image = new Image { UserID = 3, Path = "utsaduw", Title = "newImage%¤", Description = "012euad23", CreatedDate = new DateTime(2020, 05, 10, 8, 14, 0), Comments = new List<int>(), isPublished = true, LikedBy = likes.ToList() };
        _dbContext.Add(image);
        _dbContext.SaveChanges();
        var id = _dbContext.Images.Count();
        var expected = likes.Count() + 1;

        var result = await _sut.AddLike(id, 10);

        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    [DataRow(new int[] { })]
    [DataRow(new int[] { 1, 2, 3, 4, 5 })]
    public async Task When_Successfully_Adding_A_Like_Add_User_To_Liked_List(int[] likes)
    {
        var image = new Image { UserID = 3, Path = "utsaduw", Title = "newImage%¤", Description = "012euad23", CreatedDate = new DateTime(2020, 05, 10, 8, 14, 0), Comments = new List<int>(), isPublished = true, LikedBy = likes.ToList() };
        _dbContext.Add(image);
        _dbContext.SaveChanges();
        var id = _dbContext.Images.Count();
        var userId = 10;

        var result = await _sut.AddLike(id, userId);
        var likedImage = await _dbContext.Images.FindAsync(id);

        Assert.IsTrue(likedImage?.LikedBy.Contains(userId));
    }
    [TestMethod]
    public async Task When_Adding_A_Like_To_Non_Existing_Image_Return_Null()
    {
        var result = await _sut.AddLike(100, 1);

        Assert.IsNull(result);
    }
    [TestMethod]
    public async Task When_Trying_To_Add_Like_From_User_Already_On_Like_List_Dont_Add_Them()
    {
        var likes = new int[] { 1, 2, 3, 4, 5 };
        var image = new Image { UserID = 3, Path = "utsaduw", Title = "newImage%¤", Description = "012euad23", CreatedDate = new DateTime(2020, 05, 10, 8, 14, 0), Comments = new List<int>(), isPublished = true, LikedBy = likes.ToList() };
        _dbContext.Add(image);
        _dbContext.SaveChanges();
        var id = _dbContext.Images.Count();
        var userId = 4;
        var expected = likes.Count();

        var result = await _sut.AddLike(id, userId);

        Assert.AreEqual(expected, result);
    }
    #endregion
    #region Remove Like
    [TestMethod]
    [DataRow(new int[] { 5 }, 5)]
    [DataRow(new int[] { 1, 2, 3, 4, 5 }, 5)]
    public async Task When_Successfully_Removing_A_Like_Return_New_Like_Count(int[] likes, int removedUserId)
    {
        var image = new Image { UserID = 3, Path = "utsaduw", Title = "newImage%¤", Description = "012euad23", CreatedDate = new DateTime(2020, 05, 10, 8, 14, 0), Comments = new List<int>(), isPublished = true, LikedBy = likes.ToList() };
        _dbContext.Add(image);
        _dbContext.SaveChanges();
        var id = _dbContext.Images.Count();
        var expected = likes.Count() - 1;

        var result = await _sut.RemoveLike(id, removedUserId);

        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    [DataRow(new int[] { 5 }, 5)]
    [DataRow(new int[] { 1, 2, 3, 4, 5 }, 5)]
    public async Task When_Successfully_Removing_A_Like_Remove_User_To_Liked_List(int[] likes, int removedUserId)
    {
        var image = new Image { UserID = 3, Path = "utsaduw", Title = "newImage%¤", Description = "012euad23", CreatedDate = new DateTime(2020, 05, 10, 8, 14, 0), Comments = new List<int>(), isPublished = true, LikedBy = likes.ToList() };
        _dbContext.Add(image);
        _dbContext.SaveChanges();
        var id = _dbContext.Images.Count();

        var result = await _sut.RemoveLike(id, removedUserId);
        var likedImage = await _dbContext.Images.FindAsync(id);

        Assert.IsFalse(likedImage?.LikedBy.Contains(removedUserId));
    }
    [TestMethod]
    public async Task When_Removing_A_Like_From_Non_Existing_Image_Return_Null()
    {
        var result = await _sut.RemoveLike(100, 1);

        Assert.IsNull(result);
    }
    [TestMethod]
    public async Task When_Trying_To_Remove_Like_From_User_That_Dont_Like_The_Image_Do_Nothing()
    {
        var likes = new int[] { 1, 2, 3, 4, 5 };
        var image = new Image { UserID = 3, Path = "utsaduw", Title = "newImage%¤", Description = "012euad23", CreatedDate = new DateTime(2020, 05, 10, 8, 14, 0), Comments = new List<int>(), isPublished = true, LikedBy = likes.ToList() };
        _dbContext.Add(image);
        _dbContext.SaveChanges();
        var id = _dbContext.Images.Count();
        var userId = 10;
        var expected = likes.Count();

        var result = await _sut.RemoveLike(id, userId);

        Assert.AreEqual(expected, result);
    }
    #endregion
    //Add Comment
    //Remove Comment
    #region Get Published Status
    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task When_Getting_Published_Status_From_Exsisting_Image_Return_Published_Status_As_Bool(bool expected)
    {
        var image = new Image { UserID = 3, Path = "utsaduw", Title = "newImage%¤", Description = "012euad23", CreatedDate = new DateTime(2020, 05, 10, 8, 14, 0), Comments = new List<int>(), isPublished = expected, LikedBy = new List<int>() };
        _dbContext.Add(image);
        _dbContext.SaveChanges();
        var id = _dbContext.Images.Count();

        var result = await _sut.IsPublished(id);

        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    public async Task When_Getting_Published_Status_From_Non_Existing_Image_Return_Null()
    {
        var id = 100;

        var result = await _sut.IsPublished(id);

        Assert.IsNull(result);
    }
    #endregion
    #region Change published Status
    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task When_Changing_Published_Status_Return_New_Value(bool expected)
    {
        var image = new Image { UserID = 3, Path = "utsaduw", Title = "newImage%¤", Description = "012euad23", CreatedDate = new DateTime(2020, 05, 10, 8, 14, 0), Comments = new List<int>(), isPublished = !expected, LikedBy = new List<int>() };
        _dbContext.Add(image);
        _dbContext.SaveChanges();
        var id = _dbContext.Images.Count();

        var result = await _sut.SetPublished(id, expected);

        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public async Task When_Changing_Published_Status_Update_Database(bool expected)
    {
        var image = new Image { UserID = 3, Path = "utsaduw", Title = "newImage%¤", Description = "012euad23", CreatedDate = new DateTime(2020, 05, 10, 8, 14, 0), Comments = new List<int>(), isPublished = !expected, LikedBy = new List<int>() };
        _dbContext.Add(image);
        _dbContext.SaveChanges();
        var id = _dbContext.Images.Count();

        await _sut.SetPublished(id, expected);
        var result = (await _dbContext.Images.FindAsync(id))?.isPublished;

        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    public async Task When_Trying_To_Change_Status_Of_Non_Existing_Image_Return_Null()
    {
        var id = 100;

        var result = (await _sut.SetPublished(id, true));

        Assert.IsNull(result);
    }
    #endregion
}