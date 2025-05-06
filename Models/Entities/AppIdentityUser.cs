using Microsoft.AspNetCore.Identity;

namespace MyPortfolio.Models.Entities
{
    public class AppIdentityUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Website { get; set; }
        public string Degree { get; set; }
        public string Address { get; set; }
        public string ProfilePictureFileName { get; set; }
        public string ResumeFileName { get; set; }

    }
}
