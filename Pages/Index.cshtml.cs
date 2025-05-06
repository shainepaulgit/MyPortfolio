using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyPortfolio.Models.Entities;
using MyPortfolio.Models.Repositories.Contracts;
using MyPortfolio.Models.Services;

namespace MyPortfolio.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly FileHandling _fileUploader;
        private readonly IBaseRepository<Viewer> _viewerRepo;
        public IndexModel(IHttpContextAccessor httpContextAccessor,UserManager<AppIdentityUser> userManager, FileHandling fileUploader,SignInManager<AppIdentityUser> signInManager,IBaseRepository<Viewer> viewerRepo)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _fileUploader = fileUploader;
            _signInManager = signInManager;
            _viewerRepo = viewerRepo;
        }

        public AppIdentityUser UserData { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            string userIpAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";

            // Get the first user (consider adding a condition if needed)
            var user = await _userManager.Users.FirstOrDefaultAsync();
            if (user == null)
                return BadRequest("No User Retrieved");

            UserData = user;

            // Check if viewer with the same IP exists
            var viewer = (await _viewerRepo.GetAll())
                .OrderByDescending(x => x.Id)
                .FirstOrDefault(x => x.UserIpAddress == userIpAddress);

            DateTime now = DateTime.UtcNow.AddHours(8);

            if (viewer == null || viewer.CreatedAt.AddDays(1) <= DateTime.UtcNow)
            {
                await _viewerRepo.Add(new Viewer
                {
                    UserIpAddress = userIpAddress,
                    CreatedAt = now,
                    UpdatedAt = now
                });
            }

            Console.WriteLine(userIpAddress);
            return Page();
        }

        public async Task<IActionResult> OnGetDownloadFile(string fileName)
        {
            try
            {
                const string folderName = "ResumeFiles";
                byte[] fileBytes = await _fileUploader.DownloadFile(folderName,fileName);
                var splitedFileName = fileName.Split("=");
                return File(fileBytes, "application/octet-stream", splitedFileName[splitedFileName.Length - 1]);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}
