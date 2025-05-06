using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Models.InputModels
{
    public class PersonalInformationInput
    {
        [Required,DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Middle Name")]
        public string? MiddleName { get; set; }
        [Required, DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required, DisplayName("Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [DisplayName("Email Address")]
        public string Email { get; set; }
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [Required, DisplayName("Website")]
        public string Website { get; set; }
        [Required]
        public string Degree { get; set; }
        public string Address { get; set; }
        [Required]
        public string ProfilePictureFileName { get; set; }
        [Required]
        public string ResumeFileName { get; set; }
    }
}
