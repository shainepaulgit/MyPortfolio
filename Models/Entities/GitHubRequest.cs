namespace MyPortfolio.Models.Entities
{
    public class GitHubRequest:BaseEntity
    {
        public string GitHubUserName { get; set; }
        public string RepositoryName { get; set; }
        public GitHubRequestStatus Status { get; set; }
    }
}
