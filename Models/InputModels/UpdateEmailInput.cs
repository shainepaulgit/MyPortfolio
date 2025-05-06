using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Models.InputModels
{
    public class UpdateEmailInput
    {
        [Required,DataType(DataType.EmailAddress),DisplayName("New Email Address")]
        public string Email { get; set; }
    }
}
