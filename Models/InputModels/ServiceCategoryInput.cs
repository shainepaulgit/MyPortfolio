using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Models.InputModels
{
    public class ServiceCategoryInput:BaseInput
    {
        [Required,DisplayName("Icon Name")]
        public string IconName { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
