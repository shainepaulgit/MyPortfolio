using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Models.InputModels
{
    public class UpdateUserNameInput
    {
        [Required,DisplayName("New User Name")]
        public string UserName { get; set;  }
    }
}
