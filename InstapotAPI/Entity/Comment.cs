namespace InstapotAPI.Entity
{
    public class Comment
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Text { get; set; }
        public int UserID { get; set; }
        public int ImageID { get; set; }
    }
}
