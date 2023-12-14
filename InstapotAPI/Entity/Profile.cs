using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InstapotAPI.Entity
{
    public class Profile
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoggedIn { get; set; } = DateTime.Now;
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ProfilePicture { get; set; } = string.Empty;
        public bool IsVerified { get; set; } = false;
        public bool LoginStatus { get; set; } = false; 
        public List<int> Images { get; set; } = new List<int>();
        public List<int> Comments { get; set; } = new List<int>(); 
    }
}
