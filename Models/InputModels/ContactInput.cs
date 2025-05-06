using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Models.InputModels
{
    public class ContactInput
    {
        [Required,DisplayName("Your Name")]
        public string Name { get; set; }
        [Required, DisplayName("Your Name")]
        public string Email { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
       
    }
}
