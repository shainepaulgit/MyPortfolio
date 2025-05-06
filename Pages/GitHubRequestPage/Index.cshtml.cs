using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Options;
using MyPortfolio.Configurations;
using MyPortfolio.Models.Entities;
using MyPortfolio.Models.InputModels;
using MyPortfolio.Models.Repositories.Contracts;
using System.Text;

namespace MyPortfolio.Pages.GitHubRequestPage
{
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IBaseRepository<GitHubRequest> _gitHubRepo;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly GitHubSettings _githubSettings;
        public IndexModel(
            IBaseRepository<GitHubRequest> gitHubRepo,
            IMapper mapper,
            IOptions<GitHubSettings> op,
            HttpClient httpClient
            )
        {
            _gitHubRepo = gitHubRepo;
            _mapper = mapper;
            _githubSettings = op.Value;
            _httpClient = httpClient;
        }
        public async Task<JsonResult> OnGetGitHubRequests()
        {
            var requests = await _gitHubRepo.GetAll();
            return new JsonResult(requests);
        }

        public async Task<IActionResult> OnPostRequestAction(string requestId, GitHubRequestStatus status)
        {
            var requestRecord = await _gitHubRepo.GetOne(requestId);
            requestRecord.Status = status;
            requestRecord.UpdatedAt = DateTime.UtcNow.AddHours(8);
            if (status == GitHubRequestStatus.Declined)
            {
                await _gitHubRepo.Update(requestRecord, requestId);
                TempData["alert-message"] = "Successfully declined the request.";
                return RedirectToPage();
            }

            requestRecord.RepositoryName = requestRecord.RepositoryName.Replace($"https://github.com/{_githubSettings.UserName}/", "").Replace(".git", "");
            string apiUrl = $"https://api.github.com/repos/{_githubSettings.UserName}/{requestRecord.RepositoryName}/collaborators/{requestRecord.GitHubUserName}";
            var request = new HttpRequestMessage(HttpMethod.Put, apiUrl);
            request.Headers.Add("User-Agent", "MyGitHubApp");
            request.Headers.Add("Authorization", $"Bearer {_githubSettings.Token}");
            request.Headers.Add("Accept", "application/vnd.github.v3+json");
            request.Content = new StringContent("{}", Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                await _gitHubRepo.Update(requestRecord, requestId);
                TempData["alert-message"] = "Successfully approved the request";
                return RedirectToPage();
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                TempData["alert-message"] =  error;
                return RedirectToPage();
            }
        }
        public virtual async Task<IActionResult> OnPostDelete(string recordId)
        {
            var request = await _gitHubRepo.GetOne(recordId);
            TempData["alert-message"] = "Can't delete this request record";
            if (request == null)
                return RedirectToPage();
            await _gitHubRepo.Delete(recordId);
            TempData["alert-message"] = "Successfully deleted";
            return RedirectToPage();
        }
    }
}
