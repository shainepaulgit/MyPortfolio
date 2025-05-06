using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Models.InputModels
{
    public class ProjectCategoryInput:BaseInput
    {
        [Required,DisplayName("Icon Name")]
        public string IconName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
