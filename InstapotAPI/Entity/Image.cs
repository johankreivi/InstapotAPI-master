
namespace InstapotAPI.Entity;
public class Image
{
    public int Id { get; set; }
    public string Path { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public int UserID { get; set; }
    public List<int> Comments { get; set; }
    public bool isPublished { get; set; }
    public List<int> LikedBy { get; set; }
}
