using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using MyPortfolio.Configurations;
using MyPortfolio.Models.Entities;
using MyPortfolio.Models.InputModels;
using MyPortfolio.Models.Repositories.Contracts;
using System.Text;

namespace MyPortfolio.Pages.ServicesPage
{
   
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly GitHubSettings _githubSettings;
        private readonly IBaseRepository<ServiceCategory> _serviceCategRepo;
        private readonly IBaseRepository<Project> _projectRepo;
        private readonly IBaseRepository<ProjectCategory> _projectCateRepo;
        private readonly IBaseRepository<GitHubRequest> _gitHubRepo;
        private readonly IMapper _mapper;
        public IndexModel(
            IOptions<GitHubSettings> op, 
            HttpClient httpClient, 
            IBaseRepository<ServiceCategory> serviceCategRepo,
            IBaseRepository<Project> projectRepo,
            IBaseRepository<ProjectCategory> projectCategRepo,
            IBaseRepository<GitHubRequest> gitHubRepo,
            IMapper mapper)
        {
            _githubSettings = op.Value;
            _httpClient = httpClient;
            _serviceCategRepo = serviceCategRepo;
            _projectRepo = projectRepo;
            _projectCateRepo = projectCategRepo;
            _gitHubRepo = gitHubRepo;
            _mapper = mapper;

        }

        public List<ServiceCategory> ServiceCategories { get; set; }
        public List<Project> Projects { get; set; }
        public List<ProjectCategory> ProjectCategories { get; set; }

        public async Task OnGetAsync()
        {
            ServiceCategories = await _serviceCategRepo.GetAll();
            Projects = await _projectRepo.GetAll();
            ProjectCategories = await _projectCateRepo.GetAll();
        }

        public async Task <IActionResult> OnPostAsync(GitHubRequestInput input)
        {
            if(!TryValidateModel(input))
                return BadRequest(ModelState);  
            var mapped = _mapper.Map<GitHubRequest>(input);
            await _gitHubRepo.Add(mapped);
            TempData["alert-message"] = "Successfully sent your request.";
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostInviteCollaborator(GitHubRequestInput input)
        {
            input.RepositoryName = input.RepositoryName.Replace("https://github.com/shainepaulgit/", "").Replace(".git", "");
            string apiUrl = $"https://api.github.com/repos/shainepaulgit/{input.RepositoryName}/collaborators/{input.GitHubUserName}";
            var request = new HttpRequestMessage(HttpMethod.Put, apiUrl);
            request.Headers.Add("User-Agent", "MyGitHubApp");
            request.Headers.Add("Authorization", $"Bearer {_githubSettings.Token}");
            request.Headers.Add("Accept", "application/vnd.github.v3+json");
            request.Content = new StringContent("{}", Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                TempData["alert-message"] = "Successfully sent your invitation request.";
                return RedirectToPage();
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                TempData["alert-message"] = $"Error inviting collaborator: {error}";
                return RedirectToPage();
            }
        }

    }
}
