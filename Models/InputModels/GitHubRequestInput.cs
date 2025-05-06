using MyPortfolio.Models.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Models.InputModels
{
    public class GitHubRequestInput
    {
        [Required]
        public string RepositoryName { get; set; }
        [Required,DisplayName("GitHub Username")]
        public string GitHubUserName { get; set; }
        public GitHubRequestStatus Status { get; set; }
    }
}
